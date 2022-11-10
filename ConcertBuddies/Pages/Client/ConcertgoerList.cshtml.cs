using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class ConcertgoerListModel : PageModel
    {
        public List<ConcertgoerInfo> listConcertgoer = new List<ConcertgoerInfo>();
        String errorMessage = "";
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
                            Value = 5,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ConcertgoerInfo concertgoer = new ConcertgoerInfo();
                            concertgoer.ID = Convert.ToInt32(reader["ConcertgoerID"]);
                            concertgoer.name = reader["Name"].ToString();
                            concertgoer.bio = reader["Bio"].ToString();
                            concertgoer.username = reader["Username"].ToString();


                            listConcertgoer.Add(concertgoer);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

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
        public void OnPost()
        {
            IFormFile file = Request.Form.Files[0];
            List<ConcertgoerInfo> imported = new List<ConcertgoerInfo>();
            using (Stream stream = file.OpenReadStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Read();
                        csvReader.ReadHeader();



                        while (csvReader.Read())
                        {
                            var record = new ConcertgoerInfo
                            {
                                name = csvReader.GetField("Name").ToString().Trim(),
                                bio = csvReader.GetField("Bio").ToString().Trim(),
                                username = csvReader.GetField("Username").ToString().Trim(),
                                password = csvReader.GetField("Password").ToString().Trim()
                            };
                            imported.Add((ConcertgoerInfo)record); 
                        }
                    }
                }
            }
            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (ConcertgoerInfo importedConcertgoer in imported)
                    {
                        using (SqlCommand command = new SqlCommand("InsertConcertgoer", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter name = new SqlParameter
                            {
                                ParameterName = "@concertgoerName",
                                Value = importedConcertgoer.name,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter bio = new SqlParameter
                            {
                                ParameterName = "@concertgoerBio",
                                Value = importedConcertgoer.bio,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter username = new SqlParameter
                            {
                                ParameterName = "@username",
                                Value = importedConcertgoer.username,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter password = new SqlParameter
                            {
                                ParameterName = "@password",
                                Value = getHash(importedConcertgoer.password + getSalt()),
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter salt = new SqlParameter
                            {
                                ParameterName = "@PasswordSalt",
                                Value = getSalt(),
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };

                            command.Parameters.Add(name);
                            command.Parameters.Add(bio);
                            command.Parameters.Add(username);
                            command.Parameters.Add(password);
                            command.Parameters.Add(salt);
                            command.ExecuteNonQuery();
                        }

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
    public class ConcertgoerInfo
    {
        public int ID;
        public String name;
        public String bio;
        public String username;
        public String password;
    }
}
