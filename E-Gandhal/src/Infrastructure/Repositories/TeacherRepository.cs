using E_Gandhal.src.Domain.DTO.TeacherDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Teachers;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace E_Gandhal.Infrastructure.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<TeacherRepository> _logger;

        public TeacherRepository(ApplicationDbContext applicationDbContext, ILogger<TeacherRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task<TeacherDTO?> GetTeacherByIdAsync(int teacherId, CancellationToken cancellationToken)
        {
            var teacher = await _applicationDbContext.Teachers
                .Include(t => t.Matieres)
                .FirstOrDefaultAsync(t => t.TeacherId == teacherId, cancellationToken);

            return teacher == null ? null : MapToDTO(teacher);
        }

        public async Task<IEnumerable<TeacherDTO>> GetAllTeachersAsync(CancellationToken cancellationToken)
        {
            var teachers = await _applicationDbContext.Teachers
                .Include(t => t.Matieres)
                .ToListAsync(cancellationToken);

            return teachers.Select(MapToDTO);
        }

        public async Task<TeacherDTO> AddTeacherAsync(TeacherDTO teacherDto, CancellationToken cancellationToken)
        {
            if (teacherDto == null)
            {
                throw new ArgumentNullException(nameof(teacherDto));
            }

            var teacher = new Teacher
            {
                ImageProfil = teacherDto.ImageProfil,
                LastName = teacherDto.LastName,
                Firstname = teacherDto.Firstname,
                Description = teacherDto.Description
            };

            await _applicationDbContext.Teachers.AddAsync(teacher, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Nouvel enseignant ajouté avec l'ID {teacher.TeacherId}");

            return MapToDTO(teacher);
        }

        public async Task DeleteTeacherAsync(int teacherId, CancellationToken cancellationToken)
        {
            var existingTeacher = await _applicationDbContext.Teachers
                .FirstOrDefaultAsync(u => u.TeacherId == teacherId, cancellationToken);

            if (existingTeacher == null)
            {
                _logger.LogWarning($"Tentative de suppression d'un enseignant inexistant (ID: {teacherId})");
                throw new Exception($"Cet enseignant {teacherId} n'existe pas !");
            }

            _applicationDbContext.Teachers.Remove(existingTeacher);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Enseignant supprimé avec l'ID {teacherId}");
        }


        public async Task<byte[]> GetInformationPdf(int teacherId, CancellationToken cancellationToken)
        {

            var existentTeacher = await _applicationDbContext.Teachers.Include(t => t.Matieres).FirstOrDefaultAsync(t => t.TeacherId == teacherId, cancellationToken);

            if (existentTeacher == null)
            {
                _logger.LogInformation("Nous n'avons pas trouvé cet enseignant");
                throw new Exception($"Cet enseignant {teacherId} n'existe pas ! ");
            }

            // Création
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12, XFontStyle.Regular);

            // Titre
            graphics.DrawString("Informations sur l'enseignant", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Black, new XPoint(40, 50));

            // Vérification et ajout de l'image de profil
            if (!string.IsNullOrEmpty(existentTeacher.ImageProfil))
            {
                var imagePath = Path.Combine("wwwroot", existentTeacher.ImageProfil.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    // Fonction qui retourne un flux
                    Func<Stream> imageStreamFunc = () => new FileStream(imagePath, FileMode.Open, FileAccess.Read);

                    // Charger l'image à partir du flux
                    var profileImage = XImage.FromStream(imageStreamFunc);
                    graphics.DrawImage(profileImage, 40, 80, 100, 100); // Taille et position de l'image
                }
                else
                {
                    graphics.DrawString("Photo de profil : Image introuvable", font, XBrushes.Black, new XPoint(40, 150));
                }
            }

            // Informations de l'étudiant
            graphics.DrawString($"ID : {existentTeacher.TeacherId}", font, XBrushes.Black, new XPoint(160, 100));
            graphics.DrawString($"Firstname : {existentTeacher.Firstname}", font, XBrushes.Black, new XPoint(160, 130));
            graphics.DrawString($"Lastname : {existentTeacher.LastName}", font, XBrushes.Black, new XPoint(160, 160));
            graphics.DrawString($"Date de Naissance: {existentTeacher.Description}", font, XBrushes.Black, new XPoint(160, 190));
            graphics.DrawString($"Lieu de Naissance: {existentTeacher.Matieres}", font, XBrushes.Black, new XPoint(160, 220));


            // Convertir le PDF en flux de mémoire
            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                var pdfBytes = stream.ToArray();
                return pdfBytes;
            }
        }

        public async Task UploadImageProfil(int teacherId, IFormFile imgProfil, CancellationToken cancellationToken)
        { // Vérifie si le fichier et l'entité teacher sont valides
            var teacher = await _applicationDbContext.Teachers.FirstOrDefaultAsync(u => u.TeacherId == teacherId, cancellationToken);
            if (imgProfil == null || teacher == null)
            {
                throw new ArgumentNullException("L'image ou l'enseignant ne peut pas être null.");
            }

            // Vérifie si le fichier est une image (facultatif)
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imgProfil.FileName);
            if (!validExtensions.Contains(extension.ToLower()))
            {
                throw new InvalidOperationException("Le fichier doit être une image (.jpg, .jpeg, .png, .gif).");
            }

            // Définir le chemin où l'image sera enregistrée
            var uploadsFolder = Path.Combine("wwwroot", "images", "profiles");
            Directory.CreateDirectory(uploadsFolder); // Crée le dossier s'il n'existe pas

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            // Enregistre l'image sur le serveur
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imgProfil.CopyToAsync(fileStream, cancellationToken);
            }

            // Met à jour la propriété ImageProfil de l'enseignant avec le chemin relatif
            teacher.ImageProfil = $"/images/profiles/{uniqueFileName}";

            // Sauvegarde les modifications dans la base de données
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AttribuerNoteAsync(int studentId, int matiereId, double note, CancellationToken cancellationToken)
        {
            if (note < 0 || note > 20)
            {
                throw new ArgumentOutOfRangeException(nameof(note), "La note est comprise entre 0 à 20");
            }

            var verifStudent = await _applicationDbContext.Students.FirstOrDefaultAsync(s => s.Id == studentId, cancellationToken);
            if (verifStudent == null)
            {
                _logger.LogInformation("Nous n'avons pas trouvé cet élève");
                throw new Exception($"Cet elève {studentId} n'existe pas ! ");
            }

            var verifMatiere = await _applicationDbContext.Matieres.FirstOrDefaultAsync(m => m.Id == matiereId, cancellationToken);
            if (verifMatiere == null)
            {
                _logger.LogInformation("Cette matière n'existe pas pour cette classe");
                throw new Exception($"Cet matière {matiereId} n'existe pas ! ");
            }

            if (verifStudent.ClasseId != verifMatiere.ClasseId)
            {
                _logger.LogInformation("L'étudiant n'est pas inscrit dans cette classe pour cette matiere");
                throw new Exception($"Cet matière {matiereId} n'existe pas ! ");
            }

            var notation = new Note
            {
                StudentId = studentId,
                MatiereId = matiereId,
                //Students = verifStudent,
                //Matieres = verifMatiere,
                Value = note,
                DateAdded = DateTime.UtcNow
            };

            _applicationDbContext.Notes.Add(notation);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AttribuerNoteParProfesseurAsync(int teacherId, int studentId, int matiereId, double note, CancellationToken cancellationToken)
        {
            var notation = _applicationDbContext.Matieres
                .FirstOrDefaultAsync(s => s.Id == matiereId && s.TeacherId == teacherId, cancellationToken);
            if (notation == null)
            {
                throw new UnauthorizedAccessException("Ce professeur n'enseigne pas cette matière. ");
            }

            await AttribuerNoteAsync(studentId, matiereId, note, cancellationToken);
        }


        public async Task UpdateTeacherInformation(int teacherId, TeacherDTO teacherDTO, CancellationToken cancellationToken)
        {
            var existentTeacher = await _applicationDbContext.Teachers.FirstOrDefaultAsync(u => u.TeacherId == teacherId, cancellationToken);

            if (existentTeacher == null)
            {
                _logger.LogInformation("Nous n'avons pas trouvé cet enseignant");
                throw new Exception($"Cet élève {teacherId} n'existe pas ! ");
            }

            existentTeacher.Firstname = teacherDTO.Firstname;
            existentTeacher.LastName = teacherDTO.LastName;
            existentTeacher.Description = teacherDTO.Description;
            existentTeacher.ImageProfil = teacherDTO.ImageProfil;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<TeacherDTO?> UpdateTeacherAsync(TeacherDTO teacherDto, CancellationToken cancellationToken)
        {
            var existingTeacher = await _applicationDbContext.Teachers
                .FirstOrDefaultAsync(t => t.TeacherId == teacherDto.TeacherId, cancellationToken);

            if (existingTeacher == null)
            {
                _logger.LogWarning($"Tentative de mise à jour d'un enseignant inexistant (ID: {teacherDto.TeacherId})");
                return null;
            }

            existingTeacher.ImageProfil = teacherDto.ImageProfil;
            existingTeacher.LastName = teacherDto.LastName;
            existingTeacher.Firstname = teacherDto.Firstname;
            existingTeacher.Description = teacherDto.Description;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Enseignant mis à jour avec l'ID {existingTeacher.TeacherId}");

            return MapToDTO(existingTeacher);
        }


        private TeacherDTO MapToDTO(Teacher teacher)
        {
            return new TeacherDTO
            {
                TeacherId = teacher.TeacherId,
                ImageProfil = teacher.ImageProfil,
                LastName = teacher.LastName,
                Firstname = teacher.Firstname,
                Description = teacher.Description,
                MatiereIds = teacher.Matieres?.Select(m => m.Id).ToList() ?? new List<int>()
            };
        }
    }
}
