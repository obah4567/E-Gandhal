using E_Gandhal.src.Application.DTOs.AuthentificationDTO;
using E_Gandhal.src.Application.IServices;
using E_Gandhal.src.Domain.Models.Authentification;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.src.Presentation.Controllers.User
{
    [ApiController]
    [Route("api/Schools")]
    public class SchoolController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ISchoolCreation schoolCreationRepository;

        public SchoolController(ApplicationDbContext context, ISchoolCreation schoolCreationRepository)
        {
            _context = context;
            this.schoolCreationRepository = schoolCreationRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSchool([FromBody] AuthSchoolDTO authSchoolDTO, CancellationToken cancellation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingSchool = await _context.AuthSchools.AnyAsync(s => s.Name == authSchoolDTO.Name, cancellation);
            if (existingSchool)
            {
                return BadRequest($"Une école avec le nom {authSchoolDTO.Name} existe déjà");
            }

            var matricule = await schoolCreationRepository.GeneratedSchoolAcces(cancellation);

            var school = new AuthSchool
            {
                Name = authSchoolDTO.Name,
                Logo = authSchoolDTO.Logo,
                Matricule = matricule
            };

            _context.AuthSchools.Add(school);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Ecole créée avec succès", matricule = school.Matricule });
        }

    }
}
