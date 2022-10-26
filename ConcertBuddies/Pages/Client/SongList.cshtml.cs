using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class SongListModel : PageModel
    {
        public List<SongInfo> listSong = new List<SongInfo>();
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
                    String sql = "SELECT * FROM Song";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SongInfo song = new SongInfo();
                                song.ID = reader.GetInt32(0);
                                song.name = reader.GetString(1);
                                // song.album = reader.GetString(2);
                                song.genre = reader.GetString(3);

                                // Adds new song to the list of songs.
                                listSong.Add(song);

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

    public class SongInfo
    {
        public int ID;
        public String name;
        public String album;
        public String genre;
    }
}
