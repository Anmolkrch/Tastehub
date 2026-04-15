# Tastehub Deployment Guide

## 1. Local Development Setup

### Prerequisites
- .NET Framework 4.7.2+ or .NET Core 6.0+
- SQL Server 2016+
- Visual Studio 2019 or later
- Git

### Steps
```bash
# Clone the repository
git clone https://github.com/Anmolkrch/Tastehub.git
cd Tastehub

# Restore NuGet packages
dotnet restore

# Build the solution
dotnet build

# Apply database migrations
Update-Database

# Run locally
dotnet run
```

Access at `http://localhost:5000`

---

## 2. Azure App Service Deployment

### Prerequisites
- Azure subscription
- Azure CLI installed

### Deployment Steps

1. **Create Azure resources:**
   ```bash
   # Create resource group
   az group create --name TastehubRG --location eastus
   
   # Create App Service plan
   az appservice plan create --name TastehubPlan --resource-group TastehubRG --sku B2
   
   # Create web app
   az webapp create --resource-group TastehubRG --plan TastehubPlan --name tastehub-app
   
   # Create SQL Server
   az sql server create --name tastehub-sql-server --resource-group TastehubRG --admin-user sqladmin --admin-password YourPassword123!
   
   # Create SQL Database
   az sql db create --resource-group TastehubRG --server tastehub-sql-server --name TasteHub
   ```

2. **Prepare application for deployment:**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

3. **Deploy to Azure:**
   ```bash
   # Method 1: Using ZipDeploy
   cd publish
   zip -r ../tastehub.zip *
   az webapp deployment source config-zip --resource-group TastehubRG --name tastehub-app --src ../tastehub.zip
   
   # Method 2: Using Git deployment
   az webapp deployment source config-local-git --name tastehub-app --resource-group TastehubRG
   git remote add azure <git-clone-url>
   git push azure main
   ```

4. **Configure connection strings in Azure:**
   ```bash
   az webapp config connection-string set --name tastehub-app --resource-group TastehubRG --settings TasteHubConnection="Server=tcp:tastehub-sql-server.database.windows.net,1433;Initial Catalog=TasteHub;Persist Security Info=False;User ID=sqladmin;Password=YourPassword123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" --connection-string-type SQLServer
   ```

5. **Enable HTTPS:**
   ```bash
   # Add SSL certificate (or use Azure-managed certificate)
   az webapp config ssl bind --certificate-thumbprint <thumbprint> --ssl-type SNI --name tastehub-app --resource-group TastehubRG
   ```

---

## 3. IIS Deployment (On-Premises/VPS)

### Prerequisites
- Windows Server 2016+
- IIS 10+
- .NET Framework 4.7.2+ installed
- SQL Server accessible from the server

### Deployment Steps

1. **On your local machine, publish the application:**
   ```bash
   dotnet publish -c Release -o ./publish
   ```

2. **On Windows Server, install IIS with ASP.NET:**
   ```powershell
   # Install IIS with ASP.NET support
   Install-WindowsFeature Web-Server
   Install-WindowsFeature Web-Asp-Net45
   ```

3. **Create IIS Application Pool:**
   ```powershell
   Import-Module WebAdministration
   New-WebAppPool -Name "TastehubPool" -Force
   Set-ItemProperty "IIS:\AppPools\TastehubPool" -Name "managedRuntimeVersion" -Value "v4.0"
   Set-ItemProperty "IIS:\AppPools\TastehubPool" -Name "managedPipelineMode" -Value "Integrated"
   ```

4. **Create Website in IIS:**
   ```powershell
   New-Website -Name "Tastehub" `
     -PhysicalPath "C:\inetpub\wwwroot\Tastehub" `
     -ApplicationPool "TastehubPool" `
     -Port 80 `
     -Force
   ```

5. **Deploy files:**
   ```powershell
   # Copy published files to IIS directory
   Copy-Item -Path ".\publish\*" -Destination "C:\inetpub\wwwroot\Tastehub\" -Recurse -Force
   ```

6. **Set folder permissions:**
   ```powershell
   # Grant IIS App Pool user read/write access
   $acl = Get-Acl "C:\inetpub\wwwroot\Tastehub"
   $rule = New-Object System.Security.AccessControl.FileSystemAccessRule("IIS AppPool\TastehubPool", "Modify", "ContainerInherit,ObjectInherit", "None", "Allow")
   $acl.SetAccessRule($rule)
   Set-Acl "C:\inetpub\wwwroot\Tastehub" $acl
   ```

7. **Update Web.config with production settings:**
   ```xml
   <configuration>
     <connectionStrings>
       <add name="TasteHubConnection" connectionString="Server=YOUR_SQL_SERVER;Database=TasteHub;User Id=sa;Password=YOUR_PASSWORD;" providerName="System.Data.SqlClient" />
     </connectionStrings>
     <system.web>
       <compilation debug="false" targetFramework="4.7.2" />
       <customErrors mode="RemoteOnly" defaultRedirect="~/Error" />
     </system.web>
   </configuration>
   ```

---

## 4. Docker Deployment

### Create Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/framework/aspnet:4.8 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /src
COPY ["Tastehub.csproj", "./"]
RUN dotnet restore "Tastehub.csproj"
COPY . .
RUN dotnet build "Tastehub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Tastehub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tastehub.dll"]
```

### Build and Deploy

```bash
# Build Docker image
docker build -t tastehub:latest .

# Run locally
docker run -p 8080:80 -e "TasteHubConnection=Server=sql-server;Database=TasteHub;User Id=sa;Password=YOUR_PASSWORD;" tastehub:latest

# Push to registry (Azure Container Registry, Docker Hub, etc.)
docker tag tastehub:latest your-registry.azurecr.io/tastehub:latest
docker push your-registry.azurecr.io/tastehub:latest

# Deploy to Azure Container Instances
az container create --resource-group TastehubRG --name tastehub-container --image your-registry.azurecr.io/tastehub:latest --cpu 1 --memory 1.5 --port 80
```

---

## 5. GitHub Actions CI/CD Pipeline

Create `.github/workflows/deploy.yml`:

```yaml
name: Deploy Tastehub

on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-deploy:
    runs-on: windows-latest

    steps:
    - uses: actions/checkout@v2

    - name: Setup .NET
      uses: actions/setup-dotnet@v1
      with:
        dotnet-version: '4.7.2'

    - name: Restore dependencies
      run: dotnet restore

    - name: Build
      run: dotnet build --configuration Release --no-restore

    - name: Publish
      run: dotnet publish -c Release -o ./publish

    - name: Deploy to Azure
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'tastehub-app'
        publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
        package: './publish'
```

---

## 6. Pre-Deployment Checklist

- ✅ Update `Web.config` connection strings for production environment
- ✅ Set `compilation debug="false"` in Web.config
- ✅ Configure custom error pages
- ✅ Enable HTTPS/SSL certificates
- ✅ Set up database backups and recovery plans
- ✅ Configure firewall rules for database access
- ✅ Test thoroughly in staging environment
- ✅ Set up logging and monitoring (Application Insights for Azure)
- ✅ Configure application pool recycling settings (IIS)
- ✅ Set up email notifications for errors
- ✅ Review and configure security headers
- ✅ Implement rate limiting for API endpoints

---

## 7. Post-Deployment Verification

1. **Test core functionality:**
   - User login
   - Table reservations
   - Order management
   - Menu operations
   - Reports generation

2. **Monitor performance:**
   - Application health checks
   - Database connectivity
   - Response times
   - Error logs

3. **Security verification:**
   - HTTPS enabled
   - SQL injection prevention
   - CSRF token validation
   - Authentication working correctly

---

## 8. Troubleshooting

### Database Connection Issues
```
Error: "Cannot connect to database"
Solution:
- Verify connection string in Web.config
- Check SQL Server firewall rules
- Ensure SQL Server is running
- Verify user credentials and permissions
```

### Application Pool Crashes (IIS)
```
Solution:
- Check Application Event Viewer for detailed errors
- Verify .NET Framework version matches
- Check file permissions on application folder
- Review IIS logs in C:\inetpub\logs\LogFiles\
```

### Azure Deployment Failed
```
Solution:
- Check Azure deployment logs
- Verify app settings and connection strings
- Run: az webapp log tail --name tastehub-app --resource-group TastehubRG
```

---

## 9. Maintenance & Updates

### Regular Tasks
- Monitor application logs daily
- Backup database weekly
- Update security patches monthly
- Review performance metrics
- Clean up old logs

### Deployment Updates
```bash
# Pull latest changes
git pull origin main

# Publish new version
dotnet publish -c Release -o ./publish

# Deploy using your chosen method (Azure, IIS, etc.)
```

---

## Support

For issues or questions:
- Check application logs
- Review deployment guide troubleshooting section
- Contact development team
- Open GitHub issue: https://github.com/Anmolkrch/Tastehub/issues

Last Updated: 2026-04-15 16:52:46