# HMS Integration Example

Данный проект содержит пример обмена сообщениями через интеграционную шину NServiceBus между HMS-системой и МИС "Санаториум". В коде продемонстрировано, как можно уведомлять Санаториум о новых бронях (гостях), а также показаны возможности для начисления (закрытия) мед. услуг на номер.

## Предварительные требования

Для разработки и запуска требуется следующее окружение:

1. Visual Studio 2013 и выше любой редакции
2. MSSQL Server 2008 Express и выше (можно 2012, 2014)

## Запуск проекта

1. Требуется создать локальную базу данных с именем *servicebus* (либо исправить строки соединения в файле конфигурации app.config)
2. Открыть проект в Visual Studio и пересобрать
3. Запустить проект. Он умеет работать как обычное консольное приложение, но в то же время в промышленной эксплуатации можно запустить как сервис, подробности см. командой
```
HmsSanatoriumBridge.exe help
```

## Реализация механизмов интеграции

В качестве примера разработаны две заглушки для сообщений разного типа.

1. *PostPaymentHandler.cs* - обработка событий закрытия услуг из МИС Санаториум на счёт гостя/брони/компании.
2. *ReservationsWorker.cs* - пример отправки сообщений из HMS в Санаториум с информацией об изменениях в брони.
3. *CreateProfileRequestHandler.cs* - обработка событий создания/изменения в МИС амбулаторных карт.
