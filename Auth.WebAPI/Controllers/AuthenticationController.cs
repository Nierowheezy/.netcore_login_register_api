using Auth.WebAPI.Core.CustomExceptions;
using Auth.WebAPI.Core.DTOS;
using Auth.WebAPI.Core.Service;
using Auth.WebAPI.DB;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Auth.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
                _userService = userService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpUserDTO signUpUser)
        {
            try
            {
                var result = await _userService.SignUp(signUpUser);
                return Created("", result);
            }
            catch (UsernameAlreadyExistsException e)
            {
                return StatusCode(409, e.Message);
                
            }
           
        }

        [HttpPost("signin")]
        public  async Task<IActionResult> SignIn(SignInUserDTO signInUser)
        {
            try
            {
                var result = await _userService.SignIn(signInUser);
                return Ok(result);
            }
            catch (InvalidUsernamePasswordException e)
            {

                return StatusCode(401, e.Message);
            }
        }
    }
}
