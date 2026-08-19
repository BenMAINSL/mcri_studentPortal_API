using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MCRI_Student_Employee_Data.Models;

public partial class Person
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PersonType { get; set; } = null!;

    public string DepartmentOrProgramme { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? FunFact { get; set; }

    public string? ImageUrl { get; set; }

    public string? Gender { get; set; }

    public string? Cohort { get; set; }

    public string? Phase { get; set; }

    // The profile image itself, held in the database for now. JsonIgnore keeps
    // the bytes out of every people response - they are served by
    // GET api/people/{id}/image instead, which is what ImageUrl points at.
    [JsonIgnore]
    public byte[]? ImageData { get; set; }

    [JsonIgnore]
    public string? ImageContentType { get; set; }
}
