using ARK_Backend.Core.Dtos.Observations;
using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ARK_Backend.Core.Services.Readers
{
	public interface IReadersService
	{
		Task<GenericServiceResponse<IEnumerable<ReaderDto>>> GetAllReaders(int businessUserId);
		Task<GenericServiceResponse<ReaderDto>> AddReader(int businessUserId, ReaderDto dto);
		Task<GenericServiceResponse<ReaderDto>> RemoveReader(int businessUserId, int readerId);
		Task<GenericServiceResponse<ObservationResponse>> Observe(ObservationDto dto);
		Task<GenericServiceResponse<ReaderDataDto>> GetReaderData(int readerId);
	}
}
