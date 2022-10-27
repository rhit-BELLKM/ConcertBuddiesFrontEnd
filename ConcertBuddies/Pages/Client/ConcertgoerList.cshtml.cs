using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class ConcertgoerListModel : PageModel
    {
        public List<ConcertgoerInfo> listConcertgoer = new List<ConcertgoerInfo>();
        public void OnGet()
        {
            try
            {
                // Establishes the connection to the database
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";

                // Creates connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("ReadTables", connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Identifier", 5);
                    connection.Open();
                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();
                    
                    while(reader.Read())
                    {
                        ConcertgoerInfo concertgoer = new ConcertgoerInfo();
                        concertgoer.ID = Convert.ToInt32(reader["ConcertgoerID"]);
                        concertgoer.name = reader["Name"].ToString();
                        concertgoer.bio = reader["Bio"].ToString();
                        concertgoer.username = reader["Username"].ToString();
                        concertgoer.password = reader["Password"].ToString();


                        listConcertgoer.Add(concertgoer);
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }
    }
    public class ConcertgoerInfo
    {
        public int ID;
        public String name;
        public String bio;
        public String username;
        public String password;
    }
}
