using ProjectWasel.Models;
using ProjectWasel.Models.ModelsDTO;
using AutoMapper;

namespace ProjectWasel.Profiles;

public class IncidentProfile : Profile
{
    public IncidentProfile() 
    {
        CreateMap<IncidentCreateDTO, Incident>();
    }
    
    
}