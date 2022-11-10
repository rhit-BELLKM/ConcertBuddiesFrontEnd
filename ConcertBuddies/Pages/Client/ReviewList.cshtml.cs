using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConcertBuddies.Pages.Client
{
    public class ReviewListModel : PageModel
    {
        public List<ReviewInfo> listReview = new List<ReviewInfo>();
        String errorMessage = "";

        private readonly IConfiguration configuration;

        public ReviewListModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {
        
            try
            {
                // Establishes the connection to the database
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));

                // Creates connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("ReadTables", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter Identifier = new SqlParameter
                        {
                            ParameterName = "@Identifier",
                            Value = 7,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ReviewInfo review = new ReviewInfo();
                            review.concertID = 1;
                            review.concertgoerID = 1;
                            review.description = reader["description"].ToString();
                            review.rating = Convert.ToInt32(reader["rating"]);


                            listReview.Add(review);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
    public class ReviewInfo
    {
        public int concertID;
        public int concertgoerID;
        public String description;
        public int rating;
    }

}
