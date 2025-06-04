using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;

namespace BlogApp.DataLayer.Concrete.EfCore
{
    public class EfUserRepository : IUserRepository
    {
        private BlogDbContext _context;
        public EfUserRepository(BlogDbContext context)
        {
            _context = context;
        }
        public IQueryable<User> Users => _context.Users;
        public void CreateUser(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.RegisterDate = DateTime.Now;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public User ValidateUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user == null)
                return null;

            try
            {

                bool validPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (validPassword)
                    return user;

                return null;
            }
            catch
            {

                return null;
            }
        }

        public async Task<bool> UpdateUserImage(int userId, string imageName)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            user.Image = imageName;
            return await _context.SaveChangesAsync() > 0;
        }

        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public User GetByUserId(int id)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == id);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }

        public User? GetUserByUsernameAndPassword(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null) return null;

            bool valid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return valid ? user : null;
        }

    }
}
