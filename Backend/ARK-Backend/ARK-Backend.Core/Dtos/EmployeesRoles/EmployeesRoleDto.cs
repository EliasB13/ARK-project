﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.EmployeesRoles
{
	public class EmployeesRoleDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsAnonymous { get; set; }
	}
}
