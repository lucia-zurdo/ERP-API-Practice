# 🔌 ERP API Practice
 
> ASP.NET Core REST API for a business management (ERP) system. Manages clients, suppliers, articles, and purchase/sales invoicing. Secured with Auth0 JWT authentication and backed by SQL Server via Entity Framework Core.
 
![Status](https://img.shields.io/badge/status-completed%20%7C%20open%20to%20changes-brightgreen)
![License](https://img.shields.io/badge/license-MIT-blue)
![.NET](https://img.shields.io/badge/.NET-10.0-512BD4)
 
---
 
## 📋 Table of Contents
 
- [Description](#-description)
- [Tech Stack](#-tech-stack)
- [Prerequisites](#-prerequisites)
- [Installation](#-installation)
- [Usage](#-usage)
- [Project Structure](#-project-structure)
- [Environment Variables](#-environment-variables)
- [API Endpoints](#-api-endpoints)
- [License](#-license)
- [Contact](#-contact)
 
---
 
## 📖 Description
 
ERP API Practice is the backend layer of an ERP system, built as a RESTful API with ASP.NET Core. It exposes endpoints to perform full CRUD operations across the following business modules:
 
- **Clients** — customer management
- **Suppliers** — vendor management
- **Articles** — product catalogue
- **Purchase Invoices** — incoming invoicing
- **Sales Invoices** — outgoing invoicing
 
All endpoints are protected with **Auth0 JWT Bearer** authentication. The database layer uses **Entity Framework Core** with **SQL Server**, and the API is documented via **Swagger / OpenAPI**.
 
---
 
## 🛠 Tech Stack
 
| Category        | Technology                                                                                      |
|-----------------|-------------------------------------------------------------------------------------------------|
| Framework       | [ASP.NET Core](https://learn.microsoft.com/aspnet/core) (.NET 10)                              |
| Language        | C#                                                                                              |
| Database        | [SQL Server](https://www.microsoft.com/sql-server)                                             |
| ORM             | [Entity Framework Core 10](https://learn.microsoft.com/ef/core)                               |
| Authentication  | [Auth0](https://auth0.com/) via JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`)   |
| API Docs        | [Swagger / OpenAPI](https://swagger.io/) via Swashbuckle                                       |
 
---
 
## ✅ Prerequisites
 
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- A running **SQL Server** instance (local or remote)
- An [Auth0](https://auth0.com/) account with a configured **API** (audience + domain)
 
---
 
## 🚀 Installation
 
```bash
# 1. Clone the repository
git clone https://github.com/lucia-zurdo/ERP-API-Practice.git
 
# 2. Navigate into the folder
cd ERP-API-Practice
 
# 3. Restore NuGet packages
dotnet restore
 
# 4. Apply database migrations
dotnet ef database update
```
 
Then fill in your connection string and Auth0 settings in `appsettings.json` (see [Environment Variables](#-environment-variables)).
 
---
 
## 💡 Usage
 
```bash
# Start the API in development mode
dotnet run
```
 
The API will be available at `https://localhost:{port}`. Open the Swagger UI at:
 
```
https://localhost:{port}/swagger
```
 
---
 
## 📁 Project Structure
 
```
PracticeAPI/
├── Properties/
│   └── launchSettings.json      # Launch profiles (ports, env)
├── src/
│   ├── Controllers/             # API controllers (one per module)
│   ├── Data/                    # DbContext and database configuration
│   ├── DTOs/                    # Data Transfer Objects
│   │   ├── Articulos/
│   │   ├── Clientes/
│   │   ├── FacturasCompra/
│   │   ├── FacturasVenta/
│   │   └── Proveedores/
│   ├── Migrations/              # EF Core migration files
│   ├── Models/                  # Entity models
│   └── Services/                # Business logic layer
├── appsettings.json             # App configuration
└── Program.cs                   # App entry point and middleware setup
```
 
---
 
## 🔐 Environment Variables
 
Configure `appsettings.json` (or use `appsettings.Development.json` for local overrides):
 
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;User Id=...;Password=...;"
  },
  "Auth0": {
    "Domain": "your-tenant.auth0.com",
    "Audience": "your-api-audience"
  }
}
```
 
| Key                            | Description                              | Required |
|--------------------------------|------------------------------------------|:--------:|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string      | ✅       |
| `Auth0:Domain`                 | Your Auth0 tenant domain                 | ✅       |
| `Auth0:Audience`               | Your Auth0 API audience identifier       | ✅       |
 
> ⚠️ Never commit sensitive credentials. Use environment variables or [.NET Secret Manager](https://learn.microsoft.com/aspnet/core/security/app-secrets) in development.
 
---
 
## 📡 API Endpoints
 
All endpoints require a valid **Bearer token** issued by Auth0.
 
### Clients `/api/clientes`
| Method | Endpoint              | Description          |
|--------|-----------------------|----------------------|
| GET    | `/api/clientes`       | Get all clients      |
| GET    | `/api/clientes/{id}`  | Get client by ID     |
| POST   | `/api/clientes`       | Create a client      |
| PUT    | `/api/clientes/{id}`  | Update a client      |
| DELETE | `/api/clientes/{id}`  | Delete a client      |
 
### Suppliers `/api/proveedores`
| Method | Endpoint                  | Description           |
|--------|---------------------------|-----------------------|
| GET    | `/api/proveedores`        | Get all suppliers     |
| GET    | `/api/proveedores/{id}`   | Get supplier by ID    |
| POST   | `/api/proveedores`        | Create a supplier     |
| PUT    | `/api/proveedores/{id}`   | Update a supplier     |
| DELETE | `/api/proveedores/{id}`   | Delete a supplier     |
 
### Articles `/api/articulos`
| Method | Endpoint                | Description          |
|--------|-------------------------|----------------------|
| GET    | `/api/articulos`        | Get all articles     |
| GET    | `/api/articulos/{id}`   | Get article by ID    |
| POST   | `/api/articulos`        | Create an article    |
| PUT    | `/api/articulos/{id}`   | Update an article    |
| DELETE | `/api/articulos/{id}`   | Delete an article    |
 
### Purchase Invoices `/api/facturascompra`
| Method | Endpoint                       | Description                   |
|--------|--------------------------------|-------------------------------|
| GET    | `/api/facturascompra`          | Get all purchase invoices     |
| GET    | `/api/facturascompra/{id}`     | Get purchase invoice by ID    |
| POST   | `/api/facturascompra`          | Create a purchase invoice     |
| PUT    | `/api/facturascompra/{id}`     | Update a purchase invoice     |
| DELETE | `/api/facturascompra/{id}`     | Delete a purchase invoice     |
 
### Sales Invoices `/api/facturasventa`
| Method | Endpoint                     | Description                 |
|--------|------------------------------|-----------------------------|
| GET    | `/api/facturasventa`         | Get all sales invoices      |
| GET    | `/api/facturasventa/{id}`    | Get sales invoice by ID     |
| POST   | `/api/facturasventa`         | Create a sales invoice      |
| PUT    | `/api/facturasventa/{id}`    | Update a sales invoice      |
| DELETE | `/api/facturasventa/{id}`    | Delete a sales invoice      |
 
> 📄 Full interactive documentation available at `/swagger` when running the API.
 
---
 
## 📬 Contact
 
**Lucía Zurdo** — [@lucia-zurdo](https://github.com/lucia-zurdo) — [![LinkedIn](https://img.shields.io/badge/LinkedIn-0077B5?style=flat&logo=linkedin&logoColor=white)](https://www.linkedin.com/in/luc%C3%ADa-zurdo-928390370/)
 
🔗 Project link: [https://github.com/lucia-zurdo/ERP-API-Practice](https://github.com/lucia-zurdo/ERP-API-Practice)
