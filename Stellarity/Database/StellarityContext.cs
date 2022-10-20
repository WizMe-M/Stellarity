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
    public DbSet<AccountEntity> Accounts { get; set; } = null!;
    public DbSet<KeyEntity> Keys { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured) return;
        DatabaseConfiguration config;
        try
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var path = Path.Join(currentDirectory, "config.xml");
            config = DatabaseXmlReader.ParseXmlFile(path);
        }
        catch (FileNotFoundException)
        {
            config = DatabaseConfiguration.FromDefault();
        }
        catch (DirectoryNotFoundException)
        {
            config = DatabaseConfiguration.FromDefault();
        }

        optionsBuilder.UseNpgsql(connectionString: config.ToString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum<Roles>();

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
                .WithMany(p => p.CommentWhereIsAuthor)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_comments_author_id");

            entity.HasOne(d => d.Profile)
                .WithMany(p => p.CommentWhereIsProfile)
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

            entity.Property(e => e.SingleImageId)
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

            entity.HasOne(d => d.SingleImageEntity)
                .WithMany(p => p.Games)
                .HasForeignKey(d => d.SingleImageId)
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

        modelBuilder.Entity<KeyEntity>(entity =>
        {
            entity.HasKey(e => e.KeyValue)
                .HasName("pk_key");

            entity.ToTable("key");

            entity.Property(e => e.KeyValue)
                .HasColumnName("key_value");

            entity.Property(e => e.GameId)
                .HasColumnName("game_id");

            entity.Property(e => e.AccountId)
                .HasColumnName("buyer_id")
                .IsRequired(false);

            entity.HasIndex(e => new { e.AccountId, e.GameId })
                .IsUnique();

            entity.Property(e => e.PurchaseDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("purchase_date")
                .IsRequired(false);

            entity.HasOne(d => d.Game)
                .WithMany(p => p.Keys)
                .HasForeignKey(d => d.GameId)
                .HasConstraintName("fk_key_game");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.Keys)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_key_user");
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
                .HasColumnName("about")
                .IsRequired(false);

            entity.Property(e => e.SingleImageId)
                .HasColumnName("avatar_guid");

            entity.Property(e => e.Balance)
                .HasPrecision(10, 2)
                .HasColumnName("balance")
                .HasDefaultValue(0);

            entity.Property(e => e.Banned)
                .HasColumnName("banned")
                .HasDefaultValue(false);

            entity.Property(e => e.Activated)
                .HasColumnName("activated")
                .HasDefaultValue(false);

            entity.Property(e => e.Email)
                .HasMaxLength(320)
                .HasColumnName("email");

            entity.Property(e => e.Nickname)
                .HasMaxLength(20)
                .HasColumnName("nickname")
                .IsRequired(false);

            entity.Property(e => e.Password)
                .HasMaxLength(32)
                .HasColumnName("password");

            entity.Property(e => e.RegistrationDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("registration_date")
                .HasDefaultValueSql("now()");

            entity.Property(e => e.Role)
                .HasColumnName("user_role")
                .HasConversion<string>();

            entity.HasOne(d => d.SingleImageEntity)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.SingleImageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_avatar_guid");
        });

        modelBuilder.Entity<AccountEntity>().HasData(new AccountEntity
        {
            Id = 1,
            Role = Roles.Administrator,
            Email = "admin@mail.ru",
            Nickname = "Stellarity.Desktop",
            Password = "P@ssw0rd"
        });
    }
}