using ARK_Backend.Core.Dtos.Observations;
using ARK_Backend.Core.Dtos.Readers;
using ARK_Backend.Core.Helpers;
using ARK_Backend.Core.Mappers;
using ARK_Backend.Core.Services.Communication;
using ARK_Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARK_Backend.Core.Services.Readers
{
	public class ReadersService : IReadersService
	{
		private readonly ApplicationContext dbContext;

		public ReadersService(ApplicationContext context)
		{
			dbContext = context;
		}

		public async Task<GenericServiceResponse<ReaderDto>> AddReader(int businessUserId, ReaderDto dto)
		{
			try
			{
				var businessUser = await dbContext.BusinessUsers.FindAsync(businessUserId);

				var reader = dto.ToReader();
				reader.BusinessUser = businessUser;

				await dbContext.Readers.AddAsync(reader);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<ReaderDto>(dto);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderDto>("Error | Adding reader: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<IEnumerable<ReaderDto>>> GetAllReaders(int businessUserId)
		{
			try
			{
				var readers = await dbContext.Readers.Where(r => r.BusinessUser.Id == businessUserId).ToListAsync();
				if (readers.Count == 0)
					return new GenericServiceResponse<IEnumerable<ReaderDto>>(new List<ReaderDto>());

				var readerDtos = readers.Select(r => r.ToDto());
				return new GenericServiceResponse<IEnumerable<ReaderDto>>(readerDtos);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<IEnumerable<ReaderDto>> ("Error | Getting all readers: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<ReaderDto>> GetReaderById(int businessUserId, int readerId)
		{
			try
			{
				var reader = await dbContext.Readers.SingleOrDefaultAsync(r => r.ReaderId == readerId && r.BusinessUser.Id == businessUserId);
				if (reader == null)
					return new GenericServiceResponse<ReaderDto>($"Business user with id: { businessUserId } doesn't have reader with id: { readerId }", ErrorCode.READER_NOT_FOUND);

				return new GenericServiceResponse<ReaderDto>(reader.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderDto>("Error | Getting reader by id: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<ObservationResponse>> Observe(ObservationDto dto)
		{
			try
			{
				var reader = await dbContext.Readers.Include(r => r.BusinessUser).SingleOrDefaultAsync(r => r.ReaderId == dto.ReaderId);
				if (reader == null)
					return new GenericServiceResponse<ObservationResponse>($"Reader with id wasn't found: { dto.ReaderId }", ErrorCode.READER_NOT_FOUND);

				var person = await dbContext.PersonCards
					.Include(pc => pc.Employees)
						.ThenInclude(e => e.EmployeesRole)
					.SingleOrDefaultAsync(pc => pc.RFIDNumber == dto.PersonCardRfid);
				if (person == null) 
					return new GenericServiceResponse<ObservationResponse>($"Person card wasn't found", ErrorCode.PERSON_CARD_NOT_FOUND);

				var personRoles = person.Employees.Select(e => e.EmployeesRole.Id);
				var isRestricted = await dbContext.RestrictedRoleReaders
					.AnyAsync(rrr => rrr.Reader.ReaderId == dto.ReaderId && personRoles.Contains(rrr.EmployeesRole.Id));

				var observation = dto.ToObservation();
				observation.Person = person;
				observation.Reader = reader;
				observation.IsRestricted = isRestricted;

				await dbContext.Observations.AddAsync(observation);
				await dbContext.SaveChangesAsync();

				var response = new ObservationResponse
				{
					IsRestricted = isRestricted
				};

				return new GenericServiceResponse<ObservationResponse>(response);
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ObservationResponse>("Error | Saving observe: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}

		public async Task<GenericServiceResponse<ReaderDto>> RemoveReader(int businessUserId, int readerId)
		{
			try
			{
				var reader = await dbContext.Readers.SingleOrDefaultAsync(r => r.ReaderId == readerId && r.BusinessUser.Id == businessUserId);
				if (reader == null)
					return new GenericServiceResponse<ReaderDto>($"Business user with id: { businessUserId } doesn't have reader with id: { readerId }", ErrorCode.READER_NOT_FOUND);

				dbContext.Readers.Remove(reader);
				await dbContext.SaveChangesAsync();

				return new GenericServiceResponse<ReaderDto>(reader.ToDto());
			}
			catch (Exception ex)
			{
				return new GenericServiceResponse<ReaderDto>("Error | Getting reader by id: " + ex.Message, ErrorCode.INTERNAL_EXCEPTION);
			}
		}
	}
}
