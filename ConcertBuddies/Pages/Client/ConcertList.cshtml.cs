using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ConcertBuddies.Pages.Client
{
    public class ConcertListModel : PageModel
    {
        public List<ConcertInfo> listConcert = new List<ConcertInfo>();
<<<<<<< Updated upstream
        String errorMessage = "";
=======

        private readonly IConfiguration configuration;

        public ConcertListModel(IConfiguration config)
        {
            configuration = config;
        }
>>>>>>> Stashed changes
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
                            Value = 4,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ConcertInfo concert = new ConcertInfo();
                            concert.ID = Convert.ToInt32(reader["ConcertID"]);
                            concert.description = reader["description"].ToString();
                            concert.location = reader["location"].ToString();


                            listConcert.Add(concert);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void OnPost()
        {
            IFormFile file = Request.Form.Files[0];
            List<ConcertInfo> imported = new List<ConcertInfo>();
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
                            var record = new ConcertInfo
                            {
                                description = csvReader.GetField("Description").ToString().Trim(),
                                location = csvReader.GetField("Location").ToString().Trim()
                            };
                            imported.Add((ConcertInfo)record); 
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
                    foreach (ConcertInfo importedConcert in imported)
                    {
                        using (SqlCommand command = new SqlCommand("InsertConcert", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter description = new SqlParameter
                            {
                                ParameterName = "@description",
                                Value = importedConcert.description,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter location = new SqlParameter
                            {
                                ParameterName = "@location",
                                Value = importedConcert.location,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };

                            command.Parameters.Add(description);
                            command.Parameters.Add(location);
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



            Response.Redirect("/Client/ConcertList");



        }

    }
    public class ConcertInfo
    {
        public int ID;
        public String description;
        public String location;
    }
}
