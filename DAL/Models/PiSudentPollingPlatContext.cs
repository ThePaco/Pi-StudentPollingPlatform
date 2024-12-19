using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class PiSudentPollingPlatContext : DbContext
{
    public PiSudentPollingPlatContext()
    {
    }

    public PiSudentPollingPlatContext(DbContextOptions<PiSudentPollingPlatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kolegij> Kolegijs { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Poll> Polls { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAnswer> UserAnswers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.;Database=piSudentPollingPlat;User=sa;Password=SQL;TrustServerCertificate=True;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kolegij>(entity =>
        {
            entity.HasKey(e => e.Idkolegij).HasName("PK__Kolegij__2D60268DE904C4D1");

            entity.ToTable("Kolegij");

            entity.Property(e => e.Idkolegij).HasColumnName("IDKolegij");
            entity.Property(e => e.KolegijName).HasMaxLength(200);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.Level).HasMaxLength(50);
        });

        modelBuilder.Entity<Poll>(entity =>
        {
            entity.ToTable("Poll");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.KolegijId).HasColumnName("KolegijID");
            entity.Property(e => e.PollDate).HasColumnType("datetime");
            entity.Property(e => e.Tekst).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(50);

            entity.HasOne(d => d.Kolegij).WithMany(p => p.Polls)
                .HasForeignKey(d => d.KolegijId)
                .HasConstraintName("FK__Poll__KolegijID__5DCAEF64");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.ToTable("Question");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.PollId).HasColumnName("PollID");
            entity.Property(e => e.Tekst).HasMaxLength(255);

            entity.HasOne(d => d.Poll).WithMany(p => p.Questions)
                .HasForeignKey(d => d.PollId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_Poll");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PwdHash).HasMaxLength(256);
            entity.Property(e => e.PwdSalt).HasMaxLength(256);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<UserAnswer>(entity =>
        {
            entity.ToTable("UserAnswer");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Question).WithMany(p => p.UserAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAnswer_Question");

            entity.HasOne(d => d.User).WithMany(p => p.UserAnswers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserAnswer_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
