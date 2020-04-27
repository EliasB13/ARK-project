using ARK_Backend.Core.Dtos.EmployeesRoles;
using ARK_Backend.Core.Dtos.PersonCards;
using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Core.Services.Communication;
using ARK_Backend.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ARK_Backend.Core.Helpers;
using ARK_Backend.Core.Mappers;
using System.Threading.Tasks;
using ARK_Backend.Domain.Entities;

namespace ARK_Backend.Core.Services.EmployeesRoles
{
	public class EmployeesRolesService : IEmployeesRolesService
	{
		private readonly ApplicationContext dbContext;

		public EmployeesRolesService(ApplicationContext context)
		{
			dbContext = context;
		}

		public async Task<GenericServiceResponse<IEnumerable<EmployeesRoleDto>>> GetEmployeesRoles(int businessUserId)
		{
			try
			{
				var roles = await dbContext.EmployeesRoles.Where(er => er.BusinessUser.Id == businessUserId).ToListAsync();
				if (roles.Count == 0)
					return new GenericServiceResponse<IEnumerable<EmployeesRoleDto>>(new List<EmployeesRoleDto>());

				var roleDtos = roles.Select(r => r.ToDto());

				return new GenericServiceResponse<IEnumerable<EmployeesRoleDto>>(roleDtos);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<EmployeesRoleDto>>("Error | Get employees roles: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		} 

		public async Task<GenericServiceResponse<IEnumerable<PersonCardDto>>> GetEmployeesInRole(int businessUserId, int roleId)
		{
			try
			{
				var role = await dbContext.EmployeesRoles
				.Include(er => er.BusinessUser)
				.Include(er => er.Employees)
					.ThenInclude(e => e.PersonCard)
				.SingleOrDefaultAsync(er => er.Id == roleId);

				if (role == null)
					return new GenericServiceResponse<IEnumerable<PersonCardDto>>($"Role with id: { roleId } weren't found", ErrorCode.ROLE_NOT_FOUND);

				if (role.BusinessUser.Id != businessUserId)
					return new GenericServiceResponse<IEnumerable<PersonCardDto>>($"Role with id: { roleId } doesn't belong to business user with id: { businessUserId }", ErrorCode.ROLE_DOESNT_BELONG_TO_BUSINESS_USER);

				var cards = role.Employees.Select(e =>
					new PersonCardDto
					{
						PersonCardId = e.PersonCard.Id,
						Name = e.PersonCard.Name,
						EmployeesRoleId = e.EmployeesRole.Id,
						Surname = e.PersonCard.Surname,
						IsEmployee = e.PersonCard.IsEmployee,
						WorkingDayStartTime = TimeSpanDtoConverter.TimeSpanToString(e.WorkingDayStartTime),
						WorkingDayEndTime = TimeSpanDtoConverter.TimeSpanToString(e.WorkingDayEndTime),
						Photo = e.PersonCard.Photo
					}
				).ToList();

				return new GenericServiceResponse<IEnumerable<PersonCardDto>>(cards);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<PersonCardDto>>("Error | Get employees in role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> GetEmployeesRole(int businessUserId, int roleId)
		{
			try
			{
				var role = await dbContext.EmployeesRoles
					.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);

				if (role == null)
					return new GenericServiceResponse<EmployeesRoleDto>($"Business user with id: { businessUserId } doesn't have role with id: { roleId }", ErrorCode.ROLE_DOESNT_BELONG_TO_BUSINESS_USER);

				return new GenericServiceResponse<EmployeesRoleDto>(role.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Get employees role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<AddEmployeesRoleRequest>> AddEmployeesRole(int businessUserId, AddEmployeesRoleRequest roleDto)
		{
			try
			{
				var sameNameRoleExists = await dbContext.EmployeesRoles.AnyAsync(er => er.Name == roleDto.Name);
				if (sameNameRoleExists)
					return new GenericServiceResponse<AddEmployeesRoleRequest>($"Role with name: { roleDto.Name } already exists", ErrorCode.ROLE_ALREADY_EXISTS);

				var businessUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

				var role = roleDto.ToRole();
				role.BusinessUser = businessUser;

				await dbContext.EmployeesRoles.AddAsync(role);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<AddEmployeesRoleRequest>(roleDto);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<AddEmployeesRoleRequest>("Error | Adding employees role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> DeleteEmployeesRole(int businessUserId, int roleId)
		{
			try
			{
				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<EmployeesRoleDto>($"Role with id: { roleId } doesn't exists", ErrorCode.ROLE_NOT_FOUND);

				dbContext.EmployeesRoles.Remove(role);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<EmployeesRoleDto>(role.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Deleting employees role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> UpdateEmployeesRole(int businessUserId, EmployeesRoleDto updateDto)
		{
			try
			{
				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == updateDto.Id && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<EmployeesRoleDto>($"Role with id: { updateDto.Id } doesn't exists", ErrorCode.ROLE_NOT_FOUND);

				role.UpdateFromDto(updateDto);

				dbContext.EmployeesRoles.Update(role);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<EmployeesRoleDto>(role.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Updating employees role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<PersonCardDto>> AddEmployeeToRole(int businessUserId, int roleId, int personCardId)
		{
			try
			{
				var personCard = await dbContext.PersonCards.Include(pc => pc.Employees).ThenInclude(e => e.EmployeesRole).SingleOrDefaultAsync(pc => pc.Id == personCardId);
				if (personCard == null)
					return new GenericServiceResponse<PersonCardDto>($"Person card with id: { personCardId } doesn't exists", ErrorCode.PERSON_CARD_NOT_FOUND);

				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<PersonCardDto>($"Role with id: { roleId } doesn't exists", ErrorCode.ROLE_NOT_FOUND);

				var isEmployeeExists = await dbContext.PersonCards.AnyAsync(pc => pc.Id == personCardId && pc.Employees.Any(e => e.EmployeesRole.Id == roleId));
				if (isEmployeeExists)
					return new GenericServiceResponse<PersonCardDto>($"Employee with person card id: { personCardId } in role with: { roleId } already exists", ErrorCode.EMPLOYEE_ALREADY_EXISTS);

				if (!personCard.IsEmployee)
					return new GenericServiceResponse<PersonCardDto>($"Person card is not an employee", ErrorCode.PERSON_NOT_EMPLOYEE);

				var employee = personCard.Employees.FirstOrDefault();
				employee.EmployeesRole = role;

				dbContext.Employees.Update(employee);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<PersonCardDto>(personCard.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardDto>("Error | Adding employee to role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<PersonCardDto>> RemoveEmployeeFromRole(int businessUserId, int roleId, int personCardId)
		{
			try
			{
				var personCard = await dbContext.PersonCards.Include(pc => pc.Employees).ThenInclude(e => e.EmployeesRole).SingleOrDefaultAsync(pc => pc.Id == personCardId);
				if (personCard == null)
					return new GenericServiceResponse<PersonCardDto>($"Person card with id: { personCardId } doesn't exists", ErrorCode.PERSON_CARD_NOT_FOUND);

				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<PersonCardDto>($"Role with id: { roleId } doesn't exists", ErrorCode.ROLE_NOT_FOUND);

				var isEmployeeExists = await dbContext.PersonCards.AnyAsync(pc => pc.Id == personCardId && !pc.Employees.Any(e => e.EmployeesRole.Id == roleId));
				if (isEmployeeExists)
					return new GenericServiceResponse<PersonCardDto>($"Employee with person card id: { personCardId } in role with: { roleId } doesn't exists", ErrorCode.EMPLOYEE_NOT_FOUND);


				var employee = personCard.Employees.SingleOrDefault(pc => pc.EmployeesRole.Id == role.Id);
				var unassignedRole = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.IsAnonymous);
				employee.EmployeesRole = unassignedRole;

				dbContext.Employees.Update(employee);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<PersonCardDto>(personCard.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardDto>("Error | Removing employee to role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<ReaderDto>> RestrictReaderInRole(int businessUserId, int roleId, int readerId)
		{
			try
			{
				var reader = await dbContext.Readers.SingleOrDefaultAsync(r => r.BusinessUser.Id == businessUserId && r.ReaderId == readerId);
				if (reader == null)
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } wasn't found in business user with id: { businessUserId }", ErrorCode.READER_NOT_FOUND);

				var role = await dbContext.EmployeesRoles.Include(er => er.RestrictedRoleReaders).SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.Id == roleId);
				if (role == null)
					return new GenericServiceResponse<ReaderDto>($"Role with id: { roleId } wasn't found in business user with id: { businessUserId }", ErrorCode.ROLE_NOT_FOUND);

				if (role.RestrictedRoleReaders.Any(rrr => rrr.Reader.ReaderId == readerId))
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } already restricted for role with id: { roleId }", ErrorCode.READER_ALREADY_RESTRICTED);

				var rrr = new RestrictedRoleReader
				{
					EmployeesRole = role,
					Reader = reader
				};

				await dbContext.RestrictedRoleReaders.AddAsync(rrr);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<ReaderDto>(reader.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderDto>("Error | Restricting reader for role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<ReaderDto>> UnrestrictReaderInRole(int businessUserId, int roleId, int readerId)
		{
			try
			{
				var reader = await dbContext.Readers.SingleOrDefaultAsync(r => r.BusinessUser.Id == businessUserId && r.ReaderId == readerId);
				if (reader == null)
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } wasn't found in business user with id: { businessUserId }", ErrorCode.READER_NOT_FOUND);

				var role = await dbContext.EmployeesRoles.Include(er => er.RestrictedRoleReaders).SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.Id == roleId);
				if (role == null)
					return new GenericServiceResponse<ReaderDto>($"Role with id: { roleId } wasn't found in business user with id: { businessUserId }", ErrorCode.ROLE_NOT_FOUND);

				var rrr = await dbContext.RestrictedRoleReaders.SingleOrDefaultAsync(rrr => rrr.Reader.ReaderId == readerId && rrr.EmployeesRole.Id == role.Id);
				if (rrr == null)
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } is not restricted in role with id: { roleId }", ErrorCode.READER_NOT_RESTRICTED);

				dbContext.RestrictedRoleReaders.Remove(rrr);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<ReaderDto>(reader.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderDto>("Error | Unrestricting reader for role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<IEnumerable<RestrictReaderDto>>> GetRestrictedRoleReaders(int businessUserId, int roleId)
		{
			try
			{
				var role = await dbContext.EmployeesRoles
					.Include(er => er.RestrictedRoleReaders)
						.ThenInclude(er => er.Reader)
					.SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.Id == roleId);
				if (role == null)
					return new GenericServiceResponse<IEnumerable<RestrictReaderDto>>($"Role with id: { roleId } wasn't found in business user with id: { businessUserId }", ErrorCode.ROLE_NOT_FOUND);

				var rrrs = role.RestrictedRoleReaders.Select(rrr => new RestrictReaderDto
				{
					ReaderId = rrr.Reader.ReaderId,
					RoleId = role.Id
				});

				return new GenericServiceResponse<IEnumerable<RestrictReaderDto>>(rrrs);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<RestrictReaderDto>>("Error | Get employees role: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}
	}
}
