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
		public DbSet<PersonCard> PersonCards { get; set; }
		public DbSet<Reader> Readers { get; set; }
		public DbSet<RestrictedRoleReader> RestrictedRoleReaders { get; set; }

		public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PersonCard>()
				.HasMany(pc => pc.Employees)
				.WithOne(e => e.PersonCard)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<BusinessUser>()
				.HasMany(bu => bu.EmployeesRoles)
				.WithOne(er => er.BusinessUser)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
