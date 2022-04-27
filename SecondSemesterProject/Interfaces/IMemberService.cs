using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Interfaces
{
    public interface IMemberService
    {
        public void CreateMember(IMember member);
        public void UpdateMember(int id, IMember member);
        public void DeleteMember(int id);

        public IMember Login(string email, string password);
        public void Logout();

        public IMember GetMemberByID(int id);
        public List<IMember> GetMembersByName(string name);
        public List<IMember> GetAllMembers();
    }
}