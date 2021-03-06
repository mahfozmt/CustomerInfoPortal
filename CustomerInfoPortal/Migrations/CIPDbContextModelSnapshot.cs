// <auto-generated />
using System;
using CustomerInfoPortal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CustomerInfoPortal.Migrations
{
    [DbContext(typeof(CIPDbContext))]
    partial class CIPDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CustomerInfoPortal.Models.Country", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryName")
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("ID");

                    b.ToTable("Country");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            CountryName = "Bangladesh"
                        },
                        new
                        {
                            ID = 2,
                            CountryName = "India"
                        },
                        new
                        {
                            ID = 3,
                            CountryName = "Nepal"
                        });
                });

            modelBuilder.Entity("CustomerInfoPortal.Models.Customer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CountryID")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("CustomerPhoto")
                        .HasColumnType("varbinary(MAX)");

                    b.Property<string>("FatherName")
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("MaritalStatus")
                        .HasColumnType("int");

                    b.Property<string>("MotherName")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("ID");

                    b.HasIndex("CountryID");

                    b.ToTable("Customer");
                });

            modelBuilder.Entity("CustomerInfoPortal.Models.CustomerAddress", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnName("CustomerAddress")
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("CustomerID");

                    b.ToTable("CustomerAddress");
                });

            modelBuilder.Entity("CustomerInfoPortal.Models.Customer", b =>
                {
                    b.HasOne("CustomerInfoPortal.Models.Country", "Country")
                        .WithMany("Customers")
                        .HasForeignKey("CountryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CustomerInfoPortal.Models.CustomerAddress", b =>
                {
                    b.HasOne("CustomerInfoPortal.Models.Customer", "Customer")
                        .WithMany("CustomerAddresses")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
