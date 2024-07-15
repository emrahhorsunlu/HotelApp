using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using HotelFinder.Entities;

namespace Reservation.Entities
{
    public class Reservation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public Hotel Hotel { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
