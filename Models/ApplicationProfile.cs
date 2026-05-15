using AutoMapper;
using PersonApi.Models.DTOs;
using PersonApi.Models.Requests;

namespace PersonApi.Models;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        // Model → DTO ( GET)
        CreateMap<Person, PersonDTO>();
        CreateMap<Adress, AdressDTO>();

        // DTO → Model ( UPDATE)
        CreateMap<AdressDTO, Adress>();

        // Request → Model ( CREATE/UPDATE)
        CreateMap<CreatePersonRequest, Person>()
            .ForMember(dest => dest.Adresses, opt => opt.Ignore());
    }
}