using MCRI_Student_Employee_Data.Data;
using MCRI_Student_Employee_Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MCRI_Student_Employee_Data.Repositories;

public class PersonRepository : IPersonRepository
{
    private readonly AppDbContext db;

    public PersonRepository(AppDbContext db)
    {
        this.db = db;
    }

    public async Task<List<Person>> GetAll(string? search, PersonType? personType)
    {
        var query = db.People.AsQueryable();

        if (personType != null)
        {
            // The column stores the name as text ("Student"/"Employee").
            var typeName = personType.Value.ToString();
            query = query.Where(p => p.PersonType == typeName);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            // SQL Server's default collation is case-insensitive, so LIKE is enough.
            var term = $"%{search.Trim()}%";
            query = query.Where(p =>
                EF.Functions.Like(p.FirstName, term) ||
                EF.Functions.Like(p.LastName, term));
        }

        return await query
            .OrderBy(p => p.FirstName)
            .ThenBy(p => p.LastName)
            .ToListAsync();
    }

    public async Task<Person?> GetById(int id)
    {
        return await db.People.FindAsync(id);
    }

    public async Task Add(Person person)
    {
        db.People.Add(person);
        await db.SaveChangesAsync();
    }

    public async Task Update(Person person)
    {
        db.People.Update(person);
        await db.SaveChangesAsync();
    }

    public async Task Delete(Person person)
    {
        db.People.Remove(person);
        await db.SaveChangesAsync();
    }
}
