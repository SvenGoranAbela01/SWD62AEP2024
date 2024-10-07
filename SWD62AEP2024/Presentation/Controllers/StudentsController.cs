using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class StudentsController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
