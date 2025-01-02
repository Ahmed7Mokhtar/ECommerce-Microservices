using ECommerce.Core.DTOs;
using ECommerce.Core.ServiceContracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<AuthResponseDTO?>> Register(RegisterDTO registerDTO)
        {
            if (registerDTO is null)
                return BadRequest("Invalid registeration data!");

            var response = await _usersService.Register(registerDTO);
            if (response is null || !response.Success)
                return BadRequest(response);

            return Ok(response);
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponseDTO?>> Login(LoginDTO loginDTO)
        {
            if (loginDTO is null)
                return BadRequest("Invalid login data!");

            var response = await _usersService.Login(loginDTO);
            if(response is null || !response.Success)
                return Unauthorized(response);

            return Ok(response);
        }
    }
}
