using EMS.BLL.DTOs.Responce;
using EMS.DAL.EF.Entities;
using Mapster;

namespace EMS.BLL.Config;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Attendee, AttendeeFullResponseDTO>.NewConfig()
            .Map(dest => dest.Phone, src => src.PhoneNumber);
    }
}