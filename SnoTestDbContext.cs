using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SNO;

public partial class SnoTestDbContext : DbContext
{
    public SnoTestDbContext()
    {
    }

    public SnoTestDbContext(DbContextOptions<SnoTestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name = ConnectionStrings:SnoTestDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Begindate).HasColumnName("begindate");
            entity.Property(e => e.Estimationdate).HasColumnName("estimationdate");
            entity.Property(e => e.Mddescription)
                .HasMaxLength(3500)
                .HasColumnName("mddescription");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("title");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
