using E_Gandhal.src.Domain.DTO.TeachersDTO;
using E_Gandhal.src.Domain.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Controllers.Notes
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _noteService;

        public NoteController(INoteRepository noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("GetNotesBy/{id}")]
        public async Task<ActionResult<NoteDTO>> GetNote(int id)
        {
            var note = await _noteService.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet("GetAllNotes")]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetAllNotes()
        {
            var notes = await _noteService.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpPost("CreateNotes")]
        public async Task<ActionResult<NoteDTO>> CreateNote(NoteDTO noteDTO)
        {
            var createdNote = await _noteService.CreateNoteAsync(noteDTO);
            return CreatedAtAction(nameof(GetNote), new { id = createdNote.Id }, createdNote);
        }

        [HttpPut("UpdatesNotes/{id}")]
        public async Task<IActionResult> UpdateNote(int id, NoteDTO noteDTO)
        {
            if (id != noteDTO.Id)
            {
                return BadRequest();
            }

            var updatedNote = await _noteService.UpdateNoteAsync(noteDTO);
            if (updatedNote == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("DeletesNotes/{id}")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var result = await _noteService.DeleteNoteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("GetNotesByStudent/{studentId}")]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetNotesByStudent(int studentId)
        {
            var notes = await _noteService.GetNotesByStudentAsync(studentId);
            return Ok(notes);
        }

        [HttpGet("GetMatiereById/{matiereId}")]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetNotesByMatiere(int matiereId)
        {
            var notes = await _noteService.GetNotesByMatiereAsync(matiereId);
            return Ok(notes);
        }

        [HttpGet("GetAverage/student/{studentId}")]
        public async Task<ActionResult<double>> GetAverageByStudent(int studentId)
        {
            var average = await _noteService.GetAverageByStudentAsync(studentId);
            return Ok(average);
        }
    }

}
