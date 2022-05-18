using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class MemberService : Connection, IMemberService
    {
        // SELECT MEMBER
        private string selectSql = "SELECT * FROM JO22_Member";
        private string selectByIdSql = "SELECT * FROM JO22_Member WHERE Id = @ID";
        private string selectByFamilyGroupIdSql = "SELECT * FROM JO22_Member WHERE FamilyGroupId = @ID";
        private string selectByNameSql = "SELECT * FROM JO22_Member WHERE Name LIKE @Name";

        // CREATE, UPDATE, DELETE MEMBER
        private string insertSql = "INSERT INTO JO22_Member VALUES (NULL, @Name, @Email, @Password, @PhoneNumber, @BoardMember, @HygieneCertified, DEFAULT)";
        private string updateSql = "UPDATE JO22_Member SET FamilyGroupId = @FamilyGroupId, Name = @Name, Email = @Email, Password = @Password, PhoneNumber = @PhoneNumber, BoardMember = @BoardMember, HygieneCertified = @HygieneCertified, ImageFileName = @ImageFileName WHERE Id = @ID";
        private string deleteSql = "DELETE FROM JO22_Member WHERE Id = @ID";

        // LOGIN
        private string loginSql = "SELECT * FROM JO22_Member WHERE Email = @Email AND Password = @Password";

        // FAMILY GROUP
        private string selectFamilyGroupSql = "SELECT * FROM JO22_FamilyGroup";
        private string selectFamilyGroupIdSql = "SELECT MAX(Id) FROM JO22_FamilyGroup";
        private string insertFamilyGroupSql = "INSERT INTO JO22_FamilyGroup DEFAULT VALUES";
        private string deleteFamilyGroupSql = "DELETE FROM JO22_FamilyGroup WHERE Id = @ID";

        // SHIFT TYPE
        private string selectMemberShiftTypeSql = "SELECT * FROM JO22_MemberShiftType WHERE MemberId = @ID";
        private string insertMemberShiftTypeSql = "INSERT INTO JO22_MemberShiftType VALUES (@MemberID, @ShiftTypeID)";
        private string deleteMemberShiftTypeSql = "DELETE FROM JO22_MemberShiftType WHERE MemberId = @MemberID AND ShiftTypeId = @ShiftTypeID";

        // CURRENT MEMBER
        private static IMember CurrentMember;

        public MemberService(IConfiguration configuration) : base(configuration)
        {

        }

        public IMember GetCurrentMember()
        {
            if (CurrentMember != null)
            {
                return CurrentMember;
            }
            else
            {
                return new Member();
            }
        }

        public async Task UpdateCurrentMember(int id)
        {
            CurrentMember = await GetMemberByID(id);
        }

        public bool CheckCurrentMember()
        {
            if (CurrentMember != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Takes an IMember as parameter and inserts it into the database.
        /// </summary>
        /// <param name="member"></param>
        public async Task CreateMember(IMember member)
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
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        public async Task UpdateMember(int id, IMember member)
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
                    command.Parameters.AddWithValue("@ImageFileName", member.ImageFileName);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        public async Task DeleteMember(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        public async Task<bool> CheckMemberInfo(IMember checkMember)
        {
            foreach (IMember member in await GetAllMembers())
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

        public async Task Login(string email, string password)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(loginSql, connection);
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        int memberId = reader.GetInt32(0);
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        string imageFileName = (reader[8] as string) ?? null;

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, imageFileName);

                        CurrentMember = member;
                    }
                    else
                    {
                        throw new NullReferenceException();
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

        public void Logout()
        {
            CurrentMember = null;
        }

        private async Task InsertFamilyGroup()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertFamilyGroupSql, connection);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        public async Task CreateFamilyGroup(List<IMember> members)
        {
            await InsertFamilyGroup();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectFamilyGroupIdSql, connection);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        int familyGroupId = reader.GetInt32(0);

                        foreach (IMember member in members)
                        {
                            member.FamilyGroupID = familyGroupId;

                            await UpdateMember(member.ID, member);
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

        public async Task UpdateFamilyGroup(List<IMember> members, int id)
        {
            List<IMember> familyGroupMembers = await GetAllFamilyGroupMembers(id);

            foreach (IMember member in familyGroupMembers)
            {
                member.FamilyGroupID = null;

                await UpdateMember(member.ID, member);
            }

            foreach (IMember member in members)
            {
                member.FamilyGroupID = id;

                await UpdateMember(member.ID, member);
            }
        }

        public async Task DeleteFamilyGroup(int id)
        {
            List<IMember> familyGroupMembers = await GetAllFamilyGroupMembers(id);

            foreach (IMember member in familyGroupMembers)
            {
                member.FamilyGroupID = null;

                await UpdateMember(member.ID, member);
            }

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteFamilyGroupSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        private async Task CreateMemberShiftType(int memberId, int shiftTypeId)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertMemberShiftTypeSql, connection);
                    command.Parameters.AddWithValue("@MemberID", memberId);
                    command.Parameters.AddWithValue("@ShiftTypeID", shiftTypeId);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        public async Task UpdateMemberShiftTypes(int id, Dictionary<int, bool> shiftTypes)
        {
            List<int> memberShiftTypes = await GetMemberShiftTypes(id);

            // INSERT
            foreach (KeyValuePair<int, bool> shiftType in shiftTypes)
            {
                if (!memberShiftTypes.Contains(shiftType.Key) && shiftType.Value)
                {
                    await CreateMemberShiftType(id, shiftType.Key);
                }
            }

            // DELETE
            foreach (KeyValuePair<int, bool> shiftType in shiftTypes)
            {
                if (memberShiftTypes.Contains(shiftType.Key) && !shiftType.Value)
                {
                    await DeleteMemberShiftTypes(id, shiftType.Key);
                }
            }
        }

        private async Task DeleteMemberShiftTypes(int memberId, int shiftTypeId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(deleteMemberShiftTypeSql, connection);
                    command.Parameters.AddWithValue("@MemberID", memberId);
                    command.Parameters.AddWithValue("@ShiftTypeID", shiftTypeId);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
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

        public async Task<List<int>> GetMemberShiftTypes(int id)
        {
            List<int> shiftTypes = new List<int>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectMemberShiftTypeSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int shiftTypeId = reader.GetInt32(1);

                        shiftTypes.Add(shiftTypeId);
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

            return shiftTypes;
        }

        public async Task<Dictionary<int, List<IMember>>> GetAllFamilyGroups()
        {
            Dictionary<int, List<IMember>> familyGroups = new Dictionary<int, List<IMember>>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectFamilyGroupSql, connection);
                    await command.Connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int familyGroupId = reader.GetInt32(0);

                        List<IMember> familyGroupMembers = await GetAllFamilyGroupMembers(familyGroupId);

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

        public async Task<IMember> GetMemberByID(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectByIdSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        int memberId = reader.GetInt32(0);
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        string imageFileName = (reader[8] as string) ?? null;

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, imageFileName);

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

        public async Task<List<IMember>> GetMembersByName(string name)
        {
            List<IMember> memberList = new List<IMember>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectByNameSql, connection);
                    command.Parameters.AddWithValue("@Name", name);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int memberId = reader.GetInt32(0);
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        string imageFileName = (reader[8] as string) ?? null;

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, boardMember, hygieneCertified, imageFileName);

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

        public async Task<List<IMember>> GetAllMembers()
        {
            List<IMember> memberList = new List<IMember>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectSql, connection);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        int memberId = reader.GetInt32(0);
                        int? familyGroupId = (reader[1] as int?) ?? null;

                        string memberName = reader.GetString(2);
                        string memberEmail = reader.GetString(3);
                        string memberPassword = reader.GetString(4);
                        string memberPhoneNumber = reader.GetString(5);

                        bool boardMember = reader.GetBoolean(6);
                        bool hygieneCertified = reader.GetBoolean(7);

                        string imageFileName = (reader[8] as string) ?? null;

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, hygieneCertified, boardMember, imageFileName);

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

        public async Task<List<IMember>> GetAllFamilyGroupMembers(int id)
        {
            List<IMember> memberList = new List<IMember>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(selectByFamilyGroupIdSql, connection);
                    command.Parameters.AddWithValue("@ID", id);
                    await command.Connection.OpenAsync();

                    SqlDataReader reader = await command.ExecuteReaderAsync();

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

                        string imageFileName = (reader[8] as string) ?? null;

                        IMember member = new Member(memberId, familyGroupId, memberName, memberEmail, memberPassword, memberPhoneNumber, hygieneCertified, boardMember, imageFileName);

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