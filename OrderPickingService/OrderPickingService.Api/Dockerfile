FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
#USER $APP_UID
WORKDIR /app
RUN apt-get -y update && apt-get -y install curl && apt-get clean
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrderPickingService/OrderPickingService.Api/OrderPickingService.Api.csproj", "OrderPickingService/OrderPickingService.Api/"]
COPY OrderPickingService/OrderPickingService.Api/ OrderPickingService/OrderPickingService.Api/
COPY OrderPickingService/OrderPickingService.Domain/ OrderPickingService/OrderPickingService.Domain/
COPY OrderPickingService/OrderPickingService.Infrastructure.Database/ OrderPickingService/OrderPickingService.Infrastructure.Database/
COPY OrderPickingService/OrderPickingService.Services/ OrderPickingService/OrderPickingService.Services/
COPY OrderPickingService/OrderPickingService.Services.Repositories.Abstractions/ OrderPickingService/OrderPickingService.Services.Repositories.Abstractions/
COPY Common/ Common/
COPY OrderPickingService/OrderPickingService.Infrastructure.ExternalServices/ OrderPickingService/OrderPickingService.Infrastructure.ExternalServices/
COPY OrderPickingService/OrderPickingService.Infrastructure.Outbox/ OrderPickingService/OrderPickingService.Infrastructure.Outbox/
RUN dotnet restore "OrderPickingService/OrderPickingService.Api/OrderPickingService.Api.csproj"
WORKDIR "/src/OrderPickingService/OrderPickingService.Api"
RUN dotnet build "OrderPickingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrderPickingService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderPickingService.Api.dll"]
