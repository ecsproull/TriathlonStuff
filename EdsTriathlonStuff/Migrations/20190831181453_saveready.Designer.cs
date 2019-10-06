﻿// <auto-generated />
using EdsTriathlonStuff.App_Code;
using EdsTriathlonStuff.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace EdsTriathlonStuff.Migrations
{
    [DbContext(typeof(SwimDataContext))]
    [Migration("20190831181453_saveready")]
    partial class saveready
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("EdsTriathlonStuff.App_Code.SwimSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<string>("Distance");

                    b.Property<int>("Group");

                    b.Property<int>("RepCount");

                    b.Property<int>("Rest");

                    b.Property<string>("Speed");

                    b.Property<int>("StepNumber");

                    b.Property<int>("Total");

                    b.Property<int>("WorkoutId");

                    b.HasKey("Id");

                    b.ToTable("SwimSets");
                });

            modelBuilder.Entity("EdsTriathlonStuff.App_Code.WokoutSet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Index");

                    b.Property<int>("SetId");

                    b.Property<int>("WorkoutId");

                    b.Property<string>("WorkoutSection");

                    b.HasKey("Id");

                    b.ToTable("WorkoutSets");
                });

            modelBuilder.Entity("EdsTriathlonStuff.App_Code.Workout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AthleteName");

                    b.Property<int>("CoolDownStepCount");

                    b.Property<string>("Description");

                    b.Property<int>("MainStepCount");

                    b.Property<string>("Name");

                    b.Property<int>("TotalMeters");

                    b.Property<int>("TotalYards");

                    b.Property<string>("UserId");

                    b.Property<int>("WarmUpStepCount");

                    b.HasKey("Id");

                    b.ToTable("Workouts");
                });
#pragma warning restore 612, 618
        }
    }
}
