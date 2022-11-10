using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;

namespace ConcertBuddies.Pages.Client
{
    public class CreateNewReviewModel : PageModel
    {
        public ReviewInfo newReview = new ReviewInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {
            int concertID = 1;
            int concertgoerID = 90;
            newReview.description = Request.Form["reviewDescription"];
            newReview.rating = Convert.ToInt32(Request.Form["reviewRating"]);
            if (newReview.description.Length == 0)
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
                    using (SqlCommand command = new SqlCommand("InsertReview", connection))
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
                        SqlParameter Description = new SqlParameter
                        {
                            ParameterName = "@description",
                            Value = newReview.description,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter Rating = new SqlParameter
                        {
                            ParameterName = "@rating",
                            Value = newReview.rating,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(ConcertID);
                        command.Parameters.Add(ConcertgoerID);
                        command.Parameters.Add(Description);
                        command.Parameters.Add(Rating);
                        command.ExecuteNonQuery();
                    }
                }

            } catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/ReviewList");
        }
    }
}
