using System;
using System.Collections.Generic;
using System.Text;
using ARK_Backend.Domain.Entities;

namespace ARK_Backend.Core.Dtos.Statistics
{
	public class ReaderStatisticDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsEntrance { get; set; }

		public IEnumerable<Observation> Observations { get; set; }
	}
}
