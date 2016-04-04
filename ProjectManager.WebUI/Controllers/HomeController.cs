using System.Web.Mvc;
using ProjectManager.ServiceLayer.Abstract;

namespace ProjectManager.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountServices _accountServices;

        public HomeController(IAccountServices accountServices)
        {
            _accountServices = accountServices;
        }

        // GET: Home
        public ActionResult Index()
        {
            int value = _accountServices.TotalCount(1, 7);
            return View(value);
        }
    }
}