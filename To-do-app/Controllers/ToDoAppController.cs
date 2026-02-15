using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using To_do_app.Data;
using To_do_app.Models;

namespace To_do_app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoAppController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ToDoAppController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetNotes")]
        public async Task<IActionResult> GetNotes()
        {
            var notes = await _context.Notes.ToListAsync();
            return new JsonResult(notes);
        }

        [HttpPost("AddNote")]
        public async Task<IActionResult> AddNote([FromForm] string newNotes)
        {
            var note = new Note { Description = newNotes };
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return new JsonResult("Added Successfully");
        }

        [HttpDelete("DeleteNote")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound("Note not found");
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();
            return new JsonResult("Deleted Successfully");
        }

        [HttpPut("UpdateNote")]
        public async Task<IActionResult> UpdateNote([FromForm] int id, [FromForm] string description)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound("Note not found");
            }

            note.Description = description;
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
            return new JsonResult("Updated Successfully");
        }
    }
}
