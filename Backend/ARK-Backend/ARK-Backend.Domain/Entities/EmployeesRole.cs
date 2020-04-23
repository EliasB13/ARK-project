using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class EmployeesRole
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsAnonymous { get; set; }
		
		public BusinessUser BusinessUser { get; set; }
		public IEnumerable<RestrictedRoleReader> RestrictedRoleReaders { get; set; }
		public IEnumerable<Employee> Employees { get; set; }
	}
}
