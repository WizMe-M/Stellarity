using Microsoft.EntityFrameworkCore;
using Stellarity.Database.Entities;
using Stellarity.Database.XmlConfiguration;

namespace Stellarity.Database;

internal sealed class StellarityContext : DbContext
{
    /// <summary>
    /// Checks is database existing and creates it if not
    /// </summary>
    /// <returns>Did database exist</returns>
    public static async Task<bool> CreateDatabaseAsync()
    {
        await using var context = new StellarityContext();
        return await context.Database.EnsureCreatedAsync();
    }

    public DbSet<CommentEntity> Comments { get; set; } = null!;
    public DbSet<GameEntity> Games { get; set; } = null!;
    public DbSet<ImageEntity> Images { get; set; } = null!;
    public DbSet<LibraryEntity> Libraries { get; set; } = null!;
    public DbSet<RoleEntity> Roles { get; set; } = null!;
    public DbSet<AccountEntity> Accounts { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        DatabaseXmlReader.DatabaseConfiguration config;
        try
        {
            config = DatabaseXmlReader.ParseXmlFile("B:\\database_config.xml");
        }
        catch (FileNotFoundException)
        {
            config = new DatabaseXmlReader.DatabaseConfiguration()
            {
                Host = "localhost",
                Port = "5432",
                Database = "Stellaris",
                UserId = "postgres",
                Password = "password"
            };
        }
        catch (DirectoryNotFoundException)
        {
            config = new DatabaseXmlReader.DatabaseConfiguration()
            {
                Host = "localhost",
                Port = "5432",
                Database = "Stellaris",
                UserId = "postgres",
                Password = "password"
            };
        }

        optionsBuilder.UseNpgsql(config.ToString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CommentEntity>(entity =>
        {
            entity.ToTable("comments");

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.AuthorId)
                .HasColumnName("author_id");

            entity.Property(e => e.Body)
                .HasMaxLength(500)
                .HasColumnName("body");

            entity.Property(e => e.CommentDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("comment_date")
                .HasDefaultValueSql("now()");

            entity.Property(e => e.ProfileId).HasColumnName("profile_id");

            entity.HasOne(d => d.Author)
                .WithMany(p => p.CommentAuthors)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comments_author_id");

            entity.HasOne(d => d.Profile)
                .WithMany(p => p.CommentProfiles)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comments_profile_id");
        });

        modelBuilder.Entity<GameEntity>(entity =>
        {
            entity.ToTable("games");

            entity.HasIndex(e => e.Title, "uq_games_name")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Cost)
                .HasPrecision(10, 2)
                .HasColumnName("cost");

            entity.Property(e => e.CoverGuid)
                .HasColumnName("cover_guid");

            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("description");

            entity.Property(e => e.Developer)
                .HasMaxLength(30)
                .HasColumnName("developer");

            entity.Property(e => e.Title)
                .HasMaxLength(25)
                .HasColumnName("name");

            entity.Property(e => e.AddDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("add_date")
                .HasDefaultValueSql("now()");

            entity.HasOne(d => d.Cover)
                .WithMany(p => p.Games)
                .HasForeignKey(d => d.CoverGuid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_games_cover_guid");
        });

        modelBuilder.Entity<ImageEntity>(entity =>
        {
            entity.HasKey(e => e.Guid)
                .HasName("pk_images");

            entity.ToTable("images");

            entity.Property(e => e.Guid)
                .HasColumnName("guid")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(e => e.Data)
                .HasColumnName("data");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<LibraryEntity>(entity =>
        {
            entity.HasKey(e => new { UserId = e.AccountId, e.GameId })
                .HasName("pk_library");

            entity.ToTable("library");

            entity.Property(e => e.AccountId)
                .HasColumnName("user_id");

            entity.Property(e => e.GameId)
                .HasColumnName("game_id");

            entity.Property(e => e.PurchaseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("purchase_date")
                .HasDefaultValueSql("now()");

            entity.HasOne(d => d.Game)
                .WithMany(p => p.Libraries)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("fk_library_game");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Library)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_library_user");
        });

        modelBuilder.Entity<RoleEntity>(entity =>
        {
            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "uq_roles_name")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");
        });

        modelBuilder.Entity<AccountEntity>(entity =>
        {
            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "uq_users_email")
                .IsUnique();

            entity.HasIndex(e => e.Nickname, "uq_users_nickname")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.About)
                .HasMaxLength(250)
                .HasColumnName("about");

            entity.Property(e => e.SingleImageId)
                .HasColumnName("avatar_guid");

            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance");

            entity.Property(e => e.Deleted)
                .HasColumnName("deleted");

            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");

            entity.Property(e => e.Nickname)
                .HasMaxLength(20)
                .HasColumnName("nickname");

            entity.Property(e => e.Password)
                .HasMaxLength(12)
                .HasColumnName("password");

            entity.Property(e => e.RegistrationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("registration_date")
                .HasDefaultValueSql("now()");

            entity.Property(e => e.RoleId)
                .HasColumnName("role_id");

            entity.HasOne(d => d.SingleImageEntity)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.SingleImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_avatar_guid");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_role_id");
        });

        modelBuilder.Entity<RoleEntity>().HasData(
            new RoleEntity { Id = 1, Name = "Администратор" },
            new RoleEntity { Id = 2, Name = "Игрок" });

        modelBuilder.Entity<AccountEntity>().HasData(new AccountEntity
        {
            Id = 1,
            RoleId = 1,
            Email = "admin@mail.ru",
            Nickname = "Stellarity.Desktop",
            Password = "P@ssw0rd"
        });
    }
}