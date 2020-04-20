using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class Reader
	{
		public int ReaderId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public byte[] SecretHash { get; set; }
		public byte[] SecretSalt { get; set; }

		public BusinessUser BusinessUser { get; set; }
		public IEnumerable<RestrictedRoleReader> RestrictedRoleReaders { get; set; }
		public IEnumerable<AllowedRoleReader> AllowedRoleReaders { get; set; }
	}
}
