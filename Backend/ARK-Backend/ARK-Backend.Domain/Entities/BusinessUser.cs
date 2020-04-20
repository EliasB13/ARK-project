using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class BusinessUser
	{
		public int Id { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public string CompanyName { get; set; }
		public string Description { get; set; }
		public string Address { get; set; }
		public string Phone { get; set; }
		public byte[] PasswordHash { get; set; }
		public byte[] PasswordSalt { get; set; }
		public bool IsEmailConfirmed { get; set; }
		public string Photo { get; set; }

		public IEnumerable<Reader> Readers { get; set; }
		public IEnumerable<EmployeesRole> EmployeesRoles { get; set; }
	}
}
