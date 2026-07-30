using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;

namespace SchoolCenter.API.Controllers
{
    [RoutePrefix("api/courses")]
    public class CoursesController : ApiController
    {
        private string GetConnectionString()
        {
            var connStrSetting = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (connStrSetting != null && !string.IsNullOrEmpty(connStrSetting.ConnectionString))
            {
                return connStrSetting.ConnectionString;
            }
            return "Server=.\\SQLEXPRESS;Database=SchoolCenterDB;Integrated Security=True;";
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult GetCourses(string search = null)
        {
            try
            {
                var list = new List<CourseResponse>();

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "SELECT CourseID, CourseName, Cost FROM Courses";
                    if (!string.IsNullOrEmpty(search))
                    {
                        query += " WHERE CourseName LIKE @Filter";
                    }
                    query += " ORDER BY CourseID DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (!string.IsNullOrEmpty(search))
                        {
                            cmd.Parameters.AddWithValue("@Filter", "%" + search.Trim() + "%");
                        }

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new CourseResponse
                                {
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString() ?? string.Empty,
                                    Cost = Convert.ToDecimal(reader["Cost"])
                                });
                            }
                        }
                    }
                }

                return Ok(list);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب قائمة الدورات", error = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetCourseById")]
        public IHttpActionResult GetCourse(int id)
        {
            try
            {
                CourseResponse course = null;

                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "SELECT CourseID, CourseName, Cost FROM Courses WHERE CourseID = @CourseID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseID", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                course = new CourseResponse
                                {
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString() ?? string.Empty,
                                    Cost = Convert.ToDecimal(reader["Cost"])
                                };
                            }
                        }
                    }
                }

                if (course == null)
                {
                    return Content(System.Net.HttpStatusCode.NotFound, new { message = "الدورة التدريبية غير موجودة" });
                }

                return Ok(course);
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء جلب بيانات الدورة", error = ex.Message });
            }
        }

        [HttpPost]
        [Route("")]
        public IHttpActionResult CreateCourse([FromBody] CourseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CourseName) || request.Cost < 0)
            {
                return BadRequest("يرجى إدخال اسم الدورة وتكلفة صحيحة أكبر من أو تساوي الصفر");
            }

            try
            {
                int newCourseID = -1;
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    string query = "INSERT INTO Courses (CourseName, Cost) VALUES (@CourseName, @Cost); SELECT SCOPE_IDENTITY();";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseName", request.CourseName.Trim());
                        cmd.Parameters.AddWithValue("@Cost", request.Cost);

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            newCourseID = Convert.ToInt32(result);
                        }
                    }
                }

                return CreatedAtRoute("GetCourseById", new { id = newCourseID }, new { courseID = newCourseID, message = "تم إضافة الدورة بنجاح" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء إضافة الدورة", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public IHttpActionResult UpdateCourse(int id, [FromBody] CourseRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.CourseName) || request.Cost < 0)
            {
                return BadRequest("يرجى إدخال اسم الدورة وتكلفة صحيحة أكبر من أو تساوي الصفر");
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Check if course exists
                    string checkQuery = "SELECT COUNT(*) FROM Courses WHERE CourseID = @CourseID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@CourseID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return Content(System.Net.HttpStatusCode.NotFound, new { message = "الدورة غير موجودة" });
                        }
                    }

                    string query = "UPDATE Courses SET CourseName = @CourseName, Cost = @Cost WHERE CourseID = @CourseID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseName", request.CourseName.Trim());
                        cmd.Parameters.AddWithValue("@Cost", request.Cost);
                        cmd.Parameters.AddWithValue("@CourseID", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "تم تعديل الدورة بنجاح" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء تعديل الدورة", error = ex.Message });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult DeleteCourse(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GetConnectionString()))
                {
                    conn.Open();

                    // Check if course exists
                    string checkQuery = "SELECT COUNT(*) FROM Courses WHERE CourseID = @CourseID";
                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@CourseID", id);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) == 0)
                        {
                            return Content(System.Net.HttpStatusCode.NotFound, new { message = "الدورة غير موجودة" });
                        }
                    }

                    string query = "DELETE FROM Courses WHERE CourseID = @CourseID";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseID", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                return Ok(new { message = "تم حذف الدورة بنجاح" });
            }
            catch (Exception ex)
            {
                return Content(System.Net.HttpStatusCode.InternalServerError, new { message = "حدث خطأ أثناء حذف الدورة", error = ex.Message });
            }
        }
    }

    public class CourseRequest
    {
        public string CourseName { get; set; }
        public decimal Cost { get; set; }
    }

    public class CourseResponse
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public decimal Cost { get; set; }
    }
}
