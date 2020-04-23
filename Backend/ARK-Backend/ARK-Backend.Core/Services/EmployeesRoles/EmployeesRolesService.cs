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
					return new GenericServiceResponse<IEnumerable<EmployeesRoleDto>>($"Roles were not found with business user id: { businessUserId }", ErrorCode.ERROR_MOQ);

				var roleDtos = roles.Select(r => r.ToDto());

				return new GenericServiceResponse<IEnumerable<EmployeesRoleDto>>(roleDtos);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<EmployeesRoleDto>>("Error | Get employees roles: " + ex.Message, ErrorCode.ERROR_MOQ);
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
					return new GenericServiceResponse<IEnumerable<PersonCardDto>>($"Role with id: { roleId } weren't found", ErrorCode.ERROR_MOQ);

				if (role.BusinessUser.Id != businessUserId)
					return new GenericServiceResponse<IEnumerable<PersonCardDto>>($"Role with id: { roleId } doesn't belong to business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				if (role.IsAnonymous)
					return new GenericServiceResponse<IEnumerable<PersonCardDto>>($"Role with id: { roleId } is anonymous", ErrorCode.ERROR_MOQ);

				var cards = role.Employees.Select(e =>
					new PersonCardDto
					{
						Name = e.PersonCard.Name,
						EmployeesRoleId = e.EmployeesRole.Id,
						Surname = e.PersonCard.Surname,
						IsEmployee = e.PersonCard.IsEmployee,
						WorkingDayStartTime = TimeSpanDtoConverter.TimeSpanToString(e.WorkingDayStartTime),
						WorkingDayEndTime = TimeSpanDtoConverter.TimeSpanToString(e.WorkingDayEndTime)
					}
				).ToList();

				return new GenericServiceResponse<IEnumerable<PersonCardDto>>(cards);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<PersonCardDto>>("Error | Get employees in role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> GetEmployeesRole(int businessUserId, int roleId)
		{
			try
			{
				var role = await dbContext.EmployeesRoles
				.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);

				if (role == null)
					return new GenericServiceResponse<EmployeesRoleDto>($"Business user with id: { businessUserId } doesn't have role with id: { roleId }", ErrorCode.ERROR_MOQ);

				return new GenericServiceResponse<EmployeesRoleDto>(role.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Get employees role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> AddEmployeesRole(int businessUserId, EmployeesRoleDto roleDto)
		{
			try
			{
				var sameNameRoleExists = await dbContext.EmployeesRoles.AnyAsync(er => er.Name == roleDto.Name);
				if (sameNameRoleExists)
					return new GenericServiceResponse<EmployeesRoleDto>($"Role with name: { roleDto.Name } already exists", ErrorCode.ERROR_MOQ);

				var role = roleDto.ToRole();
				role.BusinessUser.Id = businessUserId;

				await dbContext.EmployeesRoles.AddAsync(role);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<EmployeesRoleDto>(roleDto);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Adding employees role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> DeleteEmployeesRole(int businessUserId, int roleId)
		{
			try
			{
				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<EmployeesRoleDto>($"Role with id: { roleId } doesn't exists", ErrorCode.ERROR_MOQ);

				dbContext.EmployeesRoles.Remove(role);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<EmployeesRoleDto>(role.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Deleting employees role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<EmployeesRoleDto>> UpdateEmployeesRole(int businessUserId, EmployeesRoleDto updateDto)
		{
			try
			{
				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == updateDto.Id && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<EmployeesRoleDto>($"Role with id: { updateDto.Id } doesn't exists", ErrorCode.ERROR_MOQ);

				role.UpdateFromDto(updateDto);

				dbContext.EmployeesRoles.Update(role);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<EmployeesRoleDto>(role.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<EmployeesRoleDto>("Error | Updating employees role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<PersonCardDto>> AddEmployeeToRole(int businessUserId, int roleId, int personCardId)
		{
			try
			{
				var personCard = await dbContext.PersonCards.Include(pc => pc.Employees).ThenInclude(e => e.EmployeesRole).SingleOrDefaultAsync(pc => pc.Id == personCardId);
				if (personCard == null)
					return new GenericServiceResponse<PersonCardDto>($"Person card with id: { personCardId } doesn't exists", ErrorCode.ERROR_MOQ);

				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<PersonCardDto>($"Role with id: { roleId } doesn't exists", ErrorCode.ERROR_MOQ);

				var isEmployeeExists = await dbContext.PersonCards.AnyAsync(pc => pc.Id == personCardId && pc.Employees.Any(e => e.EmployeesRole.Id == roleId));
				if (isEmployeeExists)
					return new GenericServiceResponse<PersonCardDto>($"Employee with person card id: { personCardId } in role with: { roleId } already exists", ErrorCode.ERROR_MOQ);

				var employee = personCard.Employees.FirstOrDefault();
				employee.EmployeesRole = role;

				dbContext.Employees.Update(employee);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<PersonCardDto>(personCard.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardDto>("Error | Adding employee to role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<PersonCardDto>> RemoveEmployeeFromRole(int businessUserId, int roleId, int personCardId)
		{
			try
			{
				var personCard = await dbContext.PersonCards.Include(pc => pc.Employees).ThenInclude(e => e.EmployeesRole).SingleOrDefaultAsync(pc => pc.Id == personCardId);
				if (personCard == null)
					return new GenericServiceResponse<PersonCardDto>($"Person card with id: { personCardId } doesn't exists", ErrorCode.ERROR_MOQ);

				var role = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.Id == roleId && er.BusinessUser.Id == businessUserId);
				if (role == null)
					return new GenericServiceResponse<PersonCardDto>($"Role with id: { roleId } doesn't exists", ErrorCode.ERROR_MOQ);

				var isEmployeeExists = await dbContext.PersonCards.AnyAsync(pc => pc.Id == personCardId && !pc.Employees.Any(e => e.EmployeesRole.Id == roleId));
				if (isEmployeeExists)
					return new GenericServiceResponse<PersonCardDto>($"Employee with person card id: { personCardId } in role with: { roleId } doesn't exists", ErrorCode.ERROR_MOQ);

				var employee = personCard.Employees.SingleOrDefault(pc => pc.EmployeesRole.Id == role.Id);
				var unassignedRole = await dbContext.EmployeesRoles.SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.IsAnonymous);
				employee.EmployeesRole = unassignedRole;

				dbContext.Employees.Update(employee);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<PersonCardDto>(personCard.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<PersonCardDto>("Error | Removing employee to role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<ReaderDto>> RestrictReaderInRole(int businessUserId, int roleId, int readerId)
		{
			try
			{
				var reader = await dbContext.Readers.SingleOrDefaultAsync(r => r.BusinessUser.Id == businessUserId && r.ReaderId == readerId);
				if (reader == null)
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } wasn't found in business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				var role = await dbContext.EmployeesRoles.Include(er => er.RestrictedRoleReaders).SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.Id == roleId);
				if (role == null)
					return new GenericServiceResponse<ReaderDto>($"Role with id: { roleId } wasn't found in business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				if (role.RestrictedRoleReaders.Any(rrr => rrr.Reader.ReaderId == readerId))
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } already restricted for role with id: { roleId }", ErrorCode.ERROR_MOQ);

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
				return new GenericServiceResponse<ReaderDto>("Error | Restricting reader for role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}

		public async Task<GenericServiceResponse<ReaderDto>> UnrestrictReaderInRole(int businessUserId, int roleId, int readerId)
		{
			try
			{
				var reader = await dbContext.Readers.SingleOrDefaultAsync(r => r.BusinessUser.Id == businessUserId && r.ReaderId == readerId);
				if (reader == null)
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } wasn't found in business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				var role = await dbContext.EmployeesRoles.Include(er => er.RestrictedRoleReaders).SingleOrDefaultAsync(er => er.BusinessUser.Id == businessUserId && er.Id == roleId);
				if (role == null)
					return new GenericServiceResponse<ReaderDto>($"Role with id: { roleId } wasn't found in business user with id: { businessUserId }", ErrorCode.ERROR_MOQ);

				var rrr = await dbContext.RestrictedRoleReaders.SingleOrDefaultAsync(rrr => rrr.Reader.ReaderId == readerId && rrr.EmployeesRole.Id == role.Id);
				if (rrr == null)
					return new GenericServiceResponse<ReaderDto>($"Reader with id: { readerId } is not restricted in role with id: { roleId }", ErrorCode.ERROR_MOQ);

				dbContext.RestrictedRoleReaders.Remove(rrr);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<ReaderDto>(reader.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderDto>("Error | Unrestricting reader for role: " + ex.Message, ErrorCode.ERROR_MOQ);
			}
		}
	}
}
