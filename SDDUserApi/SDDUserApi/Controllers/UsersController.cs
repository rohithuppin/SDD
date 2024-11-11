using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SDDUserApi.Data.DTO;
using SDDUserApi.Data.Model;
using SDDUserApi.Services;
using SDDUserApi.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SDDUserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuditService _auditService;
        private readonly IConfiguration _configuration;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, IAuditService auditService, IConfiguration configuration, IPasswordHasher passwordHasher, ILogger<UsersController> logger)
        {
            _userService = userService;
            _auditService = auditService;
            _configuration = configuration;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        // GET: api/user
        [HttpGet]
        //[Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("Fetching user data.");
            _auditService.LogActivity(1,"READ", $"Get all user");
            var userDtos = await _userService.GetAllUsersAsync();
            return Ok(userDtos);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            _logger.LogInformation("Fetching user id - {0} data.",id);
            _auditService.LogActivity(1, "READById", $"Get user by Id");
            var userDto = await _userService.GetUserByIdAsync(id);
            if (userDto == null)
                return NotFound();

            return Ok(userDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            _logger.LogInformation("Fetching user name - {0} data.", model.Username);

            _auditService.LogActivity(1, "LOGIN", $"Authorization of user :" + model.Username);
            var user = await _userService.GetUserByUsernameAsync(model.Username);
            if (user == null)
            {
                _logger.LogWarning("No user exists- {0} data.", model.Username);
                return Unauthorized(new { message = "Invalid username or password." });
            }

            var tokenJWT = GenerateJwtToken(user);

            var match = _passwordHasher.VerifyPasswordHash(model.Password, user.PasswordHash);

            if (match)
            {
                _logger.LogWarning("Password incorrect for user - {0}", model.Username);
                return Unauthorized(new { message = "Invalid username or password." });
            }
            else
                return Ok(new { Token = tokenJWT, User = user });
        }        

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.EmailId)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // POST: api/user
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] UserDto userDto)
        {
            if (userDto == null)
                return BadRequest();
            _auditService.LogActivity(1, "CREATE", $"Create new user");

            var hashPwd = _passwordHasher.HashPassword("password");//Default password for everyone during first time creation
            await _userService.AddUserAsync(userDto,hashPwd);
            return CreatedAtAction(nameof(GetUserById), new { id = userDto.Username }, userDto);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            _auditService.LogActivity(1, "UPDATE", $"Update user");

            var existingUser = await _userService.GetUserByIdAsync(id);
            if (existingUser == null)
                return NotFound();
            userDto.UserId = id;

            await _userService.UpdateUserAsync(id, userDto);
            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            _auditService.LogActivity(1, "DELETE", $"Deleting user id - " + id);
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
