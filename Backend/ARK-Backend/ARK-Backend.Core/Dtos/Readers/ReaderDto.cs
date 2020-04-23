using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Dtos.Readers
{
	public class ReaderDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsEntrance { get; set; }
	}
}
