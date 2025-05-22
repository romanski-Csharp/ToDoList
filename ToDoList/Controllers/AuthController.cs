using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        // Просто для тесту — список користувачів в пам'яті
        private static List<UserModel> users = new();

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || user.Username.Length < 3)
                return BadRequest("Ім'я користувача має бути щонайменше 3 символи");

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 5)
                return BadRequest("Пароль має бути щонайменше 5 символів");

            if (users.Any(u => u.Username == user.Username))
                return BadRequest("Такий користувач вже існує");

            users.Add(user);
            return Ok("Користувача зареєстровано");
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            var valid = users.FirstOrDefault(u =>
                u.Username == user.Username && u.Password == user.Password);

            if (valid == null)
                return Unauthorized("Invalid credentials");

            var token = GenerateToken(valid.Username);
            return Ok(new { token });
        }

        private string GenerateToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
