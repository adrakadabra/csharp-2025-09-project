using AutoMapper;
using ShiftService.Application.DTO;
using ShiftService.Domain.Entities;

namespace ShiftService.Application.Mappings
{
    public class ShiftMappingProfile : Profile
    {
        public ShiftMappingProfile()
        {
            CreateMap<Shift, ShiftResponse>()
                .ForMember(dest => dest.ShiftId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.EndTime == null));
        }
    }
}