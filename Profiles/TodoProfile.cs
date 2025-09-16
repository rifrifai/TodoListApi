using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using todo.Dtos;
using todo.Models;

namespace todo.Profiles;
public class TodoProfile : Profile
{
    public TodoProfile()
    {
        // mapping dari dto untuk membuat data ke model
        // ex: POST /api/todo
        CreateMap<CreateTodoDto, TodoItem>();

        // mapping dari dto untuk update (patch) ke model
        // ini akan memberitahu automapper untuk hanya memetakan properti tidak null,
        // yang sangat berguna untuk operasi PATCH.
        CreateMap<UpdateTodoDto, TodoItem>().ForAllMembers(options => options.Condition((src, dest, srcMember) => srcMember != null));

        // mapping dari model ke dto untuk response
        // ex: GET /api/todo/{id}
        CreateMap<TodoItem, TodoDto>().ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
    }
}