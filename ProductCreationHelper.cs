using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Manny_Tools_Claude.DTO;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Helper class for creating and updating products in the database.
    /// Implements the 5-phase SQL process:
    /// 1. Validation (check existence, validate SizeLink and MinorDepartment)
    /// 2. INSERT ProductInfo (for new products) OR UPDATE ProductInfo (for existing products)
    /// 3. Log Prices (ProductPriceUpdateLog - tracks old vs new prices)
    /// 4. UPDATE ProductInfo (refresh DateUpdate timestamp)
    /// 5. Create/Update Supplier Link
    /// </summary>
    public class ProductCreationHelper
    {
        private readonly string _connectionString;
        private readonly int _userId;
        private readonly int _workStationId;

        public ProductCreationHelper(string connectionString, int userId = 1, int workStationId = 0)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _userId = userId;
            _workStationId = workStationId;
        }

        #region Public Methods

        /// <summary>
        /// Creates or updates a product in the database using the 5-phase process.
        /// Returns a ProductCreationResult indicating success or failure.
        /// </summary>
        public ProductCreationResult CreateOrUpdateProduct(KingsleyProductItem product)
        {
            System.Diagnostics.Debug.WriteLine($"\n=== ProductCreationHelper.CreateOrUpdateProduct START ===");

            if (product == null)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Product is null");
                return new ProductCreationResult("", false, "Product is null");
            }

            System.Diagnostics.Debug.WriteLine($"ProductCode: {product.ProductCode}");
            System.Diagnostics.Debug.WriteLine($"Description: {product.Description}");

            if (string.IsNullOrWhiteSpace(product.ProductCode))
            {
                System.Diagnostics.Debug.WriteLine("ERROR: Product Code is required");
                return new ProductCreationResult("", false, "Product Code is required");
            }

            try
            {
                // Phase 1: Validation
                System.Diagnostics.Debug.WriteLine("\n--- Phase 1: Validation ---");
                var validationResult = ValidateProduct(product);
                System.Diagnostics.Debug.WriteLine($"Validation Result: IsSuccess={validationResult.IsSuccess}, IsNew={validationResult.IsNew}");

                if (!validationResult.IsSuccess)
                {
                    System.Diagnostics.Debug.WriteLine($"Validation FAILED: {validationResult.ErrorMessage}");
                    return validationResult;
                }

                bool productExists = validationResult.IsNew == false;
                bool isNewProduct = validationResult.IsNew;
                System.Diagnostics.Debug.WriteLine($"Product Exists: {productExists}");
                System.Diagnostics.Debug.WriteLine($"Is New Product: {isNewProduct}");

                // Phase 2: INSERT or UPDATE ProductInfo
                if (isNewProduct)
                {
                    System.Diagnostics.Debug.WriteLine("\n--- Phase 2: INSERT ProductInfo (NEW PRODUCT) ---");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("\n--- Phase 2: UPDATE ProductInfo (EXISTING PRODUCT) ---");
                }

                try
                {
                    Phase2_InsertOrUpdateProductInfo(product, isNewProduct);
                    System.Diagnostics.Debug.WriteLine("Phase 2 completed successfully");
                }
                catch (SqlException sqlEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Phase 2 SQL EXCEPTION:");
                    System.Diagnostics.Debug.WriteLine($"  Message: {sqlEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"  SQL Error Number: {sqlEx.Number}");
                    System.Diagnostics.Debug.WriteLine($"  Line Number: {sqlEx.LineNumber}");
                    System.Diagnostics.Debug.WriteLine($"  Procedure: {sqlEx.Procedure}");
                    System.Diagnostics.Debug.WriteLine($"  Server: {sqlEx.Server}");
                    throw new Exception($"Phase 2 (INSERT ProductInfo) failed: {sqlEx.Message}", sqlEx);
                }
                catch (Exception ex2)
                {
                    System.Diagnostics.Debug.WriteLine($"Phase 2 EXCEPTION: {ex2.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex2.StackTrace}");
                    throw new Exception($"Phase 2 (INSERT ProductInfo) failed: {ex2.Message}", ex2);
                }

                // Phase 3: Log Prices
                System.Diagnostics.Debug.WriteLine("\n--- Phase 3: Log Prices ---");
                try
                {
                    Phase3_LogPrices(product, productExists);
                    System.Diagnostics.Debug.WriteLine("Phase 3 completed successfully");
                }
                catch (SqlException sqlEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Phase 3 SQL EXCEPTION:");
                    System.Diagnostics.Debug.WriteLine($"  Message: {sqlEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"  SQL Error Number: {sqlEx.Number}");
                    System.Diagnostics.Debug.WriteLine($"  Line Number: {sqlEx.LineNumber}");
                    throw new Exception($"Phase 3 (Log Prices) failed: {sqlEx.Message}", sqlEx);
                }
                catch (Exception ex3)
                {
                    System.Diagnostics.Debug.WriteLine($"Phase 3 EXCEPTION: {ex3.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex3.StackTrace}");
                    throw new Exception($"Phase 3 (Log Prices) failed: {ex3.Message}", ex3);
                }

                // Phase 4: UPDATE ProductInfo (refresh DateUpdate timestamp)
                System.Diagnostics.Debug.WriteLine("\n--- Phase 4: UPDATE ProductInfo ---");
                try
                {
                    Phase4_UpdateProductInfo(product);
                    System.Diagnostics.Debug.WriteLine("Phase 4 completed successfully");
                }
                catch (SqlException sqlEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Phase 4 SQL EXCEPTION:");
                    System.Diagnostics.Debug.WriteLine($"  Message: {sqlEx.Message}");
                    System.Diagnostics.Debug.WriteLine($"  SQL Error Number: {sqlEx.Number}");
                    throw new Exception($"Phase 4 (UPDATE ProductInfo) failed: {sqlEx.Message}", sqlEx);
                }
                catch (Exception ex4)
                {
                    System.Diagnostics.Debug.WriteLine($"Phase 4 EXCEPTION: {ex4.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex4.StackTrace}");
                    throw new Exception($"Phase 4 (UPDATE ProductInfo) failed: {ex4.Message}", ex4);
                }

                // Phase 5: Create/Update Supplier Link
                if (!string.IsNullOrWhiteSpace(product.SupplierNumber) || !string.IsNullOrWhiteSpace(product.SupplierCode))
                {
                    System.Diagnostics.Debug.WriteLine("\n--- Phase 5: Create/Update Supplier Link ---");
                    System.Diagnostics.Debug.WriteLine($"SupplierNumber: {product.SupplierNumber}");
                    System.Diagnostics.Debug.WriteLine($"SupplierCode: {product.SupplierCode}");
                    try
                    {
                        Phase5_CreateSupplierLink(product);
                        System.Diagnostics.Debug.WriteLine("Phase 5 completed successfully");
                    }
                    catch (SqlException sqlEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Phase 5 SQL EXCEPTION:");
                        System.Diagnostics.Debug.WriteLine($"  Message: {sqlEx.Message}");
                        System.Diagnostics.Debug.WriteLine($"  SQL Error Number: {sqlEx.Number}");
                        throw new Exception($"Phase 5 (Create/Update Supplier Link) failed: {sqlEx.Message}", sqlEx);
                    }
                    catch (Exception ex5)
                    {
                        System.Diagnostics.Debug.WriteLine($"Phase 5 EXCEPTION: {ex5.Message}");
                        System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex5.StackTrace}");
                        throw new Exception($"Phase 5 (Create/Update Supplier Link) failed: {ex5.Message}", ex5);
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("\n--- Phase 5: Skipped (no supplier info) ---");
                }

                System.Diagnostics.Debug.WriteLine("\n=== ProductCreationHelper.CreateOrUpdateProduct SUCCESS ===\n");
                return new ProductCreationResult(
                    product.ProductCode,
                    true,
                    "",
                    !productExists,
                    product.Description ?? ""
                );
            }
            catch (SqlException sqlEx)
            {
                System.Diagnostics.Debug.WriteLine($"\n=== ProductCreationHelper.CreateOrUpdateProduct FAILED (SQL Exception) ===");
                System.Diagnostics.Debug.WriteLine($"SQL EXCEPTION: {sqlEx.Message}");
                System.Diagnostics.Debug.WriteLine($"SQL Error Number: {sqlEx.Number}");
                System.Diagnostics.Debug.WriteLine($"Line Number: {sqlEx.LineNumber}");
                System.Diagnostics.Debug.WriteLine($"Server: {sqlEx.Server}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {sqlEx.StackTrace}");
                System.Diagnostics.Debug.WriteLine("=== END ===\n");

                string errorMsg = $"SQL Error {sqlEx.Number}: {sqlEx.Message}";
                return new ProductCreationResult(
                    product.ProductCode,
                    false,
                    errorMsg,
                    false,
                    product.Description ?? ""
                );
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\n=== ProductCreationHelper.CreateOrUpdateProduct FAILED ===");
                System.Diagnostics.Debug.WriteLine($"EXCEPTION: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException?.Message ?? "None"}");
                System.Diagnostics.Debug.WriteLine("=== END ===\n");

                return new ProductCreationResult(
                    product.ProductCode,
                    false,
                    $"Error creating product: {ex.Message}",
                    false,
                    product.Description ?? ""
                );
            }
        }

        /// <summary>
        /// Validates a product without making any database changes.
        /// Checks if product exists, validates SizeLink and MinorDepartment.
        /// </summary>
        public ProductCreationResult ValidateProduct(KingsleyProductItem product)
        {
            if (product == null)
            {
                return new ProductCreationResult("", false, "Product is null");
            }

            if (string.IsNullOrWhiteSpace(product.ProductCode))
            {
                return new ProductCreationResult("", false, "Product Code is required");
            }

            try
            {
                // Check if product exists in database
                bool productExists = CheckProductExists(product.ProductCode);

                // If ClothingItem, validate SizeLink
                if (product.ClothingItem == "1" && !string.IsNullOrWhiteSpace(product.SizeLink))
                {
                    if (!ValidateSizeLink(product.SizeLink, product.ProductSize))
                    {
                        return new ProductCreationResult(
                            product.ProductCode,
                            false,
                            $"Invalid SizeLink: {product.SizeLink} for ProductSize: {product.ProductSize}",
                            !productExists,
                            product.Description ?? ""
                        );
                    }
                }

                // Validate MinorDepartment if specified
                if (!string.IsNullOrWhiteSpace(product.MinorDepartment) && !string.IsNullOrWhiteSpace(product.Department))
                {
                    if (!ValidateMinorDepartment(product.MinorDepartment, product.Department))
                    {
                        return new ProductCreationResult(
                            product.ProductCode,
                            false,
                            $"Invalid MinorDepartment: {product.MinorDepartment} for Department: {product.Department}",
                            !productExists,
                            product.Description ?? ""
                        );
                    }
                }

                return new ProductCreationResult(
                    product.ProductCode,
                    true,
                    "",
                    !productExists,
                    product.Description ?? ""
                );
            }
            catch (Exception ex)
            {
                return new ProductCreationResult(
                    product.ProductCode,
                    false,
                    $"Validation error: {ex.Message}",
                    false,
                    product.Description ?? ""
                );
            }
        }

        #endregion

        #region Phase 1: Validation

        private bool CheckProductExists(string productCode)
        {
            string query = "SELECT COUNT(*) FROM ProductInfo WHERE ProductCode = @ProductCode AND GCRecord IS NULL";
            var parameters = new { ProductCode = productCode };

            int count = SQL_Get_Generic_List.ExecuteScalar<int>(_connectionString, query, parameters);
            return count > 0;
        }

        private bool ValidateSizeLink(string sizeLinkOID, string productSizeNumber)
        {
            if (string.IsNullOrWhiteSpace(sizeLinkOID) || string.IsNullOrWhiteSpace(productSizeNumber))
                return false;

            string query = @"
                SELECT COUNT(*)
                FROM SizeLink sl
                INNER JOIN ProductSize ps ON sl.Holder = ps.OID
                WHERE sl.GCRecord IS NULL
                  AND sl.OID = @SizeLinkOID
                  AND ps.Number = @ProductSizeNumber";

            var parameters = new
            {
                SizeLinkOID = int.Parse(sizeLinkOID),
                ProductSizeNumber = int.Parse(productSizeNumber)
            };

            int count = SQL_Get_Generic_List.ExecuteScalar<int>(_connectionString, query, parameters);
            return count > 0;
        }

        private bool ValidateMinorDepartment(string minorDepartmentNumber, string parentDepartment)
        {
            if (string.IsNullOrWhiteSpace(minorDepartmentNumber) || string.IsNullOrWhiteSpace(parentDepartment))
                return false;

            string query = @"
                SELECT COUNT(*)
                FROM MinorDepartment
                WHERE GCRecord IS NULL
                  AND MinorDepartmentNumber = @MinorDepartmentNumber
                  AND ParentDepartment = @ParentDepartment";

            var parameters = new
            {
                MinorDepartmentNumber = minorDepartmentNumber,
                ParentDepartment = parentDepartment
            };

            int count = SQL_Get_Generic_List.ExecuteScalar<int>(_connectionString, query, parameters);
            return count > 0;
        }

        #endregion

        #region Phase 2: INSERT or UPDATE ProductInfo

        private void Phase2_InsertOrUpdateProductInfo(KingsleyProductItem product, bool isNewProduct)
        {
            System.Diagnostics.Debug.WriteLine("Converting to Orbit_ProductInfo...");
            // Convert to Orbit DTO
            Orbit_ProductInfo orbitProduct = product.ToOrbitProductInfo();

            // Set DateUpdate to current time (always)
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            orbitProduct.DateUpdate = now;

            System.Diagnostics.Debug.WriteLine($"DateUpdate: {orbitProduct.DateUpdate}");
            System.Diagnostics.Debug.WriteLine($"ParentCode: {orbitProduct.ParentCode}");
            System.Diagnostics.Debug.WriteLine($"CostPriceExcl: {orbitProduct.CostPriceExcl}");
            System.Diagnostics.Debug.WriteLine($"SellingPriceIncl: {orbitProduct.SellingPriceIncl}");

            if (isNewProduct)
            {
                // Set DateCreated for new products
                orbitProduct.DateCreated = now;
                System.Diagnostics.Debug.WriteLine($"DateCreated: {orbitProduct.DateCreated}");

                // Build INSERT query (using actual ProductInfo table columns)
                string insertQuery = @"
                    INSERT INTO ProductInfo (
                        ProductID, ProductCode, ParentCode, IsPack, Dormant, Discontinued, DiscontinuedReason,
                        SeasonalItem, OnceOffItem, PackSize, AvgCost, AvgPurchasePrice, LastPurchaseDate,
                        Description, BrandName, UnitSize, UnitOfMeasure, Department, SellingPriceIncl,
                        SystemProduct, Sprice, SellingPriceExcl, CostPriceExcl, CostPriceIncl,
                        ProductPriceExtra, ProductIsExtra, TaxType, TillItem, IsComponent, CombinedProduct,
                        HasContainer, ContainerID, ContainerInfluencesCost, PositiveStockItem, StockItem,
                        DateCreated, DateUpdate, FixedMarkup, Markup, SalesLocation, Batch, StockLocation,
                        SellingPrice2, SellingPrice3, SellingPrice4, SellingPrice5, TaxValue,
                        SerialnumberControlled, SizeLink, ProductSize, ClothingItem, DirectPriceChange,
                        MinorDepartment, MenuCategory, GCRecord, ObjectType
                    )
                    VALUES (
                        @ProductID, @ProductCode, @ParentCode, @IsPack, @Dormant, @Discontinued, @DiscontinuedReason,
                        @SeasonalItem, @OnceOffItem, @PackSize, @AvgCost, @AvgPurchasePrice, @LastPurchaseDate,
                        @Description, @BrandName, @UnitSize, @UnitOfMeasure, @Department, @SellingPriceIncl,
                        @SystemProduct, @Sprice, @SellingPriceExcl, @CostPriceExcl, @CostPriceIncl,
                        @ProductPriceExtra, @ProductIsExtra, @TaxType, @TillItem, @IsComponent, @CombinedProduct,
                        @HasContainer, @ContainerID, @ContainerInfluencesCost, @PositiveStockItem, @StockItem,
                        @DateCreated, @DateUpdate, @FixedMarkup, @Markup, @SalesLocation, @Batch, @StockLocation,
                        @SellingPrice2, @SellingPrice3, @SellingPrice4, @SellingPrice5, @TaxValue,
                        @SerialnumberControlled, @SizeLink, @ProductSize, @ClothingItem, @DirectPriceChange,
                        @MinorDepartment, @MenuCategory, @GCRecord, @ObjectType
                    )";

                System.Diagnostics.Debug.WriteLine("Executing INSERT query...");
                int rowsAffected = SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, insertQuery, orbitProduct);
                System.Diagnostics.Debug.WriteLine($"INSERT completed. Rows affected: {rowsAffected}");

                if (rowsAffected == 0)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: INSERT returned 0 rows affected - product may not have been inserted!");
                }
            }
            else
            {
                // Product exists - do UPDATE with all fields
                System.Diagnostics.Debug.WriteLine("Product exists - executing UPDATE query...");

                // Build UPDATE query matching ExistingItem.txt pattern (all fields)
                string updateQuery = @"
                    UPDATE ProductInfo SET
                        ProductID = @ProductID,
                        ParentCode = @ParentCode,
                        IsPack = @IsPack,
                        Dormant = @Dormant,
                        Discontinued = @Discontinued,
                        DiscontinuedReason = @DiscontinuedReason,
                        SeasonalItem = @SeasonalItem,
                        OnceOffItem = @OnceOffItem,
                        PackSize = @PackSize,
                        AvgCost = @AvgCost,
                        AvgPurchasePrice = @AvgPurchasePrice,
                        LastPurchaseDate = @LastPurchaseDate,
                        Description = @Description,
                        BrandName = @BrandName,
                        UnitSize = @UnitSize,
                        UnitOfMeasure = @UnitOfMeasure,
                        Department = @Department,
                        SellingPriceIncl = @SellingPriceIncl,
                        SystemProduct = @SystemProduct,
                        Sprice = @Sprice,
                        SellingPriceExcl = @SellingPriceExcl,
                        CostPriceExcl = @CostPriceExcl,
                        CostPriceIncl = @CostPriceIncl,
                        ProductPriceExtra = @ProductPriceExtra,
                        ProductIsExtra = @ProductIsExtra,
                        TaxType = @TaxType,
                        TillItem = @TillItem,
                        IsComponent = @IsComponent,
                        CombinedProduct = @CombinedProduct,
                        HasContainer = @HasContainer,
                        ContainerID = @ContainerID,
                        ContainerInfluencesCost = @ContainerInfluencesCost,
                        PositiveStockItem = @PositiveStockItem,
                        StockItem = @StockItem,
                        DateUpdate = @DateUpdate,
                        FixedMarkup = @FixedMarkup,
                        Markup = @Markup,
                        SalesLocation = @SalesLocation,
                        Batch = @Batch,
                        StockLocation = @StockLocation,
                        SellingPrice2 = @SellingPrice2,
                        SellingPrice3 = @SellingPrice3,
                        SellingPrice4 = @SellingPrice4,
                        SellingPrice5 = @SellingPrice5,
                        TaxValue = @TaxValue,
                        SerialnumberControlled = @SerialnumberControlled,
                        SizeLink = @SizeLink,
                        ProductSize = @ProductSize,
                        ClothingItem = @ClothingItem,
                        DirectPriceChange = @DirectPriceChange,
                        MinorDepartment = @MinorDepartment,
                        MenuCategory = @MenuCategory,
                        GCRecord = @GCRecord,
                        ObjectType = @ObjectType
                    WHERE ProductCode = @ProductCode AND GCRecord IS NULL";

                int rowsAffected = SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, updateQuery, orbitProduct);
                System.Diagnostics.Debug.WriteLine($"UPDATE completed. Rows affected: {rowsAffected}");

                if (rowsAffected == 0)
                {
                    System.Diagnostics.Debug.WriteLine("WARNING: UPDATE returned 0 rows affected - product may not have been updated!");
                }
            }
        }

        #endregion

        #region Phase 3: Log Prices

        private void Phase3_LogPrices(KingsleyProductItem product, bool productExists)
        {
            System.Diagnostics.Debug.WriteLine($"Logging prices for ProductCode: {product.ProductCode}");
            System.Diagnostics.Debug.WriteLine($"Product Exists (get old prices): {productExists}");

            // Get old prices if product exists (for audit trail)
            decimal oldPrice1 = 0, oldPrice2 = 0, oldPrice3 = 0, oldPrice4 = 0, oldPrice5 = 0;

            if (productExists)
            {
                string priceQuery = @"
                    SELECT SellingPriceIncl, SellingPrice2, SellingPrice3, SellingPrice4, SellingPrice5
                    FROM ProductInfo
                    WHERE ProductCode = @ProductCode AND GCRecord IS NULL";

                System.Diagnostics.Debug.WriteLine("Querying for existing prices...");
                var existingProduct = SQL_Get_Generic_List.ExecuteSingle<dynamic>(_connectionString, priceQuery, new { ProductCode = product.ProductCode });

                if (existingProduct != null)
                {
                    oldPrice1 = Convert.ToDecimal(existingProduct.SellingPriceIncl ?? 0);
                    oldPrice2 = Convert.ToDecimal(existingProduct.SellingPrice2 ?? 0);
                    oldPrice3 = Convert.ToDecimal(existingProduct.SellingPrice3 ?? 0);
                    oldPrice4 = Convert.ToDecimal(existingProduct.SellingPrice4 ?? 0);
                    oldPrice5 = Convert.ToDecimal(existingProduct.SellingPrice5 ?? 0);
                    System.Diagnostics.Debug.WriteLine($"Old prices retrieved: P1={oldPrice1}, P2={oldPrice2}, P3={oldPrice3}, P4={oldPrice4}, P5={oldPrice5}");
                }
            }

            string logQuery = @"
                INSERT INTO ProductPriceUpdateLog
                (Product, PriceNumber, OldPrice, NewPrice, Date, [User], WorkStation)
                VALUES
                (@Product, @PriceNumber, @OldPrice, @NewPrice, @Date, @User, @WorkStation)";

            // Log Price 1 (SellingPriceIncl)
            if (product.SellingPriceIncl > 0)
            {
                System.Diagnostics.Debug.WriteLine($"Logging Price 1: Old={oldPrice1}, New={product.SellingPriceIncl}");
                var params1 = new
                {
                    Product = product.ProductCode,
                    PriceNumber = 1,
                    OldPrice = oldPrice1,
                    NewPrice = product.SellingPriceIncl,
                    Date = DateTime.Now,
                    User = _userId,
                    WorkStation = _workStationId
                };
                int rowsAffected = SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, logQuery, params1);
                System.Diagnostics.Debug.WriteLine($"Price 1 logged. Rows affected: {rowsAffected}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Skipping Price 1 (value is 0)");
            }

            // Log Price 2
            if (product.SellingPrice2 > 0)
            {
                var params2 = new
                {
                    Product = product.ProductCode,
                    PriceNumber = 2,
                    OldPrice = oldPrice2,
                    NewPrice = product.SellingPrice2,
                    Date = DateTime.Now,
                    User = _userId,
                    WorkStation = _workStationId
                };
                SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, logQuery, params2);
            }

            // Log Price 3
            if (product.SellingPrice3 > 0)
            {
                var params3 = new
                {
                    Product = product.ProductCode,
                    PriceNumber = 3,
                    OldPrice = oldPrice3,
                    NewPrice = product.SellingPrice3,
                    Date = DateTime.Now,
                    User = _userId,
                    WorkStation = _workStationId
                };
                SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, logQuery, params3);
            }

            // Log Price 4
            if (product.SellingPrice4 > 0)
            {
                var params4 = new
                {
                    Product = product.ProductCode,
                    PriceNumber = 4,
                    OldPrice = oldPrice4,
                    NewPrice = product.SellingPrice4,
                    Date = DateTime.Now,
                    User = _userId,
                    WorkStation = _workStationId
                };
                SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, logQuery, params4);
            }

            // Log Price 5
            if (product.SellingPrice5 > 0)
            {
                var params5 = new
                {
                    Product = product.ProductCode,
                    PriceNumber = 5,
                    OldPrice = oldPrice5,
                    NewPrice = product.SellingPrice5,
                    Date = DateTime.Now,
                    User = _userId,
                    WorkStation = _workStationId
                };
                SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, logQuery, params5);
            }
        }

        #endregion

        #region Phase 4: UPDATE ProductInfo

        private void Phase4_UpdateProductInfo(KingsleyProductItem product)
        {
            // Update DateUpdate timestamp to match the SQL pattern
            string updateQuery = @"
                UPDATE ProductInfo
                SET DateUpdate = @DateUpdate
                WHERE ProductCode = @ProductCode AND GCRecord IS NULL";

            var parameters = new
            {
                DateUpdate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"),
                ProductCode = product.ProductCode
            };

            SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, updateQuery, parameters);
        }

        #endregion

        #region Phase 5: Create Supplier Link

        private void Phase5_CreateSupplierLink(KingsleyProductItem product)
        {
            System.Diagnostics.Debug.WriteLine("Checking if supplier link exists...");
            // Check if supplier link exists
            string checkQuery = @"
                SELECT COUNT(*)
                FROM SupplierProductLinks
                WHERE ProductCode = @ProductCode
                  AND CreditorNumber = @CreditorNumber
                  AND SupplierProductCode = @SupplierProductCode";

            var checkParams = new
            {
                ProductCode = product.ProductCode,
                CreditorNumber = product.SupplierNumber ?? "",
                SupplierProductCode = product.SupplierCode ?? ""
            };

            System.Diagnostics.Debug.WriteLine($"Check params: ProductCode={checkParams.ProductCode}, CreditorNumber={checkParams.CreditorNumber}, SupplierProductCode={checkParams.SupplierProductCode}");

            int linkExists = 0;
            try
            {
                linkExists = SQL_Get_Generic_List.ExecuteScalar<int>(_connectionString, checkQuery, checkParams);
                System.Diagnostics.Debug.WriteLine($"Supplier link check result: {linkExists} existing link(s) found");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FAILED to check supplier link: {ex.Message}");
                throw;
            }

            if (linkExists > 0)
            {
                System.Diagnostics.Debug.WriteLine("Updating existing supplier link...");
                // Update existing supplier link
                string updateQuery = @"
                    UPDATE SupplierProductLinks
                    SET ListedCostPrice = @ListedCostPrice,
                        DateUpdated = @DateUpdated
                    WHERE ProductCode = @ProductCode
                      AND CreditorNumber = @CreditorNumber
                      AND SupplierProductCode = @SupplierProductCode";

                var updateParams = new
                {
                    ListedCostPrice = product.CostPriceExcl,
                    DateUpdated = DateTime.Now,
                    ProductCode = product.ProductCode,
                    CreditorNumber = product.SupplierNumber,
                    SupplierProductCode = product.SupplierCode
                };

                try
                {
                    int rowsAffected = SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, updateQuery, updateParams);
                    System.Diagnostics.Debug.WriteLine($"Supplier link updated. Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"FAILED to update supplier link: {ex.Message}");
                    throw;
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Inserting new supplier link...");
                // Insert new supplier link
                string insertQuery = @"
                    INSERT INTO SupplierProductLinks (
                        CreditorNumber, ProductCode, SupplierProductCode, ListedCostPrice,
                        Discount1, Discount2, Discount3, DefaultSupplier, Origin, Transport,
                        MinimumOrderQty, TradeDiscount, Tally, BonusQty, RebateCostType,
                        TransportCostType, Rebate, CurrencyID, ParentBuyersGroup, WeekNumber,
                        DateCreated, DateUpdated, BranchID
                    )
                    VALUES (
                        @CreditorNumber, @ProductCode, @SupplierProductCode, @ListedCostPrice,
                        @Discount1, @Discount2, @Discount3, @DefaultSupplier, @Origin, @Transport,
                        @MinimumOrderQty, @TradeDiscount, @Tally, @BonusQty, @RebateCostType,
                        @TransportCostType, @Rebate, @CurrencyID, @ParentBuyersGroup, @WeekNumber,
                        @DateCreated, @DateUpdated, @BranchID
                    )";

                var insertParams = new
                {
                    CreditorNumber = product.SupplierNumber,
                    ProductCode = product.ProductCode,
                    SupplierProductCode = product.SupplierCode,
                    ListedCostPrice = product.CostPriceExcl,
                    Discount1 = 0,
                    Discount2 = 0,
                    Discount3 = 0,
                    DefaultSupplier = 0,
                    Origin = 0,
                    Transport = 0.0,
                    MinimumOrderQty = 0.0,
                    TradeDiscount = 0.0,
                    Tally = 0,
                    BonusQty = 0,
                    RebateCostType = 0,
                    TransportCostType = 0,
                    Rebate = 0.0m,
                    CurrencyID = 0,
                    ParentBuyersGroup = 0,
                    WeekNumber = 0,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    BranchID = 0
                };

                System.Diagnostics.Debug.WriteLine($"Insert params: CreditorNumber={insertParams.CreditorNumber}, ListedCostPrice={insertParams.ListedCostPrice}");

                try
                {
                    int rowsAffected = SQL_Get_Generic_List.ExecuteNonQuery(_connectionString, insertQuery, insertParams);
                    System.Diagnostics.Debug.WriteLine($"Supplier link inserted. Rows affected: {rowsAffected}");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"FAILED to insert supplier link: {ex.Message}");
                    if (ex.InnerException != null)
                        System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    throw;
                }
            }
        }

        #endregion
    }
}
