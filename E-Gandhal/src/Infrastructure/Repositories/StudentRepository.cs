﻿using E_Gandhal.src.Domain.DTO.StudentDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Student;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.EntityFrameworkCore;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

namespace E_Gandhal.src.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly ILogger<StudentRepository> _logger;

        public StudentRepository(ApplicationDbContext applicationDbContext, ILogger<StudentRepository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
        }

        public async Task AddStudent(Student student, CancellationToken cancellationToken)
        {
            await _applicationDbContext.Students.AddAsync(student, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteStudent(int studentId, CancellationToken cancellationToken)
        {
            var existentStudent = await _applicationDbContext.Students.FirstOrDefaultAsync(u => u.Id == studentId);
            if (existentStudent == null)
            {
                _logger.LogInformation("Nous n'avons pas trouvé cet élève");
                throw new Exception($"Cet élève {studentId} n'existe pas ! ");
            }
            _applicationDbContext.Students.Remove(existentStudent);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<Student>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Students.ToListAsync(cancellationToken);
        }

        public async Task<byte[]> GetInformationPdf(int studentId, CancellationToken cancellationToken)
        {
            var existentStudent = await _applicationDbContext.Students.FirstOrDefaultAsync(u => u.Id == studentId);
            if (existentStudent == null) 
            {
                _logger.LogInformation($"Nous n'avons pas trouvé cet élève {studentId}");
                throw new Exception($"Elève avec l'ID {studentId} introuvable.");
            }

            // Création
            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var graphics = XGraphics.FromPdfPage(page);
            var font = new XFont("Arial", 12, XFontStyle.Regular);

            // Titre
            graphics.DrawString("Informations sur l'étudiant", new XFont("Arial", 16, XFontStyle.Bold), XBrushes.Black, new XPoint(40, 50));

            // Vérification et ajout de l'image de profil
            if (!string.IsNullOrEmpty(existentStudent.ImageProfil))
            {
                var imagePath = Path.Combine("wwwroot", existentStudent.ImageProfil.TrimStart('/'));
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
            graphics.DrawString($"ID : {existentStudent.Id}", font, XBrushes.Black, new XPoint(160, 100));
            graphics.DrawString($"Firstname : {existentStudent.Firstname}", font, XBrushes.Black, new XPoint(160, 130));
            graphics.DrawString($"Lastname : {existentStudent.Lastname}", font, XBrushes.Black, new XPoint(160, 160));
            graphics.DrawString($"Date de Naissance: {existentStudent.DateOfBirth:dd/MM/yyyy}", font, XBrushes.Black, new XPoint(160, 190));
            graphics.DrawString($"Lieu de Naissance: {existentStudent.PlaceOfBirth}", font, XBrushes.Black, new XPoint(160, 220));
            graphics.DrawString($"Classe: {existentStudent.Classe}", font, XBrushes.Black, new XPoint(160, 250));


            // Convertir le PDF en flux de mémoire
            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                var pdfBytes = stream.ToArray();
                return pdfBytes;
            }
        }

        public async Task UpdateImageProfil(int studentId, IFormFile imgProfil,CancellationToken cancellationToken)
        {
            var student = await _applicationDbContext.Students.FirstOrDefaultAsync(u => u.Id == studentId, cancellationToken);
            if (imgProfil == null || student == null)
            {
                throw new ArgumentNullException("L'image ou l'étudiant ne peut pas être null.");
            }

            student.ImageProfil = imgProfil.ToString();
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task UpdateStudentInformation(int studentId, StudentDTO student, CancellationToken cancellationToken)
        {
            var existentStudent = await _applicationDbContext.Students.FirstOrDefaultAsync(u => u.Id == studentId, cancellationToken);
            
            if (existentStudent == null)
            {
                _logger.LogInformation("Nous n'avons pas trouvé cet élève");
                throw new Exception($"Cet élève {studentId} n'existe pas ! ");
            }

            existentStudent.Firstname = student.Firstname;
            existentStudent.Lastname = student.Lastname;
            existentStudent.Classe = student.Classe;
            existentStudent.DateOfBirth = student.DateOfBirth;
            existentStudent.ImageProfil = student.ImageProfil;
            existentStudent.PlaceOfBirth = existentStudent.PlaceOfBirth;

            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UploadImageProfil(int studentId, IFormFile imgProfil, CancellationToken cancellationToken)
        {
            // Vérifie si le fichier et l'entité Student sont valides
            var student = await _applicationDbContext.Students.FirstOrDefaultAsync(u => u.Id==studentId, cancellationToken);
            if (imgProfil == null || student == null)
            {
                throw new ArgumentNullException("L'image ou l'étudiant ne peut pas être null.");
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

            // Met à jour la propriété ImageProfil de l'étudiant avec le chemin relatif
            student.ImageProfil = $"/images/profiles/{uniqueFileName}";

            // Sauvegarde les modifications dans la base de données
            await _applicationDbContext.SaveChangesAsync(cancellationToken);
        }

    }
}