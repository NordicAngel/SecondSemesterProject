using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class ShiftTypeService : Connection, IShiftTypeService
    {
        private string insertSql = "insert into JO22_ShiftType Values (@Id, @Name, @Color)";
        private string getTypeFromId = "select * from JO22_ShiftType where Id == @Id";
        private string getAllTypes = "select * from JO22_ShiftType";


        public List<ShiftType> Types { get; set; }

        public async Task<bool> CreateShiftTypeAsync(ShiftType shiftType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);
                command.Parameters.AddWithValue("@Id", shiftType.ShiftTypeId);
                command.Parameters.AddWithValue("@Name", shiftType.Name);
                command.Parameters.AddWithValue("@Color", shiftType.Color);
                await command.Connection.OpenAsync();
                int noOfRows = await command.ExecuteNonQueryAsync();
                return noOfRows == 1;
            }

        }

        public async Task<ShiftType> GetShiftTypeAsync(int shiftTypeId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(getTypeFromId, connection);
                command.Parameters.AddWithValue("@Id", shiftTypeId);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int TypeId = reader.GetInt32(0);
                    string TypeName = reader.GetString(1);
                    Color TypeColor = Color.FromArgb(reader.GetInt32(2));
                    ShiftType shiftType = new ShiftType(TypeName, TypeColor, TypeId);
                }
            }

            return null;
        }

        public async Task<List<ShiftType>> GetAllShiftTypesAsync()
        {
            List<ShiftType> sTypes;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(getAllTypes, connection);

            }

            return new List<ShiftType>()
            {
                new ShiftType() {Name = "Bager", Color = Color.Blue,  ShiftTypeId = 1}
            };
        }

        public void UpdateShiftTypeAsync(int shiftTypeId, ShiftType newShiftType)
        {
            throw new NotImplementedException();
        }

        public void DeleteShiftTypeAsync(int shiftTypeId)
        {
            throw new NotImplementedException();
        }

        public ShiftTypeService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
