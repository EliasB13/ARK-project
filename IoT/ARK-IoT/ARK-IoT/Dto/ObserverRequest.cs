using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_IoT.Dto
{
	public class ObserveRequest
	{
		public int ReaderId { get; set; }
		public string Secret { get; set; }
		public int PersonCardRfid { get; set; }
		public DateTime Time { get; set; }
	}
}
