using ARK_Backend.Core.Helpers;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Services.Communication
{
	public class GenericServiceResponse<T> : BaseServiceResponse where T : class
	{
		public T Item { get; set; }
		public ErrorCode ErrorCode { get; set; }

		private GenericServiceResponse(T item, bool success, string message) : base(success, message)
		{
			Item = item;
		}

		public GenericServiceResponse(T item) : this(item, true, string.Empty)
		{ }

		public GenericServiceResponse(string errorMessage, ErrorCode errorCode) : this(null, false, errorMessage)
		{
			ErrorCode = errorCode;
			Log.Logger.Error(errorMessage);
		}
	}
}
