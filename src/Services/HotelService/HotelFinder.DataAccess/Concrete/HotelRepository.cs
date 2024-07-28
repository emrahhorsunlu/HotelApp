using HotelFinder.DataAccess.Abstract;
using HotelFinder.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelFinder.DataAccess.Concrete
{
    public class HotelRepository : IHotelRepository
    {
        
        public Hotel CreateHotel(Hotel hotel)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                var command = hotelDbContext.Database.GetDbConnection().CreateCommand();
                command.CommandText = "CreateHotel";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Name", hotel.Name));
                command.Parameters.Add(new SqlParameter("@City", hotel.City));
                var newHotelIdParameter = new SqlParameter
                {
                    ParameterName = "@NewHotelId",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Output
                };
                command.Parameters.Add(newHotelIdParameter);

                hotelDbContext.Database.OpenConnection();
                command.ExecuteNonQuery();

                hotel.Id = (int)newHotelIdParameter.Value;

                return hotel;
            }
        }

        public void DeleteHotel(int id)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                hotelDbContext.Database.ExecuteSqlInterpolated($"EXEC DeleteHotelById {id}");
            }
        }

        public List<Hotel> GetAllHotels()
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                var hotels = hotelDbContext.Hotels
                                           .FromSqlInterpolated($"EXEC GetAllHotels")
                                           .ToList();
                return hotels;
            }
        }

        public Hotel GetHotelById(int id)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                var hotels = hotelDbContext.Hotels
                                           .FromSqlInterpolated($"EXEC GetHotelById {id}")
                                           .AsEnumerable();

                var hotel = hotels.FirstOrDefault();
                return hotel;
            }
        }

        public Hotel GetHotelByName(string name)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                var hotels = hotelDbContext.Hotels
                                           .FromSqlInterpolated($"EXEC GetHotelByName {name}")
                                           .AsEnumerable();

                var hotel = hotels.FirstOrDefault();
                return hotel;
            }
        }

        public Hotel UpdateHotel(Hotel hotel)
        {
            using (var hotelDbContext = new HotelDbContext())
            {
                var idParameter = new SqlParameter("@Id", hotel.Id);
                var nameParameter = new SqlParameter("@Name", hotel.Name);
                var cityParameter = new SqlParameter("@City", hotel.City);

                hotelDbContext.Database.ExecuteSqlRaw(
                    "EXEC UpdateHotel @Id, @Name, @City",
                    idParameter, nameParameter, cityParameter);

                return hotel;
            }
        }
    }
}
