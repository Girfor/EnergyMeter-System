# Протокол тестирования и CI

Дата: 14.11.2026.

GitHub Actions workflow dotnet-ci, успешный прогон: https://github.com/Girfor/EnergyMeter-System/actions/runs/43539318127

.NET 8 / ubuntu-latest: восстановление NuGet, сборка и тестирование завершены успешно. Результат xUnit: Passed 9; Failed 0; Skipped 0; Total 9. Build: 0 warnings, 0 errors.

TRX сохранён в артефакте test-results. Браузерная CSRF-проверка с реальным входом пользователя и сценарии крайних значений 0 и 1 000 000 кВт·ч в число 9 выполненных тестов не входят.

EM-SEC-01 и EM-SEC-02 относятся к намеренно уязвимому учебному примеру examples/insecure; основное приложение содержит исправления, но требуется дополнительная проверка CSRF при интеграции с системой входа.
