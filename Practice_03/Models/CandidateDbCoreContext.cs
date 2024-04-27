using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Practice_03.Models;

public partial class CandidateDbCoreContext : DbContext
{
    public CandidateDbCoreContext()
    {
    }

    public CandidateDbCoreContext(DbContextOptions<CandidateDbCoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Candidate> Candidates { get; set; }

    public virtual DbSet<CandidateSkill> CandidateSkills { get; set; }

    public virtual DbSet<Skill> Skills { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CandidateDB_core;MultipleActiveResultSets=True;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Candidate>(entity =>
        {
            entity.HasKey(e => e.CandidateId).HasName("PK__Candidat__DF539B9CD31B83F5");

            entity.Property(e => e.CandidateName).HasMaxLength(50);
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.Phone).HasMaxLength(50);
        });

        modelBuilder.Entity<CandidateSkill>(entity =>
        {
            entity.HasKey(e => e.CandidateSkillId).HasName("PK__Candidat__BCA6023480E87B8C");

            entity.HasOne(d => d.Candidate).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.CandidateId)
                .HasConstraintName("FK__Candidate__Candi__286302EC");

            entity.HasOne(d => d.Skill).WithMany(p => p.CandidateSkills)
                .HasForeignKey(d => d.SkillId)
                .HasConstraintName("FK__Candidate__Skill__29572725");
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasKey(e => e.SkillId).HasName("PK__Skills__DFA0918799C89F60");

            entity.Property(e => e.SkillName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
