using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manny_Tools_Claude.DTO;

namespace Manny_Tools_Claude
{
    public static class Orbit_DTO_Create
    {
        public static Orbit_ProductInfo CreateProductInfoEntry
                                                            (
                                                             string ProductCode,
                                                             string ParentCode,
                                                             string Description,
                                                             string UnitOfMeasure,
                                                             string Department,
                                                             string ProductSize,
                                                             string SupplierCode,
                                                             string SupplierNumber,
                                                             string Markup,
                                                             string CostPriceExcl,
                                                             string DateCreated,
                                                             string TaxValue,
                                                             string SizeLink = null
                                                             )
        {
            Orbit_ProductInfo orbit_ProductInfo = new Orbit_ProductInfo();
            orbit_ProductInfo.ProductCode = ProductCode;
            orbit_ProductInfo.ParentCode = ParentCode;
            orbit_ProductInfo.IsPack = "0";
            orbit_ProductInfo.PackSize = "1";
            orbit_ProductInfo.Description = Description;
            orbit_ProductInfo.BrandName = string.Empty;
            orbit_ProductInfo.UnitSize = string.Empty;
            orbit_ProductInfo.UnitOfMeasure = UnitOfMeasure;
            orbit_ProductInfo.Department = Department;
            orbit_ProductInfo.SellingPriceIncl = string.Empty;
            orbit_ProductInfo.SellingPriceExcl = string.Empty;
            orbit_ProductInfo.CostPriceExcl = CostPriceExcl;
            orbit_ProductInfo.CostPriceIncl = string.Empty;
            orbit_ProductInfo.TaxType = string.Empty;
            orbit_ProductInfo.TillItem = "1";
            orbit_ProductInfo.IsComponent = "0";
            orbit_ProductInfo.CombinedProduct = "0";
            orbit_ProductInfo.HasContainer = "0";
            orbit_ProductInfo.ContainerID = string.Empty;
            orbit_ProductInfo.ContainerInfluencesCost = "0";
            orbit_ProductInfo.PositiveStockItem = "0";
            orbit_ProductInfo.StockItem = "1";
            orbit_ProductInfo.DateCreated = DateCreated;
            orbit_ProductInfo.DateUpdate = DateCreated;
            orbit_ProductInfo.FixedMarkup = "0";
            orbit_ProductInfo.Markup = Markup;
            orbit_ProductInfo.SalesLocation = string.Empty;
            orbit_ProductInfo.Batch = "0";
            orbit_ProductInfo.StockLocation = string.Empty;
            orbit_ProductInfo.SellingPrice2 = string.Empty;
            orbit_ProductInfo.SellingPrice3 = string.Empty;
            orbit_ProductInfo.SellingPrice4 = string.Empty;
            orbit_ProductInfo.SellingPrice5 = string.Empty;
            orbit_ProductInfo.TaxValue = TaxValue;
            orbit_ProductInfo.SerialNumberControlled = "0";
            orbit_ProductInfo.DirectPriceChange = "0";
            orbit_ProductInfo.MenuCategory = string.Empty;
            orbit_ProductInfo.OptimisticLockField = string.Empty;
            orbit_ProductInfo.GCRecord = string.Empty;
            orbit_ProductInfo.ObjectType = "65";
            orbit_ProductInfo.SizeLink = SizeLink ?? string.Empty;
            orbit_ProductInfo.ProductSize = ProductSize;
            orbit_ProductInfo.ClothingItem = "1";
            orbit_ProductInfo.MinorDepartment = string.Empty;
            orbit_ProductInfo.SystemProduct = "0";
            orbit_ProductInfo.Sprice = string.Empty;
            orbit_ProductInfo.Discontinued = "0";
            orbit_ProductInfo.ProductPriceExtra = "0";
            orbit_ProductInfo.ProductIsExtra = "0";
            orbit_ProductInfo.AvgCost = string.Empty;
            orbit_ProductInfo.AvgPurchasePrice = string.Empty;
            orbit_ProductInfo.LastPurchaseDate = string.Empty;
            orbit_ProductInfo.ProductID = "0";
            orbit_ProductInfo.DiscontinuedReason = "0";
            orbit_ProductInfo.SeasonalItem = "0";
            orbit_ProductInfo.OnceOffItem = "0";
            orbit_ProductInfo.Dormant = "0";
            orbit_ProductInfo.SupplierCode = SupplierCode;
            orbit_ProductInfo.SupplierNumber = SupplierNumber;

            return orbit_ProductInfo;


        }

        public static Orbit_supplierproductlinks CreateSupplierProductLinkEntry
                                                            (
                                                             string CreditorNumber,
                                                             string ProductCode,
                                                             string SupplierProductCode,
                                                             string ListedCostPrice,
                                                             string AccountName,
                                                             string linenumber
                                                            )
        {
            Orbit_supplierproductlinks orbit_SupplierProductLink = new Orbit_supplierproductlinks();
            orbit_SupplierProductLink.CreditorNumber = CreditorNumber;
            orbit_SupplierProductLink.ProductCode = ProductCode;
            orbit_SupplierProductLink.SupplierProductCode = SupplierProductCode;
            orbit_SupplierProductLink.ListedCostPrice = ListedCostPrice ;
            orbit_SupplierProductLink.Discount1 = "0";
            orbit_SupplierProductLink.Discount2 = "0";
            orbit_SupplierProductLink.Discount3 = "0";
            orbit_SupplierProductLink.DefaultSupplier = "0";
            orbit_SupplierProductLink.Origin = "0";
            orbit_SupplierProductLink.Transport = "0";
            orbit_SupplierProductLink.MinimumOrderQty = "0";
            orbit_SupplierProductLink.TradeDiscount = "0";
            orbit_SupplierProductLink.Tally = "0";
            orbit_SupplierProductLink.BonusQty = "0";
            orbit_SupplierProductLink.RebateCostType = "0";
            orbit_SupplierProductLink.TransportCostType = "0";
            orbit_SupplierProductLink.Rebate = "0";
            orbit_SupplierProductLink.CurrencyID = "0";
            orbit_SupplierProductLink.ParentBuyersGroup = "0";
            orbit_SupplierProductLink.WeekNumber = "0";
            orbit_SupplierProductLink.DateCreated = string.Empty;
            orbit_SupplierProductLink.DateUpdated = string.Empty;
            orbit_SupplierProductLink.BranchID = "0";
            orbit_SupplierProductLink.linenumber = linenumber;

            return orbit_SupplierProductLink;
        }

        public static Orbit_ProductPriceUpdateLog CreateProductPriceUpdateLogEntry
                                                            (
                                                             string OID,
                                                             string Product,
                                                             string PriceNumber,
                                                             string OldPrice,
                                                             string NewPrice,
                                                             string Date
                                                            )
        {
            Orbit_ProductPriceUpdateLog orbit_ProductPriceUpdateLog = new Orbit_ProductPriceUpdateLog();
            orbit_ProductPriceUpdateLog.OID = OID;
            orbit_ProductPriceUpdateLog.Product = Product;
            orbit_ProductPriceUpdateLog.PriceNumber = PriceNumber;
            orbit_ProductPriceUpdateLog.OldPrice = OldPrice;
            orbit_ProductPriceUpdateLog.NewPrice = NewPrice;
            orbit_ProductPriceUpdateLog.Date = Date;
            orbit_ProductPriceUpdateLog.User = string.Empty;
            orbit_ProductPriceUpdateLog.WorkStation = string.Empty;
            orbit_ProductPriceUpdateLog.OptimisticLockField = "0";
            orbit_ProductPriceUpdateLog.GCRecord = string.Empty;
            return orbit_ProductPriceUpdateLog;
        }

    }

}
