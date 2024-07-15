using Reservation.DataAccess.Abstract;
using Reservation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Reservation.DataAccess.Concrete
{
    public class ReservationRepository : IReservationRepository
    {
        public Entities.Reservation CreateReservation(Entities.Reservation reservation)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                reservationDbContext.Reservations.Add(reservation);
                reservationDbContext.SaveChanges();
                return reservation;
            }
        }

        public void DeleteReservation(int id)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var deletedReservation = GetReservationById(id);
                reservationDbContext.Remove(deletedReservation);
                reservationDbContext.SaveChanges();
            }
        }

        public List<Entities.Reservation> GetAllReservations()
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                return reservationDbContext.Reservations.ToList();
            }
        }

        public Entities.Reservation GetReservationById(int id)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var reservation = reservationDbContext.Reservations.Find(id);
                return reservation;
            }
        }

        public List <Entities.Reservation> GetReservationByStatus(bool status)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                return reservationDbContext.Reservations.Where(x => x.Status == status).ToList();
            }
        }

        public Entities.Reservation UpdateReservation(Entities.Reservation reservation)
        {
            using (var reservationDbContext = new ReservationDbContext())
            {
                var updatedReservation = GetReservationById(reservation.Id);
                reservationDbContext.Update(updatedReservation);
                reservationDbContext.SaveChanges();
                return updatedReservation;
            }
        }
    }
}
