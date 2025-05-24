
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
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
        private readonly IMongoCollection<UserModel> _users;

        public AuthController(IConfiguration config)
        {
            _config = config;
            var client = new MongoClient(config.GetConnectionString("MongoDb"));
            var db = client.GetDatabase("ToDoDb");
            _users = db.GetCollection<UserModel>("Users");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserModel user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || user.Username.Length < 3)
                return BadRequest("Ім'я користувача має бути щонайменше 3 символи");

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 5)
                return BadRequest("Пароль має бути щонайменше 5 символів");

            var existing = await _users.Find(u => u.Username == user.Username).FirstOrDefaultAsync();
            if (existing != null)
                return BadRequest("Такий користувач вже існує");

            await _users.InsertOneAsync(user);
            return Ok("Користувача зареєстровано");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserModel user)
        {
            var valid = await _users.Find(u =>
                u.Username == user.Username && u.Password == user.Password
            ).FirstOrDefaultAsync();

            if (valid == null)
                return Unauthorized("Невірне ім’я або пароль");

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
