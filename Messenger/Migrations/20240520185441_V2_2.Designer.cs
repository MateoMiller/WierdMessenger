﻿// <auto-generated />
using System;
using Messenger.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Messenger.Migrations
{
    [DbContext(typeof(AuthContext))]
    [Migration("20240520185441_V2_2")]
    partial class V2_2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.27")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Messenger.Authorization.AuthModel", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("bytea")
                        .IsFixedLength();

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Login");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("authInfo", (string)null);
                });

            modelBuilder.Entity("Messenger.Authorization.CookiesModel", b =>
                {
                    b.Property<Guid>("AuthSid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("AuthSid");

                    b.ToTable("cookies", (string)null);
                });

            modelBuilder.Entity("Messenger.Authorization.UserModel", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ImageBase64")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("users", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}