﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SuiviVaccinationCovid.Modele;

#nullable disable

namespace SuiviVaccinationCovid.Migrations
{
    [DbContext(typeof(VaccinationContext))]
    [Migration("20250115160809_initialmigration")]
    partial class initialmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SuiviVaccinationCovid.Modele.Dose", b =>
                {
                    b.Property<int>("DoseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DoseId"));

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("NAMPatient")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("VaccinId")
                        .HasColumnType("int");

                    b.HasKey("DoseId");

                    b.HasIndex("VaccinId");

                    b.ToTable("Doses");
                });

            modelBuilder.Entity("SuiviVaccinationCovid.Modele.Vaccin", b =>
                {
                    b.Property<int>("VaccinId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VaccinId"));

                    b.Property<string>("Nom")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("VaccinId");

                    b.ToTable("Vaccins");
                });

            modelBuilder.Entity("SuiviVaccinationCovid.Modele.Dose", b =>
                {
                    b.HasOne("SuiviVaccinationCovid.Modele.Vaccin", "Vaccin")
                        .WithMany()
                        .HasForeignKey("VaccinId");

                    b.Navigation("Vaccin");
                });
#pragma warning restore 612, 618
        }
    }
}
