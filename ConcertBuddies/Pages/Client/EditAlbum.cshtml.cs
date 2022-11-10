using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ConcertBuddies.Pages.Client
{
    public class EditAlbumModel : PageModel
    {
        public AlbumInfo newAlbum = new AlbumInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            int ID = Int32.Parse(HttpContext.Request.Query["id"]);
            newAlbum.name = Request.Form["AlbumName"];


            try
            {
                String connectionString = "Data Source=titan.csse.rose-hulman.edu;Initial Catalog=ConcertReviewSystem10;Persist Security Info=True;User ID=ConcertGroup;Password=UnluckyDucky_15";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("UpdateAlbum", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        SqlParameter AlbumID = new SqlParameter
                        {
                            ParameterName = "@AlbumID",
                            Value = ID,
                            SqlDbType = System.Data.SqlDbType.Int,
                            Direction = System.Data.ParameterDirection.Input
                        };
                        SqlParameter AlbumName = new SqlParameter
                        {
                            ParameterName = "@Name",
                            Value = newAlbum.name,
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input
                        };

                        command.Parameters.Add(AlbumID);
                        command.Parameters.Add(AlbumName);
                        command.ExecuteNonQuery();
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
