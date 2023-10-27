using Interfaces;
using Models;

namespace Repositories {
    public class UserRepository : IUserRepository {
        private readonly DbConnection _context = new();
        public User Add(User user) {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
        public List<User> GetAll() {
            return _context.Users.ToList();
        }

        public User GetByEmail(string email) {
            User user = _context.Users.Where(
                user => user.email == email
            ).FirstOrDefault();

            return user;
        }

        public User GetById(int id) {
            User user = _context.Users.Where(
                user => user.id == id
            ).FirstOrDefault();

            return user;
        }

        public User GetByFbId(string fbId) {
            User user = _context.Users.Where(
                user => user.facebookId == fbId
            ).FirstOrDefault();

            return user;
        }
    }
}