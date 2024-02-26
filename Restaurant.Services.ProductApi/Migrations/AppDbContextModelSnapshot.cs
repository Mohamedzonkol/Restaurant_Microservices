﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant.Services.ProductApi.DbContexts;

#nullable disable

namespace Restaurant.Services.ProductApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.1.24081.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurant.Services.ProductApi.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProductId"));

                    b.Property<string>("CatagoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("ProductId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ProductId = 1,
                            CatagoryName = "Food",
                            Description = "Delicious pizza with assorted toppings",
                            ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-muffin-creatives-1653877.jpg",
                            Name = "Pizza",
                            Price = 15.99
                        },
                        new
                        {
                            ProductId = 2,
                            CatagoryName = "Food",
                            Description = "Juicy beef burger with cheese and veggies",
                            ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-valeria-boltneva-1639565.jpg",
                            Name = "Burger",
                            Price = 12.5
                        },
                        new
                        {
                            ProductId = 3,
                            CatagoryName = "Dessert",
                            Description = "Rich chocolate cake with a decadent frosting",
                            ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-polina-tankilevitch-4109998.jpg",
                            Name = "Chocolate Cake",
                            Price = 25.0
                        },
                        new
                        {
                            ProductId = 4,
                            CatagoryName = "Dessert",
                            Description = "Creamy cheesecake with a graham cracker crust",
                            ImageUrl = "https://dotnetresturant.blob.core.windows.net/manger/pexels-cesar-de-la-cruz-3185509.jpg",
                            Name = "Cheesecake",
                            Price = 20.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
