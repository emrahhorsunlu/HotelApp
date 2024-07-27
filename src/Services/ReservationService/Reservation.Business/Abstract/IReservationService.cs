using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation.Business.Abstract
{
    public interface IReservationService
    {
        List<Entities.Reservation> GetAllReservations();
        Entities.Reservation GetReservationById(int id);
        List<Entities.Reservation> GetReservationByStatus(bool status);
        Entities.Reservation CreateReservation(Entities.Reservation reservation);
        Entities.Reservation UpdateReservation(Entities.Reservation reservation);
        void DeleteReservation(int id);
    }
}
