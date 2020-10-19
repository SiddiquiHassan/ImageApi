using ImgImpExp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ImgImpExp.Controllers
{
    [RoutePrefix("api/images")]
    public class ImagesController : ApiController
    {
        readonly SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        SqlCommand sqlCommand;
        DataSet dataSet;

        public ImagesController()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
        }
        // GET: api/Images
        [Route("")]
        public IHttpActionResult GetImages()
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand("select * from tbl_Image");
            sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            dataSet = new DataSet();

            try
            {
                sqlDataAdapter.Fill(dataSet);
                if (dataSet == null)
                    return NotFound();
                else
                    return Ok(dataSet);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlCommand.Dispose();
                sqlDataAdapter.Dispose();
                dataSet.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        // GET: api/Images/5
        [Route("{id}")]
        public IHttpActionResult GetImages(int id)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand("select * from tbl_Image where Id =" + id);
            sqlDataAdapter = new SqlDataAdapter(sqlCommand);
            dataSet = new DataSet();

            try
            {
                sqlDataAdapter.Fill(dataSet);
                if (dataSet == null)
                    return NotFound();
                else
                    return Ok(dataSet);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlCommand.Dispose();
                sqlDataAdapter.Dispose();
                dataSet.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        // POST: api/Images
        [HttpPost,Route("")]
        public IHttpActionResult PostImages(ImageData imageData)
        {
            sqlConnection.Open();
            sqlCommand = new SqlCommand("insert into tbl_image values(@image)");
            sqlCommand.Parameters.AddWithValue("@image", imageData.Image);

            try
            {
                sqlConnection.Open();
                var roswsUpdated = sqlCommand.ExecuteNonQuery();
                if (roswsUpdated < 0)
                    return NotFound();
                else
                    return Created(new Uri("http://www.google.com"),"Image Inserted Successfully");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                sqlCommand.Dispose();
                sqlDataAdapter.Dispose();
                dataSet.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}
