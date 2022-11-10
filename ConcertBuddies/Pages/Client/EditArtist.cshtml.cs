using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class EditArtistModel : PageModel
    {
        public ArtistInfo newArtist = new ArtistInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);
            newArtist.name = Request.Form["ArtistName"];
            newArtist.bio = Request.Form["ArtistBio"];

            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateArtist", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ArtistID = new SqlParameter
                        {
                            ParameterName = "@ArtistID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter ArtistName = new SqlParameter
                        {
                            ParameterName = "@ArtistName",
                            Value = newArtist.name,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter ArtistBio = new SqlParameter
                        {
                            ParameterName = "@ArtistBio",
                            Value = newArtist.bio,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(ArtistID);
                        command.Parameters.Add(ArtistName);
                        command.Parameters.Add(ArtistBio);
                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/ArtistList");
        }

    }
}
