using Microsoft.AspNetCore.Mvc;

public class UserDashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
