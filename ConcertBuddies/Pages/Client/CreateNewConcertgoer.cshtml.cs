using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class CreateNewConcertgoerModel : PageModel
    {
        public ConcertgoerInfo newConcertgoer = new ConcertgoerInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            newConcertgoer.name = Request.Form["concertgoerName"];
            newConcertgoer.bio = Request.Form["concertgoerBio"];
            newConcertgoer.username = Request.Form["concertgoerUsername"];
            newConcertgoer.password = Request.Form["concertgoerPassword"];

            if (newConcertgoer.name.Length == 0 || newConcertgoer.bio.Length == 0 || newConcertgoer.username.Length == 0 || newConcertgoer.password.Length == 0)
            {
                errorMessage = "All fields in the form are required.";
                return;
            }

            // Save the new client in the database
            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("InsertConcertgoer", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter name = new SqlParameter
                        {
                            ParameterName = "@concertgoerName",
                            Value = newConcertgoer.name,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter bio = new SqlParameter
                        {
                            ParameterName = "@concertgoerBio",
                            Value = newConcertgoer.bio,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter username = new SqlParameter
                        {
                            ParameterName = "@username",
                            Value = newConcertgoer.username,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter password = new SqlParameter
                        {
                            ParameterName = "@password",
                            Value = newConcertgoer.password,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
 
                        command.Parameters.Add(name);
                        command.Parameters.Add(bio);
                        command.Parameters.Add(username);
                        command.Parameters.Add(password);
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
