using MCRI_Student_Employee_Data.Models;
using MCRI_Student_Employee_Data.Repositories;

namespace MCRI_Student_Employee_Data.Services;

public class PersonService : IPersonService
{
    private readonly IPersonRepository personRepository;
    private readonly IImageStorageService imageStorage;

    public PersonService(IPersonRepository personRepository, IImageStorageService imageStorage)
    {
        this.personRepository = personRepository;
        this.imageStorage = imageStorage;
    }

    public async Task<List<Person>> GetAll(string? search, PersonType? personType)
    {
        return await personRepository.GetAll(search, personType);
    }

    public async Task<Person?> GetById(int id)
    {
        return await personRepository.GetById(id);
    }

    public async Task<Person> Add(Person person)
    {
        person.Id = 0; // the database assigns the id
        await personRepository.Add(person);
        return person;
    }

    public async Task<Person?> Update(int id, Person person)
    {
        var existing = await personRepository.GetById(id);
        if (existing == null)
        {
            return null;
        }

        existing.FirstName = person.FirstName;
        existing.LastName = person.LastName;
        existing.PersonType = person.PersonType;
        existing.DepartmentOrProgramme = person.DepartmentOrProgramme;
        existing.Email = person.Email;
        existing.FunFact = person.FunFact;
        existing.ImageUrl = person.ImageUrl;
        existing.Gender = person.Gender;
        existing.Cohort = person.Cohort;
        existing.Phase = person.Phase;

        await personRepository.Update(existing);
        return existing;
    }

    public async Task<bool> Delete(int id)
    {
        var person = await personRepository.GetById(id);
        if (person == null)
        {
            return false;
        }

        await personRepository.Delete(person);
        return true;
    }

    public async Task<Person?> UploadImage(int id, IFormFile file)
    {
        var person = await personRepository.GetById(id);
        if (person == null)
        {
            return null;
        }

        // The storage service decides where the bytes live and hands back the
        // URL the client should use.
        person.ImageUrl = await imageStorage.Upload(id, file);
        await personRepository.Update(person);
        return person;
    }

    public async Task<(byte[] Data, string ContentType)?> GetImage(int id)
    {
        var person = await personRepository.GetById(id);
        if (person?.ImageData == null || person.ImageData.Length == 0)
        {
            return null;
        }

        return (person.ImageData, person.ImageContentType ?? "image/jpeg");
    }
}
