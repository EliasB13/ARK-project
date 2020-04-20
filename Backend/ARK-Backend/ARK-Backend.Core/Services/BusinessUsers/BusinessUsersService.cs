using ARK_Backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ARK_Backend.Core.Helpers;
using ARK_Backend.Core.Services.Communication;
using ARK_Backend.Core.Dtos.Auth;
using ARK_Backend.Domain.Entities;

namespace ARK_Backend.Core.Services.BusinessUsers
{
	public class BusinessUsersService : IBusinessUsersService
	{
		private readonly ApplicationContext dbContext;

		public BusinessUsersService(ApplicationContext context)
		{
			dbContext = context;
		}

		public async Task<GenericServiceResponse<BusinessUser>> AuthenticateBusinessAsync(string login, string password)
		{
			try
			{
				var user = await dbContext.BusinessUsers.SingleOrDefaultAsync(u => u.Login == login);

				if (user == null)
					return new GenericServiceResponse<BusinessUser>($"User: { login } wasn't found", ErrorCode.ERROR_MOQ);

				if (!HashingExtensions.VerifyHash(password, user.PasswordHash, user.PasswordSalt))
					return new GenericServiceResponse<BusinessUser>($"Wrong credentials", ErrorCode.ERROR_MOQ);

				return new GenericServiceResponse<BusinessUser>(user);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<BusinessUser>("Error | Authenticating: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<BusinessUser>> RegisterBusinessAsync(RegisterBusinessRequest userDto)
		{
			try
			{
				if (await dbContext.BusinessUsers.AnyAsync(x => x.Login == userDto.Login))
					return new GenericServiceResponse<BusinessUser>("Username \"" + userDto.Login + "\" is already taken", ErrorCode.ERROR_MOQ);

				if (await dbContext.BusinessUsers.AnyAsync(x => x.Email == userDto.Email))
					return new GenericServiceResponse<BusinessUser>("Email \"" + userDto.Email + "\" is already taken", ErrorCode.ERROR_MOQ);

				byte[] passwordHash, passwordSalt;
				HashingExtensions.CreateHash(userDto.Password, out passwordHash, out passwordSalt);

				BusinessUser user = new BusinessUser()
				{
					Login = userDto.Login,
					Email = userDto.Email,
					CompanyName = userDto.CompanyName,
					PasswordHash = passwordHash,
					PasswordSalt = passwordSalt
				};

				await dbContext.BusinessUsers.AddAsync(user);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<BusinessUser>(user);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<BusinessUser>("Error | Registering business user: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<BusinessUser> GetByIdAsync(int id)
		{
			return await dbContext.BusinessUsers.FindAsync(id);
		}
	}
}
