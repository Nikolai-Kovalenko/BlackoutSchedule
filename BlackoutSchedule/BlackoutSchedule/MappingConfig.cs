using AutoMapper;
using BlackoutSchedule.Models;
using BlackoutSchedule.Models.Dto;

namespace BlackoutSchedule
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Groups, GroupDto>().ReverseMap();
            CreateMap<Addresses, AddressDto>().ReverseMap();
            CreateMap<Schedules, ScheduleDto>().ReverseMap();
        }
    }
}