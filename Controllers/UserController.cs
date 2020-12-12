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
using System.Text.RegularExpressions;

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

            // TODO: Validate password before registering member.

            if (ValidateUser(user) == false) 
            {
                return BadRequest(new {
                    status = 401,
                    message = "Invalid input values"
                });
            }

            var userToCreate = new User
            {
                Username = user.Username,
                Email = user.Email
            };

            User createdUser = await _authRepo.Register(userToCreate, user.Password);

            if (createdUser == null) {
                return StatusCode(500);
            } 
            else 
            {
                await _boardRepo.CreateWhiteboard(createdUser.Id, "Board 1");

                string userToken = CreateToken(createdUser);
                return Ok(new {
                    id = createdUser.Id,
                    username = createdUser.Username,
                    token = userToken
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            user.Username = user.Username.ToLower();

            User userFromDb = await _authRepo.GetUser(user.Username);
            // User userFromDb = await _authRepo.Login(user.Username, user.Password);

            if (userFromDb == null)
            {
                return NotFound(new {
                    status = 404,
                    message = "Member with the provided username does not exist."
                });
            }

            bool inputsAreValid = ValidateUser(user);

            bool passwordIsVerified = _authRepo.VerifyPasswordHash(user.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt);

            if (inputsAreValid && passwordIsVerified)
            {
                string userToken = CreateToken(userFromDb);
                return Ok(new {
                    id = userFromDb.Id,
                    username = userFromDb.Username,
                    token = userToken
                });
            }

            return Unauthorized(new {
                status = 401,
                message = "Incorrect password."
            });
        }

        [HttpPost("userexists")]
        public async Task<IActionResult> UserExists(string username)
        {
            User user = await _authRepo.GetUser(username);

            if (user != null)
            {
                return Ok(new {message = "Username exists already"});
            } 
            else 
            {
                return NoContent();
            }
        }

        private Boolean ValidateUser(UserLoginDTO user, string userEmail = null)
        {
            Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$");
            Regex dirtyStringRegex = new Regex(@"[</>#;$%&!?:*()=+`~{}[|\]']");
            Regex mailRegex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$");

            bool mailValid = userEmail != null ? mailRegex.Match(userEmail).Success && (dirtyStringRegex.Match(userEmail).Success == false) : true;
            bool passwordValid = passwordRegex.Match(user.Password).Success && (dirtyStringRegex.Match(user.Password).Success == false);
            bool usernameIsClean = (dirtyStringRegex.Match(user.Username).Success == false);

            return (mailValid && passwordValid && usernameIsClean);
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