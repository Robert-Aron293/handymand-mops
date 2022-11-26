using Handymand.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Handymand.Data
{
    public class HandymandContext : DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Freelancer> Freelancers { get; set; }

        public DbSet<Skill> Skills { get; set; }
        public DbSet<FreelancersSkills> FreelancersSkills { get; set; }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<JobOffer> JobOffer { get; set; }
        public DbSet<JobOffersSkills> JobOffersSkills { get; set; }
        public DbSet<Offer> Offers { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<NotificationAffectedList> NotificationAffectedLists { get; set; }

        public DbSet<NotificationType> NotificationTypes { get; set; }

        public HandymandContext(DbContextOptions<HandymandContext> options) : base(options)
        {

        }


		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var configuration = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json")
				.Build();

			var connectionString = configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseSqlServer(connectionString);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //Properties

            //modelBuilder.Entity<User>().Property(u => u.Amount).HasPrecision(12, 2);

            modelBuilder.Entity<Contract>().Property(u => u.PaymentAmountPerHour).HasColumnType("float");

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.JobOffer).WithOne(j => j.Contract)
                .HasForeignKey<Contract>(c => c.JobOfferId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.JobOffer).WithOne(j => j.Feedback)
                .HasForeignKey<Feedback>(f => f.JobOfferId);

            //One to Many

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.CreationUser).WithMany(u => u.CreatedNotifications)
                .HasForeignKey(n => n.CreationUserId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.ReferredUser).WithMany(u => u.ReceivedNotifications)
                .HasForeignKey(n => n.ReferredUserId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.JobOffer).WithMany(j => j.ReferredNotifications)
                .HasForeignKey(n => n.JobOfferId);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.NotificationType).WithMany(nt => nt.Notifications)
                .HasForeignKey(n => n.NotificationTypeId);

            modelBuilder.Entity<NotificationAffectedList>()
                .HasOne(nal => nal.NotificationType).WithMany(nt => nt.NotificationAffectedLists)
                .HasForeignKey(nal => nal.NotificationTypeId);

            modelBuilder.Entity<Feedback>()
                .HasOne(f => f.CreationUser).WithMany(u => u.CreatedFeedbacks).HasForeignKey(f => f.CreationUserId);

            modelBuilder.Entity<Feedback>()
                 .HasOne(f => f.RefferedUser).WithMany(u => u.ReceivedFeedback).HasForeignKey(f => f.RefferedUserId);



            modelBuilder.Entity<Contract>()
                .HasOne(c => c.CreationUser).WithMany(u => u.CreatedContracts).HasForeignKey(c => c.CreationUserId);

            modelBuilder.Entity<Contract>()
                .HasOne(c => c.RefferedUser).WithMany(u => u.AcceptedContracts).HasForeignKey(c => c.RefferedUserId);

            modelBuilder.Entity<JobOffer>()
                .HasOne(j => j.City)
                .WithMany(c => c.JobOffers)
                .HasForeignKey(j => j.CityId);

            modelBuilder.Entity<JobOffer>()
                .HasMany(j => j.Offers)
                .WithOne(o => o.JobOffer)
                .HasForeignKey(o => o.JobOfferId);


            //One to One

            modelBuilder.Entity<User>()
                .HasOne(u => u.FreelancerAccount).WithOne(f => f.User).HasForeignKey<Freelancer>(f => f.IdUser);

            modelBuilder.Entity<User>()
                .HasOne(u => u.ClientAccount).WithOne(c => c.User).HasForeignKey<Client>(c => c.IdUser);

            //Many to Many

            modelBuilder.Entity<JobOffersSkills>().HasKey(key => new
            {
                key.IdSkill,
                key.IdJobOffer
            });

            modelBuilder.Entity<JobOffersSkills>()
                .HasOne<Skill>(js => js.Skill)
                .WithMany(s => s.JobOffersSkills)
                .HasForeignKey(js => js.IdSkill);

            modelBuilder.Entity<JobOffersSkills>()
                .HasOne<JobOffer>(js => js.JobOffer)
                .WithMany(j => j.JobOffersSkills)
                .HasForeignKey(js => js.IdJobOffer);

            modelBuilder.Entity<FreelancersSkills>().HasKey(key => new
            {
                key.IdFreelancer,
                key.IdSkill
            });

            modelBuilder.Entity<FreelancersSkills>()
                .HasOne<Freelancer>(fs => fs.Freelancer).WithMany(f => f.FreelancersSkills)
                .HasForeignKey(fs => fs.IdFreelancer);

            modelBuilder.Entity<FreelancersSkills>()
                .HasOne<Skill>(fs => fs.Skill).WithMany(s => s.FreelancersSkills)
                .HasForeignKey(fs => fs.IdSkill);



            modelBuilder.Entity<Skill>(u =>
            {
                u.Property<DateTime>("DateCreated")
                    .HasDefaultValueSql("GETDATE()");

                u.Property<DateTime?>("DateModified")
                    .HasComputedColumnSql("GETDATE()");
            });


            //Seed Citites

            modelBuilder.Entity<City>().HasData
                (
                    new City() { Id = 1, Name = "Bucuresti"},
                    new City() { Id = 2, Name = "Iasi" },
                    new City() { Id = 3, Name = "Cluj" },
                    new City() { Id = 4, Name = "Timisoara" },
                    new City() { Id = 5, Name = "Constanta" },
                    new City() { Id = 6, Name = "Craiova" },
                    new City() { Id = 7, Name = "Brasov" },
                    new City() { Id = 8, Name = "Galati" },
                    new City() { Id = 9, Name = "Ploiesti" },
                    new City() { Id = 10, Name = "Oradea" },
                    new City() { Id = 11, Name = "Braila" },
                    new City() { Id = 12, Name = "Arad" },
                    new City() { Id = 13, Name = "Buzau"},
                    new City() { Id = 14, Name = "Botosani" },
                    new City() { Id = 15, Name = "Suceava" },
                    new City() { Id = 16, Name = "Drobeta-Turnu Severin"},
                    new City() { Id = 17, Name = "Slatina" },
                    new City() { Id = 18, Name = "Deva" }
                );


            modelBuilder.Entity<NotificationType>().HasData(
                new NotificationType() { Id = 1, Description = "Create Offer", DateCreated=DateTime.Now},
                new NotificationType() { Id = 2, Description = "Accept Offer", DateCreated = DateTime.Now },
                new NotificationType() { Id = 3, Description = "Close Contract", DateCreated = DateTime.Now }
                );


            modelBuilder.Entity<NotificationAffectedList>().HasData(
                new NotificationAffectedList() { Id = 1, Name="My Active Offers", DateCreated = DateTime.Now, NotificationTypeId = 1},
                new NotificationAffectedList() { Id = 2, Name = "My Accepted Offers", DateCreated = DateTime.Now, NotificationTypeId = 2 },
                new NotificationAffectedList() { Id = 3, Name = "My Active Job Offers", DateCreated = DateTime.Now,NotificationTypeId = 1},
                new NotificationAffectedList() { Id = 4, Name = "Jobs To Pay For", DateCreated = DateTime.Now, NotificationTypeId = 2 },
                new NotificationAffectedList() { Id = 5, Name = "Closed Job Contracts", DateCreated = DateTime.Now, NotificationTypeId = 3 }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
