# Mango User Service

A microservice for managing user operations in the Mango application, built with .NET and following Clean Architecture principles.

## 🏗️ Architecture

This project follows Clean Architecture with clear separation of concerns:

- **Mango.UserService.API** - Web API layer with controllers and endpoints
- **Mango.UserService.Application** - Business logic and use cases
- **Mango.UserService.Domain** - Domain entities and business rules
- **Mango.UserService.Infrastructure** - Data access and external services
- **Mango.UserService.Tests** - Unit and integration tests

## 🚀 Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- Visual Studio 2022 or Visual Studio Code
- SQL Server (or your preferred database)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/devshahidulla/mango-user-service.git
cd mango-user-service
```

2. Restore dependencies:
```bash
dotnet restore
```

3. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "your-connection-string-here"
}
```

4. Run the application:
```bash
dotnet run --project Mango.UserService.API
```

## 🔧 Configuration

Configure the application settings in `appsettings.json`:

- **Logging**: Configure log levels for different components
- **AllowedHosts**: Set allowed hosts for the application
- **ConnectionStrings**: Database connection configuration

## 📝 API Documentation

Once the application is running, you can access:

- Swagger UI: `https://localhost:{port}/swagger`
- API endpoints: `https://localhost:{port}/api`

## 🧪 Testing

Run the test suite:

```bash
dotnet test
```

## 🛠️ Built With

- **.NET** - Framework
- **ASP.NET Core** - Web API framework
- **Entity Framework Core** - ORM (if applicable)
- **Clean Architecture** - Architectural pattern

## 📦 Project Structure

```
mango-user-service/
├── Mango.UserService.API/           # API controllers and configuration
│   ├── Controllers/                 # API controllers
│   ├── Program.cs                   # Application entry point
│   └── appsettings.json            # Configuration
├── Mango.UserService.Application/   # Business logic layer
├── Mango.UserService.Domain/        # Domain entities
├── Mango.UserService.Infrastructure/# Data access layer
└── Mango.UserService.Tests/         # Test projects
```

## 🤝 Contributing

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 👤 Author

**devshahidulla**

- GitHub: [@devshahidulla](https://github.com/devshahidulla)

## 📞 Support

For support, please open an issue in the GitHub repository.
