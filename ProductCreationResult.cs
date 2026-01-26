using System;

namespace Manny_Tools_Claude
{
    /// <summary>
    /// Represents the result of a product creation or update operation.
    /// Used to track individual product success/failure for UI display.
    /// </summary>
    public class ProductCreationResult
    {
        /// <summary>
        /// The ProductCode of the product that was created or updated
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error message if operation failed, otherwise null or empty
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// True if this was a new product (INSERT), false if it was an update (UPDATE/OVERRIDE)
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// The description of the product for display purposes
        /// </summary>
        public string Description { get; set; }

        public ProductCreationResult()
        {
            ProductCode = string.Empty;
            IsSuccess = false;
            ErrorMessage = string.Empty;
            IsNew = true;
            Description = string.Empty;
        }

        public ProductCreationResult(string productCode, bool isSuccess, string errorMessage = "", bool isNew = true, string description = "")
        {
            ProductCode = productCode;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage ?? string.Empty;
            IsNew = isNew;
            Description = description ?? string.Empty;
        }

        public override string ToString()
        {
            string status = IsNew ? "NEW" : "UPDATE";
            string result = IsSuccess ? "SUCCESS" : "FAILED";
            return $"[{status}] {ProductCode} - {result}" + (IsSuccess ? "" : $": {ErrorMessage}");
        }
    }
}
