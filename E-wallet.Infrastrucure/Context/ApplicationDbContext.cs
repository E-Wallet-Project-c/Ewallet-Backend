using System;
using System.Collections.Generic;
using E_wallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_wallet.Domain.Context;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Beneficiary> Beneficiaries { get; set; }

    public virtual DbSet<Limit> Limits { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<Transfer> Transfers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserBankAccount> UserBankAccounts { get; set; }

    public virtual DbSet<VirtualBank> VirtualBanks { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=E-Wallet-DB;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Admin_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Role).WithMany(p => p.Admins)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-RoleId");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("AuditLog_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-UserId");
        });

        modelBuilder.Entity<Beneficiary>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Beneficiary _pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.BeneficiaryWallet).WithMany(p => p.BeneficiaryBeneficiaryWallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-BeneficiaryWalletId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.BeneficiaryWallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-WalletId");
        });

        modelBuilder.Entity<Limit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Limit_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Notification_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-UserId");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Profile_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Role _pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Session_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-UserId");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Transaction_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Transfer).WithMany(p => p.Transactions).HasConstraintName("Fk-TransferId");

            entity.HasOne(d => d.Wallet).WithMany(p => p.Transactions)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-WalletId");
        });

        modelBuilder.Entity<Transfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Transfer_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.ReciverWallet).WithMany(p => p.TransferReciverWallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-ReciverWalletId");

            entity.HasOne(d => d.SenderWallet).WithMany(p => p.TransferSenderWallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-SenderWalletId");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.Profile).WithMany(p => p.Users)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-ProfileId");
        });

        modelBuilder.Entity<UserBankAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UserBankAccount_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Bank).WithMany(p => p.UserBankAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-BankId");

            entity.HasOne(d => d.User).WithMany(p => p.UserBankAccounts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-UserId");
        });

        modelBuilder.Entity<VirtualBank>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("VirtualBank_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();

            entity.HasOne(d => d.Wallet).WithMany(p => p.VirtualBanks)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-WalletId");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Wallet_pkey");

            entity.Property(e => e.Id).UseIdentityAlwaysColumn();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);

            entity.HasOne(d => d.User).WithMany(p => p.Wallets)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Fk-UserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
