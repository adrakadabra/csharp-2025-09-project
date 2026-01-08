#!/bin/bash
# Простой скрипт - ждет и проверяет импорт

echo "Ожидание запуска Keycloak..."

# Проверяем каждые 5 секунд, максимум 60 попыток
for i in {1..60}; do
    if curl -s -f http://localhost:8080/health/ready > /dev/null; then
        echo "Keycloak запущен!"
        
        # Проверяем наличие импортированных realms
        echo "Проверка импортированных realms..."
        
        if ls /opt/keycloak/data/export/*.json 1> /dev/null 2>&1; then
            echo "Найдены файлы экспорта:"
            ls -la /opt/keycloak/data/export/*.json
        fi
        
        if ls /opt/keycloak/data/import/*.json 1> /dev/null 2>&1; then
            echo "Найдены файлы для импорта:"
            ls -la /opt/keycloak/data/import/*.json
        fi
        
        exit 0
    fi
    sleep 5
    echo "Попытка $i/60..."
done

echo "Timeout! Keycloak не запустился"
exit 1