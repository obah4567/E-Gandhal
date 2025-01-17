using E_Gandhal.src.Domain.DTO.AuthentificationDTO;
using E_Gandhal.src.Domain.IServices;
using E_Gandhal.src.Domain.Models.Authentification;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Gandhal.Infrastructure.Repositories
{
    public class UserRepository : IUserAuthentificationService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IPasswordHasher<Register> _passwordHasher;

        public UserRepository(ApplicationDbContext applicationDbContext, IPasswordHasher<Register> passwordHasher)
        {
            _applicationDbContext = applicationDbContext;
            _passwordHasher = passwordHasher;
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterDTO registerDto, CancellationToken cancellationToken)
        {
            var userExisting = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email, cancellationToken);
            if (userExisting != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"This {userExisting} was already exist" });
            }
            var user = new Register
            {
                Email = registerDto.Email,
                UserName = registerDto.Username,
                FirstName = registerDto.Firstname,
                LastName = registerDto.Lastname,
                DateOfBirth = registerDto.DateOfBirth
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, registerDto.Password);

            _applicationDbContext.Users.Add(user);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }

        public async Task<SignInResult> LoginUserAsync(LoginDTO loginDto, CancellationToken cancellationToken)
        {
            var userExisting = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == loginDto.Email, cancellationToken);
            if (userExisting == null)
            {
                return SignInResult.Failed;
            }

            var result = _passwordHasher.VerifyHashedPassword(userExisting, userExisting.PasswordHash, loginDto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return SignInResult.Failed;
            }
            return SignInResult.Success;
        }

        public async Task<Register> Disconnected(int id, CancellationToken cancellationToken)
        {
            var user = await _applicationDbContext.Users.SingleOrDefaultAsync(u => u.UserId == id, cancellationToken);
            if (user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(1);
                await _applicationDbContext.SaveChangesAsync(cancellationToken);
            }
            return user;
        }

        public async Task<Register> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _applicationDbContext.Users.SingleOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public async Task<IdentityResult> UpdateUserPassword(string email, string oldPassword, string newPassword, CancellationToken cancellationToken)
        {
            var userExisting = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (userExisting == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"If this {userExisting} exist, we will send you, email for update your password" });
            }

            var verifiePassword = _passwordHasher.VerifyHashedPassword(userExisting, userExisting.PasswordHash, oldPassword);
            if (verifiePassword == PasswordVerificationResult.Failed)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Le mot de passe ne corresponds pas" });
            }
            else
            {
                userExisting.PasswordHash = _passwordHasher.HashPassword(userExisting, newPassword);
            }

            _applicationDbContext.Users.Update(userExisting);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return IdentityResult.Success;
        }
    }
}
