using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TicketManagementMVC.Controllers
{
    //[Authorize] // Ensures only authenticated users can access
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            ViewBag.Message = "Welcome to the Ticket Management System!";
            return View();
        }
    }
}
