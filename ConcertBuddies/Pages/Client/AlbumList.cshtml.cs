using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.SS.UserModel;

namespace ConcertBuddies.Pages.Client
{
    public class AlbumListModel : PageModel
    {
        public List<AlbumInfo> listAlbum = new List<AlbumInfo>();
        public void OnGet()
        {
            try
            {
                // Establishes the connection to the database
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";

                // Creates connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("ReadTables", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Identifier", 1);
                    connection.Open();
                    command.ExecuteNonQuery();

                    SqlDataReader reader = command.ExecuteReader();
                   
                    while(reader.Read())
                    {
                        AlbumInfo album = new AlbumInfo();
                        album.ID = Convert.ToInt32(reader["AlbumID"]);
                        album.name = reader["Name"].ToString();

                        listAlbum.Add(album);
                  
                    }


                  
                }
            }
            catch (Exception e)
            {
                return;
            }

        }


     

        //public void OnPost()
        //{
        //    try
        //    {
        //        // Establishes the connection to the database
        //        String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";

        //        // Creates connection
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            SqlCommand command = new SqlCommand("ReadTables", connection);
        //            command.CommandType = CommandType.StoredProcedure;
        //            connection.Open();
        //            SqlDataReader reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                AlbumInfo album = new AlbumInfo();
        //                album.name = reader["Name"].ToString();
        //                Console.WriteLine(reader["Name"].ToString());

        //                listAlbum.Add(album);
        //                Console.WriteLine(listAlbum);

        //            }

        //            command.Parameters.AddWithValue("@Identifier", 1);
        //            command.ExecuteNonQuery();

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return;
        //    }

        //}
    
    }

}
    public class AlbumInfo
    {
        public int ID;
        public String name;
    }

