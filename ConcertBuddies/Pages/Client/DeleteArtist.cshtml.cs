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
    public class DeleteArtistModel : PageModel
    {
        public ArtistInfo newArtist = new ArtistInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration configuration;

        public DeleteArtistModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {
           
        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);

            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteArtist", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ArtistID = new SqlParameter
                        {
                            ParameterName = "@ArtistID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        

                        command.Parameters.Add(ArtistID);
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
