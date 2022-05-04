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
        private string insertSql = "insert into JO22_ShiftType Values (@Name, @Color)";
        private string getTypeFromId = "select * from JO22_ShiftType where Id = @Id";
        private string getAllTypes = "select * from JO22_ShiftType";
        private string deleteType = "delete from JO22_ShiftType where Id = @Id";
        private string UpdateType = "update JO22_ShiftType" +
                                    " set Name = @Name, Color = @Color " +
                                    "Where Id = @Id";


        public List<ShiftType> Types { get; set; }

        public async Task<bool> CreateShiftTypeAsync(ShiftType shiftType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(insertSql, connection);
                //command.Parameters.AddWithValue("@Id", shiftType.ShiftTypeId);
                command.Parameters.AddWithValue("@Name", shiftType.Name);
                command.Parameters.AddWithValue("@Color", shiftType.Color.ToArgb());
                await command.Connection.OpenAsync();
                int noOfRows = await command.ExecuteNonQueryAsync();
                return noOfRows == 1;
            }

        }

        public async Task<ShiftType> GetShiftTypeAsync(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(getTypeFromId, connection);
                command.Parameters.AddWithValue("@Id", id);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int typeId = reader.GetInt32(0);
                    string typeName = reader.GetString(1);
                    Color typeColor = Color.FromArgb(reader.GetInt32(2));
                    ShiftType sType = new ShiftType(typeId, typeName, typeColor);
                    return sType;
                }
            }

            return null;
        }

        public async Task<List<ShiftType>> GetAllShiftTypesAsync()
        {
            List<ShiftType> sTypes = new List<ShiftType>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(getAllTypes, connection);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int TypeId = reader.GetInt32(0);
                    string TypeName = reader.GetString(1);   
                    Color TypeColor = Color.FromArgb(reader.GetInt32(2));
                    ShiftType shiftType = new ShiftType(TypeId, TypeName, TypeColor);
                    sTypes.Add(shiftType);
                }
            }

            return sTypes;

            //return new List<ShiftType>()
            //{
            //    new ShiftType() {Name = "Bager", Color = Color.Blue,  ShiftTypeId = 1}
            //};
        }

        public async Task<bool> UpdateShiftTypeAsync(int shiftTypeId, ShiftType shiftType)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(UpdateType, connection);
                command.Parameters.AddWithValue("@Name", shiftType.Name);
                command.Parameters.AddWithValue("@Color", shiftType.Color.ToArgb());
                command.Parameters.AddWithValue("@Id", shiftTypeId);
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return true;
        }

        public async Task<ShiftType> DeleteShiftTypeAsync(int shiftTypeId)
        {

            ShiftType shiftType = await GetShiftTypeAsync(shiftTypeId);
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                SqlCommand command = new SqlCommand(deleteType, connection);
                command.Parameters.AddWithValue("@Id", shiftType.ShiftTypeId);
                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return shiftType;
        }

        public ShiftTypeService(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
