using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ARK_Backend.Core.Dtos.Observations;
using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Core.Services.Readers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ARK_Backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class ReadersController : Controller
	{
		private readonly IReadersService readersService;

		public ReadersController(IReadersService readersService)
		{
			this.readersService = readersService;
		}

		[HttpGet("all-readers")]
		public async Task<IActionResult> GetAllReaders()
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await readersService.GetAllReaders(contextUserId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPost("add-reader")]
		public async Task<IActionResult> AddReader([FromBody]ReaderDto dto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await readersService.AddReader(contextUserId, dto);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpDelete("reader/{readerId}")]
		public async Task<IActionResult> RemoveReader(int readerId)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await readersService.RemoveReader(contextUserId, readerId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[AllowAnonymous]
		[HttpPost("observe")]
		public async Task<IActionResult> Observe([FromBody]ObservationDto dto)
		{
			var result = await readersService.Observe(dto);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok();
		}
	}
}
