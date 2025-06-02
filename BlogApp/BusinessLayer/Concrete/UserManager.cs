using BlogApp.BusinessLayer.Abstract;
using BlogApp.DataLayer.Abstract;
using BlogApp.Entities;


namespace BlogApp.BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserManager(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IQueryable<User> GetAllUsers()
        {
            return _userRepository.Users;
        }

        public void CreateUser(User user)
        {
            _userRepository.CreateUser(user);
        }

        public User ValidateUser(string email, string password)
        {
            return _userRepository.ValidateUser(email, password);
        }

        public Task<bool> UpdateUserImage(int userId, string imageName)
        {
            return _userRepository.UpdateUserImage(userId, imageName);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }

        public Task<bool> SaveChangesAsync()
        {
            return _userRepository.SaveChangesAsync();
        }

        public void DeleteUser(User user)
        {
            _userRepository.DeleteUser(user);
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetByUserId(userId);
        }
    }
}
