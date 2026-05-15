
using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;
using PersonApi.Models.DTOs;
using PersonApi.Models.Requests;
using PersonApi.Services;

namespace PersonApi.Controllers;
[ApiController]
[Route("[controller]")] // /persons
public class Personscontroller(IPersonService service) : ControllerBase
{

    private readonly IPersonService _service = service;
    

    // Maps a GET request to the "/persons" URL to return the list of persons as a JSON response. The Results.Ok() method is used to create a successful HTTP response with the list of persons as the content.
    [HttpGet]    
    public ActionResult<List<PersonDTO>> GetPersons()
        {
            return Ok(_service.GetPersons());
        }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Person?> GetPersonById(string id)
    {
        try{
        Person? found =_service.GetPersonById(id);   
    
        if (found == null)
        {
            return NotFound($"Person with ID {id} not found.");
        }

            return Ok(found);
        }
        catch (Exception){
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<Person?>> CreatePerson([FromBody]CreatePersonRequest request)
    {
        try{
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
                Person newPerson = await _service.CreatePerson(request);

                return Created("/persons/", newPerson);
        
        }catch
            {
                throw;
            }
            
        }

    [HttpPatch("{id}")]
    public async Task<ActionResult<Person?>> UpdatePerson(string id, [FromBody] CreatePersonRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            // 1. Call async
            var updatedPerson = await _service.UpdatePerson(id, request);

            // 2. If not exist 
            if (updatedPerson == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            // 3. Return updated person
            return Ok(updatedPerson);
        }
        catch
        {
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePerson(string id)
    {
        try
        {
            // 1. Verificăm dacă persoana există
            var found = _service.GetPersonById(id);
            if (found == null)
            {
                return NotFound($"Person with ID {id} not found.");
            }

            // 2. Ștergem persoana
            var deleted = await _service.DeletePerson(id);

            if (!deleted)
            {
                return StatusCode(500, "Could not delete the person.");
            }

            return Ok($"Person with ID {id} deleted successfully.");
        }
        catch
        {
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }
}
