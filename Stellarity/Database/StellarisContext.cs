using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stellarity.Database.Entities;

namespace Stellarity.Database;

public sealed class StellarisContext : DbContext
{
    /// <summary>
    /// Checks is database existing and creates it if not
    /// </summary>
    /// <returns>Did database exist</returns>
    public static async Task<bool> CreateDatabaseAsync()
    {
        await using var context = new StellarisContext();
        return await context.Database.EnsureCreatedAsync();
    }

    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<Game> Games { get; set; } = null!;
    public DbSet<Image> Images { get; set; } = null!;
    public DbSet<Library> Libraries { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Account> Users { get; set; } = null!;

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

        optionsBuilder.UseNpgsql(config.ToString());
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
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

        modelBuilder.Entity<Game>(entity =>
        {
            entity.ToTable("games");

            entity.HasIndex(e => e.Name, "uq_games_name")
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

            entity.Property(e => e.Name)
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

        modelBuilder.Entity<Image>(entity =>
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

        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.GameId })
                .HasName("pk_library");

            entity.ToTable("library");

            entity.Property(e => e.UserId)
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
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_library_user");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "uq_roles_name")
                .IsUnique();

            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .HasColumnName("name");

            entity.Ignore(e => e.CanAddGames);
        });

        modelBuilder.Entity<Account>(entity =>
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

            entity.Property(e => e.AvatarGuid)
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

            entity.HasOne(d => d.Avatar)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.AvatarGuid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_users_avatar_guid");

            entity.HasOne(d => d.Role)
                .WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_role_id");
        });

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Администратор" },
            new Role { Id = 2, Name = "Игрок" });

        modelBuilder.Entity<Account>().HasData(new Account
        {
            Id = 1,
            RoleId = 1,
            Email = "admin@mail.ru",
            Nickname = "Stellarity",
            Password = "P@ssw0rd"
        });
    }
}