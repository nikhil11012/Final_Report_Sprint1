using Fracto.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Fracto.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Specialization> Specializations => Set<Specialization>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Rating> Ratings => Set<Rating>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<Specialization>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder.Entity<Doctor>()
            .HasOne(x => x.Specialization)
            .WithMany(x => x.Doctors)
            .HasForeignKey(x => x.SpecializationId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.User)
            .WithMany(u => u.Appointments)
            .HasForeignKey(a => a.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Filtered unique index: only enforce uniqueness for non-cancelled appointments.
        // This allows a time slot to be re-booked after it has been cancelled.
        modelBuilder.Entity<Appointment>()
            .HasIndex(a => new { a.DoctorId, a.AppointmentDate, a.TimeSlot })
            .IsUnique()
            .HasFilter($"[Status] <> {(int)AppointmentStatus.Cancelled}");

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Doctor)
            .WithMany(d => d.Ratings)
            .HasForeignKey(r => r.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        // One rating per user per doctor
        modelBuilder.Entity<Rating>()
            .HasIndex(r => new { r.DoctorId, r.UserId })
            .IsUnique();
    }
}