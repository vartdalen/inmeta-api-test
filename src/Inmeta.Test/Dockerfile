FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /app

# Copy the project file(s) and restore dependencies
COPY . ./
RUN dotnet restore Startup/Startup.csproj

# Build the application
RUN dotnet build --configuration Release --no-restore

# Publish the application
RUN dotnet publish --configuration Release --no-restore --output out

# Create a new image with only the runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out ./

# Set the entry point for the container
ENTRYPOINT ["dotnet", "Startup.dll"]