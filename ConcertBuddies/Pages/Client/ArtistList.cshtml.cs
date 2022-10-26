using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class ArtistListModel : PageModel
    {
        public List<ArtistInfo> listArtist = new List<ArtistInfo>();
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
                    String sql = "SELECT * FROM Artist";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ArtistInfo artist = new ArtistInfo();
                                artist.ID = reader.GetInt32(0);
                                // artist.name = reader.GetString(1);
                                // artist.bio = reader.GetString(2);

                                // Adds new artist to the list of artists.
                                listArtist.Add(artist);

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

    public class ArtistInfo
    {
        public int ID;
        public String name;
        public String bio;
    }
}
