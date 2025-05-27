using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TicketManagementSystem.Models;

namespace TicketManagementSystem.Data;

public partial class TicketManagementDbContext : DbContext
{
    public TicketManagementDbContext()
    {
    }

    public TicketManagementDbContext(DbContextOptions<TicketManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketComment> TicketComments { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=ELW5113;Initial Catalog=TicketManagementDb;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.TicketId).HasName("PK__Tickets__712CC6079D37EB1D");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.Priority)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Subject)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_User");
        });

        modelBuilder.Entity<TicketComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TicketCo__3214EC07626F47F0");

            entity.Property(e => e.Comment).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Ticket).WithMany(p => p.TicketComments)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_Ticket");

            entity.HasOne(d => d.User).WithMany(p => p.TicketComments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Comments_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C86D2BB94");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534F230D26E").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
