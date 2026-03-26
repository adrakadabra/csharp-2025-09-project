FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY Common/ Common/
COPY ["OrdersService.Api/OrdersService.Api.csproj", "OrdersService.Api/"]
RUN dotnet restore "OrdersService.Api/OrdersService.Api.csproj"

COPY . .
RUN dotnet build "OrdersService.Api/OrdersService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrdersService.Api/OrdersService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrdersService.Api.dll"]