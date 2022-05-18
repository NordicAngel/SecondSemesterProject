using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecondSemesterProject.Exceptions;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Services
{
    public class ParticipantService : Connection, IParticipantService
    {
        private string sqlParticipantPerEvent = "select MemberId from JO22_Participant where EventId = @evId";
        private string sqlCreateParticipant = "insert into JO22_Participant values(@evId, @memId)";
        private string sqlDeleteParticipant = "delete from JO22_Participant where EventId = @evId and MemberId = @memId";
        private string sqlIsParticipating = "select * from JO22_Participant where EventId = @evId and MemberId = @memId";

        public ParticipantService(IConfiguration config) : base(config) { }

        public async Task CreateParticipantAsync(int memberId, int eventId)
        {
            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(sqlCreateParticipant, connection);
            try
            {
                command.Parameters.AddWithValue("@evId", eventId);
                command.Parameters.AddWithValue("@memId", memberId);

                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {

                throw new DatabaseException(sqlEx.Message);
            }
        }

        public async Task DeleteParticipantAsync(int memberId, int eventId)
        {
            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(sqlDeleteParticipant, connection);
            try
            {
                command.Parameters.AddWithValue("@evId", eventId);
                command.Parameters.AddWithValue("@memId", memberId);

                await command.Connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }
            catch (SqlException sqlEx)
            {

                throw new DatabaseException(sqlEx.Message);
            }
        }

        public async Task<List<int>> GetMemberIdPerEventAsync(int eventId)
        {
            List<int> memIds = new List<int>();

            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(sqlParticipantPerEvent, connection);

            try
            {
                command.Parameters.AddWithValue("@evId", eventId);
                await command.Connection.OpenAsync();

                SqlDataReader reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    memIds.Add(
                        (int)reader["MemberId"]);
                }
            }
            catch (SqlException SqlEx)
            {

                throw new DatabaseException(SqlEx.Message);
            }

            return memIds;
        }

        public async Task<bool> IsParticipating(int memberId, int eventId)
        {
            await using SqlConnection connection = new SqlConnection(ConnectionString);
            await using SqlCommand command = new SqlCommand(sqlIsParticipating, connection);

            try
            {
                command.Parameters.AddWithValue("@evId", eventId);
                command.Parameters.AddWithValue("@memId", memberId);

                await command.Connection.OpenAsync();
                SqlDataReader reader = await command.ExecuteReaderAsync();

                return await reader.ReadAsync();
            }
            catch (SqlException sqlEx)
            {

                throw new DatabaseException(sqlEx.Message);
            }
        }
    }
}
