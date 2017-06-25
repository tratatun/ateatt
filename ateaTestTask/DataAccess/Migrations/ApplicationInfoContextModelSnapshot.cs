using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DataAccess;

namespace DataAccess.Migrations
{
    [DbContext(typeof(ApplicationInfoContext))]
    partial class ApplicationInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.ApplicationInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ClientComputerId");

                    b.Property<string>("DisplayName");

                    b.Property<string>("DisplayVersion");

                    b.Property<DateTime>("InstallDate");

                    b.Property<int>("PSComputerId");

                    b.Property<int>("PublisherId");

                    b.HasKey("Id");

                    b.HasIndex("ClientComputerId");

                    b.HasIndex("PublisherId");

                    b.ToTable("ApplicationsInfos");
                });

            modelBuilder.Entity("Domain.ClientComputer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ComputerName");

                    b.HasKey("Id");

                    b.ToTable("ClientComputers");
                });

            modelBuilder.Entity("Domain.Publisher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PublisherName");

                    b.HasKey("Id");

                    b.ToTable("Publishers");
                });

            modelBuilder.Entity("Domain.ApplicationInfo", b =>
                {
                    b.HasOne("Domain.ClientComputer")
                        .WithMany("ApplicationInfos")
                        .HasForeignKey("ClientComputerId");

                    b.HasOne("Domain.Publisher")
                        .WithMany("ApplicationInfos")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
