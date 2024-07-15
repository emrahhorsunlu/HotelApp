using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reservation.Business.Abstract;
using Reservation.Entities;

namespace Reservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public IActionResult GeAllReservations()
        {
            return Ok(_reservationService.GetAllReservations()); // 200 + Data
        }

        [HttpGet("{id}")]
        public IActionResult GetReservationByID(int id) 
        { 
            var result = _reservationService.GetReservationById(id);
            if(result != null)
            {
                return Ok(result); // 200 + data
            }
            return NotFound(); // 404
        }

        [HttpPost("update")]
        public IActionResult UpdateReservation([FromBody] Reservation.Entities.Reservation reservation)
        {
            var result = _reservationService.GetReservationById(reservation.Id); 
            if(result != null) 
            { 
               return Ok(_reservationService.UpdateReservation(result)); // 200 + data
            }
            return NotFound();
        }

        [HttpPost("create")]
        public IActionResult CreateReservation([FromBody] Reservation.Entities.Reservation reservation)
        {
            var createdReservation = _reservationService.CreateReservation(reservation);
            return CreatedAtAction("GetReservationByID", new { id = createdReservation.Id }, createdReservation); // 201 + Created Reservation ID in Header
        }

        [HttpGet("{id}/delete")]
        public IActionResult DeleteReservation(int id)
        {
            var deletedReservation = _reservationService.GetReservationById(id);
            if(deletedReservation != null)
            {
                _reservationService.DeleteReservation(id);
                return Ok();
            }
            return NotFound();
        }
    }
}
