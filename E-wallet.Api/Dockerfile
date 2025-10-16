# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /src

# Copy csproj files first for caching
COPY E-wallet.Api/E-wallet.Api.csproj E-wallet.Api/
COPY E-wallet.Application/E-wallet.Application.csproj E-wallet.Application/
COPY E-wallet.Domain/E-wallet.Domain.csproj E-wallet.Domain/
COPY E-wallet.Infrastrucure/E-wallet.Infrastrucure.csproj E-wallet.Infrastrucure/

# Restore dependencies
RUN dotnet restore E-wallet.Api/E-wallet.Api.csproj

# Copy everything else
COPY . .

COPY E-wallet.Api/appsettings.json ./appsettings.json

# Publish the API project
WORKDIR /src/E-wallet.Api
RUN dotnet publish -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build-env /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "E-wallet.Api.dll"]