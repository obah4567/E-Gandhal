using E_Gandhal.src.Application.IServices;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.src.Infrastructure.Repositories
{
    public class SchoolCreationRepository : ISchoolCreation
    {

        private readonly ApplicationDbContext _applicationDbContext;

        public SchoolCreationRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        private static string GenerateUniqueMatricule()
        {
            return "SCH-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
        public async Task<string> GeneratedSchoolAcces(CancellationToken cancellationToken)
        {
            string matricule;
            do
            {
                matricule = GenerateUniqueMatricule();
            } while (await _applicationDbContext.AuthSchools.AnyAsync(s => s.Matricule == matricule, cancellationToken));

            return matricule;
        }
    }
}
