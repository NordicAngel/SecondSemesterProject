using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class MemberService : Connection, IMemberService
    {
        private static string databaseName = "";

        private string selectSql = $"select * from {databaseName}";
        private string selectByIdSql = $"select * from {databaseName} where ID = @ID";
        private string selectByFamilyGroupIdSql = $"select * from {databaseName} where ID = @ID";
        private string selectByNameSql = $"select * from {databaseName} where Name = @Name";

        private string insertSql = $"insert into {databaseName} Values (@ID)";
        private string updateSql = $"update {databaseName} set ID = @MemberID where ID = @ID";
        private string deleteSql = $"delete from {databaseName} where ID = @ID";

        private string loginSql = $"select * from {databaseName} where Email = @Email, Password = @Password";

        public MemberService(IConfiguration configuration) : base(configuration)
        {

        }

        public void CreateMember(IMember member)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
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
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
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

        public void DeleteMember(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
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

        public IMember Login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(loginSql, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int memberId = reader.GetInt32(0);
                        int familyGroupId = reader.GetInt32(1);

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);
                        bool cafeApprentice = reader.GetBoolean(8);
                        bool bakerApprentice = reader.GetBoolean(9);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, cafeApprentice, bakerApprentice);

                        return member;
                    }
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

            return new Member();
        }

        public void Logout()
        {

        }

        public IMember GetMemberByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectByIdSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int memberId = reader.GetInt32(0);
                        int familyGroupId = reader.GetInt32(1);

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);
                        bool cafeApprentice = reader.GetBoolean(8);
                        bool bakerApprentice = reader.GetBoolean(9);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, cafeApprentice, bakerApprentice);

                        return member;
                    }
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

            return new Member();
        }

        public List<IMember> GetMembersByName(string name)
        {
            List<IMember> memberList = new List<IMember>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectByNameSql, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int memberId = reader.GetInt32(0);
                        int familyGroupId = reader.GetInt32(1);

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);
                        bool cafeApprentice = reader.GetBoolean(8);
                        bool bakerApprentice = reader.GetBoolean(9);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, cafeApprentice, bakerApprentice);

                        memberList.Add(member);
                    }
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

            return memberList;
        }

        public List<IMember> GetAllMembers()
        {
            List<IMember> memberList = new List<IMember>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectSql, connection);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int memberId = reader.GetInt32(0);
                        int familyGroupId = reader.GetInt32(1);

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);
                        bool cafeApprentice = reader.GetBoolean(8);
                        bool bakerApprentice = reader.GetBoolean(9);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, cafeApprentice, bakerApprentice);

                        memberList.Add(member);
                    }
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

            return memberList;
        }

        public List<IMember> GetAllFamilyGroupMembers(int id)
        {
            List<IMember> memberList = new List<IMember>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectByFamilyGroupIdSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int memberId = reader.GetInt32(0);
                        int familyGroupId = reader.GetInt32(1);

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);
                        bool cafeApprentice = reader.GetBoolean(8);
                        bool bakerApprentice = reader.GetBoolean(9);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, cafeApprentice, bakerApprentice);

                        memberList.Add(member);
                    }
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

            return memberList;
        }
    }
}