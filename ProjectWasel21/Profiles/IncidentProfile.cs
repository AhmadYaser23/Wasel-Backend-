using ProjectWasel21.Models;
using ProjectWasel21.Models.ModelsDTO;
using AutoMapper;

namespace ProjectWasel21.Profiles;

public class IncidentProfile : Profile
{
    public IncidentProfile()
    {
        CreateMap<IncidentCreateDTO, Incident>();

        CreateMap<Incident, IncidentResponseDTO>()
            .ForMember(dest => dest.CheckpointName,
                opt => opt.MapFrom(src => src.Checkpoint != null ? src.Checkpoint.Name : null))
            .ForMember(dest => dest.CreatedByUsername,
                opt => opt.MapFrom(src => src.CreatedByUser != null ? src.CreatedByUser.Username : null))
            .ForMember(dest => dest.VerifiedByUsername,
                opt => opt.MapFrom(src => src.VerifiedByUser != null ? src.VerifiedByUser.Username : null));
    }
}