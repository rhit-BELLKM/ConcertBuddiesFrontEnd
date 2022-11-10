using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public AlbumInfo Album {get; set;}
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

        public async void OnPost(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                errorMessage = "Please select a file";
                return;
            }

            string fileExtension = Path.GetExtension(file.FileName);
            if (fileExtension != ".xls" && fileExtension != ".xlsx")
            {
                errorMessage = "File must be .xls/.xlsx format.";
                return;
            }

            var rootFolder = @"C:\Desktop";
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0];
                //var workSheet = package.Workbook.Worksheets.First();
                int totalRows = workSheet.Dimension.Rows;

                var DataList = new List<AlbumInfo>();

                for (int i = 2; i <= totalRows; i++)
                {
                    DataList.Add(new AlbumInfo(workSheet.Cells[i, 1].Value.ToString().Trim()));
                }
                System.Diagnostics.Debug.Write(DataList);
            }
            Response.Redirect("/Client/AlbumList");
        }

    }

}
    public class AlbumInfo
    {
        public AlbumInfo()
        {

        }
        public AlbumInfo(String passedName)
        {
        this.name = passedName;
        }

        public int ID;
        public String name;
    }

