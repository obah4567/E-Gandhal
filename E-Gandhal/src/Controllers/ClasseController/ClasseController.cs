using E_Gandhal.src.Domain.DTO.TeacherDTO;
using E_Gandhal.src.Domain.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Controllers.ClasseController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ClasseController : ControllerBase
    {
        private readonly IClasseRepository _classeRepository;

        public ClasseController(IClasseRepository classeService)
        {
            _classeRepository = classeService;
        }

        [HttpGet("GetClasseById/{id}")]
        public async Task<ActionResult<ClasseDTO>> GetClasse(int id)
        {
            var ClasseDTO = await _classeRepository.GetClasseByIdAsync(id);

            if (ClasseDTO == null)
                return NotFound();

            return Ok(ClasseDTO);
        }

        [HttpGet("GetAllClasses")]
        public async Task<ActionResult<IEnumerable<ClasseDTO>>> GetAllClasses()
        {
            var classesDtos = await _classeRepository.GetAllClassesAsync();
            return Ok(classesDtos);
        }

        [HttpPost("CreateClasse")]
        public async Task<ActionResult<ClasseDTO>> CreateClasse(ClasseDTO ClasseDTO)
        {
            var createdClasse = await _classeRepository.CreateClasseAsync(ClasseDTO);
            return CreatedAtAction(nameof(GetClasse), new { id = createdClasse.Id }, createdClasse);
        }

        [HttpPut("UpdateClasse/{id}")]
        public async Task<IActionResult> UpdateClasse(int id, ClasseDTO ClasseDTO)
        {
            if (id != ClasseDTO.Id)
                return BadRequest();

            var updatedClass = await _classeRepository.UpdateClasseAsync(ClasseDTO);

            if (updatedClass == null)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteClasseById/{id}")]
        public async Task<IActionResult> DeleteClasse(int id)
        {
            var result = await _classeRepository.DeleteClasseAsync(id);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("AddMatiere/{matiereId}/to/{classeId}")]
        public async Task<IActionResult> AddMatiereToClasse(int classeId, int matiereId)
        {
            var result = await _classeRepository.AddMatiereToClasseAsync(classeId, matiereId);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("DeleteMatiere/{matiereId}/to/{classeId}")]
        public async Task<IActionResult> RemoveMatiereFromClasse(int classeId, int matiereId)
        {
            var result = await _classeRepository.RemoveMatiereFromClasseAsync(classeId, matiereId);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpGet("GetMatiere/{classeId}/matieres")]
        public async Task<ActionResult<IEnumerable<SubjectDTO>>> GetMatieresForClasse(int classeId)
        {
            var matieresDtos = await _classeRepository.GetMatieresForClasseAsync(classeId);

            if (matieresDtos.Count() == 0)
                return NotFound();

            return Ok(matieresDtos);
        }
    }

}
