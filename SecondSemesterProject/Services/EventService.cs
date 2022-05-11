using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SecondSemesterProject.Exceptions;
using SecondSemesterProject.Interfaces;
using SecondSemesterProject.Models;

namespace SecondSemesterProject.Services
{
    public class EventService : Connection, IEventService
    {
        private String queryString = "select * from JO22_Event";

        private String queryStringFromID = "select * from JO22_Event where Id = @evId";

        private String insertSql = "insert into JO22_Event Values (@Name," +
                                   "@Description,@DateTime,@Place,@Image)";

        private String deleteSql = "delete from JO22_Event where EventId = @ID";
        private String updateSql = "update JO22_Event set Id = @EventID, Name=@Navn," +
                                   "Description=@Beskrivelse,DateTime=@DatoTid," +
                                   "Place=@Sted,Image=@Billede where Id = @EventID";

        //private IEventService _eventServiceImplementation;

        public EventService(IConfiguration configur) : base(configur)
        {

        }

        //public EventService(string costring) : base(costring)
        //{

        //}

        public async Task UpdateEventAsync(int evId, Event ev)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(updateSql, connection);
                    command.Parameters.AddWithValue("@NewID", ev.EventId);
                    command.Parameters.AddWithValue("@NewName", ev.Name);
                    command.Parameters.AddWithValue("@NewDscrp", ev.Description);
                    command.Parameters.AddWithValue("@evTime", ev.Time);
                    command.Parameters.AddWithValue("NewPlace", ev.Place);
                    command.Parameters.AddWithValue("NewImage", ev.Image);
                    await command.Connection.OpenAsync();

                    int noOfRows = await command.ExecuteNonQueryAsync();
                }
                catch (SqlException sqlEx)
                {
                    throw new DatabaseException(sqlEx.Message);
                }
            }
        }

        public async Task DeleteEventAsync(int evId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    Event evVariable = GetEventAsync(evId).Result;
                    SqlCommand command = new SqlCommand(deleteSql, connection);
                    command.Parameters.AddWithValue("@ID", evId);
                    await command.Connection.OpenAsync();
                    int reader = await command.ExecuteNonQueryAsync();

                }
                catch (SqlException sqlEx)
                {
                    throw new DatabaseException(sqlEx.Message);
                }
            }
        }

        public async Task<bool> CreateEventAsync(Event ev)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    //command.Parameters.AddWithValue("@evId", ev.EventId);
                    command.Parameters.AddWithValue("@Name", ev.Name);
                    command.Parameters.AddWithValue("@Description", ev.Description);
                    command.Parameters.AddWithValue("@DateTime", ev.Time);
                    command.Parameters.AddWithValue("@Place", ev.Place);
                    command.Parameters.AddWithValue("@Image", ev.Image);
                    await command.Connection.OpenAsync();
                    int noOfRows = await command.ExecuteNonQueryAsync();
                    return noOfRows == 5;
                }
                catch (SqlException sqlEx)
                {
                    throw new DatabaseException(sqlEx.Message);
                }
            }
        }

        public async Task<Event> GetEventAsync(int evId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
                try
                {
                    SqlCommand command = new SqlCommand(queryStringFromID, connection);
                    command.Parameters.AddWithValue("@evId", evId);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if (!await reader.ReadAsync())
                    {
                        return null;
                    }
                    int eventId = reader.GetInt32(0);
                    string eventName = reader.GetString(1);
                    string eventDescription = reader.GetString(2);
                    DateTime eventTime = reader.GetDateTime(3);
                    string eventPlace = reader.GetString(4);
                    string eventImage = reader.GetString(5);
                    Event @ev = new Event(eventId, eventName, eventDescription, eventTime, eventPlace, eventImage);
                }
                catch (SqlException sqlEx)
                {
                    throw new DatabaseException(sqlEx.Message);
                }
            return null;
        }

        //SqlDataReader r = ...;

        //String firstName = getString(r[COL_Firstname]);

        private  string getAString(object o)

        {

            if (o == DBNull.Value) return null;

            return (string)o;

        }

        public async Task<List<Event>> GetAllEventAsync()
        {
            List<Event> begivenheder = new List<Event>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        int eventId = reader.GetInt32(0);
                        string eventName = reader.GetString(1);
                        string eventDescription = reader.GetString(2);
                        DateTime eventTime = reader.GetDateTime(3);
                        string eventPlace = reader.GetString(4);

                        string eventImage = getAString(reader["Image"]);
                        ;
                        //var r = reader.Get["Image"];
                        //string eventImage;
                        //if (r is  DBNull)
                        //{
                        //    eventImage = r;
                        //}
                        //else
                        //{
                        //    eventImage = null;
                        //}
                        Event @ev = new Event(eventId, eventName, eventDescription, eventTime, eventPlace, eventImage);
                        begivenheder.Add(@ev);
                    }
                }
            }
            return begivenheder;
            //return null; - Pouls tilføjelse d. 28. april til at starte med få at få løst problemet
        }

        //public Task<List<Event>> GetEventAsync(string filterCriteria)
        //{
        //    throw new NotImplementedException();
        //}

        

        //Kilde: HotelDB2022... - Services, HotelService
    }
}
