using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hospital.Controllers;

public class BaseController : Controller
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        base.OnActionExecuted(context);

        ViewBag.LoggedInUserName = User.Identity?.Name!;
    }
}