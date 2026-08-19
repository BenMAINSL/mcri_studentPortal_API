# MCRI People Portal — Backend

ASP.NET Core Web API over one `Person` entity that is either a student or an employee.
No authentication, no authorization — per the spec.

Local development runs on SQL Server LocalDB; the deployed app runs on Supabase
PostgreSQL. Same code, different configuration.

## Layout

```
MCRI_Student_Employee_Data/
  Models/          Person, PersonType
  Data/            AppDbContext
  Repositories/    IPersonRepository, PersonRepository   <- database access
  Services/        IPersonService, PersonService          <- business logic
                   IImageStorageService, DatabaseImageStorageService, SupabaseStorageService
  Controllers/     PeopleController                       <- HTTP
Database/          MCRI_People_SqlServer.sql   (local)
                   MCRI_People_Postgres.sql    (Supabase)
Dockerfile         used by Render
render.yaml        Render blueprint
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
| GET | `api/people/{id}/image` | the stored bytes |
| GET | `/healthz` | Render's health check |

`PersonType` goes over the wire as `"Student"` or `"Employee"`.

Every action returns `Ok(...)` or `NotFound()`. There is no error handling beyond that —
this is a teaching app.

## Configuration

`Database:Provider` chooses the provider — `SqlServer` or `Postgres`. It is the only
setting that differs between running locally and running on Render.

| | Provider | Connection string |
| --- | --- | --- |
| `appsettings.Development.json` | `SqlServer` | LocalDB |
| `appsettings.json` | `Postgres` | Supabase (placeholders; real values come from Render env vars) |

Nothing secret belongs in either file. On Render, every value is set as an environment
variable, using `__` where the JSON key nests: `ConnectionStrings__DefaultConnection`.

## Running locally

```bash
cd MCRI_Student_Employee_Data
dotnet run
```

Swagger UI: <http://localhost:5121/swagger>.

There are no EF migrations in this project — the table is created by the scripts in
`Database/`. Run `MCRI_People_SqlServer.sql` then `MCRI_People_AddImageColumns.sql` in
SSMS before the first run.

## Deploying to Render

### 1. Create the table in Supabase

Run `Database/MCRI_People_Postgres.sql` in Supabase → SQL Editor. It is safe to re-run:
it adds only what is missing and seeds only an empty table.

The names must be quoted `"PascalCase"`, exactly as in that script. PostgreSQL folds
unquoted identifiers to lower case, while EF quotes the names from `AppDbContext`, so a
table created as `people (first_name …)` will not be found. Query 1 in the script prints
the current shape so you can compare.

### 2. Build the connection string

Use the **connection pooler**, not the direct `db.<ref>.supabase.co` host. Render's
outbound traffic is IPv4 and the direct host resolves to IPv6 only, which fails as
`Host not found` or a connection timeout.

Supabase → Project Settings → Database → Connection string → **Session pooler**, which
gives you the host, and a username shaped `postgres.<project-ref>`:

```
Host=aws-0-<region>.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.<project-ref>;Password=<db-password>;SSL Mode=Require;Trust Server Certificate=true
```

Port 5432 on the pooler is session mode and behaves like a normal PostgreSQL connection.
If you use transaction mode (port 6543) instead, add `No Reset On Close=true` and
`Max Auto Prepare=0`, because prepared statements do not survive that mode.

### 3. Create the service

Either commit `render.yaml` and use **New → Blueprint**, or **New → Web Service** with
runtime **Docker** and health check path `/healthz`.

Environment variables:

| Key | Value |
| --- | --- |
| `Database__Provider` | `Postgres` |
| `ConnectionStrings__DefaultConnection` | the string from step 2 |
| `Supabase__Url` | `https://<project>.supabase.co` |
| `Supabase__Key` | the `service_role` key |
| `Supabase__Bucket` | `profile-images` |

The last three matter only if you switch `Program.cs` from
`DatabaseImageStorageService` to `SupabaseStorageService`. Images currently live in the
`ImageData` column, so the portal needs no bucket.

Render injects `PORT`; the Dockerfile's entrypoint binds Kestrel to it.

### 4. Check it

- `https://<service>.onrender.com/healthz` → `{"status":"ok"}` (no database involved)
- `https://<service>.onrender.com/swagger` → the UI
- `GET api/people` → the seeded rows. A 500 here with `42P01` or `42703` in the Render
  logs means the table or a column name does not match step 1.

The free plan sleeps after inactivity, so the first request after a quiet spell takes
about 50 seconds. Worth knowing before demoing it to a room.

## Seed data

Ten placeholder people (4 employees, 6 students) live in both SQL scripts. Replace them
with the real MCRI list in `Database/MCRI_People_Postgres.sql` — or edit the rows
directly in the Supabase table editor.
