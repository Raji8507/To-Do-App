using AutoMapper;
using TodoDemoApi.DTOs;
using TodoDemoApi.Models;

namespace TodoDemoApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Create Todo DTO -> Entity
            CreateMap<CreateToDoDto, ToDo>();

            // Update DTO -> Existing Entity
            CreateMap<UpdateToDoDto, ToDo>()
                .ForAllMembers(opts =>
                    opts.Condition((src, dest, srcMember) =>
                        srcMember != null));

            // Entity -> Response DTO
            CreateMap<ToDo, ToDoDTO>()
                .ForCtorParam(
                    "AssignedToUsername",
                    opt => opt.MapFrom(src =>
                        src.AssignedToUser != null
                            ? src.AssignedToUser.Username
                            : null))

                .ForCtorParam(
                    "CreatedByUsername",
                    opt => opt.MapFrom(src =>
                        src.CreatedByUser != null
                            ? src.CreatedByUser.Username
                            : null));

            // User Entity -> DTO
            CreateMap<User, UserDTO>();
        }
    }
}