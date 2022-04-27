using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Models
{
    public class Member : IMember
    {
        [Required(ErrorMessage = "Required")]
        public int ID { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required")]
        public string PhoneNumber { get; set; }

        public bool BoardMember { get; set; }
        public bool HygieneCertified { get; set; }
        public bool BakerApprentice { get; set; }
        public bool CafeApprentice { get; set; }

        public Member()
        {
            
        }

        public Member(int id, string name, string email, string password, string phoneNumber, bool boardMember, bool hygieneCertified, bool bakerApprentice, bool cafeApprentice)
        {
            ID = id;
            Name = name;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            BoardMember = boardMember;
            HygieneCertified = hygieneCertified;
            BakerApprentice = bakerApprentice;
            CafeApprentice = cafeApprentice;
        }
    }
}