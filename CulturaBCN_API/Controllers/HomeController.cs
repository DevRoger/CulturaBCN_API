using System.Web.Http;
using System.Web.Mvc;

namespace CulturaBCN.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}