using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Interfaces
{
    public interface IMemberService
    {
        public IMember GetCurrentMember();
        public bool GetBoardMember();
        public Task UpdateCurrentMember(int id);

        public Task CreateMember(IMember member);
        public Task UpdateMember(int id, IMember member);
        public Task DeleteMember(int id);

        public Task<bool> CheckMemberInfo(IMember checkMember);
        public Task Login(string email, string password);
        public void Logout();

        public Task CreateFamilyGroup(List<IMember> members);
        public Task UpdateFamilyGroup(List<IMember> members, int id);
        public Task DeleteFamilyGroup(int id);

        public Task UpdateMemberShiftTypes(int id, Dictionary<int, bool> shiftTypes);

        public Task<List<int>> GetMemberShiftTypes(int id);

        public Task<Dictionary<int, List<IMember>>> GetAllFamilyGroups();

        public Task<IMember> GetMemberByID(int id);
        public Task<List<IMember>> GetMembersByName(string name);
        public Task<List<IMember>> GetAllMembers();
        public Task<List<IMember>> GetAllFamilyGroupMembers(int id);
    }
}