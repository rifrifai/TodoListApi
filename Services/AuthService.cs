using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using todo.Dtos;
using todo.Models;
using todo.Repositories;

namespace todo.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        public AuthService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> LoginAsync(LoginUserDto loginUserDto)
        {
            // ambil user dari db berdasarkan username
            var user = await _userRepo.GetByUsernameAsync(loginUserDto.Username);

            // cek user ada atau tidak
            if (user == null) throw new ArgumentException("Invalid Username");

            // cek password cocok
            var isPasswordValid = BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Invalid password");
            }

            // jika cocok, generate token
            return GenerateJwtToken(user);
        }

        public async Task<User> RegisterAsync(RegisterUserDto registerUserDto)
        {
            // bisnis logic: cek username sudah ada atau belum
            var existingUser = await _userRepo.GetByUsernameAsync(registerUserDto.Username);
            if (existingUser != null) throw new ArgumentException("Username already exists");

            // bisnis logic: hashing password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password);
            var newUser = new User
            {
                Username = registerUserDto.Username,
                PasswordHash = passwordHash,
                Role = "User"
            };

            // bisnis logic: panggil repo untuk simpan data
            var result = await _userRepo.CreateAsync(newUser);
            return result;
        }

        private string GenerateJwtToken(User user)
        {
            // siapkan claims - informasi tentang user yang akan disimpan di token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // ambil secret key dari appsettings
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));

            // buat signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // buat token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = creds,
                Issuer = _config.GetSection("Jwt:Issuer").Value,
                Audience = _config.GetSection("Jwt:Audience").Value
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // return token dalam format string
            return tokenHandler.WriteToken(token);
        }
    }
}