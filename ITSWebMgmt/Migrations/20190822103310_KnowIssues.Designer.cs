﻿// <auto-generated />
using System;
using ITSWebMgmt.Models.Log;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ITSWebMgmt.Migrations
{
    [DbContext(typeof(LogEntryContext))]
    [Migration("20190822103310_KnowIssues")]
    partial class KnowIssues
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ITSWebMgmt.Models.KnownIssue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<string>("CaseLink");

                    b.Property<string>("Description");

                    b.Property<string>("OneNoteLink");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("KnownIssues");
                });

            modelBuilder.Entity("ITSWebMgmt.Models.Log.LogEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Hidden");

                    b.Property<DateTime>("TimeStamp");

                    b.Property<int>("Type");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("ITSWebMgmt.Models.Log.LogEntryArgument", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("LogEntryId");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("LogEntryId");

                    b.ToTable("LogEntryArguments");
                });

            modelBuilder.Entity("ITSWebMgmt.Models.Log.LogEntryArgument", b =>
                {
                    b.HasOne("ITSWebMgmt.Models.Log.LogEntry")
                        .WithMany("Arguments")
                        .HasForeignKey("LogEntryId");
                });
#pragma warning restore 612, 618
        }
    }
}
