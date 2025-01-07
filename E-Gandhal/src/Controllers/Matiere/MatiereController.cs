using E_Gandhal.src.Domain.DTO.TeacherDTO;
using E_Gandhal.src.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Controllers.Matiere
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatiereController : ControllerBase
    {
        private readonly IMatiereRepository _matiereRepository;

        public MatiereController(IMatiereRepository matiereService)
        {
            _matiereRepository = matiereService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MatiereDTO>> GetMatiere(int id)
        {
            var matiereDto = await _matiereRepository.GetMatiereByIdAsync(id);

            if (matiereDto == null)
                return NotFound();

            return Ok(matiereDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MatiereDTO>>> GetAllMatieres()
        {
            var matieresDtos = await _matiereRepository.GetAllMatieresAsync();
            return Ok(matieresDtos);
        }

        [HttpPost]
        public async Task<ActionResult<MatiereDTO>> CreateMatiere(MatiereDTO matiereDto)
        {
            var createdMatiere = await _matiereRepository.CreateMatiereAsync(matiereDto);
            return CreatedAtAction(nameof(GetMatiere), new { id = createdMatiere.Id }, createdMatiere);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatiere(int id, MatiereDTO matiereDto)
        {
            if (id != matiereDto.Id)
                return BadRequest();

            var updatedMatiere = await _matiereRepository.UpdateMatiereAsync(matiereDto);

            if (updatedMatiere == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatiere(int id)
        {
            var result = await _matiereRepository.DeleteMatiereAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("classe/{classeId}")]
        public async Task<ActionResult<IEnumerable<MatiereDTO>>> GetMatieresByClasse(int classeId)
        {
            var matieresDtos = await _matiereRepository.GetMatieresByClasseAsync(classeId);

            if (!matieresDtos.Any())
                return NotFound();

            return Ok(matieresDtos);
        }
    }

}
