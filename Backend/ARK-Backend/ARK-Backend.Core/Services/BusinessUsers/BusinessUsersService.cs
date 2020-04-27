﻿using ARK_Backend.Core.Dtos.Auth;
using ARK_Backend.Core.Dtos.BusinessUsers;
using ARK_Backend.Core.Dtos.PersonCards;
using ARK_Backend.Core.Dtos.Statistics;
using ARK_Backend.Core.Helpers;
using ARK_Backend.Core.Mappers;
using ARK_Backend.Core.Services.Communication;
using ARK_Backend.Domain.Entities;
using ARK_Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
					PasswordSalt = passwordSalt,
					EmployeesRoles = new List<EmployeesRole>()
					{
						new EmployeesRole() { IsAnonymous = true },
					}
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

		public async Task<GenericServiceResponse<BusinessUserAccountData>> GetAccountData(int id)
		{
			var user = await dbContext.BusinessUsers.FindAsync(id);
			if (user == null)
				return new GenericServiceResponse<BusinessUserAccountData>("User with specified id wasn't found", ErrorCode.USER_NOT_FOUND);

			return new GenericServiceResponse<BusinessUserAccountData>(user.ToAccountData());
		}

		public async Task<GenericServiceResponse<BusinessUserAccountData>> UpdateBusinessUser(UpdateBusinessUserRequest editData, int businessUserId)
		{
			var dbUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

			dbUser.UpdateUserFromDto(editData);
			dbContext.Entry(dbUser).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();

			return new GenericServiceResponse<BusinessUserAccountData>(dbUser.ToAccountData());
		}

		public async Task<GenericServiceResponse<BusinessUser>> DeleteBusinessUser(int businessUserId)
		{
			var dbUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

			dbContext.BusinessUsers.Remove(dbUser);
			await dbContext.SaveChangesAsync();

			return new GenericServiceResponse<BusinessUser>(dbUser);
		}

		public async Task<GenericServiceResponse<PersonCardDto>> AddPersonCard(int businessUserId, PersonCardDto dto)
		{
			try
			{
				var bUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

				EmployeesRole emplRole = null;

				TimeSpan startsAt, endsAt;

				if (dto.IsEmployee)
				{
					if (!dto.EmployeesRoleId.HasValue || dto.WorkingDayStartTime == null || dto.WorkingDayEndTime == null)
						return new GenericServiceResponse<PersonCardDto>($"Wrong params", ErrorCode.ERROR_MOQ);

					emplRole = await dbContext.EmployeesRoles.FindAsync(dto.EmployeesRoleId.Value);
					if (emplRole == null)
						return new GenericServiceResponse<PersonCardDto>($"Employees role with id: { dto.EmployeesRoleId.Value } wasn't found", ErrorCode.ERROR_MOQ);

					if (!TimeSpan.TryParse(dto.WorkingDayStartTime, out startsAt) || !TimeSpan.TryParse(dto.WorkingDayEndTime, out endsAt))
						return new GenericServiceResponse<PersonCardDto>("Wrong working day bounds", ErrorCode.ERROR_MOQ);
				}
				else
				{
					emplRole = await dbContext.EmployeesRoles.SingleAsync(er => er.BusinessUser.Id == bUser.Id && er.IsAnonymous);
					if (emplRole == null)
						return new GenericServiceResponse<PersonCardDto>($"Anonymous employees role wasn't found", ErrorCode.ERROR_MOQ);

					startsAt = new TimeSpan();
					endsAt = new TimeSpan();
				}

				var card = dto.ToPersonCard();
				card.Employees = new List<Employee>()
				{
					new Employee
					{
						PersonCard = card,
						EmployeesRole = emplRole,
						WorkingDayStartTime = startsAt,
						WorkingDayEndTime = endsAt
					}
				};

				string rfid = Guid.NewGuid().ToString().Substring(0, 20);
				card.RFIDNumber = rfid;

				await dbContext.PersonCards.AddAsync(card);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<PersonCardDto>(card.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardDto>("Error | Adding person card to business: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<PersonCardDto>> DeletePersonCard(int businessUserId, int personCardId)
		{
			try
			{
				var personCard = await dbContext.PersonCards.FindAsync(personCardId);
				if (personCard == null)
					return new GenericServiceResponse<PersonCardDto>($"Person card with id: { personCardId } wasn't found", ErrorCode.ERROR_MOQ);

				dbContext.PersonCards.Remove(personCard);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<PersonCardDto>(personCard.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardDto>("Error | Adding person card to business: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<IEnumerable<ReaderStatisticDto>>> GetFullStatistic(int businessUserId, DateTime lowerBound, DateTime upperBound)
		{
			try
			{
				if (lowerBound == DateTime.MinValue || upperBound == DateTime.MinValue)
					return new GenericServiceResponse<IEnumerable<ReaderStatisticDto>>($"You should pass time bounds", ErrorCode.ERROR_MOQ);

				var bUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

				var readersDto = await dbContext.Readers
					.Include(r => r.Observations)
						.ThenInclude(o => o.Person)
					.Where(r => r.BusinessUser.Id == businessUserId)
					.Select(r =>
						new ReaderStatisticDto
						{
							Id = r.ReaderId,
							Description = r.Description,
							IsEntrance = r.IsEntrance,
							Name = r.Name,
							Observations = r.Observations.Where(o => o.Time <= upperBound && o.Time >= lowerBound)
						}).ToListAsync();

				return new GenericServiceResponse<IEnumerable<ReaderStatisticDto>>(readersDto);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<ReaderStatisticDto>>("Error | Getting full statistic: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<ReaderStatisticDto>> GetReaderStatistic(int businessUserId, int readerId, DateTime lowerBound, DateTime upperBound)
		{
			try
			{
				if (lowerBound == DateTime.MinValue || upperBound == DateTime.MinValue)
					return new GenericServiceResponse<ReaderStatisticDto>($"You should pass time bounds", ErrorCode.ERROR_MOQ);

				var bUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

				var reader = await dbContext.Readers
					.Include(r => r.Observations)
						.ThenInclude(o => o.Person)
					.SingleOrDefaultAsync(r => r.ReaderId == readerId && r.BusinessUser.Id == businessUserId);
				if (reader == null)
					return new GenericServiceResponse<ReaderStatisticDto>($"Rearder with id: { readerId } wasn't found in business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				var dto = new ReaderStatisticDto
				{
					Id = reader.ReaderId,
					Name = reader.Name,
					Description = reader.Description,
					IsEntrance = reader.IsEntrance,
					Observations = reader.Observations
						.Where(o => o.Time <= upperBound && o.Time >= lowerBound)
						.Select(o =>
						{
							o.Reader = null;
							return o;
						})
				};

				return new GenericServiceResponse<ReaderStatisticDto>(dto);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderStatisticDto>("Error | Getting reader statistic: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<PersonCardStatisticDto>> GetPersonStatistic(int businessUserId, int personId, DateTime lowerBound, DateTime upperBound)
		{
			try
			{
				if (lowerBound == DateTime.MinValue || upperBound == DateTime.MinValue)
					return new GenericServiceResponse<PersonCardStatisticDto>($"You should pass time bounds", ErrorCode.ERROR_MOQ);

				var bUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

				var person = await dbContext.PersonCards.SingleOrDefaultAsync(p => p.Id == personId && p.Employees.Any(e => e.EmployeesRole.BusinessUser.Id == businessUserId));
				if (person == null)
					return new GenericServiceResponse<PersonCardStatisticDto>($"Person card with id: { personId } wasn't found in business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				var readers = await dbContext.Readers
					.Include(r => r.Observations)
					.Where(r => r.BusinessUser.Id == businessUserId)
					.Select(r => new ReaderStatisticDto
					{
						Id = r.ReaderId,
						Name = r.Name,
						Description = r.Description,
						IsEntrance = r.IsEntrance,
						Observations = r.Observations
							.Where(o => o.Person.Id == personId && o.Time >= lowerBound && o.Time <= upperBound).Select(o => o)
					}).ToListAsync();

				var personDto = new PersonCardStatisticDto
				{
					Id = person.Id,
					IsEmployee = person.IsEmployee,
					Name = person.Name,
					Surname = person.Surname,
					Readers = readers
				};

				return new GenericServiceResponse<PersonCardStatisticDto>(personDto);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardStatisticDto>("Error | Getting person card statistic: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}
	}
}
