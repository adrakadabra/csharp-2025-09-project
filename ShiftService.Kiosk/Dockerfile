# Stage 1: Build Blazor WASM
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY ["ShiftService.Kiosk/ShiftService.Kiosk.csproj", "ShiftService.Kiosk/"]
RUN dotnet restore "ShiftService.Kiosk/ShiftService.Kiosk.csproj"

# Копируем всё и билдим проект
COPY . .
WORKDIR "/src/ShiftService.Kiosk"
RUN dotnet publish "ShiftService.Kiosk.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Используем Nginx для сервирования статических файлов
FROM nginx:alpine AS final
COPY --from=build /app/publish/wwwroot /usr/share/nginx/html

# Открываем порт 80 внутри контейнера
EXPOSE 80

# Стартуем nginx
CMD ["nginx", "-g", "daemon off;"]