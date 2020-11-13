using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Whiteboard.API.Data;
using Whiteboard.API.DTO;
using Whiteboard.API.Models;

namespace Whiteboard.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IWhiteboardRepository _boardRepo;
        private readonly IConfiguration _config;

        public UserController(IAuthRepository authRepo, IWhiteboardRepository boardRepo, IConfiguration config)
        {
            _config = config;
            _authRepo = authRepo;
            _boardRepo = boardRepo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDTO user)
        {
            // Validate request

            user.Username = user.Username.ToLower();

            if (await _authRepo.UserExists(user.Username))
            {
                return BadRequest("Already exists");
            }

            var userToCreate = new User
            {
                Username = user.Username,
                Email = user.Email
            };

            var createdUser = await _authRepo.Register(userToCreate, user.Password);
            if (createdUser != null) {
                await _boardRepo.CreateWhiteboard(createdUser.Id, "Board 1");
            }

            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            user.Username = user.Username.ToLower();

            User userFromDb = await _authRepo.Login(user.Username, user.Password);

            if (userFromDb != null)
            {
                string userToken = CreateToken(userFromDb);
                return Ok(new {
                    id = userFromDb.Id,
                    username = userFromDb.Username,
                    token = userToken
                });
            }

            return StatusCode(500);
        }

        [HttpGet("version")]
        public string Version() {
            return "0.1.0";
        }

        private string CreateToken(User user)
        {
            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value)
            );

            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}