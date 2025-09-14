using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using todo.Data;
using todo.Dtos;
using todo.Services;

namespace todo.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        try
        {
            var registeredUser = await _authService.RegisterAsync(registerUserDto);
            return StatusCode(201, new { message = "User registered successfully" });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal error occurred.");
        }
    }

    // Tambahkan metode Login ini
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUserDto loginUserDto)
    {
        try
        {
            var token = await _authService.LoginAsync(loginUserDto);
            return Ok(new { Token = token }); // Kembalikan token jika sukses
        }
        catch (ArgumentException ex)
        {
            // Untuk login gagal, status 401 Unauthorized lebih sesuai
            return Unauthorized(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "An internal error occurred.");
        }
    }
}