using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TechScreen.DBEntities
{
    public partial class TechscreenDBContext : DbContext
    {
        public TechscreenDBContext()
        {
        }

        public TechscreenDBContext(DbContextOptions<TechscreenDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<CustomScreeningQuestions> CustomScreeningQuestions { get; set; }
        public virtual DbSet<DetailedCandidateScreening> DetailedCandidateScreening { get; set; }
        public virtual DbSet<JobCategories> JobCategories { get; set; }
        public virtual DbSet<Reviewer> Reviewer { get; set; }
        public virtual DbSet<ReviewerTechnologies> ReviewerTechnologies { get; set; }
        public virtual DbSet<Screening> Screening { get; set; }
        public virtual DbSet<ScreeningCandidate> ScreeningCandidate { get; set; }
        public virtual DbSet<ScreeningQuestions> ScreeningQuestions { get; set; }
        public virtual DbSet<Technologies> Technologies { get; set; }
        public virtual DbSet<TechnologyScreeningQuestions> TechnologyScreeningQuestions { get; set; }
        public virtual DbSet<TechnologyStack> TechnologyStack { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
//            if (!optionsBuilder.IsConfigured)
//            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("Server=techscreensqlserver.database.windows.net;Initial Catalog=TechscreenDB;Persist Security Info=True;User ID=shivpinal;Password=Prat!ma48");
//            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.CompanyName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomScreeningQuestions>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.QuestionText)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.CustomScreeningQuestions)
                    .HasForeignKey(d => d.ScreeningId)
                    .HasConstraintName("FK_CustomScreeningQuestions_Screening");

                entity.HasOne(d => d.Tech)
                    .WithMany(p => p.CustomScreeningQuestions)
                    .HasForeignKey(d => d.TechId)
                    .HasConstraintName("FK_CustomScreeningQuestions_Technologies");

                entity.HasOne(d => d.TechStack)
                    .WithMany(p => p.CustomScreeningQuestions)
                    .HasForeignKey(d => d.TechStackId)
                    .HasConstraintName("FK_CustomScreeningQuestions_TechnologyStack");
            });

            modelBuilder.Entity<DetailedCandidateScreening>(entity =>
            {
                entity.HasKey(e => e.DetailScreeningId);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReviwerComments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SkillName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.DetailedCandidateScreening)
                    .HasForeignKey(d => d.CandidateId)
                    .HasConstraintName("FK_DetailedCandidateScreening_ScreeningCandidate");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.DetailedCandidateScreening)
                    .HasForeignKey(d => d.ReviewerId)
                    .HasConstraintName("FK_DetailedCandidateScreening_Reviewer");

                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.DetailedCandidateScreening)
                    .HasForeignKey(d => d.ScreeningId)
                    .HasConstraintName("FK_DetailedCandidateScreening_Screening");
            });

            modelBuilder.Entity<JobCategories>(entity =>
            {
                entity.HasKey(e => e.JobCatId);

                entity.Property(e => e.JobCatId).ValueGeneratedNever();

                entity.Property(e => e.JobCatDesc)
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Reviewer>(entity =>
            {
                entity.Property(e => e.Address1)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone1)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone2)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rate)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ResumeLink)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ReviewerTechnologies>(entity =>
            {
                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.ReviewerTechnologies)
                    .HasForeignKey(d => d.ReviewerId)
                    .HasConstraintName("FK_ReviewerTechnologies_Reviewer");
            });

            modelBuilder.Entity<Screening>(entity =>
            {
                entity.Property(e => e.AdminStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ExperienceLevel)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HiringCompanyName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.IsClientScreeningQ)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IsClientScreeningQuploaded)
                    .HasColumnName("IsClientScreeningQUploaded")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.IsJobDescUploaded)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.JobDesc).IsUnicode(false);

                entity.Property(e => e.JobLocation)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.JobRequisitionNumber)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.JobScreeningQuestions).IsUnicode(false);

                entity.Property(e => e.JobScreeningQuestionsUrl)
                    .HasColumnName("JobScreeningQuestionsURL")
                    .IsUnicode(false);

                entity.Property(e => e.JobTitle)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.OptionalSkills)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.RequiredSkills)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewerStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialRequest).IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.JobCat)
                    .WithMany(p => p.Screening)
                    .HasForeignKey(d => d.JobCatId)
                    .HasConstraintName("FK_Screening_JobCategories");

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.Screening)
                    .HasForeignKey(d => d.ReviewerId)
                    .HasConstraintName("FK_Screening_Reviewer");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Screening)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Screening_User");
            });

            modelBuilder.Entity<ScreeningCandidate>(entity =>
            {
                entity.HasKey(e => e.CandidateId);

                entity.Property(e => e.CandidateEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CandidateFirstName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CandidateLastName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CandidatePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CandidateSignInCode)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ReviewerComments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.ScreeningStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Reviewer)
                    .WithMany(p => p.ScreeningCandidate)
                    .HasForeignKey(d => d.ReviewerId)
                    .HasConstraintName("FK_ScreeningCandidate_Reviewer");

                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.ScreeningCandidate)
                    .HasForeignKey(d => d.ScreeningId)
                    .HasConstraintName("FK_ScreeningCandidate_Screening");
            });

            modelBuilder.Entity<ScreeningQuestions>(entity =>
            {
                entity.Property(e => e.QuestionText)
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.ScreeningQuestions)
                    .HasForeignKey(d => d.ScreeningId)
                    .HasConstraintName("FK_ScreeningQuestions_Screening");
            });

            modelBuilder.Entity<Technologies>(entity =>
            {
                entity.HasKey(e => e.TechId)
                    .HasName("PK_Technology");

                entity.Property(e => e.TechName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.TechStack)
                    .WithMany(p => p.Technologies)
                    .HasForeignKey(d => d.TechStackId)
                    .HasConstraintName("FK_Technologies_TechnologyStack");
            });

            modelBuilder.Entity<TechnologyScreeningQuestions>(entity =>
            {
                entity.Property(e => e.ScreeningQuestion)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Tech)
                    .WithMany(p => p.TechnologyScreeningQuestions)
                    .HasForeignKey(d => d.TechId)
                    .HasConstraintName("FK_TechnologyScreeningQuestions_Technologies");

                entity.HasOne(d => d.TechStack)
                    .WithMany(p => p.TechnologyScreeningQuestions)
                    .HasForeignKey(d => d.TechStackId)
                    .HasConstraintName("FK_TechnologyScreeningQuestions_TechnologyStack");
            });

            modelBuilder.Entity<TechnologyStack>(entity =>
            {
                entity.HasKey(e => e.TechStackId);

                entity.Property(e => e.TechSuiteName)
                    .HasMaxLength(500)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Screening)
                    .WithMany(p => p.Transaction)
                    .HasForeignKey(d => d.ScreeningId)
                    .HasConstraintName("FK_Transaction_Screening");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Designation)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.IsAdmin)
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.LastUpdated).HasColumnType("datetime");

                entity.Property(e => e.LastUpdatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserFirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserLastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserPhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Client)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.ClientId)
                    .HasConstraintName("FK_User_Client");
            });
        }
    }
}
