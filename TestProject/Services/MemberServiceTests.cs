using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecondSemesterProject.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services.Tests
{
    [TestClass()]
    public class MemberServiceTests
    {
        [TestMethod]
        public void TestCount()
        {
            // Arrange
            IMemberService service = new MemberService();

            int countBefore = service.GetAllMembers().Result.Count;

            IMember member_1 = new Member(0, null, "Test", "Test_Email", "1234", "00000000", true, true, null);

            // Act
            service.CreateMember(member_1);

            int countAfter = service.GetAllMembers().Result.Count;

            // Assert
            Assert.AreEqual(countBefore + 1, countAfter);

            service.DeleteMember(member_1.ID);
        }
    }
}