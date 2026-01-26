# Changelog

All notable changes to Manny Tools will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2025-01-16

### Added
- **Kingsley Products Create Feature**
  - Complete product creation workflow with session management
  - 5-phase SQL process for robust product insertion
  - Product review form with conflict detection
  - Support for creating/updating products with comprehensive validation
  - Price tracking across 5 price points with audit logging
  - Supplier link management

- **Form Validation System**
  - 20 hard validation rules with red/pink UI indicators
  - 4 soft warning validations with yellow indicators
  - Real-time validation feedback on all input fields
  - Required field validation for Product Code, Description, Department, Minor Department, Product Size, SizeLink, Supplier, etc.
  - Business logic validation for profit margins and pricing rules

- **Tax Value Field**
  - User-editable Tax Value field (defaults to 15% South African VAT)
  - Validation range 0-100%
  - Positioned in Product Details section

- **Database Connection Management**
  - Automatic dropdown data reload when database connection changes
  - Event-based connection change handling
  - Graceful error handling with user notifications
  - Session product clearing on connection change

- **User Interface Improvements**
  - Left-aligned pricing fields for better readability
  - Comprehensive tooltips and labels
  - Color-coded validation feedback (red for errors, yellow for warnings)
  - Version number displayed on login page

- **Version Management**
  - Centralized AppVersion class for version tracking
  - Version 1.0.0 displayed on login form
  - Release date tracking

### Technical Details
- **Database Tables**: ProductInfo, ProductPriceUpdateLog, SupplierProductLinks
- **DTOs**: KingsleyProductItem, Orbit_ProductInfo, Orbit_supplierproductlinks, Orbit_ProductPriceUpdateLog
- **Exception Handling**: All SQL operations properly throw exceptions with context
- **Decimal Formatting**: Uses CultureInfo.InvariantCulture to prevent locale issues
- **Property Name Matching**: DTO properties exactly match database column names (case-sensitive)

### Dependencies
- .NET 8.0 Windows Forms
- Dapper v2.1.66
- Microsoft.Data.SqlClient v6.0.1

### Known Issues
- None at this time

### Future Enhancements
- User acceptance testing completion
- Additional pricing automation features
- Enhanced reporting capabilities

---

## Version History

- **1.0.0** (2025-01-16) - Initial production release with Kingsley Products Create feature
