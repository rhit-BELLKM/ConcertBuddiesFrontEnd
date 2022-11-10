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
    public class EditConcertgoerModel : PageModel
    {
        public ConcertgoerInfo newConcertgoer = new ConcertgoerInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration configuration;

        public EditConcertgoerModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {

        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);
            newConcertgoer.name = Request.Form["concertgoerName"];
            newConcertgoer.bio = Request.Form["concertgoerBio"];
            newConcertgoer.username = Request.Form["concertgoerUsername"];


            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateConcertgoer", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ConcertgoerID = new SqlParameter
                        {
                            ParameterName = "@ConcertgoerID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter ConcertgoerName = new SqlParameter
                        {
                            ParameterName = "@ConcertgoerName",
                            Value = newConcertgoer.name,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter ConcertgoerBio = new SqlParameter
                        {
                            ParameterName = "@ConcertgoerBio",
                            Value = newConcertgoer.bio,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter Username = new SqlParameter
                        {
                            ParameterName = "@Username",
                            Value = newConcertgoer.username,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(ConcertgoerID);
                        command.Parameters.Add(ConcertgoerName);
                        command.Parameters.Add(ConcertgoerBio);
                        command.Parameters.Add(Username);
                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/ConcertgoerList");
        }

    }
}
