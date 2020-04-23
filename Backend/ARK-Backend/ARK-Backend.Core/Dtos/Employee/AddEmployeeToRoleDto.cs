using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.Employee
{
	public class AddEmployeeToRoleDto
	{
		public int RoleId { get; set; }
		public int PersonCardId { get; set; }
	}
}
