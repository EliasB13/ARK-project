using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class Employee
	{
		public int Id { get; set; }
		
		public Person Person { get; set; }

		public EmployeesRole EmployeesRole { get; set; }
	}
}
