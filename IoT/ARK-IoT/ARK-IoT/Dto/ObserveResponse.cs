using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_IoT.Dto
{
	public class ObserveResponse
	{
		public bool IsRestricted { get; set; }
		public string Message { get; set; }
		public int Code { get; set; }
	}
}
