# DentalClinicApp

WPF-приложение для стоматологической клиники, автоматизирующее загрузку, обработку и хранение фотографий пациентов с привязкой к их карточкам.

## Функционал

- Управление пациентами (добавление, выбор)
- Загрузка фотографий (drag&drop и выбор файлов)
- Автоматическое изменение размера фотографий до 1920x1080 с сохранением пропорций
- Хранение изображений в локальной папке `/images/{patientId}/`
- Отображение списка фотографий с датой загрузки
- Логирование операций с помощью Serilog
- Работа с базой данных PostgreSQL через Entity Framework Core

---

## Технологии

- .NET 8
- WPF (.NET)
- Entity Framework Core (PostgreSQL)
- Serilog (логирование)
- ImageSharp (обработка изображений)

---

## Запуск приложения

1. Склонируйте репозиторий

```git
git clone https://github.com/Semyon25/DentalClinicApp.git
```

или скопируйте исходники из [репозитория](https://github.com/Semyon25/DentalClinicApp)

2. Установите необходимые NuGet пакеты:

```bash
dotnet restore
```

3. Выполните сборку проекта:

```bash
dotnet build --configuration Release --no-restore
```

---

## Настройка PostgreSQL

Этот проект содержит `docker-compose.yml` для быстрого запуска PostgreSQL в контейнере для локальной разработки.

### Требования

- Установленный [Docker](https://www.docker.com/products/docker-desktop) (Docker Desktop для Windows/macOS или Docker Engine для Linux)
- Docker Compose (входит в состав Docker Desktop)

### Как запустить базу данных

1. Запустите контейнер с PostgreSQL:

```bash
docker compose up -d
```

2. Проверьте, что контейнер запущен:

```bash
docker ps
```

### Как остановить и удалить контейнер

Остановить контейнер:

```bash
docker compose down
```

Остановить и удалить тома с данными:

```bash
docker compose down -v
```

### Управление БД

Для управления базой можно использовать pgAdmin.
Он доступен по адресу: `http://localhost:5050`

Вход в pgAdmin:

- **Email**: admin@admin.com
- **Пароль**: admin

Подключение к PostgreSQL в pgAdmin:

1. В интерфейсе pgAdmin нажмите `Add New Server`.
2. Вкладка `General` -> `Name` введите любое имя (например, Postgres Docker).
3. Вкладка `Connection` 
	- `Host name/address`: postgres
    - `Port`: 5432
    - `Username`: postgres
    - `Password`: pwd
4. Сохраните и подключитесь.

---

## Настройки приложения

В файле `DentalClinicApp\appsettings.json` задается строка подключения к БД, а также путь к сохранению картинок. 
При необходимости можете изменить эти параметры.