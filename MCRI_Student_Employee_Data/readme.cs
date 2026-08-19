//using Microsoft.AspNetCore.Mvc.ViewEngines;
//using static System.Net.Mime.MediaTypeNames;

//MCRI People Portal – Project Specification

//Build a simple full-stack MCRI People Portal that will be used during an SDLC workshop. The goal is to keep the application simple while demonstrating how a modern web application is built using a React frontend, a C# Web API, and a PostgreSQL database.

//Objective

//Create a single-page web application backed by an ASP.NET Core Web API and a Supabase PostgreSQL database. The API will be hosted on Render, while the frontend can be hosted separately or run locally.

//The database should be pre-populated with both MCRI students and employees so that participants immediately see themselves when they open the application.

//The application should focus only on basic CRUD (Create, Read, Update, Delete) functionality. Do not implement authentication, authorization, user roles, messaging, notifications, dashboards, or other advanced enterprise features.

//Technology Stack
//React (TypeScript)
//ASP.NET Core Web API (.NET 8)
//Entity Framework Core
//PostgreSQL (Supabase)
//Supabase Storage for profile images
//Render for API hosting
//GitHub for source control
//Person Model

//The application manages a single Person entity that represents either a student or an employee.

//Each person should contain the following fields:

//Id
//FirstName
//LastName
//PersonType (Student or Employee)
//DepartmentOrProgramme
//Email
//FunFact
//ImageUrl

//The PersonType field determines whether the record represents a student or an employee.

//Features

//The application should allow users to:

//View all students and employees
//Search people by name
//Filter by Student or Employee
//View an individual's details
//Add a new person
//Edit an existing person
//Delete a person
//Upload a profile picture

//Images should be uploaded to Supabase Storage, with only the public image URL stored in the database.

//REST API

//Implement the following endpoints:

//GET / api / people
//GET / api / people /{ id}
//POST / api / people
//PUT / api / people /{ id}
//DELETE / api / people /{ id}
//POST / api / people /{ id}/ image

//Keep the backend architecture clean by separating Controllers, Services, and Repositories.


// ---------------------------------------------------------------------------
// Endpoint style to follow (pasted from the existing NavPlus AccountController).
// Commented out because this file is compiled as C# - it is a spec, not code.
// Note: the [Authorize] / ResetKey / credential parts are deliberately NOT
// carried over - the spec says no authentication or authorization.
// ---------------------------------------------------------------------------

//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Claims;

//[Authorize]
//[Route("CheckPermissions")]
//[HttpPost]
//public IActionResult CheckPermissions([FromBody] ComponentPermissionCheck[] permissions)
//{
//    var result = userBusiness.CheckPermissions(permissions);
//    return Ok(result);
//}

//// GET api/Account/Zendesk
//[Route("Zendesk")]
//[HttpGet]
//public IActionResult ZendeskData()
//{
//    User user = userBusiness.GetUserInfo(User.Identity as ClaimsIdentity);
//    ZendeskData data = userBusiness.ZendeskCreds(user);
//    return Ok(data);
//}

//// GET api/Account/CommonWords
//[Route("CommonWords")]
//[HttpGet]
//public IActionResult CommonWords()
//{
//    var commonWords = userBusiness.GetCommonWords();
//    var result = new Result<List<string>>(success: true, data: commonWords, message: "Common Words");
//    return Ok(result);

//}

//// POST api/Account/ResetKey
//[Route("ResetKey")]
//[HttpPost]
//public async Task<IActionResult> ResetKey([FromForm] ResetKey resetKey)

//{
//    var ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
//    var userAgent = Request.Headers["User-Agent"].FirstOrDefault();
//    await userBusiness.GetResetKey(resetKey, ipAddress, userAgent);
//    var result = new Result<string>(success: true, data: "", message: "Set ResetKey");
//    return Ok(result);
//}
