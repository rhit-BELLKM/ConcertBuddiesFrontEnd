using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class IndexModel : PageModel
    {
        public List<PersonInfo> listPeople = new List<PersonInfo>();
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
                    String sql = "SELECT * FROM Person"; 
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PersonInfo person = new PersonInfo();
                                person.ID = reader.GetInt32(0);
                                person.name = reader.GetString(1);
                                person.bio = reader.GetString(2);

                                // Adds new person to the list of people.
                                listPeople.Add(person);

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

    public class PersonInfo
    {
        public int ID;
        public String name;
        public String bio;
    }
}
