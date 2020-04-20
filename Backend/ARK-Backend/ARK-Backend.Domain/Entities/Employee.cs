using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class Employee
	{
		public int Id { get; set; }
		[Column(TypeName = "time(0)")]
		public TimeSpan WorkingDayStartTime { get; set; }
		[Column(TypeName = "time(0)")]
		public TimeSpan WorkingDayEndTime { get; set; }

		public PersonCard Person { get; set; }
		public EmployeesRole EmployeesRole { get; set; }
	}
}
