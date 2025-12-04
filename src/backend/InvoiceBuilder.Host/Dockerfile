FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "."]
COPY ["Directory.Build.props", "."]
COPY ["ModularMonolith.Host/ModularMonolith.Host.csproj", "ModularMonolith.Host/"]
COPY ["Carriers/Modules.Carriers.Features/Modules.Carriers.Features.csproj", "Carriers/Modules.Carriers.Features/"]
COPY ["Common/Modules.Common.API/Modules.Common.API.csproj", "Common/Modules.Common.API/"]
COPY ["Common/Modules.Common.Domain/Modules.Common.Domain.csproj", "Common/Modules.Common.Domain/"]
COPY ["Common/Modules.Common.Application/Modules.Common.Application.csproj", "Common/Modules.Common.Application/"]
COPY ["Carriers/Modules.Carriers.Domain/Modules.Carriers.Domain.csproj", "Carriers/Modules.Carriers.Domain/"]
COPY ["Carriers/Modules.Carriers.Infrastructure/Modules.Carriers.Infrastructure.csproj", "Carriers/Modules.Carriers.Infrastructure/"]
COPY ["Common/Modules.Common.Infrastructure/Modules.Common.Infrastructure.csproj", "Common/Modules.Common.Infrastructure/"]
COPY ["Carriers/Modules.Carriers.PublicApi/Modules.Carriers.PublicApi.csproj", "Carriers/Modules.Carriers.PublicApi/"]
COPY ["Shipments/Modules.Shipments.Features/Modules.Shipments.Features.csproj", "Shipments/Modules.Shipments.Features/"]
COPY ["Stocks/Modules.Stocks.PublicApi/Modules.Stocks.PublicApi.csproj", "Stocks/Modules.Stocks.PublicApi/"]
COPY ["Shipments/Modules.Shipments.Domain/Modules.Shipments.Domain.csproj", "Shipments/Modules.Shipments.Domain/"]
COPY ["Shipments/Modules.Shipments.Infrastructure/Modules.Shipments.Infrastructure.csproj", "Shipments/Modules.Shipments.Infrastructure/"]
COPY ["Stocks/Modules.Stocks.Features/Modules.Stocks.Features.csproj", "Stocks/Modules.Stocks.Features/"]
COPY ["Stocks/Modules.Stocks.Domain/Modules.Stocks.Domain.csproj", "Stocks/Modules.Stocks.Domain/"]
COPY ["Stocks/Modules.Stocks.Infrastructure/Modules.Stocks.Infrastructure.csproj", "Stocks/Modules.Stocks.Infrastructure/"]
COPY ["Users/Modules.Users.Features/Modules.Users.Features.csproj", "Users/Modules.Users.Features/"]
COPY ["Users/Modules.Users.Infrastructure/Modules.Users.Infrastructure.csproj", "Users/Modules.Users.Infrastructure/"]
COPY ["Users/Modules.Users.Domain/Modules.Users.Domain.csproj", "Users/Modules.Users.Domain/"]
RUN dotnet restore "ModularMonolith.Host/ModularMonolith.Host.csproj"
COPY . .
WORKDIR "/src/ModularMonolith.Host"
RUN dotnet build "./ModularMonolith.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ModularMonolith.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ModularMonolith.Host.dll"]
