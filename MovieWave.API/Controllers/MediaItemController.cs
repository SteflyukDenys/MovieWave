using Microsoft.AspNetCore.Mvc;

namespace MovieWave.API.Controllers;

public class MediaItemController : ControllerBase
{
	// GET
	public IActionResult Index()
	{
		return View();
	}
}
