﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NextHome.Realty.Persistence.Data;

#nullable disable

namespace NextHome.Realty.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240710201541_AddImageFile")]
    partial class AddImageFile
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NextHome.Realty.Domain.Entities.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Occupancy")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Sqft")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            Name = "Royal Villa",
                            Occupancy = 4,
                            Price = 200.0,
                            Sqft = 550
                        },
                        new
                        {
                            Id = 2,
                            Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            Name = "Premium Pool Villa",
                            Occupancy = 4,
                            Price = 300.0,
                            Sqft = 550
                        },
                        new
                        {
                            Id = 3,
                            Description = "Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.",
                            Name = "Luxury Pool Villa",
                            Occupancy = 4,
                            Price = 400.0,
                            Sqft = 750
                        });
                });

            modelBuilder.Entity("NextHome.Realty.Domain.Entities.VillaImage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VillaId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("VillaId");

                    b.ToTable("VillaImages");
                });

            modelBuilder.Entity("NextHome.Realty.Domain.Entities.VillaNumber", b =>
                {
                    b.Property<int>("Villa_Number")
                        .HasColumnType("int");

                    b.Property<string>("SpecialDetails")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("VillaId")
                        .HasColumnType("int");

                    b.HasKey("Villa_Number");

                    b.HasIndex("VillaId");

                    b.ToTable("VillaNumbers");

                    b.HasData(
                        new
                        {
                            Villa_Number = 101,
                            SpecialDetails = "",
                            VillaId = 1
                        },
                        new
                        {
                            Villa_Number = 102,
                            SpecialDetails = "",
                            VillaId = 1
                        },
                        new
                        {
                            Villa_Number = 103,
                            SpecialDetails = "",
                            VillaId = 1
                        },
                        new
                        {
                            Villa_Number = 104,
                            SpecialDetails = "",
                            VillaId = 1
                        },
                        new
                        {
                            Villa_Number = 201,
                            SpecialDetails = "",
                            VillaId = 2
                        },
                        new
                        {
                            Villa_Number = 202,
                            SpecialDetails = "",
                            VillaId = 2
                        },
                        new
                        {
                            Villa_Number = 203,
                            SpecialDetails = "",
                            VillaId = 2
                        });
                });

            modelBuilder.Entity("NextHome.Realty.Domain.Entities.VillaImage", b =>
                {
                    b.HasOne("NextHome.Realty.Domain.Entities.Villa", "Villa")
                        .WithMany("VillaImages")
                        .HasForeignKey("VillaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Villa");
                });

            modelBuilder.Entity("NextHome.Realty.Domain.Entities.VillaNumber", b =>
                {
                    b.HasOne("NextHome.Realty.Domain.Entities.Villa", "Villa")
                        .WithMany()
                        .HasForeignKey("VillaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Villa");
                });

            modelBuilder.Entity("NextHome.Realty.Domain.Entities.Villa", b =>
                {
                    b.Navigation("VillaImages");
                });
#pragma warning restore 612, 618
        }
    }
}
