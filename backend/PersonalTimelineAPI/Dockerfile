# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Create data directory for SQLite
RUN mkdir -p /var/data

# Install dotnet-ef tool for migrations
RUN dotnet tool install --global dotnet-ef --version 8.*
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy published app
COPY --from=build /app/out .

# Copy migration files
COPY --from=build /app/Migrations ./Migrations

# Expose port
EXPOSE 8080

# Set environment variable
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT:-8080}

# Run migrations and start app
CMD dotnet ef database update && dotnet PersonalTimelineAPI.dll