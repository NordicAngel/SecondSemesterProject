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
        // SELECT
        private string selectSql = "SELECT * FROM JO22_Member";
        private string selectByIdSql = "SELECT * FROM JO22_Member WHERE Id = @ID";
        private string selectByFamilyGroupIdSql = "SELECT * FROM JO22_Member WHERE FamilyGroupId = @ID";
        private string selectByNameSql = "SELECT * FROM JO22_Member WHERE Name LIKE @Name";

        // CREATE, UPDATE, DELETE
        private string insertSql = "INSERT INTO JO22_Member VALUES (NULL, @Name, @Email, @Password, @PhoneNumber, @BoardMember, @HygieneCertified)";
        private string updateSql = "UPDATE JO22_Member SET FamilyGroupId = @FamilyGroupId, Name = @Name, Email = @Email, Password = @Password, PhoneNumber = @PhoneNumber, BoardMember = @BoardMember, HygieneCertified = @HygieneCertified WHERE Id = @ID";
        private string deleteSql = "DELETE FROM JO22_Member WHERE Id = @ID";

        // LOGIN
        private string loginSql = "SELECT * FROM JO22_Member WHERE Email = @Email, Password = @Password";

        // FAMILY GROUP
        private string selectFamilyGroupSql = "SELECT * FROM JO22_FamilyGroup";
        private string selectFamilyGroupIdSql = "SELECT MAX(Id) FROM JO22_FamilyGroup";
        private string insertFamilyGroupSql = "INSERT INTO JO22_FamilyGroup DEFAULT VALUES";
        private string deleteFamilyGroupSql = "DELETE FROM JO22_FamilyGroup WHERE Id = @ID";

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
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@ID", id);

                    if (member.FamilyGroupID != null)
                    {
                        command.Parameters.AddWithValue("@FamilyGroupId", member.FamilyGroupID);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@FamilyGroupId", DBNull.Value);
                    }

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

        public bool CheckMemberInfo(IMember checkMember)
        {
            foreach (IMember member in GetAllMembers())
            {
                if (member.ID != checkMember.ID)
                {
                    if (string.Equals(member.Email, checkMember.Email) || string.Equals(member.PhoneNumber, checkMember.PhoneNumber))
                    {
                        return false;
                    }
                }
            }

            return true;
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
                    SqlCommand command = new SqlCommand(insertFamilyGroupSql, connection);
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

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectFamilyGroupIdSql, connection);
                    command.Connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        int familyGroupId = reader.GetInt32(0);

                        foreach (IMember member in members)
                        {
                            member.FamilyGroupID = familyGroupId;

                            UpdateMember(member.ID, member);
                        }
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

        public void UpdateFamilyGroup(List<IMember> members, int id)
        {
            List<IMember> familyGroupMembers = GetAllFamilyGroupMembers(id);

            foreach (IMember member in familyGroupMembers)
            {
                member.FamilyGroupID = null;

                UpdateMember(member.ID, member);
            }

            foreach (IMember member in members)
            {
                member.FamilyGroupID = id;

                UpdateMember(member.ID, member);
            }
        }

        public void DeleteFamilyGroup(int id)
        {
            List<IMember> familyGroupMembers = GetAllFamilyGroupMembers(id);

            foreach (IMember member in familyGroupMembers)
            {
                member.FamilyGroupID = null;

                UpdateMember(member.ID, member);
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteFamilyGroupSql, connection);
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