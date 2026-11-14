# EnergyMeter System — учебный вариант (МДК 03.02)

**Назначение:** сбор показаний электросчётчиков через ASP.NET Core Web API (.NET 8). Проект демонстрирует исправление двух рисков: открытый Swagger в Production и CSRF для cookie-аутентифицированного кабинета.

**Публичный репозиторий:** https://github.com/Girfor/EnergyMeter-System

## Структура
- `src/EnergyMeter.Api` — API, модели, сервис, in-memory репозиторий и защитные фильтры
- `tests/EnergyMeter.Tests` — unit + интеграционные тесты на xUnit и FluentAssertions
- `examples/insecure` — намеренно уязвимые учебные образцы, не компилируются с основным API
- `docs` — требования и состояние тестирования
- `../.github/workflows/dotnet.yml` — сборка и тестирование через GitHub Actions
- `../Reports_text` — текстовые копии 8 отчётов; **это не DOCX**

## Запуск Windows PowerShell
Требуется .NET 8 SDK, подключение к NuGet и Git.

```powershell
cd .\Project
$env:EnergyMeter__ApiKey = 'local-demo-key-please-change'
$env:ASPNETCORE_ENVIRONMENT = 'Development'
dotnet restore .\tests\EnergyMeter.Tests\EnergyMeter.Tests.csproj
dotnet run --project .\src\EnergyMeter.Api --urls http://localhost:5208
```

Swagger работает только в Development: http://localhost:5208/swagger . В Production путь `/swagger/v1/swagger.json` должен возвращать 404.

Пример:

```powershell
$headers = @{ 'X-API-Key' = 'local-demo-key-please-change' }
$body = @{ meterId='METER-001'; valueKwh=145.6; observedAtUtc=(Get-Date).ToUniversalTime().ToString('o') } | ConvertTo-Json
Invoke-RestMethod -Uri http://localhost:5208/api/readings -Method Post -Headers $headers -ContentType 'application/json' -Body $body
Invoke-RestMethod -Uri http://localhost:5208/api/readings/METER-001 -Headers $headers
```

Тестирование:

```powershell
dotnet test .\tests\EnergyMeter.Tests\EnergyMeter.Tests.csproj --logger "trx;LogFileName=tests.trx"
```

**Ограничения:** прототип не хранит данные в постоянной БД; настоящая пользовательская аутентификация в кабинет не подключена. Приложение нельзя разворачивать как промышленную АСКУЭ без TLS, БД, управления секретами, ролями и дополнительного тестирования.

**Статус тестов:** в локальной среде сборка не запускалась из-за отсутствия .NET SDK. Смотрите фактические результаты во вкладке [Actions](https://github.com/Girfor/EnergyMeter-System/actions).

## Восемь Word-отчётов
В `Reports_text` находятся текстовые версии ПР1–ПР7 и Э1. Оригинальные оформленные DOCX с титульными страницами и рисунками доступны в архиве, переданном в чате. Чтобы поместить эти бинарные документы в репозиторий, загрузите восемь `.docx` вручную через **Add file → Upload files** в папку `Reports`.
