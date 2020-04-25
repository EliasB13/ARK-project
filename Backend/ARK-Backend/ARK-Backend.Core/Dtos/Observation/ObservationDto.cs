using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.Observation
{
	public class ObservationDto
	{
		public DateTime Time { get; set; }
		public int ReaderId { get; set; }
		public string PersonCardRfid { get; set; }
		public string ReaderSecret { get; set; }
	}
}
