﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SalonManagementSystem.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241223114745_AddServiceImage")]
    partial class AddServiceImage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SalonManagementSystem.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<TimeSpan>("EndWorkingHours")
                        .HasColumnType("interval");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ServiceId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("StartWorkingHours")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("ServiceId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("SalonManagementSystem.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Duration")
                        .HasColumnType("integer");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("ImageType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("SalonManagementSystem.Models.Employee", b =>
                {
                    b.HasOne("SalonManagementSystem.Models.Service", "Service")
                        .WithMany("Employees")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("SalonManagementSystem.Models.Service", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
