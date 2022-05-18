using Microsoft.VisualStudio.TestTools.UnitTesting;
using SecondSemesterProject.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        [TestMethod()]
        public void CreateShiftTypeAsyncTest()
        {
            //Arrange
           var sTService = new ShiftTypeService(
                @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
           var i =  sTService.GetAllShiftTypesAsync().Result.Count;

            //Act
            sTService.CreateShiftTypeAsync(new ShiftType(0, "B", Color.Aqua)).Wait();
            int j = sTService.GetAllShiftTypesAsync().Result.Count;

            var k = sTService.GetAllShiftTypesAsync().Result;
            int m = k.Max(s => s.ShiftTypeId);
            sTService.DeleteShiftTypeAsync(m).Wait();

            //Assert
            Assert.AreEqual(i+1, j);

            
        }

    }
}