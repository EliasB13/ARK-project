using ARK_Backend.Core.Dtos.BusinessUsers;
using ARK_Backend.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Core.Mappers
{
	public static class BusinessUsersMapper
	{
		public static BusinessUserAccountData ToAccountData(this BusinessUser user)
		{
			return new BusinessUserAccountData
			{
				Address = user.Address,
				CompanyName = user.CompanyName,
				Description = user.Description,
				Email = user.Email,
				Id = user.Id,
				Login = user.Login,
				Phone = user.Phone
			};
		}

		public static void UpdateUserFromDto(this BusinessUser user, UpdateBusinessUserRequest userData)
		{
			user.Address = userData.Address;
			user.CompanyName = userData.CompanyName;
			user.Description = userData.Description;
			user.Email = userData.Email;
			user.Login = userData.Login;
			user.Phone = userData.Phone;
		}
	}
}
