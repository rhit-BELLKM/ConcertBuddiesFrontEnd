using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System;

namespace ConcertBuddies.Pages.Client
{
    public class DeleteReviewModel : PageModel
    {
        public ReviewInfo review = new ReviewInfo();
        String errorMessage = "";
        String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            int concertID = Int32.Parse(HttpContext.Request.Query["id"]);
            int concertgoerID = Int32.Parse(HttpContext.Request.Query["concertgoerID"]);

            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteReviewed", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ConcertID = new SqlParameter
                        {
                            ParameterName = "@ConcertID",
                            Value = concertID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter ConcertgoerID = new SqlParameter
                        {
                            ParameterName = "@ConcertgoerID",
                            Value = concertgoerID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(ConcertID);
                        command.Parameters.Add(ConcertgoerID);
                        command.ExecuteNonQuery();
                    }


                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/ReviewList");
        }
    }
}
