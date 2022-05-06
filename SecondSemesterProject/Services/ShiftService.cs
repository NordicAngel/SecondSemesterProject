using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecondSemesterProject.Exceptions;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class ShiftService : Connection, IShiftService
    {
        #region SQL statments

        private string createSql = "Insert into JO22_Shift (DateTimeStart, DateTimeEnd, ShiftTypeId) values (@DStart, @DEnd, @STId)";
        private string getAllSql = "select * from JO22_Shift";
        private string getShiftSql = "select * from JO22_Shift where Id = @ID";
        private string removeSql = "delete from JO22_Shift where Id = @ID";

        #endregion
        public ShiftService(IConfiguration config):base(config)
        { }
        public async Task CreateShiftAsync(Shift shift)
        {
            if (shift.DateTimeStart < DateTime.Now)
                throw new ShiftDateBeforeNow("Shift can't be added retroactively");

            if (shift.DateTimeStart > shift.DateTimeEnd)
                throw new StartAfterEndException("Start time can not after end time");

            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(createSql, connection);
            try
            {
                command.Parameters.AddWithValue("@DStart", shift.DateTimeStart);
                command.Parameters.AddWithValue("@DEnd", shift.DateTimeEnd);
                command.Parameters.AddWithValue("@STId", shift.ShiftTypeId);

                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {

                throw new DatabaseException($"Databasen havde en fejl: {sqlEx.Message}");
            }
        }

        public async Task<Shift> GetShiftAsync(int shiftId)
        {
            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(getShiftSql, connection);
            try
            {
                command.Parameters.AddWithValue("@ID", shiftId);
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                if (!await reader.ReadAsync()) return null;

                int id = (int)reader["Id"];
                int? memberId = reader["MemberId"] as int?;
                DateTime dTStart = (DateTime)reader["DateTimeStart"];
                DateTime dTEnd = (DateTime)reader["DateTimeEnd"];
                int sTId = (int)reader["ShiftTypeId"];
                return new Shift()
                {
                    ShiftId = id,
                    MemberId = memberId,
                    DateTimeStart = dTStart,
                    DateTimeEnd = dTEnd,
                    ShiftTypeId = sTId
                };
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException($"Databasen havde en fejl: {sqlEx.Message}");
            }


            return null;
        }

        public async Task<List<Shift>> GetAllShiftAsync()
        {
            List<Shift> shifts = new List<Shift>();
            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(getAllSql, connection);
            try
            {
                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int id = (int)reader["Id"];
                    int? memberId = reader["MemberId"] as int?;
                    DateTime dTStart = (DateTime)reader["DateTimeStart"];
                    DateTime dTEnd = (DateTime)reader["DateTimeEnd"];
                    int sTId = (int)reader["ShiftTypeId"];
                    shifts.Add(new Shift()
                    {
                        ShiftId = id,
                        MemberId = memberId,
                        DateTimeStart = dTStart,
                        DateTimeEnd = dTEnd,
                        ShiftTypeId = sTId
                    });
                }
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException($"Databasen havde en fejl: {sqlEx.Message}");
            }

            return shifts;
        }

        public async Task UpdateShiftAsync(int shiftId, Shift newShift)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteShiftAsync(int shiftId)
        {
            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(removeSql, connection);
            try
            {
                command.Parameters.AddWithValue("@ID", shiftId);
                await command.Connection.OpenAsync();
                command.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException($"Databasen havde en fejl: {sqlEx.Message}");
            }
        }
    }
}
