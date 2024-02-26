﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Resturant.Services.Discount.DbContexts;

#nullable disable

namespace Resturant.Services.Discount.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240220151247_SeedSomeCopuon")]
    partial class SeedSomeCopuon
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Resturant.Services.Discount.Models.Coupon", b =>
                {
                    b.Property<int>("CouponId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CouponId"));

                    b.Property<string>("CopounCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("DiscountAmount")
                        .HasColumnType("float");

                    b.HasKey("CouponId");

                    b.ToTable("Coupons");

                    b.HasData(
                        new
                        {
                            CouponId = 1,
                            CopounCode = "10FS0",
                            DiscountAmount = 10.0
                        },
                        new
                        {
                            CouponId = 2,
                            CopounCode = "10FF0",
                            DiscountAmount = 20.0
                        },
                        new
                        {
                            CouponId = 3,
                            CopounCode = "10FFF",
                            DiscountAmount = 50.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}