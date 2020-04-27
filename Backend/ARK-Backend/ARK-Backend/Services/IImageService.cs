using ARK_Backend.Core.Services.Communication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ARK_Backend.Services
{
	public interface IImageService
	{
		Task<GenericServiceResponse<string>> UploadCardPicture(IFormFile file, int personCardId);
	}
}
