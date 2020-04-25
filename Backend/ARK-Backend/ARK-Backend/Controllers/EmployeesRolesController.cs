using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ARK_Backend.Core.Dtos.Employee;
using ARK_Backend.Core.Dtos.EmployeesRoles;
using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Core.Services.EmployeesRoles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ARK_Backend.Controllers
{
	[Authorize]
    [Route("api/[controller]")]
    public class EmployeesRolesController : Controller
    {
		private readonly IEmployeesRolesService rolesService;

		public EmployeesRolesController(IEmployeesRolesService rolesService)
		{
			this.rolesService = rolesService;
		}

		[HttpGet]
		public async Task<IActionResult> GetEmployeesRoles()
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.GetEmployeesRoles(contextUserId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpGet("employees-in-role/{roleId}")]
		public async Task<IActionResult> GetEmployeesInRole(int roleId)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.GetEmployeesInRole(contextUserId, roleId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpGet("role/{roleId}")]
		public async Task<IActionResult> GetEmployeesRole(int roleId)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.GetEmployeesRole(contextUserId, roleId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPost("add-role")]
		public async Task<IActionResult> AddEmployeesRole([FromBody]EmployeesRoleDto roleDto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.AddEmployeesRole(contextUserId, roleDto);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpDelete("role/{roleId}")]
		public async Task<IActionResult> DeleteEmployeesRole(int roleId)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.DeleteEmployeesRole(contextUserId, roleId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPut("role")]
		public async Task<IActionResult> UpdateEmployeesRole([FromBody]EmployeesRoleDto updateDto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.UpdateEmployeesRole(contextUserId, updateDto);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPost("add-employee-to-role")]
		public async Task<IActionResult> AddEmployeeToRole([FromBody]AddEmployeeToRoleDto dto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.AddEmployeeToRole(contextUserId, dto.RoleId, dto.PersonCardId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPost("remove-employee-from-role")]
		public async Task<IActionResult> RemoveEmployeeFromRole([FromBody]RemoveEmployeeFromRoleDto dto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.AddEmployeeToRole(contextUserId, dto.RoleId, dto.PersonCardId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		[HttpPost("restrict-reader")]
		public async Task<IActionResult> RestrictReaderInRole([FromBody]RestrictReaderDto dto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.RestrictReaderInRole(contextUserId, dto.RoleId, dto.ReaderId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}

		public async Task<IActionResult> UnrestrictReaderInRole([FromBody]RestrictReaderDto dto)
		{
			int contextUserId = int.Parse(HttpContext.User.Identity.Name);

			var result = await rolesService.UnrestrictReaderInRole(contextUserId, dto.RoleId, dto.ReaderId);
			if (!result.Success)
				return BadRequest(new { message = result.ErrorMessage, code = result.ErrorCode });

			return Ok(result.Item);
		}
	}
}