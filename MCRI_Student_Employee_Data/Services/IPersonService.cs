using MCRI_Student_Employee_Data.Models;

namespace MCRI_Student_Employee_Data.Services;

public interface IPersonService
{
    Task<List<Person>> GetAll(string? search, PersonType? personType);

    Task<Person?> GetById(int id);

    Task<Person> Add(Person person);

    Task<Person?> Update(int id, Person person);

    Task<bool> Delete(int id);

    Task<Person?> UploadImage(int id, IFormFile file);

    Task<(byte[] Data, string ContentType)?> GetImage(int id);
}
