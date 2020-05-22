using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_IoT.Configuration
{
	public class ReaderSettings
	{
		public int ReaderId { get; set; }
		public string ConnectionString { get; set; }
		public string SecretKey { get; set; }
	}
}
