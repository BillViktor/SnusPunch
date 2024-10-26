﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SnusPunch.Data.DbContexts;

#nullable disable

namespace SnusPunch.Data.Migrations
{
    [DbContext(typeof(SnusPunchDbContext))]
    partial class SnusPunchDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SnusPunch.Shared.Models.Snus.SnusModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("NicotineInMgPerGram")
                        .HasColumnType("float");

                    b.Property<int>("PortionCount")
                        .HasColumnType("int");

                    b.Property<decimal>("PriceInSek")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("WeightInGrams")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Snus");
                });
#pragma warning restore 612, 618
        }
    }
}
