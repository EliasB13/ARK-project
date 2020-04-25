using ARK_Backend.Core.Dtos.Observation;
using ARK_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Mappers
{
	public static class ObservationMapper
	{
		public static Observation ToObservation(this ObservationDto dto)
		{
			return new Observation
			{
				Time = dto.Time
			};
		}
	}
}
