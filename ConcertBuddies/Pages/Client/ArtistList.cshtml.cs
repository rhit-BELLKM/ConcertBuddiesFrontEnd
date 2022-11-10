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
    public class ArtistListModel : PageModel
    {
        public List<ArtistInfo> listArtist = new List<ArtistInfo>();
        String errorMessage = "";

        private readonly IConfiguration configuration;

        public ArtistListModel(IConfiguration config)
        {
            configuration = config;
        }
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
                            Value = 2,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            ArtistInfo artist = new ArtistInfo();
                            artist.ID = Convert.ToInt32(reader["ArtistID"]);
                            artist.name = reader["Name"].ToString();
                            artist.bio = reader["Bio"].ToString();


                            listArtist.Add(artist);
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
            List<ArtistInfo> imported = new List<ArtistInfo>();
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
                            var record = new ArtistInfo
                            {
                                name = csvReader.GetField("Name").ToString().Trim(),
                                bio = csvReader.GetField("Bio").ToString().Trim()
                            };
                            imported.Add((ArtistInfo)record); 
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
                    foreach (ArtistInfo importedArtist in imported)
                    {
                        using (SqlCommand command = new SqlCommand("InsertArtist", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter name = new SqlParameter
                            {
                                ParameterName = "@artistName",
                                Value = importedArtist.name,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter bio = new SqlParameter
                            {
                                ParameterName = "@artistBio",
                                Value = importedArtist.bio,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };

                            command.Parameters.Add(name);
                            command.Parameters.Add(bio);
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



            Response.Redirect("/Client/ArtistList");



        }
    }

    public class ArtistInfo
    {
        public int ID;
        public String name;
        public String bio;
    }
}
