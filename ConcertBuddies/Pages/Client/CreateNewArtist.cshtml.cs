using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class CreateNewArtistModel : PageModel
    {
        public ArtistInfo newArtist = new ArtistInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            newArtist.name = Request.Form["artistName"];
            newArtist.bio = Request.Form["artistBio"];

            if (newArtist.name.Length == 0 || newArtist.bio.Length == 0)
            {
                errorMessage = "All fields in the form are required.";
                return;
            }

            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("InsertArtist", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter name = new SqlParameter
                        {
                            ParameterName = "@artistName",
                            Value = newArtist.name,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter bio = new SqlParameter
                        {
                            ParameterName = "@artistBio",
                            Value = newArtist.bio,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        // command.Parameters.AddWithValue("@name", newArtist.name);
                        // command.Parameters.AddWithValue("@bio", newArtist.bio);

                        // command.ExecuteNonQuery();
                        command.Parameters.Add(name);
                        command.Parameters.Add(bio);
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
