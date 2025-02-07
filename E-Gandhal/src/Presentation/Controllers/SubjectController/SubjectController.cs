using E_Gandhal.src.Application.DTOs.TeachersDTO;
using E_Gandhal.src.Application.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Presentation.Controllers.SubjectController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _matiereRepository;

        public SubjectController(ISubjectService matiereService)
        {
            _matiereRepository = matiereService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubjectDTO>> GetMatiere(int id)
        {
            var matiereDto = await _matiereRepository.GetSubjectByIdAsync(id);

            if (matiereDto == null)
                return NotFound();

            return Ok(matiereDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetAllMatieres()
        {
            var matieresDtos = await _matiereRepository.GetAllSubjectsAsync();
            return Ok(matieresDtos);
        }

        [HttpPost]
        public async Task<ActionResult<SubjectDTO>> CreateMatiere(SubjectDTO matiereDto)
        {
            var createdMatiere = await _matiereRepository.CreateSubjectAsync(matiereDto);
            return CreatedAtAction(nameof(GetMatiere), new { id = createdMatiere.Id }, createdMatiere);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatiere(int id, SubjectDTO matiereDto)
        {
            if (id != matiereDto.Id)
                return BadRequest();

            var updatedMatiere = await _matiereRepository.UpdateSubjectAsync(matiereDto);

            if (updatedMatiere == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatiere(int id)
        {
            var result = await _matiereRepository.DeleteSubjectAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("classe/{classeId}")]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetMatieresByClasse(int classeId)
        {
            var matieresDtos = await _matiereRepository.GetSubjectsByClasseAsync(classeId);

            if (!matieresDtos.Any())
                return NotFound();

            return Ok(matieresDtos);
        }
    }

}
