using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecondSemesterProject.Interfaces;

namespace SecondSemesterProject.Services
{
    public class MemberService : Connection, IMemberService
    {
        private static string databaseName = "";

        private string selectSql = $"select * from {databaseName}";
        private string selectByIdSql = $"select * from {databaseName} where ID = @ID";
        private string insertSql = $"insert into {databaseName} Values (@ID)";
        private string deleteSql = $"delete from {databaseName} where ID = @ID";
        private string updateSql = $"update {databaseName} set ID = @ID where ID = @ID";

        public MemberService(string connectionString) : base(connectionString)
        {

        }

        public void CreateMember(IMember member)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    command.Parameters.AddWithValue("@ID", member.ID);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException sqlEx)
                {
                    throw sqlEx;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public void UpdateMember(int id, IMember member)
        {
            throw new NotImplementedException();
        }

        public void DeleteMember(int id)
        {
            throw new NotImplementedException();
        }

        public IMember GetMemberByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<IMember> GetMembersByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<IMember> GetAllMembers()
        {
            throw new NotImplementedException();
        }
    }
}