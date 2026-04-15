# Use the official .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

# Copy the csproj and restore any dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the project files and build the application
COPY . ./
RUN dotnet publish -c Release -o out

# Use the official ASP.NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose the port the app runs on
EXPOSE 80

# Set the entry point for the application
ENTRYPOINT ["dotnet", "YourProjectName.dll"]