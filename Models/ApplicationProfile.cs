using AutoMapper;
using PersonApi.Models.DTOs;

namespace PersonApi.Models;

public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<Person, PersonDTO>();
        CreateMap<Adress, AdressDTO>();
    }
}