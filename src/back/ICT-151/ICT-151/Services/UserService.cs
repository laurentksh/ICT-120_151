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
        /// <summary>
        /// Authenticates a user using it's email and password.
        /// </summary>
        /// <param name="dto">Auth request payload</param>
        /// <param name="remoteHost">Client IP Address</param>
        /// <returns>A newly created user session</returns>
        Task<UserSessionViewModel> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost);

        /// <summary>
        /// Validates a user session (Internal use only)
        /// </summary>
        /// <param name="token">Session token</param>
        /// <returns>The full user session</returns>
        Task<UserSession> ValidateUserSession(string token);

        /// <summary>
        /// Get the existing sessions
        /// </summary>
        /// <param name="userId">User GUID</param>
        /// <returns>A list containing existing user sessions</returns>
        Task<List<UserSessionSummaryViewModel>> GetSessions(Guid userId);

        /// <summary>
        /// Clear a specific session
        /// </summary>
        /// <param name="userId">User GUID</param>
        /// <param name="sessionId">Session ID</param>
        /// <returns></returns>
        Task ClearSession(Guid userId, Guid sessionId);

        /// <summary>
        /// Clear sessions for a specified user
        /// </summary>
        /// <param name="userId">User GUID</param>
        /// <returns></returns>
        Task ClearSessions(Guid userId);

        /// <summary>
        /// Clear sessions for a specified user
        /// </summary>
        /// <param name="userId">User GUID</param>
        /// <param name="remoteHost">Client IPAddress</param>
        /// <returns></returns>
        Task ClearSessions(Guid userId, IPAddress remoteHost);

        /// <summary>
        /// Returns the full user (Internal use only)
        /// </summary>
        /// <param name="userId">User GUID</param>
        /// <returns>Full user</returns>
        Task<User> GetFullUser(Guid userId);

        /// <summary>
        /// Returns the full user (Internal use only)
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Full user</returns>
        Task<User> GetFullUser(string username);

        /// <summary>
        /// Returns a summary of the user
        /// </summary>
        /// <param name="userId">User GUID</param>
        /// <returns>A summary of the user</returns>
        Task<UserSummaryViewModel> GetUser(Guid userId);

        /// <summary>
        /// Returns a summary of the user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>A summary of the user</returns>
        Task<UserSummaryViewModel> GetUser(string username);

        /// <summary>
        /// Creates a new user with the specified parameters
        /// </summary>
        /// <param name="dto">Create user request payload</param>
        /// <returns>A summary of the user</returns>
        Task<CreatedUserViewModel> CreateNew(CreateUserDto dto, IPAddress remoteHost);

        /// <summary>
        /// Updates the user with the specified parameters
        /// </summary>
        /// <param name="userId">User to update</param>
        /// <param name="dto"></param>
        /// <param name="remoteHost"></param>
        /// <returns></returns>
        Task<UserSummaryViewModel> Update(Guid userId, UpdateUserDto dto, IPAddress remoteHost);

        /// <summary>
        /// Delete an user
        /// </summary>
        /// <param name="userId">User GUID requesting the account deletion</param>
        /// <param name="toDeleteId">User GUID to delete</param>
        /// <returns></returns>
        Task Delete(Guid userId, Guid toDeleteId);

        /// <summary>
        /// Follow another user
        /// </summary>
        /// <param name="userId">User GUID following another user</param>
        /// <param name="toFollowUserId">The user to follow</param>
        /// <returns></returns>
        Task Follow(Guid userId, Guid toFollowUserId);

        /// <summary>
        /// Unfollow another user
        /// </summary>
        /// <param name="userId">User GUID unfollowing another user</param>
        /// <param name="toUnFollowUserId">The user to follow</param>
        /// <returns></returns>
        Task UnFollow(Guid userId, Guid toUnFollowUserId);

        /// <summary>
        /// Send a private message to another user
        /// </summary>
        /// <param name="userId">User GUID sending the message</param>
        /// <param name="recipientId">The recipient User GUID</param>
        /// <param name="message">The message content</param>
        /// <returns></returns>
        Task SendPrivateMessage(Guid userId, Guid recipientId, string message);

        /// <summary>
        /// Returns the private messages between two users
        /// </summary>
        /// <param name="userId">User GUID requesting the conversation</param>
        /// <param name="recipientId">Target User GUID</param>
        /// <returns>A list of private messages</returns>
        Task<List<PrivateMessageViewModel>> GetPrivateMessages(Guid userId, Guid recipientId);

        /// <summary>
        /// Block another user
        /// </summary>
        /// <param name="userId">User GUID blocking another user</param>
        /// <param name="toBlockId">User GUID to block</param>
        /// <returns></returns>
        Task Block(Guid userId, Guid toBlockId);

        /// <summary>
        /// Unblock another user
        /// </summary>
        /// <param name="userId">User GUID unblocking another user</param>
        /// <param name="toUnBlockId">User GUID to block</param>
        /// <returns></returns>
        Task UnBlock(Guid userId, Guid toUnBlockId);

        /// <summary>
        /// Check if a user with the specified GUID exists
        /// </summary>
        /// <param name="id">User GUID to check</param>
        /// <returns>True if the user exists, otherwise false</returns>
        Task<bool> Exists(Guid id);

        /// <summary>
        /// Check if a user with the specified username exists
        /// </summary>
        /// <param name="id">Username to check</param>
        /// <returns>True if the user exists, otherwise false</returns>
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
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));
            if (remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork && remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                throw new ArgumentOutOfRangeException(nameof(remoteHost), "Invalid IP Address");
            
            dto.Password = Utilities.StringUtilities.ComputeHash(dto.Password, System.Security.Cryptography.HashAlgorithmName.SHA512);

            var session = await UserRepository.AuthenticateUser(dto, remoteHost);

            return UserSessionViewModel.FromUserSession(session);
        }

        public async Task<UserSession> ValidateUserSession(string token)
        {
            if (token is null)
                throw new ArgumentNullException(nameof(token));

#if DEBUG
            if (token.ToLower() == "testtoken")
                return new UserSession() //Debug code
                {
                    Id = Guid.NewGuid(),
                    Token = Utilities.StringUtilities.SecureRandom(64, Utilities.StringUtilities.AllowedChars.All),
                    CreationDate = DateTime.UtcNow.AddDays(-5),
                    ExpiracyDate = DateTime.UtcNow.AddDays(5),
                    UserId = Guid.NewGuid(),
                    User = new User()
                    {
                        Id = Guid.NewGuid(),
                        Username = "MOCK_USER123",
                        Email = "abc@123.com",
                        PasswordHash = "testabc",
                        CreationDate = DateTime.Now.AddDays(-10),
                    }
                };
#endif

            return await UserRepository.ValidateUserSession(token);
        }

        public async Task<List<UserSessionSummaryViewModel>> GetSessions(Guid userId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            return (await UserRepository.GetSessions(userId)).ToList();
        }

        public async Task ClearSession(Guid userId, Guid sessionId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.ClearSession(userId, sessionId);
        }

        public async Task ClearSessions(Guid userId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.ClearSessions(userId);
        }

        public async Task ClearSessions(Guid userId, IPAddress remoteHost)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.ClearSessions(userId, remoteHost);
        }

        public async Task<User> GetFullUser(Guid userId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            return await UserRepository.GetFullUser(userId);
        }

        public async Task<User> GetFullUser(string username)
        {
            if (username is null || !await Exists(username))
                throw new UserNotFoundException("User does not exist.");

            return await UserRepository.GetFullUser(username);
        }

        public async Task<UserSummaryViewModel> GetUser(Guid userId)
        {
            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            return await UserRepository.GetUser(userId);
        }

        public async Task<UserSummaryViewModel> GetUser(string username)
        {
            if (username is null || !await Exists(username))
                throw new UserNotFoundException("User does not exist.");

            return await UserRepository.GetUser(username);
        }

        public async Task<CreatedUserViewModel> CreateNew(CreateUserDto dto, IPAddress remoteHost)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));
            if (remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork && remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                throw new ArgumentOutOfRangeException(nameof(remoteHost), "Invalid IP Address");
            if (await Exists(dto.Username))
                throw new ArgumentException("Username already taken.", nameof(dto));

            dto.Password = Utilities.StringUtilities.ComputeHash(dto.Password, System.Security.Cryptography.HashAlgorithmName.SHA512);
            
            return await UserRepository.CreateNew(dto, remoteHost);
        }

        public async Task<UserSummaryViewModel> Update(Guid userId, UpdateUserDto dto, IPAddress remoteHost)
        {
            if (dto is null)
                throw new ArgumentNullException(nameof(dto));
            if (remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetwork && remoteHost.AddressFamily != System.Net.Sockets.AddressFamily.InterNetworkV6)
                throw new ArgumentOutOfRangeException(nameof(remoteHost), "Invalid IP Address");

            dto.Password = Utilities.StringUtilities.ComputeHash(dto.Password, System.Security.Cryptography.HashAlgorithmName.SHA512);

            if (dto.NewPassword != null)
                dto.NewPassword = Utilities.StringUtilities.ComputeHash(dto.NewPassword, System.Security.Cryptography.HashAlgorithmName.SHA512);

            if (!await Exists(userId))
                throw new UserNotFoundException("User does not exist.");

            if (!await UserRepository.PasswordMatches(userId, dto.Password))
                throw new ForbiddenException("Wrong password.");

            //TODO: Maybe do something with the ip ?

            return await UserRepository.Update(userId, dto);
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
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.Follow(userId, toFollowUserId);
        }

        public async Task UnFollow(Guid userId, Guid toUnFollowUserId)
        {
            if (!await Exists(userId) || !await Exists(toUnFollowUserId))
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.UnFollow(userId, toUnFollowUserId);
        }

        public async Task SendPrivateMessage(Guid userId, Guid recipientId, string message)
        {
            if (message is null)
                throw new ArgumentNullException(nameof(message));
            if (!await Exists(userId) || !await Exists(recipientId))
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.SendPrivateMessage(userId, recipientId, message);
        }

        public async Task<List<PrivateMessageViewModel>> GetPrivateMessages(Guid userId, Guid recipientId)
        {
            if (!await Exists(userId) || !await Exists(recipientId))
                throw new UserNotFoundException("User does not exist.");

            return (await UserRepository.GetPrivateMessages(userId, recipientId)).ToList();
        }

        public async Task Block(Guid userId, Guid toBlockId)
        {
            if (!await Exists(userId) || !await Exists(toBlockId))
                throw new UserNotFoundException("User does not exist.");

            await UserRepository.Block(userId, toBlockId);
        }

        public async Task UnBlock(Guid userId, Guid toUnBlockId)
        {
            if (!await Exists(userId) || !await Exists(toUnBlockId))
                throw new UserNotFoundException("User does not exist.");

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
