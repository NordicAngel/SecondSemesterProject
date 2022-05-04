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
        private static string databaseName = "JO22_Member";

        private string selectSql = $"select * from {databaseName}";
        private string selectByIdSql = $"select * from {databaseName} where Id = @ID";
        private string selectByFamilyGroupIdSql = $"select * from {databaseName} where FamilyGroupId = @ID";
        private string selectByNameSql = $"select * from {databaseName} where Name like @Name";

        private string insertSql = $"insert into {databaseName} values (NULL, @Name, @Email, @Password, @PhoneNumber, @BoardMember, @HygieneCertified)";
        private string updateSql = $"update {databaseName} set FamilyGroupId = @FamilyGroupId, Name = @Name, Email = @Email, Password = @Password, PhoneNumber = @PhoneNumber, BoardMember = @BoardMember, HygieneCertified = @HygieneCertified where Id = @ID";
        private string updateNoFamilyGroupSql = $"update {databaseName} set Name = @Name, Email = @Email, Password = @Password, PhoneNumber = @PhoneNumber, BoardMember = @BoardMember, HygieneCertified = @HygieneCertified where Id = @ID";
        private string deleteSql = $"delete from {databaseName} where Id = @ID";

        private string loginSql = $"select * from {databaseName} where Email = @Email, Password = @Password";

        private string selectFamilyGroupSql = "select * from JO22_FamilyGroup";
        private string selectFamilyGroupIdSql = "select max(Id) from JO22_FamilyGroup";
        private string insertFamilyGroupSql = "insert into JO22_FamilyGroup default values";

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
                    command.Parameters.AddWithValue("@Name", member.Name);
                    command.Parameters.AddWithValue("@Email", member.Email);
                    command.Parameters.AddWithValue("@Password", member.Password);
                    command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                    command.Parameters.AddWithValue("@BoardMember", member.BoardMember);
                    command.Parameters.AddWithValue("@HygieneCertified", member.HygieneCertified);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void UpdateMember(int id, IMember member)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    if (member.FamilyGroupID != null)
                    {
                        SqlCommand command = new SqlCommand(updateSql, connection);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@FamilyGroupId", member.FamilyGroupID);
                        command.Parameters.AddWithValue("@Name", member.Name);
                        command.Parameters.AddWithValue("@Email", member.Email);
                        command.Parameters.AddWithValue("@Password", member.Password);
                        command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                        command.Parameters.AddWithValue("@BoardMember", member.BoardMember);
                        command.Parameters.AddWithValue("@HygieneCertified", member.HygieneCertified);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        SqlCommand command = new SqlCommand(updateNoFamilyGroupSql, connection);
                        command.Parameters.AddWithValue("@ID", id);
                        command.Parameters.AddWithValue("@Name", member.Name);
                        command.Parameters.AddWithValue("@Email", member.Email);
                        command.Parameters.AddWithValue("@Password", member.Password);
                        command.Parameters.AddWithValue("@PhoneNumber", member.PhoneNumber);
                        command.Parameters.AddWithValue("@BoardMember", member.BoardMember);
                        command.Parameters.AddWithValue("@HygieneCertified", member.HygieneCertified);
                        command.Connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
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
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
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
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified);

                        return member;
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return new Member();
        }

        public void Logout()
        {

        }

        public void CreateFamilyGroup(List<IMember> members)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command_1 = new SqlCommand(insertFamilyGroupSql, connection);
                    command_1.Connection.Open();
                    command_1.ExecuteNonQuery();
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command_2 = new SqlCommand(selectFamilyGroupIdSql, connection);
                    command_2.Connection.Open();

                    SqlDataReader reader = command_2.ExecuteReader();
                    reader.Read();

                    int familyGroupId = reader.GetInt32(0);

                    foreach (IMember member in members)
                    {
                        member.FamilyGroupID = familyGroupId;

                        UpdateMember(member.ID, member);
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public Dictionary<int, List<IMember>> GetAllFamilyGroups()
        {
            Dictionary<int, List<IMember>> familyGroups = new Dictionary<int, List<IMember>>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectFamilyGroupSql, connection);
                    command.Connection.Open();
                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int familyGroupId = reader.GetInt32(0);

                        List<IMember> familyGroupMembers = GetAllFamilyGroupMembers(familyGroupId);

                        familyGroups.Add(familyGroupId, familyGroupMembers);
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return familyGroups;
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
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified);

                        return member;
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
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
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified);

                        memberList.Add(member);
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
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
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, hygieneCertified, boardMember);

                        memberList.Add(member);
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
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
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, hygieneCertified, boardMember);

                        memberList.Add(member);
                    }
                }
                catch (SqlException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return memberList;
        }
    }
}