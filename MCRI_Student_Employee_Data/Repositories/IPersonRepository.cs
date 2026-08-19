using MCRI_Student_Employee_Data.Models;

namespace MCRI_Student_Employee_Data.Repositories;

public interface IPersonRepository
{
    Task<List<Person>> GetAll(string? search, PersonType? personType);

    Task<Person?> GetById(int id);

    Task Add(Person person);

    Task Update(Person person);

    Task Delete(Person person);
}
