using AutoMapper;

namespace ToDoApi.Models
{
    public class TodoItemProfile: Profile
    {
        public TodoItemProfile()
        {
            CreateMap<TodoItem, TodoItemDTO>()
                .ForMember(dest => dest.TaskName, opt => opt.MapFrom(src => src.Name))
                .ReverseMap();            
        }
    }
}

