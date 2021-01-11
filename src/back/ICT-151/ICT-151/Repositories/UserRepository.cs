using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ICT_151.Data;
using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;

namespace ICT_151.Repositories
{
    public interface IUserRepository
    {
        Task<UserSession> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost);

        Task<UserSession> ValidateUserSession(string token);

        Task<User> CreateNew(CreateUserDto dto);

        Task Delete(Guid userId);

        Task Follow(Guid userId, Guid toFollowUserId);

        Task UnFollow(Guid userId, Guid toUnFollowUserId);

        Task SendPrivateMessage(Guid userId, Guid recipientId, string message);

        Task<IEnumerable<string>> GetPrivateMessages(Guid userId, Guid recipientId);

        Task Block(Guid userId, Guid targetId);

        Task UnBlock(Guid userId, Guid targetId);
    }

    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext DbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<UserSession> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost)
        {
            var user = DbContext.Users.SingleOrDefault(x => x.Email == dto.Email);

            if (user == null)
                throw new DataNotFoundException("Email does not match any account.");

            if (user.PasswordHash != dto.Password)
                throw new WrongCredentialsException("Email and password does not match.");

            UserSession session = new UserSession()
            {
                Token = Utilities.StringUtilities.Random(64, Utilities.StringUtilities.AllowedChars.All),
                RemoteHost = remoteHost,
                CreationDate = DateTime.UtcNow,
                ExpiracyDate = dto.ExtendSession ? DateTime.UtcNow.AddDays(7) : DateTime.UtcNow.AddDays(30),
                UserId = user.Id
            };

            await DbContext.UserSessions.AddAsync(session);
            await DbContext.SaveChangesAsync();

            return session;
        }

        public async Task<UserSession> ValidateUserSession(string token)
        {
            var session = DbContext.UserSessions.SingleOrDefault(session => session.Token == token);

            if (session == null)
                return null;
            
            if (session.ExpiracyDate < DateTime.UtcNow) {
                DbContext.UserSessions.Remove(session);
                await DbContext.SaveChangesAsync();
                return null;
            }

            return session;
        }

        public Task<User> CreateNew(CreateUserDto dto)
        {
            throw new NotImplementedException();
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

        public Task SendPrivateMessage(Guid userId, Guid recipientId, string message)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetPrivateMessages(Guid userId, Guid recipientId)
        {
            throw new NotImplementedException();
        }

        public Task Block(Guid userId, Guid targetId)
        {
            throw new NotImplementedException();
        }

        public Task UnBlock(Guid userId, Guid targetId)
        {
            throw new NotImplementedException();
        }
    }
}
