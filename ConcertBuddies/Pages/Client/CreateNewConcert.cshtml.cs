using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class CreateNewConcertModel : PageModel
    {
        public ConcertInfo newConcert = new ConcertInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            newConcert.description = Request.Form["concertDescription"];
            newConcert.location = Request.Form["concertLocation"];

            if (newConcert.description.Length == 0 || newConcert.location.Length == 0)
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
                    using (SqlCommand command = new SqlCommand("InsertConcert", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
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
