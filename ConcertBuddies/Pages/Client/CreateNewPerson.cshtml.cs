using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace ConcertBuddies.Pages.Client
{
    public class CreateNewPersonModel : PageModel
    {
        public PersonInfo newPerson = new PersonInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }
        public void OnPost()
        {
            newPerson.name = Request.Form["personName"];
            newPerson.bio = Request.Form["personBio"];

            if (newPerson.name.Length == 0 || newPerson.bio.Length == 0)
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
                    using (SqlCommand command = new SqlCommand("InsertPerson", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter name = new SqlParameter
                        {
                            ParameterName = "@name",
                            Value = newPerson.name,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter bio = new SqlParameter
                        {
                            ParameterName = "@bio",
                            Value = newPerson.bio,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        // command.Parameters.AddWithValue("@name", newPerson.name);
                        // command.Parameters.AddWithValue("@bio", newPerson.bio);

                        // command.ExecuteNonQuery();
                        command.Parameters.Add(name);
                        command.Parameters.Add(bio);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Client/PeopleList");
        }
    }
}
