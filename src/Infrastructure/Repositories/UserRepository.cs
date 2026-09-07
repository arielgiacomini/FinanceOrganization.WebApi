using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FinanceOrganizationContext _context;
        private readonly ILogger _logger;

        public UserRepository(ILogger logger, FinanceOrganizationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<User?> GetById(Guid id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmail(string email)
        {
            try
            {
                var normalizedEmail = email.Trim().ToLowerInvariant();

                return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == normalizedEmail);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[UserRepository.GetByEmail()] - Ocorreu um erro ao buscar o usuário por email. Email: {Email}", email);
                throw;
            }
        }

        public async Task<User?> GetByGoogleSub(string googleSub)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.GoogleSub == googleSub);
        }

        public async Task<IList<User>> GetAll()
        {
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<int> Save(User user)
        {
            try
            {
                _context.Add(user);

                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[UserRepository.Save()] - Ocorreu um erro ao salvar o usuário. Email: {Email}", user.Email);

                throw;
            }
        }

        public async Task<int> Edit(User user)
        {
            _context.ChangeTracker.Clear();

            _context.Users.Update(user);

            return await _context.SaveChangesAsync();
        }
    }
}
