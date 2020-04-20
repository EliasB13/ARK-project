using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class Person
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		[MaxLength(20)]
		public string RFIDNumber { get; set; }

		public IEnumerable<Employee> Employees { get; set; }
	}
}
