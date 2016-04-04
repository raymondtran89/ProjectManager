using System.Linq;
using ProjectManager.DataAccessLayer.Entity;
using ProjectManager.DataAccessLayer.Repository.Abtract;

namespace ProjectManager.DataAccessLayer.Extension
{
    public static class UserRepositoryExtensions
    {
        public static User GetSingleByUsername(
            this IEntityRepository<User> userRepository, string username)
        {
            return userRepository.GetAll().FirstOrDefault(x => x.Name == username);
        }
    }
}