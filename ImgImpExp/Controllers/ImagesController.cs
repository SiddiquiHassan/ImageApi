using ImgImpExp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        SqlConnection sqlConnection;
        SqlDataAdapter sqlDataAdapter;
        SqlCommand sqlCommand;
        DataSet dataSet;

        public ImagesController()
        {
            sqlConnection = new SqlConnection("Data Source=192.168.0.100;Initial Catalog=RosettaWebApi;user id=sa;password=123456");
        }
        // GET: api/Images
        [Route("")]
        public IHttpActionResult GetImages()
        {
            sqlCommand = new SqlCommand("select * from tbl_Image", sqlConnection);
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
            sqlCommand = new SqlCommand("select * from tbl_Image where Id =" + id, sqlConnection);
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
            sqlCommand = new SqlCommand("insert into tbl_image values(@image)", sqlConnection);
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
