using System.Security.Principal;

namespace ProjectManager.ServiceLayer.Infractructure
{
    public class ValidUserContext
    {
        public UserWithRoles User { get; set; }
        public GenericPrincipal Principal { get; set; }
    }
}