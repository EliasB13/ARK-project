using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.Statistics
{
	public class PersonCardStatisticDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Surname { get; set; }
		public bool IsEmployee { get; set; }

		public IEnumerable<ReaderStatisticDto> Readers { get; set; }
	}
}
