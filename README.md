# Корпоративный мессенджер
Данный проект является дипломной работой. 
Это небольшая система для связи внутри некой компании, которая имеет приложение, непосредственно использующееся пользователями, сервер который обрабатывает все запросы и работает с бд.
## Запуск сервера
### Необходимые для запуска сервера:
- [Docker](https://www.docker.com/ "Ссылка на скачивание") - Используется для развертывания бд.
- [Java 17](https://www.oracle.com/java/technologies/javase/jdk17-archive-downloads.html "Ссылка на скачивание") - Используется для запуска сервера мессенджера.

### Первый запуск:
1. Имея установленный докер, прописать в консоли, находясь в папке проекта: ```docker-compose up```.
2. Перейти в [панель администрации MySQL сервера](http://localhost:8081).
3. Выполнить скрипт в панели администрации [скрипт](./DBsqlScript.sql).
4. Запустить сервер ```java -jar ./CorpMessagerServer-0.0.1-SNAPSHOT.jar```.

### Последующий запуск:
1. Прописать в консоле, находясь в папке проекта: ```docker-compose up```.
2. Запустить сервер ```java -jar ./CorpMessagerServer-0.0.1-SNAPSHOT.jar```.

## Запуск клиента
Пока не придумал, но скоро будет ...

### Структура бд
![](./eerDB.png "Устаревшая EER диаграмма базы данных.")