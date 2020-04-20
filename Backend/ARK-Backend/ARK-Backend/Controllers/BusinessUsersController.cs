using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ARK_Backend.Core.Dtos.Auth;
using ARK_Backend.Core.Dtos.BusinessUsers;
using ARK_Backend.Core.Dtos.PersonCards;
using ARK_Backend.Core.Helpers;
using ARK_Backend.Core.Services.BusinessUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ARK_Backend.Controllers
{
	[Authorize]
	[Route("api/[controller]")]
	public class BusinessUsersController : Controller
	{
		private readonly IBusinessUsersService usersService;
		private readonly AppSettings appSettings;

		public BusinessUsersController(IBusinessUsersService usersService,
			IOptions<AppSettings> appSettings)
		{
			this.appSettings = appSettings.Value;
			this.usersService = usersService;
		}

		[HttpGet("account-data")]
		public async Task<IActionResult> GetAccountData()
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await usersService.GetAccountData(contextUserId);
			if (!result.Success)
			{
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });
			}

			return Ok(result.Item);
		}

		[AllowAnonymous]
		[HttpPost("authenticate-business")]
		public async Task<IActionResult> AuthenticateBusiness([FromBody]AuthenticateRequest dto)
		{
			var result = await usersService.AuthenticateBusinessAsync(dto.Login, dto.Password);

			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = ErrorCode.ERROR_MOQ });

			var user = result.Item;

			var token = GetTokenString(result.Item.Id);

			return Ok(new
			{
				Id = user.Id,
				Login = user.Login,
				Token = token,
				Photo = user.Photo
			});
		}

		[AllowAnonymous]
		[HttpPost("register-business")]
		public async Task<IActionResult> RegisterBusiness([FromBody]RegisterBusinessRequest dto)
		{
			var result = await usersService.RegisterBusinessAsync(dto);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok();
		}

		[HttpPost("add-person-card")]
		public async Task<IActionResult> AddPersonCard([FromBody]PersonCardDto dto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await usersService.AddPersonCard(contextUserId, dto);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody]UpdateBusinessUserRequest businessUser)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await usersService.UpdateBusinessUser(businessUser, contextUserId);
			if (!result.Success)
			{
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });
			}

			return Ok(result.Item);
		}

		[HttpDelete]
		public async Task<IActionResult> Delete()
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await usersService.DeleteBusinessUser(contextUserId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok();
		}

		[HttpDelete("person-card/{personCardId}")]
		public async Task<IActionResult> DeletePersonCard(int personCardId)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await usersService.DeletePersonCard(contextUserId, personCardId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok();
		}

		private string GetTokenString(int userId)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(appSettings.Secret);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, userId.ToString())
				}),
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
