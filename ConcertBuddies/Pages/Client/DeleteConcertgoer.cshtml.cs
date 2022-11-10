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
    public class DeleteConcertgoerModel : PageModel
    {
        public ConcertgoerInfo newConcertgoer = new ConcertgoerInfo();
        public String errorMessage = "";
        public String successMessage = "";

        private readonly IConfiguration configuration;

        public DeleteConcertgoerModel(IConfiguration config)
        {
            configuration = config;
        }
        public void OnGet()
        {
           
        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);

            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("DeleteConcertgoer", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter ConcertgoerID = new SqlParameter
                        {
                            ParameterName = "@ConcertgoerID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        

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

            Response.Redirect("/Client/ConcertgoerList");
        }

    }
}
