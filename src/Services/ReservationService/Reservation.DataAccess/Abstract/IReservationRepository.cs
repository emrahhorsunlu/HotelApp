using System.Collections.Generic;

namespace Reservation.DataAccess.Abstract
{
    
    public interface IReservationRepository
    {
        List<Entities.Reservation> GetAllReservations();
        List<Entities.Reservation> GetReservationsByHotelId(int id);
        Entities.Reservation GetReservationById(int id);
        List<Entities.Reservation> GetReservationByStatus(bool status);
        Entities.Reservation CreateReservation(Entities.Reservation reservation);
        Entities.Reservation UpdateReservation(Entities.Reservation reservation);
        void DeleteReservation(int id);
    }
}
