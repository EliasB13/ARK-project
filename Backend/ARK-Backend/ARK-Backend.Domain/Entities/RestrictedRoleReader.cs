using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class RestrictedRoleReader
	{
		public int Id { get; set; }

		public Reader Reader { get; set; }
		public EmployeesRole EmployeesRole { get; set; }
	}
}
