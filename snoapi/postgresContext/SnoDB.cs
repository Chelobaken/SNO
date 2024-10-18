using Microsoft.EntityFrameworkCore;

namespace SNO.API;

public partial class SnoDB : DbContext
{
    public SnoDB()
    {
    }

    public SnoDB(DbContextOptions<SnoDB> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Projectauthor> Projectauthors { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder){
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Eventid).HasName("events_pkey");

            entity.ToTable("events");

            entity.Property(e => e.Eventid).HasColumnName("eventid");
            entity.Property(e => e.Begindate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("begindate");
            entity.Property(e => e.Estimationdate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("estimationdate");
            entity.Property(e => e.Mddescription).HasColumnName("mddescription");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.Projectid).HasName("projects_pkey");

            entity.ToTable("projects");

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Mddescription).HasColumnName("mddescription");
            entity.Property(e => e.Title)
                .HasMaxLength(120)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Projectauthor>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("projectauthors");

            entity.Property(e => e.Projectid).HasColumnName("projectid");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Project).WithMany()
                .HasForeignKey(d => d.Projectid)
                .HasConstraintName("projectauthors_projectid_fkey");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("projectauthors_userid_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.HasIndex(e => e.Phonenumber, "users_phonenumber_key").IsUnique();

            entity.HasIndex(e => e.Usernametg, "users_usernametg_key").IsUnique();

            entity.HasIndex(e => e.Usernamevk, "users_usernamevk_key").IsUnique();

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Firstname)
                .HasMaxLength(30)
                .HasColumnName("firstname");
            entity.Property(e => e.Lastname)
                .HasMaxLength(30)
                .HasColumnName("lastname");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(30)
                .HasColumnName("patronymic");
            entity.Property(e => e.Phonenumber).HasColumnName("phonenumber");
            entity.Property(e => e.Usernametg).HasColumnName("usernametg");
            entity.Property(e => e.Usernamevk).HasColumnName("usernamevk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
