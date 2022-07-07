﻿// <auto-generated />
using System;
using Collections.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using NpgsqlTypes;

#nullable disable

namespace Collections.Api.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Collections.Api.Entities.BoolValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<bool>("Value")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ItemId");

                    b.HasIndex("Value");

                    b.ToTable("BoolValues");
                });

            modelBuilder.Entity("Collections.Api.Entities.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OwnerId")
                        .HasColumnType("integer");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name", "Description" });

                    b.Property<int>("TopicId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.HasIndex("TopicId");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Collections.Api.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Text" });

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ItemId");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Collections.Api.Entities.DateTimeValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Value")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ItemId");

                    b.HasIndex("Value");

                    b.ToTable("DateTimeValues");
                });

            modelBuilder.Entity("Collections.Api.Entities.Field", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CollectionId")
                        .HasColumnType("integer");

                    b.Property<int>("FieldType")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.ToTable("Fields");
                });

            modelBuilder.Entity("Collections.Api.Entities.IntValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ItemId");

                    b.HasIndex("Value");

                    b.ToTable("IntValues");
                });

            modelBuilder.Entity("Collections.Api.Entities.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CollectionId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Collections.Api.Entities.Like", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ItemId");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Collections.Api.Entities.StringValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("FieldId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Value" });

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FieldId");

                    b.HasIndex("ItemId");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.HasIndex("Value");

                    b.ToTable("StringValues");
                });

            modelBuilder.Entity("Collections.Api.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ItemId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "Name" });

                    b.HasKey("Id");

                    b.HasIndex("ItemId");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("Collections.Api.Entities.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("EnName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RuName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<NpgsqlTsVector>("SimpleSearchVector")
                        .IsRequired()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("tsvector")
                        .HasAnnotation("Npgsql:TsVectorConfig", "simple")
                        .HasAnnotation("Npgsql:TsVectorProperties", new[] { "EnName", "RuName" });

                    b.HasKey("Id");

                    b.HasIndex("SimpleSearchVector");

                    NpgsqlIndexBuilderExtensions.HasMethod(b.HasIndex("SimpleSearchVector"), "GIN");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("Collections.Api.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("Admin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Collections.Api.Entities.BoolValue", b =>
                {
                    b.HasOne("Collections.Api.Entities.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("BoolValues")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.Collection", b =>
                {
                    b.HasOne("Collections.Api.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("Collections.Api.Entities.Comment", b =>
                {
                    b.HasOne("Collections.Api.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("Comments")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.DateTimeValue", b =>
                {
                    b.HasOne("Collections.Api.Entities.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("DateTimeValues")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.Field", b =>
                {
                    b.HasOne("Collections.Api.Entities.Collection", "Collection")
                        .WithMany("Fields")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("Collections.Api.Entities.IntValue", b =>
                {
                    b.HasOne("Collections.Api.Entities.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("IntValues")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.Item", b =>
                {
                    b.HasOne("Collections.Api.Entities.Collection", "Collection")
                        .WithMany("Items")
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collection");
                });

            modelBuilder.Entity("Collections.Api.Entities.Like", b =>
                {
                    b.HasOne("Collections.Api.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("Likes")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.StringValue", b =>
                {
                    b.HasOne("Collections.Api.Entities.Field", "Field")
                        .WithMany()
                        .HasForeignKey("FieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("StringValues")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Field");

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.Tag", b =>
                {
                    b.HasOne("Collections.Api.Entities.Item", "Item")
                        .WithMany("Tags")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Item");
                });

            modelBuilder.Entity("Collections.Api.Entities.Collection", b =>
                {
                    b.Navigation("Fields");

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Collections.Api.Entities.Item", b =>
                {
                    b.Navigation("BoolValues");

                    b.Navigation("Comments");

                    b.Navigation("DateTimeValues");

                    b.Navigation("IntValues");

                    b.Navigation("Likes");

                    b.Navigation("StringValues");

                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
