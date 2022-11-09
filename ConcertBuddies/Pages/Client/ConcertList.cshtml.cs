using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class ConcertListModel : PageModel
    {
        public List<ConcertInfo> listConcert = new List<ConcertInfo>();
        public void OnGet()
        {
            try
            {
                // Establishes the connection to the database
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";

                // Creates connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Concert";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ConcertInfo concert = new ConcertInfo();
                                concert.ID = reader.GetInt32(0);
                                concert.description = reader.GetString(1);
                                concert.location = reader.GetString(2);

                                // Adds new concert to the list of concerts.
                                listConcert.Add(concert);

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

    }
    public class ConcertInfo
    {
        public int ID;
        public String description;
        public String location;
    }
}
