using ARK_Backend.Core.Dtos.Auth;
using ARK_Backend.Core.Dtos.BusinessUsers;
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
		Task<GenericServiceResponse<BusinessUserAccountData>> UpdateBusinessUser(UpdateBusinessUserRequest editData, int businessUserId);
		Task<GenericServiceResponse<BusinessUser>> DeleteBusinessUser(int businessUserId);
		Task<GenericServiceResponse<BusinessUserAccountData>> GetAccountData(int id);
	}
}
