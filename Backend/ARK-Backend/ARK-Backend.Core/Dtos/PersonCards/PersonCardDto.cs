using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.PersonCards
{
	public class PersonCardDto
	{
		public int PersonCardId { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public bool IsEmployee { get; set; }
		public int? EmployeesRoleId { get; set; }
		public string WorkingDayStartTime { get; set; }
		public string WorkingDayEndTime { get; set; }
	}
}
