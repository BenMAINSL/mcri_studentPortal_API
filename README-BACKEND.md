# MCRI People Portal — Backend

ASP.NET Core Web API over a Supabase PostgreSQL database, managing one `Person` entity that is
either a student or an employee. No authentication, no authorization — per the spec.

## Layout

```
MCRI_Student_Employee_Data/
  Models/          Person, PersonType
  Data/            AppDbContext, SeedData, Migrations
  Repositories/    IPersonRepository, PersonRepository   <- database access
  Services/        IPersonService, PersonService          <- business logic
                   IImageStorageService, SupabaseStorageService
  Controllers/     PeopleController                       <- HTTP
```

Dependencies run one way: `Controller -> Service -> Repository -> DbContext`.

## Endpoints

| Method | Route | Notes |
| --- | --- | --- |
| GET | `api/people` | Optional `?search=` (first or last name) and `?personType=Student\|Employee` |
| GET | `api/people/{id}` | |
| POST | `api/people` | JSON body |
| PUT | `api/people/{id}` | JSON body |
| DELETE | `api/people/{id}` | |
| POST | `api/people/{id}/image` | `multipart/form-data`, part name `file` |

`PersonType` goes over the wire as `"Student"` or `"Employee"`.

Every action returns `Ok(...)`. There is no error handling beyond that — this is a teaching app.

## Configuration

Fill in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=db.YOUR-PROJECT.supabase.co;Port=5432;Database=postgres;Username=postgres;Password=YOUR-DB-PASSWORD;SslMode=Require"
},
"Supabase": {
  "Url": "https://YOUR-PROJECT.supabase.co",
  "Key": "YOUR-SUPABASE-SERVICE-ROLE-KEY",
  "Bucket": "profile-images"
}
```

- Connection string: Supabase → Project Settings → Database → Connection string
- Key: Supabase → Project Settings → API → `service_role` key
- Bucket: Supabase → Storage → New bucket named `profile-images`, marked **Public**

## Running

```bash
cd MCRI_Student_Employee_Data
dotnet run
```

Swagger UI: <http://localhost:5121/swagger>. Migrations run automatically at startup, so the
`people` table and its ten seeded rows are created on first run.

`MCRI_Student_Employee_Data.http` has a ready request for each endpoint.

## Seed data

`Data/SeedData.cs` has ten placeholder people (4 employees, 6 students). Replace them with the
real MCRI list, then:

```bash
dotnet ef migrations add RealSeedData
```

## Deploying to Render

The repo root has a `Dockerfile`. On Render: New → Web Service → runtime **Docker**. Set the
connection string and Supabase values as environment variables using `__` for nesting:
`ConnectionStrings__DefaultConnection`, `Supabase__Url`, `Supabase__Key`, `Supabase__Bucket`.
