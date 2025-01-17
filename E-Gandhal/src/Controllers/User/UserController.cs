using E_Gandhal.src.Domain.DTO.AuthentificationDTO;
using E_Gandhal.src.Domain.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Gandhal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserController> _logger;
        private readonly IConfiguration _configuration;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _logger = logger;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> UserRegister([FromBody] RegisterDTO registerDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.RegisterUserAsync(registerDto, cancellationToken);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
            return Ok(new { Message = "Registration successful" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> UserLogin([FromBody] LoginDTO loginDTO, CancellationToken cancellationToken)
        {
            var user = await _userRepository.LoginUserAsync(loginDTO, cancellationToken);
            if (!user.Succeeded)
            {
                return Unauthorized("Your access denied");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, loginDTO.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token) });
        }

        [HttpGet("Id/{id}")]
        public async Task<IActionResult> DisconnectedAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _userRepository.Disconnected(id, cancellationToken);
            if (result == null)
            {
                _logger.LogWarning("Failed to disconnect user with ID: {Id}", id);
                return NotFound("User not found");
            }

            _logger.LogInformation("User disconnected successfully: {Id}", id);
            return Ok(result);
        }

        [HttpGet("Email")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest("Email is required");
            }

            var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
            if (user == null)
            {
                _logger.LogWarning("User not found for email: {Email}", email);
                return NotFound("User not found");
            }

            return Ok(user);
        }

        [HttpPatch("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordDTO updatePasswordDto, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userRepository.UpdateUserPassword(updatePasswordDto.Email, updatePasswordDto.OldPassword, updatePasswordDto.NewPassword, cancellationToken);
            if (!result.Succeeded)
            {
                _logger.LogWarning("Failed to update password for user: {Email}", updatePasswordDto.Email);
                return BadRequest(result.Errors);
            }

            _logger.LogInformation("Password updated successfully for user: {Email}", updatePasswordDto.Email);
            return Ok(new { Message = "Password updated successfully" });
        }
    }

}
