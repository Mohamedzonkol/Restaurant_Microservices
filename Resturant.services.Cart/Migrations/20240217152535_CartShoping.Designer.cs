﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Resturant.services.Cart.DbContexts;

#nullable disable

namespace Resturant.services.Cart.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240217152535_CartShoping")]
    partial class CartShoping
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Resturant.services.Cart.Models.CartDetail", b =>
                {
                    b.Property<int>("CartDetailId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartDetailId"));

                    b.Property<int>("CartHeaderId")
                        .HasColumnType("int");

                    b.Property<int?>("Count")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("CartDetailId");

                    b.HasIndex("CartHeaderId");

                    b.HasIndex("ProductId");

                    b.ToTable("CartDetails");
                });

            modelBuilder.Entity("Resturant.services.Cart.Models.CartHeader", b =>
                {
                    b.Property<int>("CartHeaderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CartHeaderId"));

                    b.Property<string>("CouponCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CartHeaderId");

                    b.ToTable("CartHeaders");
                });

            modelBuilder.Entity("Resturant.services.Cart.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<string>("CatagoryName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Resturant.services.Cart.Models.CartDetail", b =>
                {
                    b.HasOne("Resturant.services.Cart.Models.CartHeader", "CartHeader")
                        .WithMany()
                        .HasForeignKey("CartHeaderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Resturant.services.Cart.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CartHeader");

                    b.Navigation("Product");
                });
#pragma warning restore 612, 618
        }
    }
}
