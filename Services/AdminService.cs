using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos;
using todo.Repositories;

namespace todo.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserRepository _userRepo;
        public AdminService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllAsync();
            var usersDto = users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            }).ToList();

            return usersDto;
        }
    }
}