using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Repositories;
using ICT_151.Models;
using ICT_151.Models.Dto;
using System.Net;

namespace ICT_151.Services
{
    public interface IUserService
    {
        Task<UserSessionViewModel> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost);

        Task<UserSession> ValidateUserSession(string token);

        Task<User> CreateNew(CreateUserDto dto);

        Task Delete(Guid userId);

        Task Follow(Guid userId, Guid toFollowUserId);

        Task UnFollow(Guid userId, Guid toUnFollowUserId);

        //void SendPrivateMessage(Guid userId, Guid recipientId, string message);

        //IEnumerable<string> GetPrivateMessages(Guid userId, Guid recipientId);

        //void Block(Guid userId, Guid toBlockId);

        //void UnBlock(Guid userId, Guid toUnBlockId);
    }

    public class UserService : IUserService
    {
        private IUserRepository UserRepository;

        public UserService(IUserRepository repository)
        {
            UserRepository = repository;
        }

        public async Task<UserSessionViewModel> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost)
        {
            if (remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork && remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                throw new ArgumentOutOfRangeException(nameof(remoteHost), "Invalid IP Address");

            dto.Password = Utilities.StringUtilities.ComputeSha256Hash(dto.Password, System.Security.Cryptography.HashAlgorithmName.SHA512);

            var session = await UserRepository.AuthenticateUser(dto, remoteHost);

            return new UserSessionViewModel
            {
                Id = session.Id,
                Token = session.Token,
                CreationDate = session.CreationDate,
                ExpiracyDate = session.ExpiracyDate,
                UserId = session.UserId
            };
        }

        public async Task<UserSession> ValidateUserSession(string token)
        {
            if (token is null)
                throw new ArgumentNullException(nameof(token));

            return token == "abc123" ? new UserSession()
            {
                Id = Guid.NewGuid(),
                Token = Utilities.StringUtilities.Random(64, Utilities.StringUtilities.AllowedChars.All),
                CreationDate = DateTime.UtcNow.AddDays(-5),
                ExpiracyDate = DateTime.UtcNow.AddDays(5),
                UserId = Guid.NewGuid(),
                User = new User()
                {
                    Id = Guid.NewGuid(),
                    Username = "MOCK_USER123",
                    Email = "abc@123.com",
                    PasswordHash = "testabc"
                }
            } : null; //Debug code
            //return await UserRepository.ValidateUserSession(token);
        }

        public Task<User> CreateNew(CreateUserDto dto)
        {
            //throw new NotImplementedException();
            return Task.FromResult(new User()
            {
                Id = Guid.NewGuid(),
                Username = "MOCK_USER1234",
                PasswordHash = Utilities.StringUtilities.ComputeSha256Hash("abc123"),
                AccountType = AccountType.User
            });
        }

        public Task Delete(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task Follow(Guid userId, Guid toFollowUserId)
        {
            throw new NotImplementedException();
        }

        public Task UnFollow(Guid userId, Guid toUnFollowUserId)
        {
            throw new NotImplementedException();
        }
    }
}
