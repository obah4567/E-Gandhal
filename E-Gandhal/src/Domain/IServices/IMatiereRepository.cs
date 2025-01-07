using E_Gandhal.src.Domain.DTO.TeacherDTO;

namespace E_Gandhal.src.Domain.IServices
{
    public interface IMatiereRepository
    {
        Task<MatiereDTO> GetMatiereByIdAsync(int id);

        Task<IEnumerable<MatiereDTO>> GetAllMatieresAsync();

        Task<MatiereDTO> CreateMatiereAsync(MatiereDTO MatiereDTO);

        Task<MatiereDTO> UpdateMatiereAsync(MatiereDTO MatiereDTO);

        Task<bool> DeleteMatiereAsync(int id);

        Task<IEnumerable<MatiereDTO>> GetMatieresByClasseAsync(int classeId);
    }
}
