using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using todo.Dtos;
using todo.Models;

namespace todo.Services
{
    public interface IAuthService
    {
        Task<User> RegisterAsync(RegisterUserDto registerUserDto);
        Task<string> LoginAsync(LoginUserDto loginUserDto);
    }
}