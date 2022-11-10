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
                    using (SqlCommand command = new SqlCommand("ReadTables", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter Identifier = new SqlParameter
                        {
                            ParameterName = "@Identifier",
                            Value = 6,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            SongInfo song = new SongInfo();
                            song.ID = Convert.ToInt32(reader["SongID"]);
                            song.name = reader["Name"].ToString();
                            song.album = reader["Album"].ToString();
                            song.genre = reader["Genre"].ToString();


                            listSong.Add(song);
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
