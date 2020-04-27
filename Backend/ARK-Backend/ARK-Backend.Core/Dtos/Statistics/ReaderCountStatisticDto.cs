using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.Statistics
{
	public class ReaderCountStatisticDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsEntrance { get; set; }

		public int EmployeesObservationsCount { get; set; }
		public int AnonymObservationsCount { get; set; }
	}
}
