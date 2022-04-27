using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondSemesterProject.Interfaces
{
    public interface IMember
    {
        public int ID { get; set; }

        public int FamilyGroupID { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string PhoneNumber { get; set; }

        public bool BoardMember { get; set; }
        public bool HygieneCertified { get; set; }
        public bool BakerApprentice { get; set; }
        public bool CafeApprentice { get; set; }
    }
}