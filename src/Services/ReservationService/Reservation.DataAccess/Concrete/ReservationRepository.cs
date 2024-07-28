using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reservation.DataAccess.Abstract;
using Reservation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reservation.DataAccess.Concrete
{
    public class ReservationRepository : IReservationRepository
    {
        public Entities.Reservation CreateReservation(Entities.Reservation reservation)
        {
            
            using (var reservationDbContext = new ReservationDbContext())
            {
                var hotelIdParameter = new SqlParameter("@HotelId", reservation.HotelId);
                var dateParameter = new SqlParameter("@Date", reservation.Date);
                var statusParameter = new SqlParameter("@Status", reservation.Status);
                var newReservationIdParameter = new SqlParameter
                {
                    ParameterName = "@NewReservationId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };

                reservationDbContext.Database.ExecuteSqlRaw(
                    "EXEC CreateReservation @HotelId, @Date, @Status, @NewReservationId OUTPUT",
                    hotelIdParameter, dateParameter, statusParameter, newReservationIdParameter);

                reservation.Id = (int)newReservationIdParameter.Value;

                return reservation;
            }
        }

        public void DeleteReservation(int id)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var idParameter = new SqlParameter("@Id", id);

                reservationDbContext.Database.ExecuteSqlRaw(
                    "EXEC DeleteReservation @Id",
                    idParameter);
            }
        }

        public List<Entities.Reservation> GetAllReservations()
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var reservations = reservationDbContext.Reservations
                    .FromSqlRaw("EXEC GetAllReservations")
                    .ToList();

                return reservations;
            }
        }

        public Entities.Reservation GetReservationById(int id)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var idParameter = new SqlParameter("@Id", id);

                var reservation = reservationDbContext.Reservations
                    .FromSqlRaw("EXEC GetReservationById @Id", idParameter)
                    .AsEnumerable()
                    .FirstOrDefault();

                return reservation;
            }
        }

        public List <Entities.Reservation> GetReservationByStatus(bool status)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var statusParameter = new SqlParameter("@Status", status);

                var reservations = reservationDbContext.Reservations
                    .FromSqlRaw("EXEC GetReservationsByStatus @Status", statusParameter)
                    .ToList();

                return reservations;
            }
        }

        public List<Entities.Reservation> GetReservationsByHotelId(int id)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var hotelIdParameter = new SqlParameter("@HotelId", id);

                var reservations = reservationDbContext.Reservations
                    .FromSqlRaw("EXEC GetReservationsByHotelId @HotelId", hotelIdParameter)
                    .ToList();

                return reservations;
            }
        }

        public Entities.Reservation UpdateReservation(Entities.Reservation reservation)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var idParameter = new SqlParameter("@Id", reservation.Id);
                var hotelIdParameter = new SqlParameter("@HotelId", reservation.HotelId);
                var dateParameter = new SqlParameter("@Date", reservation.Date);
                var statusParameter = new SqlParameter("@Status", reservation.Status);

                reservationDbContext.Database.ExecuteSqlRaw(
                    "EXEC UpdateReservation @Id, @HotelId, @Date, @Status",
                    idParameter, hotelIdParameter, dateParameter, statusParameter);

                var updatedReservation = reservationDbContext.Reservations
                    .FirstOrDefault(r => r.Id == reservation.Id);

                return updatedReservation;
            }
        }
    }
}
