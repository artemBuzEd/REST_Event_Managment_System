using System.Collections;
using EMS.DAL.EF.Entities;

namespace EMS.DAL.EF.Repositories.Contracts;

public interface IEMSRegistrationRepository : IEMSGenericRepository<Registration>
{
    Task<IEnumerable<Registration?>> GetByRegistrationDateAsync(DateTime registrationDate);
    Task<IEnumerable<Registration?>> GetByStatusAsync(string status);
    Task<IEnumerable<Registration>> GetAllBySpecificEventAsync(int id);

    Task<Registration> GetAllByAttendeeAndEventId(string attendeeId, int eventId);
}