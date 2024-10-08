﻿// <auto-generated />
using System;
using FamilyTree.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FamilyTree.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20240603153237_ChangesInPersonModel")]
    partial class ChangesInPersonModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("FamilyTree.Models.Master.VRT01", b =>
                {
                    b.Property<int>("T01F01")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("T01F01"));

                    b.Property<string>("T01F02")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F03")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F04")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("T01F05")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("T01F06")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F07")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("T01F08")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("T01F09")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F10")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F11")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F12")
                        .HasColumnType("longtext");

                    b.Property<string>("T01F13")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("T01F14")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("T01F15")
                        .HasColumnType("datetime(6)");

                    b.HasKey("T01F01");

                    b.ToTable("VRT01s");
                });

            modelBuilder.Entity("FamilyTree.Models.Master.VRT02", b =>
                {
                    b.Property<int>("T02F01")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("T02F01"));

                    b.Property<int?>("T02F02")
                        .HasColumnType("int");

                    b.Property<int?>("T02F03")
                        .HasColumnType("int");

                    b.Property<int?>("T02F04")
                        .HasColumnType("int");

                    b.Property<string>("T02F05")
                        .HasColumnType("longtext");

                    b.Property<string>("T02F06")
                        .HasColumnType("longtext");

                    b.Property<DateTime?>("T02F07")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("T02F08")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("T02F09")
                        .HasColumnType("longtext");

                    b.HasKey("T02F01");

                    b.ToTable("VRT02s");
                });

            modelBuilder.Entity("FamilyTree.Models.Master.VRT03", b =>
                {
                    b.Property<int>("T03F01")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("T03F01"));

                    b.Property<string>("T03F02")
                        .HasColumnType("longtext");

                    b.HasKey("T03F01");

                    b.ToTable("VRT03s");
                });

            modelBuilder.Entity("FamilyTree.Models.Master.VRT04", b =>
                {
                    b.Property<int>("T04F01")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("T04F01"));

                    b.Property<int?>("T04F02")
                        .HasColumnType("int");

                    b.Property<int?>("T04F03")
                        .HasColumnType("int");

                    b.Property<int?>("T04F04")
                        .HasColumnType("int");

                    b.HasKey("T04F01");

                    b.ToTable("VRT04s");
                });
#pragma warning restore 612, 618
        }
    }
}
