using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARK_Backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class ReadersController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
