FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["OrderPickingServise.Api/OrderPickingServise.Api.csproj", "OrderPickingServise.Api/"]
RUN dotnet restore "OrderPickingServise.Api/OrderPickingServise.Api.csproj"
COPY . .
WORKDIR "/src/OrderPickingServise.Api"
RUN dotnet build "OrderPickingServise.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "OrderPickingServise.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderPickingServise.Api.dll"]
