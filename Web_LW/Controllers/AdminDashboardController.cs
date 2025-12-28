using Microsoft.AspNetCore.Mvc;

public class AdminDashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
