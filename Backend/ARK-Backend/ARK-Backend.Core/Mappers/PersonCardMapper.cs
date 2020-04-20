using ARK_Backend.Core.Dtos.PersonCards;
using ARK_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Mappers
{
	public static class PersonCardMapper
	{
		public static PersonCard ToPersonCard(this PersonCardDto dto)
		{
			return new PersonCard
			{
				Name = dto.Name,
				Surname = dto.Surname,
				IsEmployee = dto.IsEmployee
			};
		}

		public static PersonCardDto ToDto(this PersonCard card)
		{
			return new PersonCardDto
			{
				Name = card.Name,
				Surname = card.Surname,
				IsEmployee = card.IsEmployee
			};
		}
	}
}
