using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Mappers
{
	public static class ReaderMapper
	{
		public static ReaderDto ToDto(this Reader reader)
		{
			return new ReaderDto
			{
				Id = reader.ReaderId,
				Description = reader.Description,
				Name = reader.Name,
				IsEntrance = reader.IsEntrance
			};
		}

		public static Reader ToReader(this ReaderDto dto)
		{
			return new Reader
			{
				Name = dto.Name,
				Description = dto.Description,
				IsEntrance = dto.IsEntrance,
			};
		}
	}
}
