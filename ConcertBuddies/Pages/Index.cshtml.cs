using ConcertBuddies.Pages.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace ConcertBuddies.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly ILogger<IndexModel> _logger;

        //public IndexModel(ILogger<IndexModel> logger)
        //{
        //    _logger = logger;
        //}


        private static string getHash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private static string getSalt()
        {
            byte[] bytes = new byte[16];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private readonly IConfiguration configuration;
        public IndexModel(IConfiguration config)
        {
            configuration = config;
        }

        public List<ConcertgoerInfo> listConcertgoer = new List<ConcertgoerInfo>();

        //public ConcertgoerInfo newConcertgoer = new ConcertgoerInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            string inputUsername = Request.Form["concertgoerUsername"];
            string inputPassword = Request.Form["concertgoerPassword"];
            String newConcertgoersalt = getSalt();

            //if (newConcertgoer.name.Length == 0 || newConcertgoer.bio.Length == 0 || newConcertgoer.username.Length == 0 || newConcertgoer.password.Length == 0)
            //{
            //    errorMessage = "All fields in the form are required.";
            //    return;
            //}

            // Save the new client in the database
            try
            {
                String connectionString = AESService.Decrypt(configuration.GetConnectionString("DefaultConnection"));
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Login", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter username = new SqlParameter
                        {
                            ParameterName = "@Username",
                            Value = inputUsername,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter password = new SqlParameter
                        {
                            ParameterName = "@password",
                            Value = inputPassword,
                            SqlDbType = System.Data.SqlDbType.NVarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };



                        command.Parameters.Add(username);
                        command.Parameters.Add(password);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        string outputSalt = "";
                        string outputPassword = "";

                        while (reader.Read())
                        {
                            outputSalt = reader["PasswordSalt"].ToString();
                            outputPassword = reader["PasswordHash"].ToString();
                        }

                        if (outputSalt.IsEmpty() || outputPassword.IsEmpty())
                        {
                            throw new Exception("Login Failed: Username or Password is Incorrect");
                        }
                        else
                        {
                            //if(getHash((inputPassword + outputSalt)).Equals(outputPassword))
                            if(String.Equals(getHash((inputPassword + outputSalt)), outputPassword))
                            {
                                successMessage = "Login Sucessful";
                                Response.Redirect("/Client/Home?user=" + inputUsername);
                            }
                            else
                            {
                                throw new Exception("output: " + outputPassword + "   input: " + getHash((inputPassword + outputSalt)));
                            }
                        }
                    }

                }
            }
            catch (Exception e)
            {
                errorMessage = e.Message;
                return;
            }

            Response.Redirect("/Home?user=" + inputUsername);
        }
    }
}
