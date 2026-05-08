using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Models;
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
    public ActionResult<List<Person>> GetPersons()
        {
            return Ok(_service.GetPersons());
        }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Person?> GetPersonById(string id)
    {
        try{
        Person? found =_service.GetPersonsById(id);   
    
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
    public ActionResult<Person?> CreatePerson([FromBody]CreatePersonRequest request)
    {
        try{
            Person newPerson = _service.CreatePerson(request);
            return Created("/persons/", newPerson);
        }catch (Exception ){
            return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    [HttpPatch]
    [Route("{id}")]
    public ActionResult<Person?> UpdatePerson(string id, [FromBody]CreatePersonRequest request)
    {
        try{
    
        Person? updatedPerson = _service.UpdatePerson(id, request);
        if (updatedPerson == null)
        {
            return NotFound($"Person with ID {id} not found.");
        }
            return Ok(updatedPerson);
        }
        catch{
            return StatusCode(500, "An error occurred while processing the request.");
            }
        }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult DeletePerson(string id)
    {
        try{
            Person? found = _service.GetPersonsById(id);
                if (found == null)
                {
                    return NotFound($"Person with ID {id} not found.");
                }
                _service.DeletePerson(id);
                return Ok();
            }
            catch{
                return StatusCode(500, "An error occurred while processing the request.");
            }
                    
                }
}