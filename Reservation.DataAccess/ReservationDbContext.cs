using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reservation.Entities;

namespace Reservation.DataAccess
{
    public class ReservationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=localhost; Database=HotelDb;uid=sa;pwd=1234;");
        }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
