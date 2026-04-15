# Tastehub - Restaurant Management System

## Project Overview
Tastehub is a comprehensive Restaurant Management System designed to streamline the operations of restaurants, enhance customer service, and improve operational efficiency. The system provides tools for table reservations, order management, menu administration, employee scheduling, and detailed analytics.

## Features
- **Table Reservation Management**: Easily manage customer reservations and seating arrangements.
- **Order Management**: Track and manage customer orders from entry to delivery.
- **Menu Management**: Create, update, and delete menu items with ease.
- **Employee Management**: Schedule shifts and manage staff roles and permissions.
- **Reporting and Analytics**: Generate comprehensive reports on sales, customer feedback, and operational metrics.
- **User-Friendly Dashboard**: Intuitive admin interface for managing all restaurant operations.

## Tech Stack
- **Frontend**: .NET MVC with Razor templating engine for building dynamic user interfaces.
- **Backend**: .NET MVC for server-side logic and business operations.
- **Database**: SQL Database for reliable and scalable data storage.
- **Architecture**: MVC (Model-View-Controller) pattern for clean separation of concerns.

## Installation

### Prerequisites
- .NET Framework or .NET Core (version 4.7.2 or higher)
- SQL Server (2016 or higher)
- Visual Studio 2019 or later (recommended)

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/Anmolkrch/Tastehub.git
   cd Tastehub
   ```

2. Open the solution file in Visual Studio:
   ```bash
   Visual Studio Tastehub.sln
   ```

3. Restore NuGet packages:
   ```bash
   dotnet restore
   ```

4. Configure the database connection string in `Web.config` or `appsettings.json`:
   ```xml
   <connectionStrings>
     <add name="TasteHubConnection" connectionString="Server=YOUR_SERVER;Database=TasteHub;User Id=sa;Password=YOUR_PASSWORD;" providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

5. Apply database migrations:
   ```bash
   Update-Database
   ```

6. Build and run the application:
   ```bash
   dotnet build
   dotnet run
   ```

## Usage
1. Access the application via your web browser at `http://localhost:5000` (or the configured port).
2. Log in with your admin credentials.
3. Use the dashboard to manage:
   - Restaurant reservations
   - Customer orders
   - Menu items
   - Employee schedules
   - View sales reports and analytics

## Project Structure
```
Tastehub/
├── Models/              # Data models and entities
├── Controllers/         # MVC controllers
├── Views/              # Razor views (.cshtml files)
├── Data/               # Database context and migrations
├── Services/           # Business logic services
├── wwwroot/            # Static files (CSS, JS, images)
└── Web.config          # Configuration file
```

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch: `git checkout -b feature/YourFeature`.
3. Make your changes and commit them: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature/YourFeature`.
5. Open a pull request.

For any questions or issues, please refer to the [contribution guidelines](CONTRIBUTING.md).

## License
This project is licensed under the MIT License. See the LICENSE file for details.

## Support
For support and questions, please contact the development team or open an issue on the repository.

---

**Last Updated**: 2026-04-15 16:38:33