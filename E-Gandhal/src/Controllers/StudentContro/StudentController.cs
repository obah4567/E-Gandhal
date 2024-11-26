using E_Gandhal.src.Domain.DTO.StudentDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Controllers.StudentContro
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet("GetStudentPdf/{id}")]
        public async Task<IActionResult> GetStudentPdf(int id, CancellationToken cancellationToken)
        {
            var pdfBytes = await _studentRepository.GetInformationPdf(id, cancellationToken);

            return File(pdfBytes, "application/pdf", $"Student_{id}.pdf");
        }

        [HttpPost("AddStudent")]
        public async Task<IActionResult> AddStudent([FromBody] Student student, CancellationToken cancellationToken)
        {
            if (student == null)    
            {
                return BadRequest("Une erreur s'est produit !");
            }
            await _studentRepository.AddStudent(student, cancellationToken);
            return Ok("L'élève à bien été ajouté !");
        }

        [HttpDelete("DeleteStudent")]
        public async Task<IActionResult> DeleteStudent(int studentId, CancellationToken cancellationToken)
        {
            await _studentRepository.DeleteStudent(studentId, cancellationToken);
            return Ok("L'élève à bien été supprimé !");
        }

        [HttpGet("GetAllStudent")]
        public async Task<IActionResult> GetAllStudent(CancellationToken cancellationToken)
        {
            var list = await _studentRepository.GetAllAsync(cancellationToken);
            return Ok(list);
        }

        [HttpPatch("Update-Student-Information")]
        public async Task<IActionResult> UpdateStudentInformation(int id, [FromBody] StudentDTO student, CancellationToken cancellationToken)
        {
            if (student == null) 
            {
                return BadRequest("Nous n'avons pas pu modifier ces informations !");
            }
            await _studentRepository.UpdateStudentInformation(id, student, cancellationToken);
            return Ok("Le changement à bien été effectué !");
        }

        [HttpPost("{studentId}/UploadImageProfil")]
        public async Task<IActionResult> UploadStudentImageProfil(int studentId, IFormFile imgProfil, CancellationToken cancellationToken)
        {
            if (imgProfil == null || imgProfil.Length == 0)
            {
                return BadRequest("Veuillez fournir une image valide !");
            }

            await _studentRepository.UploadImageProfil(studentId, imgProfil, cancellationToken);
            return Ok("La photo de profil à bien été ajouté !");
        }

        [HttpPut("UpdateProfilImage")]
        public async Task<IActionResult> UpdateProfilImageStudent(int studentId, IFormFile imgProfil,CancellationToken cancellationToken)
        {
            if (imgProfil == null || imgProfil.Length == 0)
            {
                return BadRequest("Veuillez fournir une image valide !");
            }
            await _studentRepository.UpdateImageProfil(studentId,imgProfil, cancellationToken);
            return Ok("La photo de profil à bien été mise à jour !");
        }
    }
}
