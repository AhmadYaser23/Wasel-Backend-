using AutoMapper;
using ProjectWasel21.Models;
using ProjectWasel21.Models.ModelsDTO;

namespace ProjectWasel21.Profiles
{
    public class CheckpointProfile : Profile
    {
        public CheckpointProfile()
        {
            // Maps the DTO to the Model and automatically adds the timestamp
            CreateMap<CheckpointDTO, Checkpoint>();

            // Maps the Database Model back to the DTO
            CreateMap<Checkpoint, CheckpointDTO>();
        }
    }
}