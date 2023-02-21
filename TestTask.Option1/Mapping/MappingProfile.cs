using AutoMapper;
using TestTask.Option1.Data.Entities;
using TestTask.Option1.Models.Dtos;

namespace TestTask.Option1.Mapping
{

    // This class configures mapping 

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Device, DeviceDto>();
            CreateMap<Experiment, ExperimentDto>();
            CreateMap<ExperimentValue, ExperimentValueDto>();
            CreateMap<Selection, SelectionDto>();
        }
    }
}
