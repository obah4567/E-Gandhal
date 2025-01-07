using E_Gandhal.src.Domain.DTO.TeacherDTO;
using E_Gandhal.src.Domain.IServices;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Controllers.TeacherController
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherController(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        [HttpGet("GetTeacherPdf/{id}")]
        public async Task<IActionResult> GetTeacherPdf(int id, CancellationToken cancellationToken)
        {
            var pdfBytes = await _teacherRepository.GetInformationPdf(id, cancellationToken);

            return File(pdfBytes, "application/pdf", $"Student_{id}.pdf");
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTeacherById(int id, CancellationToken cancellationToken)
        {
            var teacher = await _teacherRepository.GetTeacherByIdAsync(id, cancellationToken);

            if (teacher == null)
            {
                return NotFound($"L'enseignant avec l'ID {id} n'a pas été trouvé.");
            }

            return Ok(teacher);
        }


        [HttpPost("AddTeacher")]
        public async Task<IActionResult> AddTeacher([FromBody] TeacherDTO teacher, CancellationToken cancellationToken)
        {
            if (teacher == null)
            {
                return BadRequest("Une erreur s'est produit !");
            }
            await _teacherRepository.AddTeacherAsync(teacher, cancellationToken);
            return CreatedAtAction(nameof(GetTeacherById), new { id = teacher.TeacherId }, teacher);
        }

        [HttpDelete("DeleteTeacher")]
        public async Task<IActionResult> DeleteTeacher(int teacherId, CancellationToken cancellationToken)
        {
            await _teacherRepository.DeleteTeacherAsync(teacherId, cancellationToken);
            return Ok("L'enseignant à bien été supprimé !");
        }

        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeachers(CancellationToken cancellationToken)
        {
            var list = await _teacherRepository.GetAllTeachersAsync(cancellationToken);
            return Ok(list);
        }

        [HttpPatch("Update-Teacher-Information")]
        public async Task<IActionResult> UpdateTeacherInformation(int id, [FromBody] TeacherDTO teacher, CancellationToken cancellationToken)
        {
            if (teacher == null)
            {
                return BadRequest("Nous n'avons pas pu modifier ces informations !");
            }
            //await _teacherRepository.UpdateTeacherInformation(id, teachers, cancellationToken);

            await _teacherRepository.UpdateTeacherAsync(teacher, cancellationToken);
            return Ok("Le changement à bien été effectué !");
        }

        [HttpPost("{teacherId}/UploadImageProfil")]
        public async Task<IActionResult> UploadteacherImageProfil(int teacherId, IFormFile imgProfil, CancellationToken cancellationToken)
        {
            if (imgProfil == null || imgProfil.Length == 0)
            {
                return BadRequest("Veuillez fournir une image valide !");
            }

            await _teacherRepository.UploadImageProfil(teacherId, imgProfil, cancellationToken);
            return Ok("La photo de profil à bien été ajouté !");
        }

        /*[HttpPut("UpdateProfilImage")]
        public async Task<IActionResult> UpdateProfilImageStudent(int studentId, IFormFile imgProfil, CancellationToken cancellationToken)
        {
            if (imgProfil == null || imgProfil.Length == 0)
            {
                return BadRequest("Veuillez fournir une image valide !");
            }
            await _teacherRepository.UpdateImageProfil(studentId, imgProfil, cancellationToken);
            return Ok("La photo de profil à bien été mise à jour !");
        }*/
    }


}

