using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.Statistics
{
	public class ObservationStatistcDto
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }

		public PersonCardStatisticDto Person { get; set; }
		public ReaderStatisticDto Reader { get; set; }
	}
}
