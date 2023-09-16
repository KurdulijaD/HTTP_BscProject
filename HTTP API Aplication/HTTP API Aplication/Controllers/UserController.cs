using HTTP_API_Aplication.DTO;
using HTTP_API_Aplication.Interfaces;
using HTTP_API_Aplication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace HTTP_API_Aplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository _repository;

        public UserController(IRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<List<User>> Get()
        {
            List<User> users = _repository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("get-by-username")]
        public ActionResult<List<User>> GetByUsername(string username)
        {
            List<User> users = _repository.GetAllUsers();
            User user = users.FirstOrDefault(u => u.Username == username);
            if (user != null)
                return Ok(user);
            return NotFound();
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserDto userDto)
        {
            if (String.IsNullOrEmpty(userDto.Name) || 
                String.IsNullOrEmpty(userDto.Lastname) || 
                String.IsNullOrEmpty(userDto.Username))
                return BadRequest();

            List<User> users = _repository.GetAllUsers();
            if (users.Any(u => u.Username == userDto.Username))
                return Conflict();
            
            User user = new User() { Name = userDto.Name, Lastname = userDto.Lastname, Username = userDto.Username };
            int lastUserId = users[users.Count - 1].Id;
            user.Id = ++lastUserId;
            _repository.AddUser(user);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            List<User> users = _repository.GetAllUsers();
            User userToDelete = users.FirstOrDefault(u => u.Id == id);
            if (userToDelete == null)
                return NotFound();

            _repository.DeleteUser(userToDelete);
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserDto updatedUserDto)
        {
            List<User> users = _repository.GetAllUsers();
            if (String.IsNullOrEmpty(updatedUserDto.Name) || String.IsNullOrEmpty(updatedUserDto.Lastname) || String.IsNullOrEmpty(updatedUserDto.Username))
                return BadRequest();
            if (users.Any(u => u.Username == updatedUserDto.Username))
                return Conflict();

            User userToUpdate = users.FirstOrDefault(u => u.Id == id);
            if (userToUpdate == null)
                return NotFound();

            userToUpdate.Name = updatedUserDto.Name;
            userToUpdate.Lastname = updatedUserDto.Lastname;
            userToUpdate.Username = updatedUserDto.Username;

            _repository.UpdateUser(userToUpdate);
            return Ok(userToUpdate);
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(int id, [FromBody] UserDto updatedUserDto)
        {
            List<User> users = _repository.GetAllUsers();
            if (users.Any(u => u.Username == updatedUserDto.Username))
                return Conflict();

            User userToUpdate = users.FirstOrDefault(u => u.Id == id);
            if (userToUpdate == null)
                return NotFound();

            if (!string.IsNullOrEmpty(updatedUserDto.Name))
                userToUpdate.Name = updatedUserDto.Name;

            if (!string.IsNullOrEmpty(updatedUserDto.Lastname))
                userToUpdate.Lastname = updatedUserDto.Lastname;

            if (!string.IsNullOrEmpty(updatedUserDto.Username))
                userToUpdate.Username = updatedUserDto.Username;

            _repository.UpdateUser(userToUpdate);
            return Ok(userToUpdate);
        }

        [HttpHead]
        public IActionResult Head()
        {
            var responseMessage = "This is the HEAD method response. It does not return a body, only headers.";
            Response.Headers.Add("Custom-Header", "Value");
            return Ok(responseMessage);
        }

        [HttpOptions]
        public IActionResult Options()
        {
            var responseMessage = "This is the OPTIONS method response. It provides information about supported HTTP methods.";
            var supportedMethods = new[] { "GET", "POST", "HEAD", "OPTIONS" };
            Response.Headers.Add("Allow", string.Join(",", supportedMethods));
            return Ok(responseMessage);
        }


        [HttpGet("connect")]
        public IActionResult Connect()
        {
            var responseMessage = "This is the CONNECT method response. It is typically used for establishing a network tunnel.";
            return Ok(responseMessage);
        }

        [HttpGet("trace")]
        public IActionResult Trace()
        {
            var requestInfo = $"Request method: {Request.Method}, Path: {Request.Path}";
            var responseMessage = $"This is the TRACE method response. It reflects back information about the request.{Environment.NewLine}{requestInfo}";
            return Ok(responseMessage);
        }
    }
}
