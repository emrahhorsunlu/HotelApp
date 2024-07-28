using HotelFinder.Business.Abstract;
using HotelFinder.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HotelFinder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private IHotelService _hotelService;

        public HotelsController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_hotelService.GetAllHotels()); // 200 + Data
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetHotelById(int id)
        {
            var hotel = _hotelService.GetHotelById(id);
            if (hotel != null)
                return Ok(hotel); // 200 + Data
            
            return NotFound();
        }

        [HttpGet("[action]/{name}")]
        public IActionResult GetHotelByName(string name)
        {
            var hotel = _hotelService.GetHotelByName(name);
            if(hotel != null)
                return Ok(hotel); // 200 + data
            
            return NotFound();
        }

        [HttpPost("create")]
        public IActionResult CreateHotel([FromBody]Hotel hotel)
        {
             var createdHotel = _hotelService.CreateHotel(hotel);
             return CreatedAtAction("Get", new { id=createdHotel.Id },createdHotel); // 201 + Created Hotel ID in Header
        }

        [HttpPost("update")]
        public IActionResult UpdateHotel([FromBody] Hotel hotel)
        {
            if(_hotelService.GetHotelById(hotel.Id) != null)
                return Ok(_hotelService.UpdateHotel(hotel)); // 200 + data

            return NotFound();
        }

        [HttpGet("{id}/delete")]
        public IActionResult DeleteHotel(int id)
        {
            if(_hotelService.GetHotelById(id) != null)
            {
                _hotelService.DeleteHotel(id);
                return Ok(); // 200
            }
            return NotFound();
        }

    }
}
