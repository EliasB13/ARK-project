using ARK_IoT.Configuration;
using ARK_IoT.Dto;
using ARK_IoT.Menu;
using Flurl;
using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace ARK_IoT.Services
{
	public class ReaderService
	{
		private readonly ReaderSettings readerSettings;

		public ReaderService(ReaderSettings readerSettings)
		{
			this.readerSettings = readerSettings;
		}

		public async Task<ObserveResponse> Observe(string rfid)
		{
			try
			{
				var res = await readerSettings.ConnectionString
					.AppendPathSegments("api", "Readers", "observe")
					.AllowHttpStatus("400-404")
					.PostJsonAsync(new { time = DateTime.Now, readerId = readerSettings.ReaderId, personCardRfid = rfid, secret = readerSettings.SecretKey })
					.ReceiveJson<ObserveResponse>();
				return res;
			}
			catch (FlurlHttpException ex)
			{
				ConsoleMenu.DrawError(ex.Message);
				Environment.Exit(1);
				return new ObserveResponse() { Message = ex.Message };
			}
		}

		public async Task<ReaderDataResponse> GetReaderData()
		{
			try
			{
				var res = await readerSettings.ConnectionString
					.AppendPathSegments("api", "Readers", "reader-data", readerSettings.ReaderId.ToString())
					.AllowHttpStatus("400-404")
					.GetJsonAsync<ReaderDataResponse>();
				return res;
			}
			catch (FlurlHttpException ex)
			{
				ConsoleMenu.DrawError(ex.Message);
				Environment.Exit(1);
				return new ReaderDataResponse() { Message = ex.Message };
			}
		}
	}
}
