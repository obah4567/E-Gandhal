﻿using E_Gandhal.src.Application.DTOs.StudentDTO;
using E_Gandhal.src.Domain.Models.Students;

namespace E_Gandhal.src.Application.IServices
{
    public interface IStudentService
    {
        Task<int> CountStudentsAsync(CancellationToken cancellationToken);
        Task AddStudent(Student student, CancellationToken cancellationToken);

        Task UpdateStudentInformation(int studentId, StudentDTO student, CancellationToken cancellationToken);

        Task<List<Student>> GetAllAsync(CancellationToken cancellationToken);

        Task DeleteStudent(int studentId, CancellationToken cancellationToken);

        Task UpdateImageProfil(int studentId, IFormFile imgProfil, CancellationToken cancellationToken);

        Task UploadImageProfil(int studentId, IFormFile imgProfil, CancellationToken cancellationToken);

        Task<byte[]> SchoolCertificatePdf(int studentId, CancellationToken cancellationToken);

        Task<byte[]> GetInformationPdf(int studentId, CancellationToken cancellationToken);
    }
}
