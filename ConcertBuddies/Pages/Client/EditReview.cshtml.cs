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
    public class EditReviewModel : PageModel
    {
        public ReviewInfo newReview = new ReviewInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration configuration;

        public EditReviewModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {

        }

        public void OnPost()
        {
            int concertgoerID = Int32.Parse(HttpContext.Request.Query["concertgoerid"]);
            int concertID = Int32.Parse(HttpContext.Request.Query["concertid"]);
            newReview.description = Request.Form["description"];
            newReview.rating = Int32.Parse(Request.Form["rating"]);



            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateReview", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ConcertgoerID = new SqlParameter
                        {
                            ParameterName = "@ConcertID",
                            Value = concertID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter ConcertID = new SqlParameter
                        {
                            ParameterName = "@ConcertgoerID",
                            Value = concertgoerID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter Description = new SqlParameter
                        {
                            ParameterName = "@Description",
                            Value = newReview.description,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter Rating = new SqlParameter
                        {
                            ParameterName = "@Rating",
                            Value = newReview.rating,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(ConcertID);
                        command.Parameters.Add(ConcertgoerID);
                        command.Parameters.Add(Description);
                        command.Parameters.Add(Rating);
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
