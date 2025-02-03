﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace thaicodelab_api.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250126044335_ChangeBirthdateToDate")]
    partial class ChangeBirthdateToDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Department", b =>
                {
                    b.Property<int>("department_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("department_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("department")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("department_code")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("department_id");

                    b.ToTable("tb_departments", "sc_center_users");

                    b.HasData(
                        new
                        {
                            department_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            department = "Human Resources",
                            department_code = "DEP0000001",
                            description = "Human Resources",
                            is_deleted = false,
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1
                        });
                });

            modelBuilder.Entity("Equipment", b =>
                {
                    b.Property<int>("equipment_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("equipment_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("equipment")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("equipment_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("equipment_group_id")
                        .HasColumnType("integer");

                    b.Property<int>("equipment_type_id")
                        .HasColumnType("integer");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("equipment_id");

                    b.HasIndex("equipment_group_id");

                    b.HasIndex("equipment_type_id");

                    b.ToTable("tb_equipments", "sc_equipments");
                });

            modelBuilder.Entity("EquipmentGroup", b =>
                {
                    b.Property<int>("equipment_group_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("equipment_group_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<int>("department_id")
                        .HasColumnType("integer");

                    b.Property<string>("equipment_group")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("equipment_group_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("equipment_group_id");

                    b.HasIndex("department_id");

                    b.ToTable("tb_equipment_groups", "sc_equipments");
                });

            modelBuilder.Entity("EquipmentTransaction", b =>
                {
                    b.Property<int>("equipment_transaction_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("equipment_transaction_id"));

                    b.Property<int>("approve_user_id")
                        .HasColumnType("integer");

                    b.Property<int>("equipment_id")
                        .HasColumnType("integer");

                    b.Property<string>("equipment_transaction_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("equipment_transaction_timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("equipment_transaction_user_id")
                        .HasColumnType("integer");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("note")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("status_equipment_transaction")
                        .HasColumnType("integer");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("equipment_transaction_id");

                    b.HasIndex("approve_user_id");

                    b.HasIndex("equipment_id");

                    b.HasIndex("equipment_transaction_user_id");

                    b.ToTable("tb_equipment_transactions", "sc_equipments");
                });

            modelBuilder.Entity("EquipmentType", b =>
                {
                    b.Property<int>("equipment_type_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("equipment_type_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("equipment_type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("equipment_type_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("equipment_type_id");

                    b.ToTable("tb_equipment_types", "sc_equipments");
                });

            modelBuilder.Entity("Gender", b =>
                {
                    b.Property<int>("gender_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("gender_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("gender")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("gender_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("gender_id");

                    b.ToTable("tb_genders", "sc_center_users");

                    b.HasData(
                        new
                        {
                            gender_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            gender = "Male",
                            gender_code = "GDR0000001",
                            is_deleted = false,
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1
                        });
                });

            modelBuilder.Entity("Permission", b =>
                {
                    b.Property<int>("permission_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("permission_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("permission")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("permission_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("permission_id");

                    b.ToTable("tb_permissions", "sc_center_users");

                    b.HasData(
                        new
                        {
                            permission_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            description = "index page",
                            is_deleted = false,
                            permission = "index",
                            permission_code = "PMS0000001",
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1
                        });
                });

            modelBuilder.Entity("Rank", b =>
                {
                    b.Property<int>("rank_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("rank_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("full_rank")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("rank_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("sequence")
                        .HasColumnType("integer");

                    b.Property<string>("short_rank")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("rank_id");

                    b.ToTable("tb_ranks", "sc_center_users");

                    b.HasData(
                        new
                        {
                            rank_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            full_rank = "Mister",
                            is_deleted = false,
                            rank_code = "RAK0000001",
                            sequence = 1,
                            short_rank = "Mr.",
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1
                        });
                });

            modelBuilder.Entity("Role", b =>
                {
                    b.Property<int>("role_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("role_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("role")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("role_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("role_id");

                    b.ToTable("tb_roles", "sc_center_users");

                    b.HasData(
                        new
                        {
                            role_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            description = "Super Admin",
                            is_deleted = false,
                            role = "Super Admin",
                            role_code = "ROE0000001",
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1
                        });
                });

            modelBuilder.Entity("RolePermission", b =>
                {
                    b.Property<int>("role_permission_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("role_permission_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<int>("permission_id")
                        .HasColumnType("integer");

                    b.Property<int>("role_id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.HasKey("role_permission_id");

                    b.ToTable("tb_role_permissions", "sc_center_users");

                    b.HasData(
                        new
                        {
                            role_permission_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            is_deleted = false,
                            permission_id = 1,
                            role_id = 1,
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1
                        });
                });

            modelBuilder.Entity("User", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("user_id"));

                    b.Property<DateOnly>("birthdate")
                        .HasColumnType("date");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<int>("department_id")
                        .HasColumnType("integer");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("firstname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("gender_id")
                        .HasColumnType("integer");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("lastname")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("phone_number")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("rank_id")
                        .HasColumnType("integer");

                    b.Property<string>("recovery_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("recovery_expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("role_id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.Property<string>("user_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("user_password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("user_status_id")
                        .HasColumnType("integer");

                    b.HasKey("user_id");

                    b.HasIndex("department_id");

                    b.HasIndex("gender_id");

                    b.HasIndex("rank_id");

                    b.HasIndex("role_id");

                    b.HasIndex("user_status_id");

                    b.ToTable("tb_users", "sc_center_users");

                    b.HasData(
                        new
                        {
                            user_id = 1,
                            birthdate = new DateOnly(2024, 1, 1),
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            department_id = 1,
                            email = "superadmin@superadmin.com",
                            firstname = "Super",
                            gender_id = 1,
                            is_deleted = false,
                            lastname = "Admin",
                            phone_number = "0123456789",
                            rank_id = 1,
                            recovery_code = "123456",
                            recovery_expiration = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            role_id = 1,
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1,
                            user_code = "USR0000001",
                            user_password = "$2a$11$tfK5h9B0tIRWH1pkTbX6ke3rk4AOvaFhiHqbOvqQeMQdao5YyBSb.",
                            user_status_id = 1
                        });
                });

            modelBuilder.Entity("UserStatus", b =>
                {
                    b.Property<int>("user_status_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("user_status_id"));

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("created_by")
                        .HasColumnType("integer");

                    b.Property<bool>("is_deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("updated_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("updated_by")
                        .HasColumnType("integer");

                    b.Property<string>("user_status")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("user_status_code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("user_status_id");

                    b.ToTable("tb_user_status", "sc_center_users");

                    b.HasData(
                        new
                        {
                            user_status_id = 1,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            is_deleted = false,
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1,
                            user_status = "Active",
                            user_status_code = "USS0000001"
                        },
                        new
                        {
                            user_status_id = 2,
                            created_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            created_by = 1,
                            is_deleted = false,
                            updated_at = new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            updated_by = 1,
                            user_status = "Inactive",
                            user_status_code = "USS0000002"
                        });
                });

            modelBuilder.Entity("Equipment", b =>
                {
                    b.HasOne("EquipmentGroup", null)
                        .WithMany()
                        .HasForeignKey("equipment_group_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EquipmentType", null)
                        .WithMany()
                        .HasForeignKey("equipment_type_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EquipmentGroup", b =>
                {
                    b.HasOne("Department", null)
                        .WithMany()
                        .HasForeignKey("department_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("EquipmentTransaction", b =>
                {
                    b.HasOne("User", null)
                        .WithMany()
                        .HasForeignKey("approve_user_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Equipment", null)
                        .WithMany()
                        .HasForeignKey("equipment_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("User", null)
                        .WithMany()
                        .HasForeignKey("equipment_transaction_user_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("User", b =>
                {
                    b.HasOne("Department", null)
                        .WithMany()
                        .HasForeignKey("department_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Gender", null)
                        .WithMany()
                        .HasForeignKey("gender_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Rank", null)
                        .WithMany()
                        .HasForeignKey("rank_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Role", null)
                        .WithMany()
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("UserStatus", null)
                        .WithMany()
                        .HasForeignKey("user_status_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
