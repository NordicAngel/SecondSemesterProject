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

        private String queryStringFromID = "select * from JO22_Event where EventId = @evId";

        private String insertSql = "insert into JO22_Event Values(@evId, @Name," +
                                   "@Description,@evTime,@Place,@Image";

        //private IEventService _eventServiceImplementation;

        public EventService(IConfiguration configur) : base(configur) {

        }

        //public EventService(string costring) : base(costring)
        //{

        //}

        public Task UpdateEventAsync(int evId,Event ev) {
            throw new NotImplementedException();
        }

        public Task DeleteEventAsync(int evId) {
            throw new NotImplementedException();
        }

        //public async Task<bool> CreateEventAsync(Event ev) {
        //    using( SqlConnection connection = new SqlConnection(ConnectionString) ) {
        //        try {
        //            SqlCommand command = new SqlCommand(insertSql,connection);
        //            command.Parameters.AddWithValue("@evId",ev.EventId);
        //            await command.Connection.OpenAsync();
        //            int noOfRows = await command.ExecuteNonQueryAsync();
        //            return noOfRows == 1;
        //        }
        //        catch( SqlException sqlEx ) {
        //            throw new DatabaseException(sqlEx.Message);
        //        }
        //    }
        //    return false;
        //}

        public async Task<Event> GetEventAsync(int evId) {
            using( SqlConnection connection = new SqlConnection(ConnectionString) )
                try {
                    SqlCommand command = new SqlCommand(queryStringFromID,connection);
                    command.Parameters.AddWithValue("@evId",evId);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if( !await reader.ReadAsync() ) {
                        return null;
                    }
                    int eventId = reader.GetInt32(0);
                    string eventName = reader.GetString(1);
                    string eventDescription = reader.GetString(2);
                    DateTime eventTime = reader.GetDateTime(3);
                    string eventPlace = reader.GetString(4);
                    string eventImage = reader.GetString(5);
                    Event @ev = new Event(eventId,eventName,eventDescription,eventTime,eventPlace,eventImage);
                }
                catch( SqlException sqlEx ) {
                    throw new DatabaseException(sqlEx.Message);
                }
            return null;
        }

        public async Task<List<Event>> GetAllEventAsync() {
            List<Event> begivenheder = new List<Event>();
            using( SqlConnection connection = new SqlConnection(ConnectionString) ) {
                using( SqlCommand command = new SqlCommand(queryString,connection) ) {
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while( await reader.ReadAsync() ) {
                        int eventId = reader.GetInt32(0);
                        string eventName = reader.GetString(1);
                        string eventDescription = reader.GetString(2);
                        DateTime eventTime = reader.GetDateTime(3);
                        string eventPlace = reader.GetString(4);
                        string eventImage = reader.GetString(5);
                        Event @ev = new Event(eventId,eventName,eventDescription,eventTime,eventPlace,eventImage);
                        begivenheder.Add(@ev);
                    }
                }
            }
            return begivenheder;
            //return null; - Pouls tilføjelse d. 28. april til at starte med få at få løst problemet
        }

        public Task CreateEventAsync(Event ev) {
            throw new NotImplementedException();
        }



        //Kilde: HotelDB2022... - Services, HotelService
    }
}
