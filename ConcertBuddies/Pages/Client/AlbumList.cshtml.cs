using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using static NPOI.HSSF.Util.HSSFColor;

namespace ConcertBuddies.Pages.Client
{
    public class AlbumListModel : PageModel
    {
        public List<AlbumInfo> listAlbum = new List<AlbumInfo>();
        public IFormFile file = null;
        public String errorMessage = "";
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
                                Value = 1,
                                SqlDbType = System.Data.SqlDbType.Int,
                                Direction = System.Data.ParameterDirection.Input
                            };

                            command.Parameters.Add(Identifier);
                            command.ExecuteNonQuery();

                            SqlDataReader reader = command.ExecuteReader();

                            while (reader.Read())
                            {
                                AlbumInfo album = new AlbumInfo();
                                album.ID = Convert.ToInt32(reader["albumID"]);
                                album.name = reader["Name"].ToString();

                                listAlbum.Add(album);
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
            List<AlbumInfo> imported = new List<AlbumInfo>();
            using (Stream stream = file.OpenReadStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    using(CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Read();
                        csvReader.ReadHeader();

                        while(csvReader.Read())
                        {
                            var record = new AlbumInfo
                            {
                                name = csvReader.GetField("Name").ToString().Trim()
                            };
                            imported.Add((AlbumInfo)record); // WORKS UP UNTIL HERE, NEED TO RUN SPROC TO ADD TO DATABASE
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
                    foreach (AlbumInfo importedAlbum in imported)
                    {
                        using (SqlCommand command = new SqlCommand("InsertAlbum", connection))
                        {
                            command.CommandType = System.Data.CommandType.StoredProcedure;
                            SqlParameter name = new SqlParameter
                            {
                                ParameterName = "@name",
                                Value = importedAlbum.name,
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

}
    public class AlbumInfo
    {
        public int ID;
        public String name;
    }

