using E_Gandhal.src.Domain.DTO.StudentDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Students;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Gandhal.src.Controllers.StudentController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepository _studentRepository;

        public StudentController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        [HttpGet("GetStudentPdf/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentPdf(int id, CancellationToken cancellationToken)
        {
            try
            {
                var pdfBytes = await _studentRepository.GetInformationPdf(id, cancellationToken);
                return File(pdfBytes, "application/pdf", $"Student_{id}.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Nous avons pas trouvé cet élève !");
            }
        }

        [HttpGet("SchoolCertificatePdf/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SchoolCertificatePdf(int id, CancellationToken cancellationToken)
        {
            try
            {
                var pdfBytes = await _studentRepository.SchoolCertificatePdf(id, cancellationToken);
                return File(pdfBytes, "application/pdf", $"Student_{id}.pdf");
            }
            catch (Exception ex) 
            {
                return StatusCode(StatusCodes.Status404NotFound, "Nous avons pas trouvé cet élève !");
            }
        }

        [HttpPost("AddStudent")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteStudent(int studentId, CancellationToken cancellationToken)
        {
           try
           {
                await _studentRepository.DeleteStudent(studentId, cancellationToken);
                //return Ok("L'élève à bien été supprimé !");
                return NoContent();
           }
           catch (Exception)
           {
                return StatusCode(StatusCodes.Status404NotFound, "L'élève à bien été supprimé !");
           }
        }

        [HttpGet("GetAllStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllStudent(CancellationToken cancellationToken)
        {
            try
            {
                var list = await _studentRepository.GetAllAsync(cancellationToken);
                return Ok(list);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Nous avons rien trouvé !");
            }
        }

        [HttpPatch("Update-Student-Information")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateStudentInformation(int id, [FromBody] StudentDTO student, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Nous n'avons pas pu modifier ces informations !");
            }
            try
            {
                await _studentRepository.UpdateStudentInformation(id, student, cancellationToken);
                return NoContent();
                //return Ok("Le changement à bien été effectué !");
            }
            catch (Exception)
            {
                return StatusCode(500, "Une erreur est survenue lors de la mise à jour de l'élève");
            }
            
        }

        [HttpPost("{studentId}/UploadImageProfil")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        public async Task<IActionResult> UpdateProfilImageStudent(int studentId, IFormFile imgProfil, CancellationToken cancellationToken)
        {
            if (imgProfil == null || imgProfil.Length == 0)
            {
                return BadRequest("Veuillez fournir une image valide !");
            }
            await _studentRepository.UpdateImageProfil(studentId, imgProfil, cancellationToken);
            return Ok("La photo de profil à bien été mise à jour !");
        }
    }
}
