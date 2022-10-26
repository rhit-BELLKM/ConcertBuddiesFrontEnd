using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class BandListModel : PageModel
    {
        public List<BandInfo> listBand = new List<BandInfo>();
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
                    String sql = "SELECT * FROM Band";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                BandInfo band = new BandInfo();
                                band.ID = reader.GetInt32(0);
                                band.name = reader.GetString(1);

                                // Adds new concert to the list of concerts.
                                listBand.Add(band);

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
    public class BandInfo
    {
        public int ID;
        public String name;
    }
}
