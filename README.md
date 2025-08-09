# DentalClinicApp

WPF-���������� ��� ����������������� �������, ���������������� ��������, ��������� � �������� ���������� ��������� � ��������� � �� ���������.

## ����������

- ���������� ���������� (����������, �����)
- �������� ���������� (drag&drop � ����� ������)
- �������������� ��������� ������� ���������� �� 1920x1080 � ����������� ���������
- �������� ����������� � ��������� ����� `/images/{patientId}/`
- ����������� ������ ���������� � ����� ��������
- ����������� �������� � ������� Serilog
- ������ � ����� ������ PostgreSQL ����� Entity Framework Core

---

## ����������

- .NET 8
- WPF (.NET)
- Entity Framework Core (PostgreSQL)
- Serilog (�����������)
- ImageSharp (��������� �����������)

---

## ������ ����������

1. ����������� �����������

```git
git clone https://github.com/Semyon25/DentalClinicApp.git
```

��� ���������� ��������� �� [�����������](https://github.com/Semyon25/DentalClinicApp)

2. ���������� ����������� NuGet ������:

```bash
dotnet restore
```

3. ��������� ������ �������:

```bash
dotnet build --configuration Release --no-restore
```

---

## ��������� PostgreSQL

���� ������ �������� `docker-compose.yml` ��� �������� ������� PostgreSQL � ���������� ��� ��������� ����������.

### ����������

- ������������� [Docker](https://www.docker.com/products/docker-desktop) (Docker Desktop ��� Windows/macOS ��� Docker Engine ��� Linux)
- Docker Compose (������ � ������ Docker Desktop)

### ��� ��������� ���� ������

1. ��������� ��������� � PostgreSQL:

```bash
docker compose up -d
```

2. ���������, ��� ��������� �������:

```bash
docker ps
```

### ��� ���������� � ������� ���������

���������� ���������:

```bash
docker compose down
```

���������� � ������� ���� � �������:

```bash
docker compose down -v
```

### ���������� ��

��� ���������� ����� ����� ������������ pgAdmin.
�� �������� �� ������: `http://localhost:5050`

���� � pgAdmin:

- **Email**: admin@admin.com
- **������**: admin

����������� � PostgreSQL � pgAdmin:

1. � ���������� pgAdmin ������� `Add New Server`.
2. ������� `General` -> `Name` ������� ����� ��� (��������, Postgres Docker).
3. ������� `Connection` 
	- `Host name/address`: postgres
    - `Port`: 5432
    - `Username`: postgres
    - `Password`: pwd
4. ��������� � ������������.

---

## ��������� ����������

� ����� `DentalClinicApp\appsettings.json` �������� ������ ����������� � ��, � ����� ���� � ���������� ��������. 
��� ������������� ������ �������� ��� ���������.