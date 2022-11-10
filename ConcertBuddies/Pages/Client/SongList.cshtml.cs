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

namespace ConcertBuddies.Pages.Client
{
    public class SongListModel : PageModel
    {
        public List<SongInfo> listSong = new List<SongInfo>();
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
                            Value = 6,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(Identifier);
                        command.ExecuteNonQuery();

                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            SongInfo song = new SongInfo();
                            song.ID = Convert.ToInt32(reader["SongID"]);
                            song.name = reader["Name"].ToString();
                            song.album = reader["Album"].ToString();
                            song.genre = reader["Genre"].ToString();


                            listSong.Add(song);
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
            List<SongInfo> imported = new List<SongInfo>();
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
                            var record = new SongInfo
                            {
                                name = csvReader.GetField("Name").ToString().Trim(),
                                album = csvReader.GetField("Album").ToString().Trim(),
                                genre = csvReader.GetField("Genre").ToString().Trim()

                            };
                            imported.Add((SongInfo)record); 
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
                    foreach (SongInfo importedSong in imported)
                    {
                        using (SqlCommand command = new SqlCommand("InsertSong", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter name = new SqlParameter
                            {
                                ParameterName = "@name",
                                Value = importedSong.name,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter album = new SqlParameter
                            {
                                ParameterName = "@albumID",
                                Value = Int32.Parse(importedSong.album),
                                SqlDbType = System.Data.SqlDbType.Int,
                                Direction = System.Data.ParameterDirection.Input
                            };
                            SqlParameter genre = new SqlParameter
                            {
                                ParameterName = "@genre",
                                Value = importedSong.genre,
                                SqlDbType = System.Data.SqlDbType.NVarChar,
                                Direction = System.Data.ParameterDirection.Input
                            };

                            command.Parameters.Add(name);
                            command.Parameters.Add(album);
                            command.Parameters.Add(genre);
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



            Response.Redirect("/Client/SongList");



        }
    }

    public class SongInfo
    {
        public int ID;
        public String name;
        public String album;
        public String genre;
    }
}
