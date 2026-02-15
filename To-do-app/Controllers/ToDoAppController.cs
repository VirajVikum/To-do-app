using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace To_do_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoAppController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ToDoAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetSqlConnectionString() => _configuration.GetConnectionString("todoAppCon");

        [HttpGet("GetNotes")]
        public IActionResult GetNotes()
        {
            string query = "SELECT * FROM notes";
            DataTable table = new DataTable();
            using (SqlConnection myCon = new SqlConnection(GetSqlConnectionString()))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    using (SqlDataReader myReader = myCommand.ExecuteReader())
                    {
                        table.Load(myReader);
                    }
                }
            }
            return new JsonResult(table);
        }

        [HttpPost("AddNote")]
        public IActionResult AddNote([FromForm] string newNotes)
        {
            // Note: If sending JSON from React, use [FromBody] instead of [FromForm]
            string query = "INSERT INTO notes (description) VALUES (@newNotes)";
            using (SqlConnection myCon = new SqlConnection(GetSqlConnectionString()))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@newNotes", newNotes);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpDelete("DeleteNote")]
        public IActionResult DeleteNote(int id)
        {
            string query = "DELETE FROM notes WHERE id = @id";
            using (SqlConnection myCon = new SqlConnection(GetSqlConnectionString()))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

        [HttpPut("UpdateNote")]
        public IActionResult UpdateNote([FromForm] int id, [FromForm] string description)
        {
            string query = "UPDATE notes SET description = @description WHERE id = @id";
            using (SqlConnection myCon = new SqlConnection(GetSqlConnectionString()))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myCommand.Parameters.AddWithValue("@description", description);
                    myCommand.ExecuteNonQuery();
                }
            }
            return new JsonResult("Updated Successfully");
        }
    }
}