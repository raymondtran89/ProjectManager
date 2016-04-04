using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using ProjectManager.DataAccessLayer.Entity;
using ProjectManager.DataAccessLayer.Extension;
using ProjectManager.DataAccessLayer.Repository.Abtract;
using ProjectManager.DataAccessLayer.Repository.Helper;
using ProjectManager.ServiceLayer.Abstract;
using ProjectManager.ServiceLayer.Infractructure;

namespace ProjectManager.ServiceLayer.Concrete
{
    public class MembershipService : IMembershipService
    {
        private readonly ICryptoService _cryptoService;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IEntityRepository<UserInRole> _userInRoleRepository;
        private readonly IEntityRepository<User> _userRepository;

        public MembershipService(
            IEntityRepository<User> userRepository,
            IEntityRepository<Role> roleRepository,
            IEntityRepository<UserInRole> userInRoleRepository,
            ICryptoService cryptoService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userInRoleRepository = userInRoleRepository;
            _cryptoService = cryptoService;
        }

        public ValidUserContext ValidateUser(string username, string password)
        {
            var userCtx = new ValidUserContext();
            User user = _userRepository.GetSingleByUsername(username);
            if (user != null && IsUserValid(user, password))
            {
                IEnumerable<Role> userRoles = GetUserRoles(user.Key);
                Role[] enumerable = userRoles as Role[] ?? userRoles.ToArray();
                userCtx.User = new UserWithRoles
                {
                    User = user,
                    Roles = enumerable
                };
                var identity = new GenericIdentity(user.Name);
                userCtx.Principal = new GenericPrincipal(
                    identity,
                    enumerable.Select(x => x.Name).ToArray());
            }
            return userCtx;
        }

        public OperationResult<UserWithRoles> CreateUser(
            string username, string email, string password)
        {
            return CreateUser(username, password, email, roles: null);
        }

        public OperationResult<UserWithRoles> CreateUser(
            string username, string email, string password, string role)
        {
            return CreateUser(username, password, email, new[] { role });
        }

        public OperationResult<UserWithRoles> CreateUser(
            string username, string email, string password, string[] roles)
        {
            bool existingUser = _userRepository.GetAll().Any(
                x => x.Name == username);
            if (existingUser)
            {
                return new OperationResult<UserWithRoles>(false);
            }
            string passwordSalt = _cryptoService.GenerateSalt();
            var user = new User
            {
                Name = username,
                Salt = passwordSalt,
                Email = email,
                IsLocked = false,
                HashedPassword =
                    _cryptoService.EncryptPassword(password, passwordSalt),
                CreatedOn = DateTime.Now
            };
            _userRepository.Add(user);
            _userRepository.Save();
            if (roles != null || roles.Length > 0)
            {
                foreach (string roleName in roles)
                {
                    addUserToRole(user, roleName);
                }
            }
            return new OperationResult<UserWithRoles>(true)
            {
                Entity = GetUserWithRoles(user)
            };
        }

        public UserWithRoles UpdateUser(User user, string username, string email)
        {
            throw new NotImplementedException();
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public bool AddToRole(Guid userKey, string role)
        {
            throw new NotImplementedException();
        }

        public bool AddToRole(string username, string role)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFromRole(string username, string role)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Role> GetRoles()
        {
            throw new NotImplementedException();
        }

        public Role GetRole(Guid key)
        {
            throw new NotImplementedException();
        }

        public Role GetRole(string name)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<UserWithRoles> GetUsers(int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public UserWithRoles GetUser(Guid key)
        {
            throw new NotImplementedException();
        }

        public UserWithRoles GetUser(string name)
        {
            throw new NotImplementedException();
        }

        private UserWithRoles GetUserWithRoles(User user)
        {
            var roles = _userInRoleRepository.FindBy(x => x.UserKey == user.Key).Select(x => x.Role).ToList();
            return new UserWithRoles
            {
                User = user,
                Roles = roles
            };
        }

        private bool IsUserValid(User user, string password)
        {
            if (IsPasswordValid(user, password))
            {
                return !user.IsLocked;
            }
            return false;
        }

        private IEnumerable<Role> GetUserRoles(Guid userKey)
        {
            List<UserInRole> userInRoles = _userInRoleRepository
                .FindBy(x => x.UserKey == userKey).ToList();
            if (userInRoles != null && userInRoles.Count > 0)
            {
                Guid[] userRoleKeys = userInRoles.Select(
                    x => x.RoleKey).ToArray();
                IQueryable<Role> userRoles = _roleRepository
                    .FindBy(x => userRoleKeys.Contains(x.Key));
                return userRoles;
            }
            return Enumerable.Empty<Role>();
        }

        private bool IsPasswordValid(User user, string password)
        {
            return string.Equals(
                _cryptoService.EncryptPassword(
                    password, user.Salt), user.HashedPassword);
        }

        private void addUserToRole(User user, string roleName)
        {
            Role role = _roleRepository.GetSingleByRoleName(roleName);
            if (role == null)
            {
                var tempRole = new Role
                {
                    Name = roleName
                };
                _roleRepository.Add(tempRole);
                _roleRepository.Save();
                role = tempRole;
            }
            var userInRole = new UserInRole
            {
                RoleKey = role.Key,
                UserKey = user.Key
            };
            _userInRoleRepository.Add(userInRole);
            _userInRoleRepository.Save();
        }
    }
}