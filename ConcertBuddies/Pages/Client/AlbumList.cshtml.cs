using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class AlbumListModel : PageModel
    {
        public List<AlbumInfo> listAlbum = new List<AlbumInfo>();
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
                    String sql = "SELECT * FROM Album"; 
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                AlbumInfo album = new AlbumInfo();
                                album.ID = reader.GetInt32(0);
                                album.name = reader.GetString(1);

                                // Adds new person to the list of people.
                                listAlbum.Add(album);

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
}
    public class AlbumInfo
    {
        public int ID;
        public String name;
    }

