# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore dependencies
COPY *.sln ./
COPY WebAPI/WebAPI.csproj WebAPI/
COPY Business/Business.csproj Business/
COPY Core/Core.csproj Core/
COPY DataAccess/DataAccess.csproj DataAccess/
COPY Entities/Entities.csproj Entities/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build and publish the application
RUN dotnet publish WebAPI/WebAPI.csproj -c Release -o /app/out --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN addgroup --system --gid 1001 dotnetgroup
RUN adduser --system --uid 1001 --ingroup dotnetgroup dotnetuser

# Copy published application
COPY --from=build /app/out .

# Set ownership to non-root user
RUN chown -R dotnetuser:dotnetgroup /app
USER dotnetuser

# Expose port
EXPOSE 5000

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Docker
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "WebAPI.dll"]
