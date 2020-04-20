using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Domain.Entities
{
	public class Observation
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }

		public PersonCard Person { get; set; }
		public Reader Reader { get; set; }
	}
}
