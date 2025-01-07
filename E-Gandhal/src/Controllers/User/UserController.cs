using E_Gandhal.src.Domain.DTO.AuthentificationDTO;
using E_Gandhal.src.Domain.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Gandhal.src.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> UserRegister([FromBody] RegisterDTO registerDto, CancellationToken cancellationToken)
        {
            var user = await _userRepository.RegisterUserAsync(registerDto, cancellationToken);
            if (!user.Succeeded)
            {
                return BadRequest(user.Errors);
            }
            return Ok(user);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            var user = await _userRepository.LoginUserAsync(loginDTO, cancellationToken);
            if (!user.Succeeded)
            {
                return Unauthorized("Your access denied");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginDTO.Email)
            };
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));

            Console.WriteLine("Cookie issued successfully for user: " + loginDTO.Email); // Ajout du journal

            return Ok(new { Message = "Login successful" });
        }

        [HttpGet("Id")]
        public async Task<IActionResult> DisconnectedAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userRepository.Disconnected(id, cancellationToken);

            return Ok(user);
        }

        [HttpGet("Email")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                return BadRequest();
            }
            return Ok(user);
        }

        [HttpPatch]
        public async Task<IActionResult> UpdatePassword([FromQuery] RegisterDTO register, string newMessage, CancellationToken cancellationToken)
        {
            var user = _userRepository.UpdateUserPassword(register.Email, register.Password, newMessage, cancellationToken);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}
