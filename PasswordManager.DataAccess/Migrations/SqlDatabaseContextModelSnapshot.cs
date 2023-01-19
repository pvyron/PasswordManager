﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PasswordManager.DataAccess.Implementations;

#nullable disable

namespace PasswordManager.DataAccess.Migrations
{
    [DbContext(typeof(SqlDatabaseContext))]
    partial class SqlDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.PasswordCategoryDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("EditedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PasswordCategories");
                });

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.PasswordDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("EditedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Username")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("UserId");

                    b.ToTable("Passwords");
                });

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.UserDbModel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("EditedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.PasswordCategoryDbModel", b =>
                {
                    b.HasOne("PasswordManager.DataAccess.DbModels.UserDbModel", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.PasswordDbModel", b =>
                {
                    b.HasOne("PasswordManager.DataAccess.DbModels.PasswordCategoryDbModel", "Category")
                        .WithMany("Passwords")
                        .HasForeignKey("CategoryId");

                    b.HasOne("PasswordManager.DataAccess.DbModels.UserDbModel", "User")
                        .WithMany("Passwords")
                        .HasForeignKey("UserId");

                    b.Navigation("Category");

                    b.Navigation("User");
                });

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.PasswordCategoryDbModel", b =>
                {
                    b.Navigation("Passwords");
                });

            modelBuilder.Entity("PasswordManager.DataAccess.DbModels.UserDbModel", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("Passwords");
                });
#pragma warning restore 612, 618
        }
    }
}
