using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PasswordManager.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PasswordsController : ControllerBase
{
    // GET: api/<PasswordsController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<PasswordsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<PasswordsController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<PasswordsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<PasswordsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
