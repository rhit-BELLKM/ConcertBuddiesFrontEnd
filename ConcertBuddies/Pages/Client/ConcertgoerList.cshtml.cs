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
                    connection.Open();
                    String sql = "SELECT * FROM Concertgoer";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ConcertgoerInfo concertgoer = new ConcertgoerInfo();
                                concertgoer.ID = reader.GetInt32(0);
                                // concertgoer.name = reader.GetString(1);
                                // concertgoer.bio = reader.GetString(2);
                                concertgoer.username = reader.GetString(1);
                                concertgoer.password = reader.GetString(2);

                                // Adds new concertgoer to the list of people.
                                listConcertgoer.Add(concertgoer);

                            }
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
    public class ConcertgoerInfo
    {
        public int ID;
        public String name;
        public String bio;
        public String username;
        public String password;
    }
}
