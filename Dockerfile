# # Use an official .NET Core runtime as a parent image
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# # Set the working directory in the container to /app
# WORKDIR /source

# # Copy the application from the build context to the working directory.
# COPY . .

# # Restore as necessary
# RUN dotnet restore "./back-map/back-map.csproj" --disable-parallel
# RUN dotnet publish "./back-map/back-map.csproj" -c release -o /app

# FROM mcr.microsoft.com/dotnet/aspnet:7.0 
# WORKDIR /app
# COPY --from=build /app ./

# # Expose the ASP.NET Core port
# EXPOSE 5000

# # Set the entry point to the web application
# ENTRYPOINT ["dotnet", "back-map.dll"]
# Use the official .NET 8 runtime as a parent image
# Use the official .NET 8 runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:8.0

# Set the working directory in the container
WORKDIR /app

# Copy the published application into the container
COPY . .

# Expose port 80
EXPOSE 80

# Run the application
ENTRYPOINT ["dotnet", "back-map.dll"]