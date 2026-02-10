# TimeScale API

WebAPI для работы с CSV данными и вычисления результатов обработки.

---

## Технологии

- .NET 10
- EF Core 8
- PostgreSQL
- Swagger
- FluentValidation
- xUnit + FluentAssertions
- Clean Architecture (API / Application / DataAccess)

---

## Setup

1. Database

```bash
dotnet ef database update -s .\\TimeScale.Api\ -p .\\TimeScale.DataAccess\\
```

---

## API Методы

### 1. Загрузка CSV

**POST** `/api/files/upload`

- CSV: `Date;ExecutionTime;Value`
- Валидация:
  - `Date` между 01.01.2000 и текущей датой
  - `ExecutionTime` ≥ 0
  - `Value` ≥ 0
  - Строк: 1–10 000
- Результаты сохраняются в `Results`
- Перезапись существующего файла

### 2. Получение результатов

**GET** `/api/results`

- Фильтры: `fileName`, `startDateFrom`, `startDateTo`, `avgValueFrom`, `avgValueTo`, `avgExecutionTimeFrom`, `avgExecutionTimeTo`
- Возвращает список `ResultDto`

### 3. Последние 10 значений

**GET** `/api/values/last/{fileName}`

- Возвращает последние 10 `ValueRecord` по `Date` по убыванию

---
