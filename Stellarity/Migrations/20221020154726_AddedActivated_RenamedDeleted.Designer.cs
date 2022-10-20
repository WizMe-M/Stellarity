﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Stellarity.Database;

#nullable disable

namespace Stellarity.Migrations
{
    [DbContext(typeof(StellarityContext))]
    [Migration("20221020154726_AddedActivated_RenamedDeleted")]
    partial class AddedActivated_RenamedDeleted
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresEnum(modelBuilder, "roles", new[] { "administrator", "player" });
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Stellarity.Database.Entities.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("About")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("about");

                    b.Property<bool>("Activated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("activated");

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasDefaultValue(0m)
                        .HasColumnName("balance");

                    b.Property<bool>("Banned")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("banned");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)")
                        .HasColumnName("email");

                    b.Property<string>("Nickname")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("nickname");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("character varying(32)")
                        .HasColumnName("password");

                    b.Property<DateTime>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("registration_date")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_role");

                    b.Property<Guid?>("SingleImageId")
                        .HasColumnType("uuid")
                        .HasColumnName("avatar_guid");

                    b.HasKey("Id");

                    b.HasIndex("SingleImageId");

                    b.HasIndex(new[] { "Email" }, "uq_users_email")
                        .IsUnique();

                    b.HasIndex(new[] { "Nickname" }, "uq_users_nickname")
                        .IsUnique();

                    b.ToTable("users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Activated = false,
                            Balance = 0m,
                            Banned = false,
                            Email = "admin@mail.ru",
                            Nickname = "Stellarity.Desktop",
                            Password = "P@ssw0rd",
                            RegistrationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Role = "Administrator"
                        });
                });

            modelBuilder.Entity("Stellarity.Database.Entities.CommentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AuthorId")
                        .HasColumnType("integer")
                        .HasColumnName("author_id");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)")
                        .HasColumnName("body");

                    b.Property<DateTime>("CommentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("comment_date")
                        .HasDefaultValueSql("now()");

                    b.Property<int>("ProfileId")
                        .HasColumnType("integer")
                        .HasColumnName("profile_id");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("ProfileId");

                    b.ToTable("comments", (string)null);
                });

            modelBuilder.Entity("Stellarity.Database.Entities.GameEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("AddDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("add_date")
                        .HasDefaultValueSql("now()");

                    b.Property<decimal>("Cost")
                        .HasPrecision(10, 2)
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("cost");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("description");

                    b.Property<string>("Developer")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("developer");

                    b.Property<Guid?>("SingleImageId")
                        .HasColumnType("uuid")
                        .HasColumnName("cover_guid");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)")
                        .HasColumnName("name");

                    b.HasKey("Id");

                    b.HasIndex("SingleImageId");

                    b.HasIndex(new[] { "Title" }, "uq_games_name")
                        .IsUnique();

                    b.ToTable("games", (string)null);
                });

            modelBuilder.Entity("Stellarity.Database.Entities.ImageEntity", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("guid")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("bytea")
                        .HasColumnName("data");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("Guid")
                        .HasName("pk_images");

                    b.ToTable("images", (string)null);
                });

            modelBuilder.Entity("Stellarity.Database.Entities.KeyEntity", b =>
                {
                    b.Property<string>("KeyValue")
                        .HasColumnType("text")
                        .HasColumnName("key_value");

                    b.Property<int?>("AccountId")
                        .HasColumnType("integer")
                        .HasColumnName("buyer_id");

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<DateTime?>("PurchaseDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("purchase_date");

                    b.HasKey("KeyValue")
                        .HasName("pk_key");

                    b.HasIndex("GameId");

                    b.HasIndex("AccountId", "GameId")
                        .IsUnique();

                    b.ToTable("key", (string)null);
                });

            modelBuilder.Entity("Stellarity.Database.Entities.LibraryEntity", b =>
                {
                    b.Property<int>("AccountId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.Property<int>("GameId")
                        .HasColumnType("integer")
                        .HasColumnName("game_id");

                    b.Property<DateTime>("PurchaseDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("purchase_date")
                        .HasDefaultValueSql("now()");

                    b.HasKey("AccountId", "GameId")
                        .HasName("pk_library");

                    b.HasIndex("GameId");

                    b.ToTable("library", (string)null);
                });

            modelBuilder.Entity("Stellarity.Database.Entities.AccountEntity", b =>
                {
                    b.HasOne("Stellarity.Database.Entities.ImageEntity", "SingleImageEntity")
                        .WithMany("Users")
                        .HasForeignKey("SingleImageId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_users_avatar_guid");

                    b.Navigation("SingleImageEntity");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.CommentEntity", b =>
                {
                    b.HasOne("Stellarity.Database.Entities.AccountEntity", "Author")
                        .WithMany("CommentWhereIsAuthor")
                        .HasForeignKey("AuthorId")
                        .IsRequired()
                        .HasConstraintName("fk_comments_author_id");

                    b.HasOne("Stellarity.Database.Entities.AccountEntity", "Profile")
                        .WithMany("CommentWhereIsProfile")
                        .HasForeignKey("ProfileId")
                        .IsRequired()
                        .HasConstraintName("fk_comments_profile_id");

                    b.Navigation("Author");

                    b.Navigation("Profile");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.GameEntity", b =>
                {
                    b.HasOne("Stellarity.Database.Entities.ImageEntity", "SingleImageEntity")
                        .WithMany("Games")
                        .HasForeignKey("SingleImageId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("fk_games_cover_guid");

                    b.Navigation("SingleImageEntity");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.KeyEntity", b =>
                {
                    b.HasOne("Stellarity.Database.Entities.AccountEntity", "Account")
                        .WithMany("Keys")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_key_user");

                    b.HasOne("Stellarity.Database.Entities.GameEntity", "Game")
                        .WithMany("Keys")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_key_game");

                    b.Navigation("Account");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.LibraryEntity", b =>
                {
                    b.HasOne("Stellarity.Database.Entities.AccountEntity", "Account")
                        .WithMany("Library")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_library_user");

                    b.HasOne("Stellarity.Database.Entities.GameEntity", "Game")
                        .WithMany("Libraries")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_library_game");

                    b.Navigation("Account");

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.AccountEntity", b =>
                {
                    b.Navigation("CommentWhereIsAuthor");

                    b.Navigation("CommentWhereIsProfile");

                    b.Navigation("Keys");

                    b.Navigation("Library");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.GameEntity", b =>
                {
                    b.Navigation("Keys");

                    b.Navigation("Libraries");
                });

            modelBuilder.Entity("Stellarity.Database.Entities.ImageEntity", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
