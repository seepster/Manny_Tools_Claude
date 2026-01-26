using System;
using System.ComponentModel;
using System.Globalization;

namespace Manny_Tools_Claude.DTO
{
    public class KingsleyProductItem : INotifyPropertyChanged
    {
        #region Constants
        
        // South African VAT rate (15%)
        private const decimal SA_VAT_RATE = 1.15m;
        private const decimal SA_VAT_PERCENTAGE = 15m;
        
        #endregion
        
        #region Private Fields
        private string _productCode = string.Empty;
        private string _description = string.Empty;
        private string _brandName = string.Empty;
        private string _unitSize = string.Empty;
        private string _unitOfMeasure = string.Empty;
        private string _department = string.Empty;
        private decimal _sellingPriceIncl = 0m;
        private decimal _sellingPriceExcl = 0m;
        private decimal _costPriceExcl = 0m;
        private decimal _costPriceIncl = 0m;
        private decimal _markup = 0m;
        private bool _suppressRecalculation = false;
        private decimal _sellingPrice2 = 0m;
        private decimal _sellingPrice3 = 0m;
        private decimal _sellingPrice4 = 0m;
        private decimal _sellingPrice5 = 0m;
        private decimal _taxValue = 15m;
        private string _sizeLink = string.Empty;
        private string _productSize = string.Empty;
        private string _clothingItem = "0";
        private string _minorDepartment = string.Empty;
        private string _discontinued = "0";
        private string _stockItem = "1";
        private string _tillItem = "1";
        private string _packSize = "1";
        private string _supplierCode = string.Empty;
        private string _supplierNumber = string.Empty;
        #endregion

        #region Public Properties - Editable Fields

        public string ProductCode
        {
            get => _productCode;
            set
            {
                if (_productCode != value)
                {
                    _productCode = value;
                    OnPropertyChanged(nameof(ProductCode));
                    OnPropertyChanged(nameof(ParentCode)); // ParentCode = ProductCode
                }
            }
        }

        public string ParentCode => ProductCode; // ParentCode equals ProductCode

        public string PackSize
        {
            get => _packSize;
            set
            {
                if (_packSize != value)
                {
                    _packSize = value;
                    OnPropertyChanged(nameof(PackSize));
                }
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string BrandName
        {
            get => _brandName;
            set
            {
                if (_brandName != value)
                {
                    _brandName = value;
                    OnPropertyChanged(nameof(BrandName));
                }
            }
        }

        public string UnitSize
        {
            get => _unitSize;
            set
            {
                if (_unitSize != value)
                {
                    _unitSize = value;
                    OnPropertyChanged(nameof(UnitSize));
                }
            }
        }

        public string UnitOfMeasure
        {
            get => _unitOfMeasure;
            set
            {
                if (_unitOfMeasure != value)
                {
                    _unitOfMeasure = value;
                    OnPropertyChanged(nameof(UnitOfMeasure));
                }
            }
        }

        public string Department
        {
            get => _department;
            set
            {
                if (_department != value)
                {
                    _department = value;
                    OnPropertyChanged(nameof(Department));
                }
            }
        }

        public decimal SellingPriceIncl
        {
            get => _sellingPriceIncl;
            set
            {
                if (_sellingPriceIncl != value)
                {
                    _sellingPriceIncl = value;
                    OnPropertyChanged(nameof(SellingPriceIncl));
                    // Automatic recalculation disabled - values saved exactly as entered
                    // if (!_suppressRecalculation)
                    // {
                    //     CalculateSellingPriceExcl();
                    //     RecalculateMarkupFromPrices();
                    // }
                }
            }
        }

        public decimal SellingPriceExcl
        {
            get => _sellingPriceExcl;
            set
            {
                if (_sellingPriceExcl != value)
                {
                    _sellingPriceExcl = value;
                    OnPropertyChanged(nameof(SellingPriceExcl));
                    // Automatic recalculation disabled - values saved exactly as entered
                    // if (!_suppressRecalculation)
                    // {
                    //     CalculateSellingPriceInclFromExcl();
                    //     RecalculateMarkupFromPrices();
                    // }
                }
            }
        }

        public decimal CostPriceExcl
        {
            get => _costPriceExcl;
            set
            {
                if (_costPriceExcl != value)
                {
                    _costPriceExcl = value;
                    OnPropertyChanged(nameof(CostPriceExcl));
                    // Automatic recalculation disabled - values saved exactly as entered
                    // if (!_suppressRecalculation)
                    // {
                    //     CalculateCostPriceIncl();
                    // }
                }
            }
        }

        public decimal CostPriceIncl
        {
            get => _costPriceIncl;
            set
            {
                if (_costPriceIncl != value)
                {
                    _costPriceIncl = value;
                    OnPropertyChanged(nameof(CostPriceIncl));
                    // Automatic recalculation disabled - values saved exactly as entered
                    // if (!_suppressRecalculation)
                    // {
                    //     CalculateCostPriceExclFromIncl();
                    // }
                }
            }
        }

        public string StockItem
        {
            get => _stockItem;
            set
            {
                if (_stockItem != value)
                {
                    _stockItem = value;
                    OnPropertyChanged(nameof(StockItem));
                }
            }
        }

        public string TillItem
        {
            get => _tillItem;
            set
            {
                if (_tillItem != value)
                {
                    _tillItem = value;
                    OnPropertyChanged(nameof(TillItem));
                }
            }
        }

        public decimal Markup
        {
            get => _markup;
            set
            {
                if (_markup != value)
                {
                    _markup = value;
                    OnPropertyChanged(nameof(Markup));
                    if (!_suppressRecalculation)
                    {
                        RecalculateSellingPricesFromMarkup();
                    }
                }
            }
        }

        public decimal SellingPrice2
        {
            get => _sellingPrice2;
            set
            {
                if (_sellingPrice2 != value)
                {
                    _sellingPrice2 = value;
                    OnPropertyChanged(nameof(SellingPrice2));
                }
            }
        }

        public decimal SellingPrice3
        {
            get => _sellingPrice3;
            set
            {
                if (_sellingPrice3 != value)
                {
                    _sellingPrice3 = value;
                    OnPropertyChanged(nameof(SellingPrice3));
                }
            }
        }

        public decimal SellingPrice4
        {
            get => _sellingPrice4;
            set
            {
                if (_sellingPrice4 != value)
                {
                    _sellingPrice4 = value;
                    OnPropertyChanged(nameof(SellingPrice4));
                }
            }
        }

        public decimal SellingPrice5
        {
            get => _sellingPrice5;
            set
            {
                if (_sellingPrice5 != value)
                {
                    _sellingPrice5 = value;
                    OnPropertyChanged(nameof(SellingPrice5));
                }
            }
        }

        public string SizeLink
        {
            get => _sizeLink;
            set
            {
                if (_sizeLink != value)
                {
                    _sizeLink = value;
                    OnPropertyChanged(nameof(SizeLink));
                }
            }
        }

        public string ProductSize
        {
            get => _productSize;
            set
            {
                if (_productSize != value)
                {
                    _productSize = value;
                    OnPropertyChanged(nameof(ProductSize));
                }
            }
        }

        public string ClothingItem
        {
            get => _clothingItem;
            set
            {
                if (_clothingItem != value)
                {
                    _clothingItem = value;
                    OnPropertyChanged(nameof(ClothingItem));
                }
            }
        }

        public string MinorDepartment
        {
            get => _minorDepartment;
            set
            {
                if (_minorDepartment != value)
                {
                    _minorDepartment = value;
                    OnPropertyChanged(nameof(MinorDepartment));
                }
            }
        }

        public string Discontinued
        {
            get => _discontinued;
            set
            {
                if (_discontinued != value)
                {
                    _discontinued = value;
                    OnPropertyChanged(nameof(Discontinued));
                }
            }
        }

        public decimal TaxValue
        {
            get => SA_VAT_PERCENTAGE; // Fixed South African VAT rate of 15%
            set
            {
                // South African VAT rate is fixed at 15%, but we keep the setter for compatibility
                // Store the value but always use 15% for calculations
                if (_taxValue != value)
                {
                    _taxValue = value;
                    OnPropertyChanged(nameof(TaxValue));
                }
            }
        }

        #endregion

        #region Default System Fields

        public string IsPack => "0";
        public string TaxType => null;
        public string IsComponent => "0";
        public string CombinedProduct => "0";
        public string HasContainer => "0";
        public string ContainerID => "0";
        public string ContainerInfluencesCost => "0";
        public string PositiveStockItem => "0";
        public string DateCreated => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string DateUpdate => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string FixedMarkup => "0";
        public string SalesLocation => null;
        public string Batch => "0";
        public string StockLocation => null;
        public string SerialNumberControlled => "0";
        public string DirectPriceChange => "0";
        public string MenuCategory => null;
        public string OptimisticLockField => null;
        public string GCRecord => null;
        public string ObjectType => "65";
        public string SystemProduct => "0";
        public string Sprice => "0";
        public string ProductPriceExtra => "0";
        public string ProductIsExtra => "0";
        public string AvgCost => "0";
        public string AvgPurchasePrice => "0";
        public string LastPurchaseDate => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        public string ProductID => "0";
        public string DiscontinuedReason => "0";
        public string SeasonalItem => "0";
        public string OnceOffItem => "0";
        public string Dormant => "0";

        #endregion

        #region Supplier Properties

        public string SupplierCode
        {
            get => _supplierCode;
            set
            {
                if (_supplierCode != value)
                {
                    _supplierCode = value;
                    OnPropertyChanged(nameof(SupplierCode));
                }
            }
        }

        public string SupplierNumber
        {
            get => _supplierNumber;
            set
            {
                if (_supplierNumber != value)
                {
                    _supplierNumber = value;
                    OnPropertyChanged(nameof(SupplierNumber));
                }
            }
        }

        #endregion

        #region Helper Methods

        public void CalculateSellingPriceExcl()
        {
            // South African VAT: SellingPriceExcl = SellingPriceIncl / 1.15
            _suppressRecalculation = true;
            _sellingPriceExcl = SellingPriceIncl / SA_VAT_RATE;
            _suppressRecalculation = false;
            OnPropertyChanged(nameof(SellingPriceExcl));
        }

        public void CalculateSellingPriceInclFromExcl()
        {
            // South African VAT: SellingPriceIncl = SellingPriceExcl * 1.15
            _suppressRecalculation = true;
            _sellingPriceIncl = SellingPriceExcl * SA_VAT_RATE;
            _suppressRecalculation = false;
            OnPropertyChanged(nameof(SellingPriceIncl));
        }

        public void CalculateCostPriceIncl()
        {
            // South African VAT: CostPriceIncl = CostPriceExcl * 1.15
            _suppressRecalculation = true;
            _costPriceIncl = CostPriceExcl * SA_VAT_RATE;
            _suppressRecalculation = false;
            OnPropertyChanged(nameof(CostPriceIncl));
        }

        public void CalculateCostPriceExclFromIncl()
        {
            // South African VAT: CostPriceExcl = CostPriceIncl / 1.15
            _suppressRecalculation = true;
            _costPriceExcl = CostPriceIncl / SA_VAT_RATE;
            _suppressRecalculation = false;
            OnPropertyChanged(nameof(CostPriceExcl));
            // Also recalculate selling prices based on new cost
            RecalculateSellingPricesFromMarkup();
        }

        public void RecalculateMarkupFromPrices()
        {
            // Calculate markup based on current prices: Markup % = ((SellingPriceExcl - CostPriceExcl) / CostPriceExcl) * 100
            if (CostPriceExcl > 0)
            {
                _suppressRecalculation = true;
                _markup = ((SellingPriceExcl - CostPriceExcl) / CostPriceExcl) * 100;
                _suppressRecalculation = false;
                OnPropertyChanged(nameof(Markup));
            }
        }

        public void RecalculateSellingPricesFromMarkup()
        {
            // Calculate selling prices based on markup: SellingPriceExcl = CostPriceExcl * (1 + (Markup / 100))
            if (CostPriceExcl > 0)
            {
                _suppressRecalculation = true;
                _sellingPriceExcl = CostPriceExcl * (1 + (Markup / 100));
                
                // Calculate SellingPriceIncl using South African VAT: SellingPriceIncl = SellingPriceExcl * 1.15
                _sellingPriceIncl = _sellingPriceExcl * SA_VAT_RATE;
                
                _suppressRecalculation = false;
                OnPropertyChanged(nameof(SellingPriceExcl));
                OnPropertyChanged(nameof(SellingPriceIncl));
            }
        }

        public void LoadFromTemplate(Orbit_ProductInfo template)
        {
            if (template == null) return;

            // Suppress all recalculations during template loading
            _suppressRecalculation = true;

            try
            {
                // Load all fields except ProductCode and ParentCode
                PackSize = template.PackSize ?? "1";
                Description = template.Description ?? string.Empty;
                BrandName = template.BrandName ?? string.Empty;
                UnitSize = template.UnitSize ?? string.Empty;
                UnitOfMeasure = template.UnitOfMeasure ?? string.Empty;
                Department = template.Department ?? string.Empty;

                // Parse decimal values safely - load EXACTLY as stored in database
                if (decimal.TryParse(template.SellingPriceIncl, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sellingInclValue))
                    _sellingPriceIncl = sellingInclValue;

                if (decimal.TryParse(template.SellingPriceExcl, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal sellingExclValue))
                    _sellingPriceExcl = sellingExclValue;

                if (decimal.TryParse(template.CostPriceExcl, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal costExclValue))
                    _costPriceExcl = costExclValue;

                if (decimal.TryParse(template.CostPriceIncl, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal costInclValue))
                    _costPriceIncl = costInclValue;

                if (decimal.TryParse(template.Markup, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal markupValue))
                    _markup = markupValue;

                if (decimal.TryParse(template.SellingPrice2, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal selling2Value))
                    _sellingPrice2 = selling2Value;

                if (decimal.TryParse(template.SellingPrice3, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal selling3Value))
                    _sellingPrice3 = selling3Value;

                if (decimal.TryParse(template.SellingPrice4, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal selling4Value))
                    _sellingPrice4 = selling4Value;

                if (decimal.TryParse(template.SellingPrice5, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal selling5Value))
                    _sellingPrice5 = selling5Value;

                if (decimal.TryParse(template.TaxValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal taxValue))
                    _taxValue = taxValue;

                // Convert boolean strings "True"/"False" or "1"/"0" to "1"/"0"
                StockItem = (template.StockItem?.Equals("True", StringComparison.OrdinalIgnoreCase) == true || template.StockItem == "1") ? "1" : "0";
                TillItem = (template.TillItem?.Equals("True", StringComparison.OrdinalIgnoreCase) == true || template.TillItem == "1") ? "1" : "0";
                Discontinued = (template.Discontinued?.Equals("True", StringComparison.OrdinalIgnoreCase) == true || template.Discontinued == "1") ? "1" : "0";

                SizeLink = template.SizeLink ?? string.Empty;
                ProductSize = template.ProductSize ?? string.Empty;
                ClothingItem = template.ClothingItem ?? string.Empty;
                MinorDepartment = template.MinorDepartment ?? string.Empty;
                SupplierCode = template.SupplierCode ?? string.Empty;
                SupplierNumber = template.SupplierNumber ?? string.Empty;

                // Notify all property changes at once
                OnPropertyChanged(nameof(SellingPriceIncl));
                OnPropertyChanged(nameof(SellingPriceExcl));
                OnPropertyChanged(nameof(CostPriceExcl));
                OnPropertyChanged(nameof(CostPriceIncl));
                OnPropertyChanged(nameof(Markup));
                OnPropertyChanged(nameof(SellingPrice2));
                OnPropertyChanged(nameof(SellingPrice3));
                OnPropertyChanged(nameof(SellingPrice4));
                OnPropertyChanged(nameof(SellingPrice5));
                OnPropertyChanged(nameof(TaxValue));
            }
            finally
            {
                _suppressRecalculation = false;
            }
        }

        public Orbit_ProductInfo ToOrbitProductInfo()
        {
            return new Orbit_ProductInfo
            {
                ProductCode = ProductCode,
                ParentCode = ParentCode,
                IsPack = IsPack,
                PackSize = PackSize,
                Description = Description,
                BrandName = BrandName,
                UnitSize = UnitSize,
                UnitOfMeasure = UnitOfMeasure,
                Department = Department,
                SellingPriceIncl = SellingPriceIncl.ToString(CultureInfo.InvariantCulture),
                SellingPriceExcl = SellingPriceExcl.ToString(CultureInfo.InvariantCulture),
                CostPriceExcl = CostPriceExcl.ToString(CultureInfo.InvariantCulture),
                CostPriceIncl = CostPriceIncl.ToString(CultureInfo.InvariantCulture),
                TaxType = TaxType,
                TillItem = TillItem,
                IsComponent = IsComponent,
                CombinedProduct = CombinedProduct,
                HasContainer = HasContainer,
                ContainerID = ContainerID,
                ContainerInfluencesCost = ContainerInfluencesCost,
                PositiveStockItem = PositiveStockItem,
                StockItem = StockItem,
                DateCreated = DateCreated,
                DateUpdate = DateUpdate,
                FixedMarkup = FixedMarkup,
                Markup = Markup.ToString(CultureInfo.InvariantCulture),
                SalesLocation = SalesLocation,
                Batch = Batch,
                StockLocation = StockLocation,
                SellingPrice2 = SellingPrice2.ToString(CultureInfo.InvariantCulture),
                SellingPrice3 = SellingPrice3.ToString(CultureInfo.InvariantCulture),
                SellingPrice4 = SellingPrice4.ToString(CultureInfo.InvariantCulture),
                SellingPrice5 = SellingPrice5.ToString(CultureInfo.InvariantCulture),
                TaxValue = TaxValue.ToString(CultureInfo.InvariantCulture),
                SerialNumberControlled = SerialNumberControlled,
                DirectPriceChange = DirectPriceChange,
                MenuCategory = MenuCategory,
                OptimisticLockField = OptimisticLockField,
                GCRecord = GCRecord,
                ObjectType = ObjectType,
                SizeLink = SizeLink,
                ProductSize = ProductSize,
                ClothingItem = ClothingItem,
                MinorDepartment = MinorDepartment,
                SystemProduct = SystemProduct,
                Sprice = Sprice,
                Discontinued = Discontinued,
                ProductPriceExtra = ProductPriceExtra,
                ProductIsExtra = ProductIsExtra,
                AvgCost = AvgCost,
                AvgPurchasePrice = AvgPurchasePrice,
                LastPurchaseDate = LastPurchaseDate,
                ProductID = ProductID,
                DiscontinuedReason = DiscontinuedReason,
                SeasonalItem = SeasonalItem,
                OnceOffItem = OnceOffItem,
                Dormant = Dormant,
                SupplierCode = SupplierCode,
                SupplierNumber = SupplierNumber
            };
        }

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}