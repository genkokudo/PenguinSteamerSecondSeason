﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PenguinSteamerSecondSeason;

namespace PenguinSteamerSecondSeason.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.Candle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Begin");

                    b.Property<DateTime>("BeginTime");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<decimal>("End");

                    b.Property<DateTime>("EndTime");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("MBoardId");

                    b.Property<int?>("MTimeScaleId");

                    b.Property<decimal>("Max");

                    b.Property<decimal>("Min");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedDate");

                    b.Property<decimal>("Volume");

                    b.HasKey("Id");

                    b.HasIndex("MBoardId");

                    b.HasIndex("MTimeScaleId");

                    b.ToTable("Candles");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.MBoard", b =>
                {
                    b.Property<int>("MBoardId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("MCurrency1MCurrencyId");

                    b.Property<int?>("MCurrency2MCurrencyId");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("MBoardId");

                    b.HasIndex("MCurrency1MCurrencyId");

                    b.HasIndex("MCurrency2MCurrencyId");

                    b.ToTable("MBoards");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.MCurrency", b =>
                {
                    b.Property<int>("MCurrencyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Name");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("MCurrencyId");

                    b.ToTable("MCurrencies");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.MTimeScale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DisplayName");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("SecondsValue");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("MTimeScales");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.MWebSocket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Category");

                    b.Property<string>("ChannelName");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("EndPoint");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsEnabled");

                    b.Property<int?>("MBoardId");

                    b.Property<string>("UpdatedBy")
                        .HasMaxLength(255);

                    b.Property<DateTime?>("UpdatedDate");

                    b.HasKey("Id");

                    b.HasIndex("MBoardId");

                    b.ToTable("MWebSockets");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.Candle", b =>
                {
                    b.HasOne("PenguinSteamerSecondSeason.Models.MBoard", "MBoard")
                        .WithMany()
                        .HasForeignKey("MBoardId");

                    b.HasOne("PenguinSteamerSecondSeason.Models.MTimeScale", "MTimeScale")
                        .WithMany()
                        .HasForeignKey("MTimeScaleId");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.MBoard", b =>
                {
                    b.HasOne("PenguinSteamerSecondSeason.Models.MCurrency", "MCurrency1")
                        .WithMany()
                        .HasForeignKey("MCurrency1MCurrencyId");

                    b.HasOne("PenguinSteamerSecondSeason.Models.MCurrency", "MCurrency2")
                        .WithMany()
                        .HasForeignKey("MCurrency2MCurrencyId");
                });

            modelBuilder.Entity("PenguinSteamerSecondSeason.Models.MWebSocket", b =>
                {
                    b.HasOne("PenguinSteamerSecondSeason.Models.MBoard", "MBoard")
                        .WithMany()
                        .HasForeignKey("MBoardId");
                });
#pragma warning restore 612, 618
        }
    }
}
