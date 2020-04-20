using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ARK_Backend.Core.Dtos.Auth
{
	public class AuthenticateRequest
	{
		[Required]
		public string Login { get; set; }
		[Required]
		public string Password { get; set; }
	}
}
