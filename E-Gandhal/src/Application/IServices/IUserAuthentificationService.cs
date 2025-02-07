using E_Gandhal.src.Application.DTOs.AuthentificationDTO;
using E_Gandhal.src.Domain.Models.Authentification;
using Microsoft.AspNetCore.Identity;

namespace E_Gandhal.src.Application.IServices
{
    public interface IUserAuthentificationService
    {
        Task<IdentityResult> RegisterUserAsync(RegisterDTO registerDto, CancellationToken cancellationToken);

        Task<SignInResult> LoginUserAsync(LoginDTO loginDto, CancellationToken cancellationToken);

        Task<Register> GetUserByEmailAsync(string email, CancellationToken cancellationToken);

        //Task<IdentityResult> UpdateUserAsync(int id, UpdateUserDTO updateUserDto, CancellationToken cancelToken);

        Task<Register> Disconnected(int id, CancellationToken cancellationToken);

        Task<IdentityResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken);

    }
}
