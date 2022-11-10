using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class EditSongModel : PageModel
    {
        public SongInfo newSong = new SongInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);
            newSong.name = Request.Form["SongName"];
            newSong.album = Request.Form["SongAlbum"];
            newSong.genre = Request.Form["SongGenre"];


            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateSong", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter SongID = new SqlParameter
                        {
                            ParameterName = "@SongID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter SongName = new SqlParameter
                        {
                            ParameterName = "@Name",
                            Value = newSong.name,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter SongAlbum = new SqlParameter
                        {
                            ParameterName = "@Album",
                            Value = Int32.Parse(newSong.album),
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter SongGenre = new SqlParameter
                        {
                            ParameterName = "@Genre",
                            Value = newSong.genre,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(SongID);
                        command.Parameters.Add(SongName);
                        command.Parameters.Add(SongAlbum);
                        command.Parameters.Add(SongGenre);
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
