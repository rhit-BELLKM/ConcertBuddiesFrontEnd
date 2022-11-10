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
                    using (SqlCommand command = new SqlCommand("ReadTables", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter Identifier = new SqlParameter
                        {
                            ParameterName = "@Identifier",
                            Value = 2,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ArtistInfo artist = new ArtistInfo();
                            artist.ID = Convert.ToInt32(reader["ArtistID"]);
                            artist.name = reader["Name"].ToString();
                            artist.bio = reader["Bio"].ToString();


                            listArtist.Add(artist);
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
