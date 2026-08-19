namespace MCRI_Student_Employee_Data.Models;

/// <summary>
/// Used for the ?personType= filter on GET api/people.
/// Person.PersonType itself is a string, because that is how the People table stores it
/// ("Student" / "Employee") and how the scaffolded model reads it. Taking the enum on the
/// query string means invalid values are rejected before they reach the database, and
/// Swagger renders a dropdown instead of a free-text box.
/// </summary>
public enum PersonType
{
    Student,
    Employee
}
