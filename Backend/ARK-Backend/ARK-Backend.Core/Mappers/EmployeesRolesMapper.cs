using ARK_Backend.Core.Dtos.EmployeesRoles;
using ARK_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Mappers
{
	public static class EmployeesRolesMapper
	{
		public static EmployeesRoleDto ToDto(this EmployeesRole role)
		{
			return new EmployeesRoleDto
			{
				Id = role.Id,
				Name = role.Name,
				Description = role.Description,
				IsAnonymous = role.IsAnonymous
			};
		}

		public static EmployeesRole ToRole(this AddEmployeesRoleRequest dto)
		{
			return new EmployeesRole
			{
				Name = dto.Name,
				Description = dto.Description
			};
		}

		public static void UpdateFromDto(this EmployeesRole role, EmployeesRoleDto dto)
		{
			role.Name = dto.Name;
			role.Description = dto.Description;
		}
	}
}
