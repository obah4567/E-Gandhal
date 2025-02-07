namespace E_Gandhal.src.Application.IServices
{
    public interface ISchoolCreation
    {
        Task<string> GeneratedSchoolAcces(CancellationToken cancellationToken);
    }
}
