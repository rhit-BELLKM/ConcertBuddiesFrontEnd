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
    public class EditConcertModel : PageModel
    {
        public ConcertInfo newConcert = new ConcertInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration configuration;

        public EditConcertModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);
            newConcert.description = Request.Form["concertDescription"];
            newConcert.location = Request.Form["concertLocation"];

            if (newConcert.description.Length == 0 || newConcert.location.Length == 0)
            {
                errorMessage = "All fields in the form are required.";
                return;
            }

            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("UpdateConcert", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ConcertgoerID = new SqlParameter
                        {
                            ParameterName = "@ConcertID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter description = new SqlParameter
                        {
                            ParameterName = "@description",
                            Value = newConcert.description,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter location = new SqlParameter
                        {
                            ParameterName = "@location",
                            Value = newConcert.location,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(ConcertgoerID);
                        command.Parameters.Add(description);
                        command.Parameters.Add(location);
                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/ConcertList");
        }

    }
}
