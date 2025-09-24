using UsersCRUD.Models;
using UsersCRUD.Repositories.Interfaces;

namespace UsersCRUD.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<User>> GetAllAsync() => await _repository.GetAllAsync();

        public async Task<User?> GetByIdAsync(string id) => await _repository.GetByIdAsync(id);

        public async Task<User> CreateAsync(User user)
        {
            if (string.IsNullOrWhiteSpace(user.Id))
                user.Id = Guid.NewGuid().ToString();

            await _repository.AddAsync(user);
            return user;
        }

        public async Task<User?> UpdateAsync(User user)
        {
            var existing = await _repository.GetByIdAsync(user.Id);
            if (existing is null) return null;

            existing.Name = user.Name;
            existing.LastName = user.LastName;
            existing.Email = user.Email;
            existing.PasswordHash = user.PasswordHash;
            existing.IsActive = user.IsActive;

            await _repository.UpdateAsync(existing);
            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null) return false;

            await _repository.DeleteAsync(existing);
            return true;
        }
    }
}
