using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ConcertBuddies.Pages.Client
{
    public class ReviewListModel : PageModel
    {
        public List<ReviewInfo> listReview = new List<ReviewInfo>();
        public void OnGet()
        {
        
            try
            {
                // Establishes the connection to the database
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";

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
                            review.concertgoerID = 90;
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
