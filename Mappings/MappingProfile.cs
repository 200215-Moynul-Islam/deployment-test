using AutoMapper;
using ELTBackend.DTOs;
using ELTBackend.Models;

namespace ELTBackend.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User Mappings
            CreateMap<UserCreateDto, User>();
            CreateMap<User, UserReadDto>();
            CreateMap<UserUpdateDto, User>();
            CreateMap<User, UserWithLeavesDto>();
            #endregion

            #region Leave Mappings
            CreateMap<LeaveCreateDto, Leave>();
            CreateMap<Leave, LeaveReadDto>();
            CreateMap<Leave, LeaveReadDetailsDto>();
            #endregion
        }
    }
}
