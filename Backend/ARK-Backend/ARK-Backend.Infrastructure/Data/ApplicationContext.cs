using ARK_Backend.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ARK_Backend.Infrastructure.Data
{
	public class ApplicationContext : DbContext
	{
		public DbSet<BusinessUser> BusinessUsers { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<EmployeesRole> EmployeesRoles { get; set; }
		public DbSet<Observation> Observations { get; set; }
		public DbSet<Person> Persons { get; set; }
		public DbSet<Reader> Readers { get; set; }
		public DbSet<RestrictedRoleReader> RestrictedRoleReaders { get; set; }
		public DbSet<AllowedRoleReader> AllowedRoleReaders { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{

		}
	}
}
