using Microsoft.AspNetCore.Mvc;

namespace StudentCountApp.Controllers;

// UI / pages
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
