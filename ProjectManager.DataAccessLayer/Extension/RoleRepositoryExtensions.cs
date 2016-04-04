using System.Linq;
using ProjectManager.DataAccessLayer.Entity;
using ProjectManager.DataAccessLayer.Repository.Abtract;

namespace ProjectManager.DataAccessLayer.Extension
{
    public static class RoleRepositoryExtensions
    {
        public static Role GetSingleByRoleName(
            this IEntityRepository<Role> roleRepository, string roleName)
        {
            return roleRepository.GetAll().FirstOrDefault(x => x.Name == roleName);
        }
    }
}