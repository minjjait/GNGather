﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.DB;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Server.DB.AccountDb", b =>
                {
                    b.Property<int>("AccountDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AccountDbId"), 1L, 1);

                    b.Property<string>("AccountName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AccountDbId");

                    b.HasIndex("AccountName")
                        .IsUnique()
                        .HasFilter("[AccountName] IS NOT NULL");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Server.DB.FestivalDb", b =>
                {
                    b.Property<int>("FestivalDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FestivalDbId"), 1L, 1);

                    b.Property<string>("FestivalExplain")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FestivalName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("RegionDbId")
                        .HasColumnType("int");

                    b.Property<string>("RelatedAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FestivalDbId");

                    b.HasIndex("FestivalName")
                        .IsUnique()
                        .HasFilter("[FestivalName] IS NOT NULL");

                    b.HasIndex("RegionDbId");

                    b.ToTable("Festival");
                });

            modelBuilder.Entity("Server.DB.ItemDb", b =>
                {
                    b.Property<int>("ItemDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ItemDbId"), 1L, 1);

                    b.Property<int?>("OwnerDbId")
                        .HasColumnType("int");

                    b.Property<int>("TemplateId")
                        .HasColumnType("int");

                    b.HasKey("ItemDbId");

                    b.HasIndex("OwnerDbId");

                    b.ToTable("Item");
                });

            modelBuilder.Entity("Server.DB.PlayerDb", b =>
                {
                    b.Property<int>("PlayerDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PlayerDbId"), 1L, 1);

                    b.Property<int>("AccountDbId")
                        .HasColumnType("int");

                    b.Property<string>("PlayerName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("PlayerDbId");

                    b.HasIndex("AccountDbId");

                    b.HasIndex("PlayerName")
                        .IsUnique()
                        .HasFilter("[PlayerName] IS NOT NULL");

                    b.ToTable("Player");
                });

            modelBuilder.Entity("Server.DB.RegionDb", b =>
                {
                    b.Property<int>("RegionDbId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RegionDbId"), 1L, 1);

                    b.Property<string>("RegionName")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("RegionDbId");

                    b.HasIndex("RegionName")
                        .IsUnique()
                        .HasFilter("[RegionName] IS NOT NULL");

                    b.ToTable("Region");
                });

            modelBuilder.Entity("Server.DB.FestivalDb", b =>
                {
                    b.HasOne("Server.DB.RegionDb", "Region")
                        .WithMany("Festivals")
                        .HasForeignKey("RegionDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("Server.DB.ItemDb", b =>
                {
                    b.HasOne("Server.DB.PlayerDb", "Owner")
                        .WithMany("Items")
                        .HasForeignKey("OwnerDbId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Server.DB.PlayerDb", b =>
                {
                    b.HasOne("Server.DB.AccountDb", "Account")
                        .WithMany("Players")
                        .HasForeignKey("AccountDbId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Server.DB.AccountDb", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("Server.DB.PlayerDb", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Server.DB.RegionDb", b =>
                {
                    b.Navigation("Festivals");
                });
#pragma warning restore 612, 618
        }
    }
}
