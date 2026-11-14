# Подтверждённые результаты GitHub Actions

**Репозиторий:** https://github.com/Girfor/EnergyMeter-System

**Проверка:** [dotnet-ci run #1 (14 ноября 2026)](https://github.com/Girfor/EnergyMeter-System/actions/runs/43539318127)

| Показатель | Результат |
| --- | --- |
| Платформа | ubuntu-latest, .NET 8 |
| Restore | success |
| Build Release | success, 0 warnings, 0 errors |
| xUnit + FluentAssertions | Passed: 9, Failed: 0, Skipped: 0, Total: 9 |
| Сохранённые материалы | TRX test-results |

В исходниках содержатся пять методов проверки в ReadingServiceTests (один параметризован двумя значениями, всего шесть тестов) и три интеграционных теста ReadingsApiTests. Все девять успешно завершились в GitHub Actions.

**Ограничения:** отсутствие постоянной БД, полноценной пользовательской аутентификации и интеграционного CSRF-теста после реального входа. Отдельные тесты на граничные значения 0 и 1 000 000 кВт·ч не автоматизированы.

**Документы:** в `Reports_text` размещены текстовые копии восьми отчётов из исходного архива. Актуализированный оригинальный отчёт ПР6 в формате Word включает этот результат CI, но бинарные `.docx` пока требуют отдельной загрузки в папку `Reports`.
