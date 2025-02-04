using ECommerce.Core.DTOs;
using ECommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO?>> GetById(Guid id)
        {
            //await Task.Delay(10000);
            //throw new NotImplementedException();

            if (id == Guid.Empty)
                return BadRequest("Invalid user Id");

            var user = await _usersService.GetById(id);
            if (user is null)
                return NotFound(user);

            return Ok(user);
        }
    }
}
