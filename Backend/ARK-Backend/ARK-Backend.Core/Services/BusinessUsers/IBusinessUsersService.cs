using ARK_Backend.Core.Dtos.Auth;
using ARK_Backend.Core.Dtos.BusinessUsers;
using ARK_Backend.Core.Dtos.PersonCards;
using ARK_Backend.Core.Dtos.Statistics;
using ARK_Backend.Core.Services.Communication;
using ARK_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ARK_Backend.Core.Services.BusinessUsers
{
	public interface IBusinessUsersService
	{
		Task<GenericServiceResponse<BusinessUser>> RegisterBusinessAsync(RegisterBusinessRequest userDto);
		Task<GenericServiceResponse<BusinessUser>> AuthenticateBusinessAsync(string login, string password);
		Task<BusinessUser> GetByIdAsync(int id);
		Task<GenericServiceResponse<IEnumerable<PersonCardDto>>> GetPersonCards(int businessUserId);
		Task<GenericServiceResponse<BusinessUserAccountData>> UpdateBusinessUser(UpdateBusinessUserRequest editData, int businessUserId);
		Task<GenericServiceResponse<BusinessUser>> DeleteBusinessUser(int businessUserId);
		Task<GenericServiceResponse<BusinessUserAccountData>> GetAccountData(int id);
		Task<GenericServiceResponse<PersonCardDto>> AddPersonCard(int businessUserId, PersonCardDto personCard);
		Task<GenericServiceResponse<PersonCardDto>> DeletePersonCard(int businessUserId, int personCardId);
		Task<GenericServiceResponse<IEnumerable<ReaderStatisticDto>>> GetFullStatistic(int businessUserId, DateTime lowerBound, DateTime upperBound);
		Task<GenericServiceResponse<IEnumerable<ReaderCountStatisticDto>>> GetFullCountStatistic(int businessUserId, DateTime lowerBound, DateTime upperBound);
		Task<GenericServiceResponse<ReaderStatisticDto>> GetReaderStatistic(int businessUserId, int readerId, DateTime lowerBound, DateTime upperBound);
		Task<GenericServiceResponse<ReaderCountStatisticDto>> GetReaderCountStatistic(int businessUserId, int readerId, DateTime lowerBound, DateTime upperBound);
		Task<GenericServiceResponse<PersonCardStatisticDto>> GetPersonStatistic(int businessUserId, int personId, DateTime lowerBound, DateTime upperBound);
	}
}
