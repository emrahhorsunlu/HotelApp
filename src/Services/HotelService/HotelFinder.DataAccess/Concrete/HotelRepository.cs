using HotelFinder.DataAccess.Abstract;
using HotelFinder.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinder.DataAccess.Concrete
{
    public class HotelRepository : IHotelRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public HotelRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MainConnection");
        }

        public Hotel CreateHotel(Hotel hotel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("CreateHotel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Name", hotel.Name);
                    command.Parameters.AddWithValue("@City", hotel.City);
                    var newHotelIdParameter = new SqlParameter
                    {
                        ParameterName = "@NewHotelId",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(newHotelIdParameter);

                    connection.Open();
                    command.ExecuteNonQuery();

                    hotel.Id = (int)newHotelIdParameter.Value;

                    return hotel;
                }
            }
        }

        public void DeleteHotel(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("DeleteHotelById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Hotel> GetAllHotels()
        {
            var hotels = new List<Hotel>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetAllHotels", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var hotel = new Hotel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                City = reader.GetString(reader.GetOrdinal("City"))
                            };
                            hotels.Add(hotel);
                        }
                    }
                }
            }
            return hotels;
        }

        public Hotel GetHotelById(int id)
        {
            Hotel hotel = null;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetHotelById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hotel = new Hotel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                City = reader.GetString(reader.GetOrdinal("City"))
                            };
                        }
                    }
                }
            }
            return hotel;
        }

        public Hotel GetHotelByName(string name)
        {
            Hotel hotel = null;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetHotelByName", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Name", name);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            hotel = new Hotel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                City = reader.GetString(reader.GetOrdinal("City"))
                            };
                        }
                    }
                }
            }
            return hotel;
        }

        public Hotel UpdateHotel(Hotel hotel)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("UpdateHotel", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", hotel.Id);
                    command.Parameters.AddWithValue("@Name", hotel.Name);
                    command.Parameters.AddWithValue("@City", hotel.City);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            return hotel;
        }
    }
}
