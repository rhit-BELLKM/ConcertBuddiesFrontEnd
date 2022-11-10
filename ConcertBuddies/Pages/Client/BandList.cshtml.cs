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
    public class BandListModel : PageModel
    {
        public List<BandInfo> listBand = new List<BandInfo>();
<<<<<<< Updated upstream
        String errorMessage = "";
=======

        private readonly IConfiguration configuration;

        public BandListModel(IConfiguration config)
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
                            Value = 3,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            BandInfo band = new BandInfo();
                            band.ID = Convert.ToInt32(reader["BandID"]);
                            band.name = reader["Name"].ToString();


                            listBand.Add(band);
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
            List<BandInfo> imported = new List<BandInfo>();
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
                            var record = new BandInfo
                            {
                                name = csvReader.GetField("Name").ToString().Trim()
                            };
                            imported.Add((BandInfo)record); 
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
                    foreach (BandInfo importedBand in imported)
                    {
                        using (SqlCommand command = new SqlCommand("InsertBand", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter name = new SqlParameter
                            {
                                ParameterName = "@name",
                                Value = importedBand.name,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };



                            command.Parameters.Add(name);
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



            Response.Redirect("/Client/AlbumList");



        }
    }
    public class BandInfo
    {
        public int ID;
        public String name;
    }
}
