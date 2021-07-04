# FiguresDotStore
## Предыстория

Для своего удобства я не смог не разбить файл на несколько :)
Далее я понял, что правильно исправить архитектуру решения будет проще и надежнее, если код будет разбит по слоям.

В итоге получилось решение из нескольких проектов:
- Figures.Core - ядро системы, состоящее из домена и бизнес-логики. Также в этом ядре содержатся интерфейсы для хранилищ данных (применена инверсия зависимостей), таким образом ядро системы не зависит от источника данных.
- Figures.Data - код хранилищ даных.
- Figures.Web - ASP.NET Core Web API. Этот проект можно запустить и протестировать работу через Swagger.
- Figures.Tests - тесты. Я считаю, что разработка кода сопровождается написанием unit тестов. Тесты изолированные, тестируют компоненты по отдельности.


## Список обнаруженных проблем при код-ревью
### Проблемы с доменными классами
- Поля класса Figure были не инкапсулированы.
- Инициализация полей и методы валидации вынесены в конструктор (таким образом создать невалидный объект не удастся).
- При невалидных параметрах создания фигур вместо InvalidOperationException генерируем ArgumentOutOfRangeException.
- Для типа фигур добавлен энам FigureType.
- Создание конкретных объектов фигур вынесено в фабрику FiguresFactory.
- Добавлен метод GetPrice для класса Figure с дефолтной реализацией (предположил, что по-умолчанию стоимость будет равняться площади, полученной методом GetArea), которую переопределяют наследники при необходимости. Также в исходном коде были проблемы при получении цены (для квадрата не было case):
```sh
Positions.Select(p => p switch
				{
					Triangle => (decimal) p.GetArea() * 1.2m,
					Circle => (decimal) p.GetArea() * 0.9m
				})
				.Sum())
```
- В заказе был список фигур без задания количества позиции. Для решения этой проблемы добавлена сущность OrderPosition. У этой сущности появился метод получения суммы по позиции заказа GetSubTotal:
```sh
public decimal GetSubTotal() => Figure.GetPrice() * Count;
```
- Метод GetTotal у Order теперь не знает детали реализации конкретных объектов фигур:
```sh
public decimal GetTotal() => Positions.Sum(x => x.GetSubTotal());
```

### Бизнес-логика
- Часть логики инкапсулирована в доменных объектах.
- Логика по созданию заказа вынесена в отдельный сервис OrderService.
- При резервировании позиций заказа сначала происходит группировка по типу фигуры, чтобы минимизировать обращение к хранилищу.
- Есть проблема с блокировкой одновременного доступа к хранилищу для резервирования позиций заказа. Я решил пока что это тем, что при проверке и резервировании происходит синхронизация потоков с помощью lock (FiguresStorageLocker). Но если будет несколько инстансов приложения, то стоит задумать над улучшением этого механизма. Возможно, cтоит использовать механизм Redlock.
- В случае возникновении ошибки при сохранении заказа сделана отмена резерва позиций заказа: IFiguresStorage.UndoReserve

###  Web Api контроллер
- Контроллер неправильно вызывал асинхронный метод (без await).
- Логика вынесена из контроллера: остается только маппинг и вызов сервиса.
- Отсутствовало логирование.
- Название метода+контроллера не соответствует шаблону REST. Нужно либо изменить название контроллера+метода назвать Post, либо добавить атрибут: [Route(nameof(Order))]
- Добавлен XML-комментарий. 

### Дополнительные улучшения
- FiguresStorage сделан не статиком. Также не статиком теперь является клиент IRedisClient. Сделано для упрощения тестирования. Если потребуется один инстанс на все приложение, это можно будет настроить через DI контейнер.
- Исправлены модификаторы видимости интерфейсов/классов.
- Разработаны тесты :)

## Итоги 
В результате проведенного рефакторинга получился прототип сервиса, архитектура которого соответствует принципам SOLID. Код можно будет легко поддерживать и дорабатывать. 