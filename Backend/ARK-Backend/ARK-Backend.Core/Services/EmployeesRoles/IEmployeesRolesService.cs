using ARK_Backend.Core.Dtos.EmployeesRoles;
using ARK_Backend.Core.Dtos.PersonCards;
using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Core.Services.Communication;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ARK_Backend.Core.Services.EmployeesRoles
{
	public interface IEmployeesRolesService
	{
		Task<GenericServiceResponse<IEnumerable<EmployeesRoleDto>>> GetEmployeesRoles(int businessUserId);
		Task<GenericServiceResponse<IEnumerable<PersonCardDto>>> GetEmployeesInRole(int businessUserId, int roleId);
		Task<GenericServiceResponse<EmployeesRoleDto>> GetEmployeesRole(int businessUserId, int roleId);
		Task<GenericServiceResponse<IEnumerable<RestrictReaderDto>>> GetRestrictedRoleReaders(int businessUserId, int roleId);
		Task<GenericServiceResponse<AddEmployeesRoleRequest>> AddEmployeesRole(int businessUserId, AddEmployeesRoleRequest roleDto);
		Task<GenericServiceResponse<EmployeesRoleDto>> DeleteEmployeesRole(int businessUserId, int roleId);
		Task<GenericServiceResponse<EmployeesRoleDto>> UpdateEmployeesRole(int businessUserId, EmployeesRoleDto roleDto);
		Task<GenericServiceResponse<PersonCardDto>> AddEmployeeToRole(int businessUserId, int roleId, int personCardId);
		Task<GenericServiceResponse<PersonCardDto>> RemoveEmployeeFromRole(int businessUserId, int roleId, int employeeId);
		Task<GenericServiceResponse<ReaderDto>> RestrictReaderInRole(int businessUserId, int roleId, int readerId);
		Task<GenericServiceResponse<ReaderDto>> UnrestrictReaderInRole(int businessUserId, int roleId, int readerId);
	}
}
