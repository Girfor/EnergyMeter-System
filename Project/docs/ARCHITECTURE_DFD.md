# DFD уровня 1 — EnergyMeter System

Граница доверия охватывает сервер EnergyMeter API на ASP.NET Core .NET 8. Устройства передают показания через API-ключ, а браузерный кабинет использует cookies и защиту от CSRF. БД и реальная система входа пока не интегрированы.

```mermaid
flowchart LR
    Device["Устройство: счётчик / API key"]
    Operator["Оператор: cookie + CSRF"]
    subgraph Server["Граница доверия: EnergyMeter API (.NET 8)"]
        P1["P1 Приём API: POST /api/readings"]
        P2["P2 Валидация: MeterReadingService"]
        D1[("D1 Хранилище: in-memory")]
        P3["P3 Ручной ввод: Authorize + Antiforgery"]
        P4["P4 Чтение: GET /api/readings/{meterId}"]
    end
    Device -->|"HTTPS + X-API-Key"| P1
    P1 -->|"DTO"| P2
    P2 -->|"TryAdd"| D1
    Operator -->|"POST + Anti-forgery token"| P3
    P3 -->|"Manual DTO"| P2
    Operator -->|"GET + API-Key"| P4
    D1 -->|"Список показаний"| P4
```

Слои защиты: Swagger только Development; API-ключ для устройств; `[Authorize]` и anti-forgery проверка для cookie-действия; TLS в промышленной среде; журналы и мониторинг. **Учебные ограничения:** нет Identity Provider и постоянной Production-БД.

Структурная DFD изображена в оригинальном Word-отчёте ПР7; этот Mermaid-граф — дополнительный редактируемый вариант схемы.
