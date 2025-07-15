using Advice_Me_APIs.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Advice_Me_APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _users;
        public UsersController(IUsers users)
        {
            _users = users;
        }
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
           return Ok(await _users.GetAllUsersAsync());
        }
    }
}
