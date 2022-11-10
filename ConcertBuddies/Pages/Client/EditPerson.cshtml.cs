using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class EditPersonModel : PageModel
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

            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=bellkm;Password=K33lan01!"; // TODO: Pls get rid of password pls dear lord heaven above
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "UPDATE Person " +
                                 "SET Name = @name, Bio = @bio " +
                                 "WHERE ID = @ID"; // TODO: Replace this with a call to a stored procedure.
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ID", newPerson.ID);
                        command.Parameters.AddWithValue("@name", newPerson.name);
                        command.Parameters.AddWithValue("@bio", newPerson.bio);

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
