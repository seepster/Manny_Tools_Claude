# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Session Management

At the start of each session, read the latest session document in the Documents/Session/ folder to understand recent work and context.

## Solution Overview

This is a multi-project C# solution containing Windows Forms applications for business management, focusing on inventory, stock management, and product sizing tools. The solution uses .NET 8.0 Windows Forms framework.

## Build Commands

```bash
# Build entire solution
dotnet build BigSolutionForAllMyProjects.sln

# Build specific project
dotnet build Manny_Tools_Claude/Manny_Tools_Claude.csproj
dotnet build SuperUserSOH/SuperUserProgram.csproj
dotnet build SimpleUserSOH/SimpleUserProgram.csproj

# Build in Release mode
dotnet build -c Release

# Clean solution
dotnet clean

# Restore NuGet packages
dotnet restore

# Run a specific project
dotnet run --project Manny_Tools_Claude/Manny_Tools_Claude.csproj
dotnet run --project SuperUserSOH/SuperUserProgram.csproj
```

## Architecture Overview

### Main Projects

1. **Manny_Tools_Claude** - Main inventory management application with:
   - User authentication and permission management
   - Product sizing tools (Baby, Descriptive, Double, Numbered sizing)
   - Stock count and stock-on-hand management
   - SQL database connectivity and schema viewing
   - Network discovery features

2. **SuperUserSOH** - Administrative stock-on-hand application with:
   - Enhanced database query capabilities
   - Column selection and filtering
   - Authorization and data lookup features

3. **SimpleUserSOH** - Simplified user interface for stock management
   - Depends on SuperUserProgram
   - Limited functionality for standard users

4. **SQL_Helpers** - Shared library for database operations
   - Generic type list retrieval
   - Schema information access

5. **DTO_Orbit** - Data Transfer Objects project
   - Shared data models across applications

### Key Dependencies

- **Dapper** (v2.1.66) - Lightweight ORM for database operations
- **Microsoft.Data.SqlClient** (v6.0.1) - SQL Server connectivity

### Database Integration Pattern

All database operations follow a consistent pattern:
- Connection management through `DatabaseConnectionManager` and `SQL_Connection_Helper`
- Schema operations via `SQL_Mapper_Schema` and `SQL_Viewer_Schema`
- Data access using Dapper with generic list helpers in `SQL_Get_Generic_List`

**IMPORTANT: Exception Handling**
- `SQL_Get_Generic_List` methods now properly throw exceptions (SqlException, Exception)
- Do NOT catch and swallow exceptions - propagate them with context
- All methods now throw exceptions instead of returning default values (0, null, empty list)
- Use try-catch in UI forms to display user-friendly error messages
- For transactions, use `ExecuteInTransaction` which has built-in rollback on error

### Authentication System

- Login forms with encrypted password storage (`DataEncryptionHelper`)
- User management and permissions system (`UserPermissions`, `UserManagementForm`)
- Default credentials stored in Documents/Project_Structure_and_Nuget.txt

### Network Features

- Network discovery functionality for finding SQL instances
- IP address selection and management
- Connection status monitoring

### Product Creation System

**Kingsley Products Create Feature:**
- `KingsleyProductsCreateForm` - UI for creating/editing products with session-based workflow
- `ProductReviewForm` - Review products before committing to database (conflict detection, remove functionality)
- `ProductCreationHelper` - Core helper class implementing 5-phase SQL process
- `ProductCreationResult` - Result tracking for per-product success/failure

**5-Phase Product Creation Process:**
1. **Phase 1: Validation** - Check product existence, validate SizeLink and MinorDepartment
2. **Phase 2: INSERT ProductInfo** - Insert into main product table with 56+ columns
3. **Phase 3: Log Prices** - Create audit trail in ProductPriceUpdateLog (supports 5 price points)
4. **Phase 4: UPDATE ProductInfo** - Refresh DateUpdate timestamp (matches original system pattern)
5. **Phase 5: Supplier Link** - Create/update entry in SupplierProductLinks table

**Database Tables:**
- `ProductInfo` - Main product information (pricing, department, supplier, sizing, flags)
- `ProductPriceUpdateLog` - Price change audit trail (Product, PriceNumber, OldPrice, NewPrice, Date, User, WorkStation)
- `SupplierProductLinks` - Product-supplier relationships (CreditorNumber, ProductCode, SupplierProductCode, ListedCostPrice)

**Key DTOs:**
- `KingsleyProductItem` - Rich domain model with INotifyPropertyChanged, validation, price calculations
- `Orbit_ProductInfo` - Database DTO with all string properties for SQL parameter passing
- `Orbit_supplierproductlinks` - Supplier link DTO
- `Orbit_ProductPriceUpdateLog` - Price log DTO

**Important Notes:**
- All decimal formatting uses `CultureInfo.InvariantCulture` to prevent locale issues
- DTO property names must exactly match database column names (case-sensitive for Dapper)
- ProductCreationHelper includes comprehensive debug logging for troubleshooting
- Supports both new product creation and updating existing products

## Development Notes

- All projects target .NET 8.0 Windows Forms
- The solution includes a WiX installer project for deployment
- Forms follow a consistent naming pattern: `*Form.cs` with corresponding Designer and resx files
- DTO pattern is used for data models in the DTO folder of Manny_Tools_Claude
- Application will be built in Visual Studio on Windows, with results reported back for verification

## Remember

### Start Session
1. Check Documents/Session/ folder for the latest session document
2. Read the most recent session file to understand context and recent changes
3. Create a new session document with format: Session_YYYY-MM-DD_Description.md
4. Document session objectives at the start

### End Session
1. Update the current session document with:
   - Work completed
   - Changes made to files
   - Any pending tasks or issues
   - Next steps for future sessions
2. Save the session document in Documents/Session/ folder
3. Update CLAUDE.md with any new patterns, features, or important learnings
4. Ensure all changes are documented for continuity

### Best Practices
- **Exception Handling:** Always propagate exceptions with context, never swallow them
- **DTO Property Names:** Must exactly match database column names (case-sensitive)
- **Decimal Formatting:** Use CultureInfo.InvariantCulture for all decimal conversions
- **Debug Logging:** Use System.Diagnostics.Debug.WriteLine for detailed troubleshooting
- **Database Operations:** Check rowsAffected after ExecuteNonQuery to detect silent failures