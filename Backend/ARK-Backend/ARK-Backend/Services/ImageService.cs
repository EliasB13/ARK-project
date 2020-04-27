using ARK_Backend.Core.Helpers;
using ARK_Backend.Core.Services.Communication;
using ARK_Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ARK_Backend.Services
{
	public class ImageService : IImageService
	{
		private readonly ApplicationContext dbContext;

		public ImageService(ApplicationContext dbContext)
		{
			this.dbContext = dbContext;
		}

		public async Task<GenericServiceResponse<string>> UploadCardPicture(IFormFile file, int personCardId)
		{
			try
			{
				if (file == null || file.Length == 0)
					return new GenericServiceResponse<string>("File was not found", ErrorCode.FILE_NOT_FOUND);

				var folderName = Path.Combine("Resources", "ProfilePics");
				var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

				if (!Directory.Exists(filePath))
				{
					Directory.CreateDirectory(filePath);
				}

				var uniqueFileName = $"person_card_{ personCardId }_pic.png";
				var dbPath = Path.Combine(folderName, uniqueFileName);

				var personCard = await dbContext.PersonCards.FindAsync(personCardId);
				if (personCard == null)
					return new GenericServiceResponse<string>($"Person card with id: { personCardId } wasn't found", ErrorCode.PERSON_CARD_NOT_FOUND);

				personCard.Photo = dbPath;
				dbContext.PersonCards.Update(personCard);
				await dbContext.SaveChangesAsync();

				using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
				}

				return new GenericServiceResponse<string>(dbPath);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<string>("Error | Updating photo person card" + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}
	}
}
