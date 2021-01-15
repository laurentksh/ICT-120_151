using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ICT_151.Repositories;
using ICT_151.Models;
using ICT_151.Models.Dto;
using System.Net;
using ICT_151.Exceptions;

namespace ICT_151.Services
{
    public interface IUserService
    {
        Task<UserSessionViewModel> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost);

        /// <summary>
        /// Internal use only
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<UserSession> ValidateUserSession(string token);

        Task<User> GetFullUser(Guid userId);

        Task<User> GetFullUser(string username);

        Task<UserSummaryViewModel> GetUser(Guid userId);

        Task<UserSummaryViewModel> GetUser(string username);

        Task<UserSummaryViewModel> CreateNew(CreateUserDto dto);

        Task Delete(Guid userId, Guid toDeleteId);

        Task Follow(Guid userId, Guid toFollowUserId);

        Task UnFollow(Guid userId, Guid toUnFollowUserId);

        Task SendPrivateMessage(Guid userId, Guid recipientId, string message);

        Task<List<string>> GetPrivateMessages(Guid userId, Guid recipientId);

        Task Block(Guid userId, Guid toBlockId);

        Task UnBlock(Guid userId, Guid toUnBlockId);

        Task<bool> Exists(Guid id);

        Task<bool> Exists(string username);
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

            dto.Password = Utilities.StringUtilities.ComputeHash(dto.Password, System.Security.Cryptography.HashAlgorithmName.SHA512);

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

            if (token.ToLower() == "testtoken")
                return new UserSession() //Debug code
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
                        PasswordHash = "testabc",
                    }
                };

            return await UserRepository.ValidateUserSession(token);
        }

        public async Task<User> GetFullUser(Guid userId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exists.");

            return await UserRepository.GetFullUser(userId);
        }

        public async Task<User> GetFullUser(string username)
        {
            if (!await Exists(username))
                throw new UserNotFoundException("User does not exists.");

            return await UserRepository.GetFullUser(username);
        }

        public async Task<UserSummaryViewModel> GetUser(Guid userId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exists.");

            return await UserRepository.GetUser(userId);
        }

        public async Task<UserSummaryViewModel> GetUser(string username)
        {
            if (!await Exists(username))
                throw new UserNotFoundException("User does not exists.");

            return await UserRepository.GetUser(username);
        }

        public async Task<UserSummaryViewModel> CreateNew(CreateUserDto dto)
        {
            dto.Password = Utilities.StringUtilities.ComputeHash(dto.Password, System.Security.Cryptography.HashAlgorithmName.SHA512);
            
            return await UserRepository.CreateNew(dto);
        }

        public async Task Delete(Guid userId, Guid toDeleteUserId)
        {
            if (!await Exists(userId) || !await Exists(toDeleteUserId))
                throw new UserNotFoundException();

            var user = await UserRepository.GetFullUser(userId);

            if (user.AccountType != AccountType.Administrator)
            {
                if (user.Id != toDeleteUserId)
                    throw new ForbiddenException($"User {user.Username}#{user.Id} does not have access to delete {toDeleteUserId}.");
            }

            await UserRepository.Delete(toDeleteUserId);
        }

        public async Task Follow(Guid userId, Guid toFollowUserId)
        {
            if (!await Exists(userId) || !await Exists(toFollowUserId))
                throw new UserNotFoundException();

            await UserRepository.Follow(userId, toFollowUserId);
        }

        public async Task UnFollow(Guid userId, Guid toUnFollowUserId)
        {
            if (!await Exists(userId) || !await Exists(toUnFollowUserId))
                throw new UserNotFoundException();

            await UserRepository.UnFollow(userId, toUnFollowUserId);
        }

        public async Task SendPrivateMessage(Guid userId, Guid recipientId, string message)
        {
            if (!await Exists(userId) || !await Exists(recipientId))
                throw new UserNotFoundException();

        }

        public async Task<List<string>> GetPrivateMessages(Guid userId, Guid recipientId)
        {
            if (!await Exists(userId) || !await Exists(recipientId))
                throw new UserNotFoundException();

            return (await UserRepository.GetPrivateMessages(userId, recipientId)).ToList();
        }

        public async Task Block(Guid userId, Guid toBlockId)
        {
            if (!await Exists(userId) || !await Exists(toBlockId))
                throw new UserNotFoundException();

            await UserRepository.Block(userId, toBlockId);
        }

        public async Task UnBlock(Guid userId, Guid toUnBlockId)
        {
            if (!await Exists(userId) || !await Exists(toUnBlockId))
                throw new UserNotFoundException();

            await UserRepository.UnBlock(userId, toUnBlockId);
        }

        public async Task<bool> Exists(Guid id)
        {
            return await UserRepository.Exists(id);
        }

        public async Task<bool> Exists(string username)
        {
            return await UserRepository.Exists(username);
        }
    }
}
