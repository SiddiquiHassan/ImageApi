using ImgImpExp.Dtos;
using ImgImpExp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace ImgImpExp.Controllers
{
    [RoutePrefix("api/emp")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeesController : ApiController
    {
        readonly SqlConnection sqlConnection;
        SqlCommand sqlCommand;
        SqlDataAdapter sqlDataAdapter;
        DataSet dataSet;

        public EmployeesController()
        {
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            sqlCommand = null;
            sqlDataAdapter = null;
            dataSet = null;
        }


        [HttpGet, Route("")]
        public IHttpActionResult GetEmployees()
        {
            try
            {
                var dt = GetDataSet(null).Tables[0];
                List<EmployeesDto> list = new List<EmployeesDto>();
                foreach (var row in dt.AsEnumerable())
                {
                    list.Add(new EmployeesDto()
                    {
                        Name = row["Name"].ToString(),
                        Age = int.Parse(row["Age"].ToString()),
                        Address = row["Address"].ToString(),
                        PhoneNo = int.Parse(row["PhoneNo"].ToString())
                    });
                }
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
                //return NotFound();
            }
        }

        [HttpGet, Route("{id}")]
        [ResponseType(typeof(EmployeesDto))]
        public IHttpActionResult GetEmployee([FromUri] int id)
        {
            try
            {
                var dt = GetDataSet(id).Tables[0];
                var employee = from row in dt.AsEnumerable()
                               select new EmployeesDto()
                               {
                                   Name = row["Name"].ToString(),
                                   Age = int.Parse(row["Age"].ToString()),
                                   Address = row["Address"].ToString(),
                                   PhoneNo = int.Parse(row["PhoneNo"].ToString())
                               };
                //return Ok(GetDataSet(id));
                return Ok(employee);
            }
            catch (Exception)
            {
                throw;
                //return NotFound();
            }
        }

        [HttpPost, Route("")]
        public IHttpActionResult PostEmployee(Employee emp)
        {
            try
            {
                if (emp != null)
                {
                    sqlCommand = new SqlCommand("insert into tbl_emp values(@name,@age,@address,@phoneNo,@image)");
                    sqlCommand.Connection = sqlConnection;
                    sqlCommand.Parameters.AddWithValue("@name", emp.Name);
                    sqlCommand.Parameters.AddWithValue("@age", emp.Age);
                    sqlCommand.Parameters.AddWithValue("@address", emp.Address);
                    sqlCommand.Parameters.AddWithValue("@phoneNo", emp.PhoneNo);
                    sqlCommand.Parameters.AddWithValue("@image", emp.Image);

                    sqlConnection.Open();
                    var roswsUpdated = sqlCommand.ExecuteNonQuery();
                    if (roswsUpdated < 0)
                        return NotFound();
                    else
                        return Created(new Uri("http://www.google.com"), "Record Inserted Successfully");
                }
                else
                {
                    return BadRequest("Employee object can't be null!");
                }
            }
            catch (Exception ex)
            {
                throw;
                //return BadRequest(ex.Message);
            }
            finally
            {
                sqlCommand.Dispose();
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }

        [DisableCors]
        [NonAction]
        private DataSet GetDataSet(int? id)
        {
            dataSet = new DataSet();
            sqlDataAdapter = new SqlDataAdapter();
            try
            {
                if (id == null)
                {
                    sqlCommand = new SqlCommand("select * from tbl_emp");
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(dataSet, "employees");

                    if (ValidateDataSet(dataSet))
                        return dataSet;
                    else
                        return null;
                }
                else
                {
                    sqlCommand = new SqlCommand("select * from tbl_emp where id = @id");
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    sqlCommand.Parameters.AddWithValue("@id", id);
                    sqlDataAdapter.SelectCommand = sqlCommand;
                    sqlDataAdapter.Fill(dataSet, "employees");

                    if (ValidateDataSet(dataSet))
                        return dataSet;
                    else
                        return null;
                }
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

        [DisableCors]
        [NonAction]
        private bool ValidateDataSet(DataSet dataSet)
        {
            if (dataSet != null)
                if (dataSet.Tables[0].Rows.Count != 0)
                    return true;
                else
                    return false;
            else
                return false;
        }

    }
}
