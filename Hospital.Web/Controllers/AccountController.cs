using Microsoft.AspNetCore.Mvc;

namespace Hospital.Controllers;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return RedirectToAction("Login", "Patients");
    }
}