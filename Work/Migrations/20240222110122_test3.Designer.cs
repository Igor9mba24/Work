﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Work.Context;

#nullable disable

namespace Work.Migrations
{
    [DbContext(typeof(ApplictaionDbContext))]
    [Migration("20240222110122_test3")]
    partial class test3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Work.Models.Person", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Work.Models.Skill", b =>
                {
                    b.Property<byte>("Level")
                        .HasColumnType("tinyint");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Level");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("Work.Models.Skill_Person", b =>
                {
                    b.Property<long>("PersonId")
                        .HasColumnType("bigint");

                    b.Property<byte>("SkillId")
                        .HasColumnType("tinyint");

                    b.HasKey("PersonId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("Skill_Person");
                });

            modelBuilder.Entity("Work.Models.Skill_Person", b =>
                {
                    b.HasOne("Work.Models.Person", "Person")
                        .WithMany("Skill_Person")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Work.Models.Skill", "Skill")
                        .WithMany("Skill_Person")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("Work.Models.Person", b =>
                {
                    b.Navigation("Skill_Person");
                });

            modelBuilder.Entity("Work.Models.Skill", b =>
                {
                    b.Navigation("Skill_Person");
                });
#pragma warning restore 612, 618
        }
    }
}
