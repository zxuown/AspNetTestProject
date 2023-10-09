﻿// <auto-generated />
using System;
using AspNetTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AspNetTest.Migrations
{
    [DbContext(typeof(SiteContext))]
    [Migration("20230902162326_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.10");

            modelBuilder.Entity("AspNetTest.AboutMe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AboutMe");
                });

            modelBuilder.Entity("AspNetTest.Models.Abilities", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("AboutMeId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AboutMeId");

                    b.ToTable("Abilities");
                });

            modelBuilder.Entity("AspNetTest.Models.Abilities", b =>
                {
                    b.HasOne("AspNetTest.AboutMe", null)
                        .WithMany("Abilities")
                        .HasForeignKey("AboutMeId");
                });

            modelBuilder.Entity("AspNetTest.AboutMe", b =>
                {
                    b.Navigation("Abilities");
                });
#pragma warning restore 612, 618
        }
    }
}