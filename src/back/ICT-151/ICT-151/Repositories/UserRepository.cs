using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ICT_151.Data;
using ICT_151.Exceptions;
using ICT_151.Models;
using ICT_151.Models.Dto;
using Microsoft.EntityFrameworkCore;

namespace ICT_151.Repositories
{
    public interface IUserRepository
    {
        Task<UserSession> AuthenticateUser(AuthUserDto dto, IPAddress remoteHost);

        Task<UserSession> ValidateUserSession(string token);

        Task<IEnumerable<UserSessionSummaryViewModel>> GetSessions(Guid userId);

        Task ClearSession(Guid userId, Guid sessionId);

        Task ClearSessions(Guid userId);

        Task ClearSessions(Guid userId, IPAddress remoteHost);

        Task<User> GetFullUser(Guid userId);

        Task<User> GetFullUser(string username);

        Task<UserSummaryViewModel> GetUser(Guid userId);

        Task<UserSummaryViewModel> GetUser(string username);

        Task<bool> PasswordMatches(Guid userId, string passwordHash);

        Task<CreatedUserViewModel> CreateNew(CreateUserDto dto, IPAddress remoteHost);

        Task<UserSummaryViewModel> Update(Guid userId, UpdateUserDto dto);

        Task Delete(Guid userId);

        Task Follow(Guid userId, Guid toFollowUserId);

        Task UnFollow(Guid userId, Guid toUnFollowUserId);

        Task SendPrivateMessage(Guid userId, Guid recipientId, string message);

        Task<IEnumerable<PrivateMessageViewModel>> GetPrivateMessages(Guid userId, Guid recipientId);

        Task Block(Guid userId, Guid targetId);

        Task UnBlock(Guid userId, Guid targetId);

        Task<bool> Exists(Guid id);

        Task<bool> Exists(string username);
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

            //The following valiations can't really be done in the service so i'm doing them here.
            if (user == null)
                throw new DataNotFoundException("Email does not match any account.");

            if (user.PasswordHash != dto.Password)
                throw new WrongCredentialsException("Email/Password does not match any account.");

            UserSession session = new UserSession()
            {
                Token = Utilities.StringUtilities.SecureRandom(64, Utilities.StringUtilities.AllowedChars.AlphabetNumbers),
                RemoteHost = remoteHost,
                CreationDate = DateTime.UtcNow,
                ExpiracyDate = dto.ExtendSession ? DateTime.UtcNow.Add(UserSession.ExtendedTokenValidity) : DateTime.UtcNow.Add(UserSession.DefaultTokenValidity),
                UserId = user.Id
            };

            await ClearSessions(user.Id, remoteHost); //Clear active sessions

            await DbContext.UserSessions.AddAsync(session);
            await DbContext.SaveChangesAsync();

            return session;
        }

        public async Task<UserSession> InternalAuthenticateUser(Guid userId, IPAddress remoteHost)
        {
            var user = DbContext.Users.SingleOrDefault(x => x.Id == userId);

            //The following valiations can't really be done in the service so i'm doing them here.
            if (user == null)
                throw new DataNotFoundException("Email does not match any account.");

            UserSession session = new UserSession()
            {
                Token = Utilities.StringUtilities.SecureRandom(64, Utilities.StringUtilities.AllowedChars.AlphabetNumbers),
                RemoteHost = remoteHost,
                CreationDate = DateTime.UtcNow,
                ExpiracyDate = DateTime.UtcNow.Add(UserSession.DefaultTokenValidity),
                UserId = user.Id
            };

            await ClearSessions(user.Id, remoteHost); //Clear active sessions

            await DbContext.UserSessions.AddAsync(session);
            await DbContext.SaveChangesAsync();

            return session;
        }

        public async Task<UserSession> ValidateUserSession(string token)
        {
            var session = DbContext.UserSessions
                .Include(t => t.User)
                .SingleOrDefault(session => session.Token == token);

            if (session == null)
                return null;
            
            if (session.ExpiracyDate < DateTime.UtcNow) {
                DbContext.UserSessions.Remove(session);
                await DbContext.SaveChangesAsync();
                return null;
            }

            return session;
        }

        public async Task<IEnumerable<UserSessionSummaryViewModel>> GetSessions(Guid userId)
        {
            var sessions = DbContext.UserSessions
                .Where(x => x.UserId == userId)
                .Select(y => UserSessionSummaryViewModel.FromUserSession(y));

            return sessions.AsEnumerable();
        }

        public async Task ClearSession(Guid userId, Guid sessionId)
        {
            var sessions = await DbContext.UserSessions
                .SingleAsync(x => x.Id == sessionId && x.UserId == userId); //Checking the userId so a user can't delete someone else's session.

            DbContext.UserSessions.Remove(sessions);
            await DbContext.SaveChangesAsync();
        }

        public async Task ClearSessions(Guid userId)
        {
            DbContext.UserSessions.RemoveRange(DbContext.UserSessions.Where(x => x.UserId == userId));
            await DbContext.SaveChangesAsync();
        }

        public async Task ClearSessions(Guid userId, IPAddress remoteHost)
        {
            DbContext.UserSessions.RemoveRange(DbContext.UserSessions.Where(x => x.UserId == userId && x.RemoteHost == remoteHost));
            await DbContext.SaveChangesAsync();
        }

        public async Task<User> GetFullUser(Guid userId)
        {
            return DbContext.Users
                .Include(x => x.UserSessions)
                .Single(x => x.Id == userId);
        }

        public async Task<User> GetFullUser(string username)
        {
            return DbContext.Users
                .Include(x => x.UserSessions)
                .Single(x => x.Username == username);
        }

        public async Task<UserSummaryViewModel> GetUser(Guid userId)
        {
            return UserSummaryViewModel.FromUser(DbContext.Users.Single(x => x.Id == userId));
        }

        public async Task<UserSummaryViewModel> GetUser(string username)
        {
            return UserSummaryViewModel.FromUser(DbContext.Users.Single(x => x.Username == username));
        }

        public async Task<bool> PasswordMatches(Guid userId, string passwordHash)
        {
            var user = DbContext.Users
                .Single(x => x.Id == userId);

            return user.PasswordHash == passwordHash;
        }

        public async Task<CreatedUserViewModel> CreateNew(CreateUserDto dto, IPAddress remoteHost)
        {
            var result = await DbContext.Users.AddAsync(new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = dto.Password,
                Birthday = dto.BirthDay,
                CreationDate = DateTime.UtcNow,
                AccountType = AccountType.User
            });

            await DbContext.SaveChangesAsync();

            var session = await InternalAuthenticateUser(result.Entity.Id, remoteHost);

            return new CreatedUserViewModel()
            {
                User = new UserSummaryViewModel
                {
                    Id = result.Entity.Id,
                    Username = result.Entity.Username,
                    CreationDate = result.Entity.CreationDate
                },
                Session = UserSessionViewModel.FromUserSession(session)
            };
        }

        public async Task<UserSummaryViewModel> Update(Guid userId, UpdateUserDto dto)
        {
            var user = DbContext.Users
                .Single(x => x.Id == userId);

            if (dto.NewPassword != null)
                user.PasswordHash = dto.NewPassword;

            if (dto.Email != null)
                user.Email = dto.Email;

            if (dto.Username != null)
                user.Username = dto.Username;

            if (dto.Biography != null)
                user.Biography = dto.Biography;

            if (dto.BirthDay != null)
                user.Birthday = dto.BirthDay.Value;

            await DbContext.SaveChangesAsync();

            return UserSummaryViewModel.FromUser(user);
        }

        public async Task Delete(Guid userId)
        {
            DbContext.Users.Remove(DbContext.Users.Single(x => x.Id == userId));
            await DbContext.SaveChangesAsync();
        }

        public async Task Follow(Guid userId, Guid toFollowUserId)
        {
            await DbContext.Follows.AddAsync(new Follow
            {
                CreationDate = DateTime.UtcNow,
                FollowerId = userId,
                FollowTargetId = toFollowUserId
            });
            await DbContext.SaveChangesAsync();
        }

        public async Task UnFollow(Guid userId, Guid toUnFollowUserId)
        {
            DbContext.Follows.Remove(await DbContext.Follows.SingleAsync(x => x.FollowerId == userId && x.FollowTargetId == toUnFollowUserId));
            await DbContext.SaveChangesAsync();
        }

        public async Task SendPrivateMessage(Guid userId, Guid recipientId, string message)
        {
            await DbContext.PrivateMessages.AddAsync(new PrivateMessage
            {
                MessageContent = message,
                CreationDate = DateTime.UtcNow,
                SenderId = userId,
                RecipientId = recipientId
            });
        }

        public async Task<IEnumerable<PrivateMessageViewModel>> GetPrivateMessages(Guid userId, Guid recipientId)
        {
            return DbContext.PrivateMessages
                .Where(x => x.SenderId == userId && x.RecipientId == recipientId)
                .Select(y => PrivateMessageViewModel.FromPrivateMessage(y))
                .AsEnumerable();
        }

        public async Task Block(Guid userId, Guid targetId)
        {
            await DbContext.Blocks.AddAsync(new Block
            {
                CreationDate = DateTime.UtcNow,
                BlockerId = userId,
                BlockTargetId = targetId
            });
            await DbContext.SaveChangesAsync();
        }

        public async Task UnBlock(Guid userId, Guid targetId)
        {
            DbContext.Blocks.Remove(await DbContext.Blocks.SingleAsync(x => x.BlockerId == userId && x.BlockTargetId == targetId));
            await DbContext.SaveChangesAsync();
        }

        public async Task<bool> Exists(Guid id)
        {
            return await DbContext.Users.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> Exists(string username)
        {
            return await DbContext.Users.AnyAsync(x => x.Username == username);
        }
    }
}
