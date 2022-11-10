using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ConcertBuddies.Pages.Client
{
    public class CreateNewSongModel : PageModel
    {
        public SongInfo newSong = new SongInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration configuration;

        public CreateNewSongModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {
        }
        public void OnPost()
        {
            newSong.name = Request.Form["songName"];
            newSong.album = Request.Form["songAlbum"];
            newSong.genre = Request.Form["songGenre"];


            if (newSong.name.Length == 0 || newSong.album.Length == 0 || newSong.genre.Length == 0)
            {
                errorMessage = "All fields in the form are required.";
                return;
            }

            // Save the new client in the database
            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("InsertSong", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter name = new SqlParameter
                        {
                            ParameterName = "@name",
                            Value = newSong.name,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter album = new SqlParameter
                        {
                            ParameterName = "@albumID",
                            Value = Int32.Parse(newSong.album),
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter genre = new SqlParameter
                        {
                            ParameterName = "@genre",
                            Value = newSong.genre,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };


                        command.Parameters.Add(name);
                        command.Parameters.Add(album);
                        command.Parameters.Add(genre);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/SongList");
        }

    }
}
