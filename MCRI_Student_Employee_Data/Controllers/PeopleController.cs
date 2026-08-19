using MCRI_Student_Employee_Data.Models;
using MCRI_Student_Employee_Data.Services;
using Microsoft.AspNetCore.Mvc;

namespace MCRI_Student_Employee_Data.Controllers;

[ApiController]
[Route("api/people")]
public class PeopleController : ControllerBase
{
    private readonly IPersonService personService;

    public PeopleController(IPersonService personService)
    {
        this.personService = personService;
    }

    // GET api/people
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetPeople(string? search, PersonType? personType)
    {
        var people = await personService.GetAll(search, personType);
        return Ok(people);
    }

    // GET api/people/{id}
    [Route("{id}")]
    [HttpGet]
    public async Task<IActionResult> GetPerson(int id)
    {
        var person = await personService.GetById(id);
        if (person == null)
        {
            return NotFound();
        }

        return Ok(person);
    }

    // POST api/people
    [Route("")]
    [HttpPost]
    public async Task<IActionResult> AddPerson([FromBody] Person person)
    {
        var created = await personService.Add(person);
        return Ok(created);
    }

    // PUT api/people/{id}
    [Route("{id}")]
    [HttpPut]
    public async Task<IActionResult> UpdatePerson(int id, [FromBody] Person person)
    {
        var updated = await personService.Update(id, person);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    // DELETE api/people/{id}
    [Route("{id}")]
    [HttpDelete]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var deleted = await personService.Delete(id);
        if (!deleted)
        {
            return NotFound();
        }

        return Ok();
    }

    // POST api/people/{id}/image
    [Route("{id}/image")]
    [HttpPost]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        try
        {
            var person = await personService.UploadImage(id, file);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }
        catch (ArgumentException ex)
        {
            // Missing / oversized / unsupported file is the caller's problem.
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET api/people/{id}/image
    [Route("{id}/image")]
    [HttpGet]
    public async Task<IActionResult> GetImage(int id)
    {
        var image = await personService.GetImage(id);
        if (image == null)
        {
            return NotFound();
        }

        return File(image.Value.Data, image.Value.ContentType);
    }
}
