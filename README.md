# Point of Sale API

## About

Point of sale system developed for a local supermarket in Upala, Costa Rica. Built as a real-world project with full business module coverage including sales, inventory, payroll and cash register management.

## Overview

REST API for a point of sale system built with ASP.NET Core and Dapper. Developed as a real-world project featuring JWT authentication, stored procedures, and full business module coverage.

## Tech Stack

- **Backend:** ASP.NET Core (.NET 8)
- **Database:** SQL Server
- **Data Access:** Dapper + Stored Procedures
- **Authentication:** JWT Bearer Tokens
- **Documentation:** Swagger / OpenAPI

## Modules

- **Auth** - JWT authentication and authorization
- **Products** - Product catalog management
- **Categories** - Product category management
- **Inventory** - Stock tracking and movements
- **Cart** - Shopping cart management
- **Invoices** - Invoice generation and management
- **Employees** - Employee management
- **Payroll** - Payroll processing
- **Suppliers** - Supplier management
- **Cash Register** - Cash register audit (arqueo)
- **Dashboard** - Sales and business metrics

## Architecture
```
PuntoVentaAPI/
├── Controllers/    → HTTP endpoints
├── Entities/       → Data models
├── Interfaces/     → Service contracts
├── Models/         → Business logic and Dapper queries
```

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
```bash
git clone https://github.com/jaime2804/PuntoVentaAPI-.git
cd PuntoVentaAPI-
```

2. Create `appsettings.json` based on the example
```bash
cp appsettings.example.json appsettings.json
```

3. Update the connection string and JWT settings in `appsettings.json`

4. Run the project
```bash
dotnet run
```

5. Open Swagger at `https://localhost:{port}/swagger`

## Technical Decisions

- **Dapper over EF Core** for direct SQL control and better performance with complex queries
- **Stored Procedures** for critical business logic kept at the database level
- **JWT Authentication** for stateless and secure API access
- **Interface-based architecture** for loose coupling between layers
