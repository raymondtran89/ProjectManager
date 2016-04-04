using ProjectManager.ServiceLayer.Abstract;

namespace ProjectManager.ServiceLayer.Concrete
{
    public class AccountServices : IAccountServices
    {
        public int TotalCount(int a, int b)
        {
            return a + b;
        }
    }
}