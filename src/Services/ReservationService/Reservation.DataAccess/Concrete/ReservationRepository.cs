using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Reservation.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Reservation.DataAccess.Concrete
{
    public class ReservationRepository : IReservationRepository

    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ReservationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("MainConnection");
        }

        public Entities.Reservation CreateReservation(Entities.Reservation reservation)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // hotel var mı kontrol
                using (var checkHotelCommand = new SqlCommand("GetHotelById", connection))
                {
                    checkHotelCommand.CommandType = CommandType.StoredProcedure;
                    checkHotelCommand.Parameters.AddWithValue("@Id", reservation.HotelId);

                    using (var reader = checkHotelCommand.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                    }
                }

                // hotel bulundu rezervasyon oluştur
                using (var command = new SqlCommand("CreateReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@HotelId", reservation.HotelId);
                    command.Parameters.AddWithValue("@Date", reservation.Date);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    var newReservationIdParameter = new SqlParameter
                    {
                        ParameterName = "@NewReservationId",
                        SqlDbType = System.Data.SqlDbType.Int,
                        Direction = System.Data.ParameterDirection.Output
                    };
                    command.Parameters.Add(newReservationIdParameter);

                    command.ExecuteNonQuery();

                    reservation.Id = (int)newReservationIdParameter.Value;

                    return reservation;
                }
            }

        }

        public void DeleteReservation(int id)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("DeleteReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Entities.Reservation> GetAllReservations()
        {
            var reservations = new List<Entities.Reservation>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetAllReservations", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new Entities.Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HotelId = reader.GetInt32(reader.GetOrdinal("HotelId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status"))
                            };

                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }

        public Entities.Reservation GetReservationById(int id)
        {
            Entities.Reservation reservation = null;

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetReservationById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            reservation = new Entities.Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HotelId = reader.GetInt32(reader.GetOrdinal("HotelId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }

            return reservation;
        }

        public List <Entities.Reservation> GetReservationByStatus(bool status)
        {
            var reservations = new List<Entities.Reservation>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetReservationsByStatus", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Status", status);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new Entities.Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HotelId = reader.GetInt32(reader.GetOrdinal("HotelId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status"))
                            };

                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }

        public List<Entities.Reservation> GetReservationsByHotelId(int id)
        {
            var reservations = new List<Entities.Reservation>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetReservationsByHotelId", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@HotelId", id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var reservation = new Entities.Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HotelId = reader.GetInt32(reader.GetOrdinal("HotelId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status"))
                            };

                            reservations.Add(reservation);
                        }
                    }
                }
            }

            return reservations;
        }

        public Entities.Reservation UpdateReservation(Entities.Reservation reservation)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("UpdateReservation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Id", reservation.Id);
                    command.Parameters.AddWithValue("@HotelId", reservation.HotelId);
                    command.Parameters.AddWithValue("@Date", reservation.Date);
                    command.Parameters.AddWithValue("@Status", reservation.Status);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }

            // güncellenen rezervasyonu almak için tekrar sorgu
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand("GetReservationById", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", reservation.Id);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Entities.Reservation
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                HotelId = reader.GetInt32(reader.GetOrdinal("HotelId")),
                                Date = reader.GetDateTime(reader.GetOrdinal("Date")),
                                Status = reader.GetBoolean(reader.GetOrdinal("Status"))
                            };
                        }
                    }
                }
            }

            return null;
        }
    }
}
