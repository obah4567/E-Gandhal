using E_Gandhal.src.Domain.DTO.AuthentificationDTO;
using E_Gandhal.src.Domain.Models.Authentification;
using E_Gandhal.src.Infrastructure.ApplicationDBContext;
using E_Gandhal.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace E_Gandhal.tests.Infrastructure.Repositories
{
    public class UserRepositoryTest : IDisposable
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private readonly Mock<IPasswordHasher<Register>> _passwordHasher;
        private readonly UserRepository _repository;

        public UserRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("TestDatabase")
            .Options;

            _applicationDbContext = new ApplicationDbContext(options);
            _passwordHasher = new Mock<IPasswordHasher<Register>>();

            _repository = new UserRepository(_applicationDbContext, _passwordHasher.Object);
        }

        public void Dispose()
        {
            //Utilisation de Dispose pour supprimer la BD temporair après chaque test
            _applicationDbContext.Database.EnsureDeleted();
            _applicationDbContext.Dispose();
        }

        #region Register
        [Fact]
        public async Task RegisterUserAsync_UserDoesNotExist_ShouldReturnSuccess()
        {
            // Arrange
            var registerDto = new RegisterDTO
            {
                Email = "test@example.com",
                Username = "userTest",
                Firstname = "Jean",
                Lastname = "Doe",
                DateOfBirth = new DateTime(2000, 12, 1),
                Password = "Password@123"
            };
            _passwordHasher.Setup(ph => ph.HashPassword(It.IsAny<Register>(), It.IsAny<string>())).Returns("hashed_password");

            // Act
            var result = await _repository.RegisterUserAsync(registerDto, CancellationToken.None);

            // Assert
            result.Should().Be(IdentityResult.Success);
            var user = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == registerDto.Email);
            user.Should().NotBeNull();
        }

        [Fact]
        public async Task RegisterUserAsync_IfUserExist_ShouldReturnFail()
        {
            // Arrange
            var existingUser = new Register
            {
                Email = "test@example.com",
                UserName = "userTest",
                PasswordHash = "hashed_password"
            };
            await _applicationDbContext.Users.AddAsync(existingUser);
            await _applicationDbContext.SaveChangesAsync();

            var registerDto = new RegisterDTO
            {
                Email = "test@example.com",
                Username = "userTest",
                Firstname = "Jean",
                Lastname = "Doe",
                DateOfBirth = new DateTime(2000, 12, 1),
                Password = "Password@123"
            };

            // Act
            var result = await _repository.RegisterUserAsync(registerDto, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.Description.Contains("was already exist"));
        }
        #endregion

        #region GetUserByEmailAsync
        [Fact]
        public async Task GetUserByEmailAsync_ShouldReturnUser_WhenEmailExists()
        {
            //Arrange
            var existingUser = new Register
            {
                Email = "test@example.com",
                UserName = "userTest",
                PasswordHash = "hashed_password"
            };
            await _applicationDbContext.Users.AddAsync(existingUser, CancellationToken.None);
            await _applicationDbContext.SaveChangesAsync();

            //Act
            var resultat = await _repository.GetUserByEmailAsync(existingUser.Email, CancellationToken.None);


            //Assert
            resultat.Should().NotBeNull();
            resultat.Email.Should().Be(existingUser.Email);
            resultat.UserName.Should().Be(existingUser.UserName);
        }

        [Fact]
        public async Task GetUserByEmailAsync_ShouldFail_WhenEmailNotExists()
        {
            //Arrange
            var existingUser = new Register
            {
                Email = "null",
                UserName = "null",
                PasswordHash = "hashed_password"
            };
            await _applicationDbContext.Users.AddAsync(existingUser, CancellationToken.None);
            await _applicationDbContext.SaveChangesAsync();

            //Act
            var resultat = await _repository.GetUserByEmailAsync("", CancellationToken.None);


            //Assert
            resultat.Should().BeNull();
            //resultat.Email.Should().BeNull();
            //resultat.UserName.Should().BeNull();
        }

        #endregion

        #region Login
        [Fact]
        public async Task LoginUserAsync_ShouldReturnSuccess_WhenUserExists()
        {
            //Arrange
            var loginDto = new LoginDTO
            {
                Email = "test@example.com",
                Password = "Password@123"
            };

            var existingUser = new Register
            {
                Email = loginDto.Email,
                UserName = "userTest",
                PasswordHash = "hashed_password"
            };

            _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<Register>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Success);

            await _applicationDbContext.Users.AddAsync(existingUser, CancellationToken.None);
            await _applicationDbContext.SaveChangesAsync();

            //Act
            var resultat = await _repository.LoginUserAsync(loginDto, CancellationToken.None);

            //Assert
            resultat.Should().Be(SignInResult.Success);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnFailed_WhenUserDontExists()
        {
            //Arrange
            var loginDto = new LoginDTO
            {
                Email = "null@example.com",
                Password = "FakePassword"
            };

            var existingUser = new Register
            {
                Email = loginDto.Email,
                UserName = "userTest",
                PasswordHash = "hashed_password"
            };

            _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<Register>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(PasswordVerificationResult.Failed);

            await _applicationDbContext.Users.AddAsync(existingUser, CancellationToken.None);
            await _applicationDbContext.SaveChangesAsync();

            //Act
            var resultat = await _repository.LoginUserAsync(loginDto, CancellationToken.None);

            //Assert
            resultat.Should().Be(SignInResult.Failed);
        }
        #endregion

        #region UpdatePassword

        [Fact]
        public async Task UpdateUserPassword_ShouldReturnSuccess_WhenValidOldPassword()
        {
            // Arrange
            var existingUser = new Register
            {
                Email = "test@example.com",
                UserName = "userTest",
                PasswordHash = "old_hashed_password"
            };

            await _applicationDbContext.Users.AddAsync(existingUser);
            await _applicationDbContext.SaveChangesAsync();

            var email = "test@example.com";
            var oldPassword = "old_hashed_password";
            var newPassword = "NewPassword@123";

            _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<Register>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Success);
            _passwordHasher.Setup(ph => ph.HashPassword(It.IsAny<Register>(), It.IsAny<string>())).Returns("new_hashed_password");

            // Act
            var result = await _repository.UpdateUserPassword(email, oldPassword, newPassword, CancellationToken.None);

            // Assert
            result.Should().Be(IdentityResult.Success);
            var updatedUser = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            updatedUser.PasswordHash.Should().Be("new_hashed_password");
        }

        [Fact]
        public async Task UpdateUserPassword_ShouldReturnFailed_WhenWrongOldPassword()
        {
            // Arrange
            var existingUser = new Register
            {
                Email = "test@example.com",
                UserName = "userTest",
                PasswordHash = "old_hashed_password"
            };

            await _applicationDbContext.Users.AddAsync(existingUser);
            await _applicationDbContext.SaveChangesAsync();

            var email = "test@example.com";
            var oldPassword = "old_hashed_passwordXXXXXXX";
            var newPassword = "NewPassword@123";

            _passwordHasher.Setup(ph => ph.VerifyHashedPassword(It.IsAny<Register>(), It.IsAny<string>(), It.IsAny<string>())).Returns(PasswordVerificationResult.Failed);

            // Act
            var result = await _repository.UpdateUserPassword(email, oldPassword, newPassword, CancellationToken.None);

            // Assert
            result.Succeeded.Should().BeFalse();
            var updatedUser = await _applicationDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
            updatedUser.PasswordHash.Should().Be("old_hashed_password");
        }
        #endregion
    }
}
