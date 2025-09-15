using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo.Dtos;
using todo.Services;

namespace todo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public AdminController(IAdminService service)
        {
            _service = service;
        }
        [HttpGet("users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var usersDto = await _service.GetAllUsersAsync();
            return Ok(usersDto);
        }

        [HttpPut("users/{id}/promote")]
        public async Task<IActionResult> PromoteUser(int id)
        {
            var result = await _service.PromoteUserAsync(id);
            if (!result) return NotFound("User tidak ditemukan");

            return Ok("User berhasil dipromosikan menjadi Admin");
        }
    }
}