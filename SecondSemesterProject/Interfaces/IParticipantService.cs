using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Interfaces
{
    public interface IParticipantService
    {
        Task CreateParticipantAsync(int memberId, int eventId);
        Task DeleteParticipantAsync(int memberId, int eventId);
        Task<List<int>> GetMemberIdPerEventAsync(int eventId);
        Task<bool> IsParticipating(int memberId, int eventId);
    }
}
