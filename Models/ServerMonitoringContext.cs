using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerApiClient.Models;

public partial class ServerMonitoringContext : DbContext
{
    public ServerMonitoringContext()
    {
    }

    public ServerMonitoringContext(DbContextOptions<ServerMonitoringContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Block> Blocks { get; set; }

    public virtual DbSet<Error> Errors { get; set; }

    public virtual DbSet<Metric> Metrics { get; set; }

    public virtual DbSet<Parameter> Parameters { get; set; }

    public virtual DbSet<Server> Servers { get; set; }

    public virtual DbSet<ServerParameter> ServerParameters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=postgres;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresExtension("pg_catalog", "adminpack")
            .HasPostgresExtension("server", "pgcrypto");

        modelBuilder.Entity<Block>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("block_pkey");

            entity.ToTable("block", "server");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Error>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("error_pkey");

            entity.ToTable("error", "server");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.FinishedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("finished_at");
            entity.Property(e => e.Importance).HasColumnName("importance");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.State).HasColumnName("state");

            entity.HasOne(d => d.Server).WithMany(p => p.Errors)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("error_server_id_fkey");
        });

        modelBuilder.Entity<Metric>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("metric_pkey");

            entity.ToTable("metric", "server");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Cpu)
                .HasDefaultValue(0)
                .HasColumnName("cpu");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Ram)
                .HasDefaultValue(0)
                .HasColumnName("ram");
            entity.Property(e => e.ServerId).HasColumnName("server_id");
            entity.Property(e => e.Strorage)
                .HasDefaultValue(0)
                .HasColumnName("strorage");

            entity.HasOne(d => d.Server).WithMany(p => p.Metrics)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("metric_server_id_fkey");
        });

        modelBuilder.Entity<Parameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("parameter_pkey");

            entity.ToTable("parameter", "server");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        modelBuilder.Entity<Server>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("server_pkey");

            entity.ToTable("server", "server");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.BlockId).HasColumnName("block_id");
            entity.Property(e => e.HostName)
                .HasColumnType("character varying")
                .HasColumnName("host_name");
            entity.Property(e => e.IpAddres)
                .HasColumnType("character varying")
                .HasColumnName("ip_addres");
            entity.Property(e => e.State)
                .HasDefaultValue(true)
                .HasColumnName("state");

            entity.HasOne(d => d.Block).WithMany(p => p.Servers)
                .HasForeignKey(d => d.BlockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("server_block_id_fkey");
        });

        modelBuilder.Entity<ServerParameter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("server_parameter_pkey");

            entity.ToTable("server_parameter", "server");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.ParameterId).HasColumnName("parameter_id");
            entity.Property(e => e.ServerId).HasColumnName("server_id");

            entity.HasOne(d => d.Parameter).WithMany(p => p.ServerParameters)
                .HasForeignKey(d => d.ParameterId)
                .HasConstraintName("server_parameter_parameter_id_fkey");

            entity.HasOne(d => d.Server).WithMany(p => p.ServerParameters)
                .HasForeignKey(d => d.ServerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("server_parameter_server_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
