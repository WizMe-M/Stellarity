using System;

namespace Stellarity.Database.Entities;

public partial class CommentEntity : IEntity
{
    /// <summary>
    /// Обязательный конструктор. Не удалять!
    /// </summary>
    public CommentEntity()
    {
    }

    public CommentEntity(AccountEntity commentator, AccountEntity profile, string text) : this()
    {
        AuthorId = commentator.Id;
        ProfileId = profile.Id;
        Body = text;
    }

    public int Id { get; set; }
    public int ProfileId { get; set; }
    public int AuthorId { get; set; }
    public string Body { get; set; } = null!;
    public DateTime CommentDate { get; set; }

    public virtual AccountEntity Author { get; set; } = null!;
    public virtual AccountEntity Profile { get; set; } = null!;

    public override bool Equals(object? obj) => obj is CommentEntity comment && comment.Id == Id;

    public static CommentEntity Add(string text, AccountEntity profile, AccountEntity? sender = null)
    {
        sender ??= profile;
        var comment = new CommentEntity
        {
            Body = text,
            AuthorId = sender.Id,
            ProfileId = profile.Id
        };
        using var context = new StellarityContext();
        context.Comments.Add(comment);
        context.SaveChanges();
        comment = context.Entry(comment).Entity;
        return comment;
    }

    public void UpdateBody(string body)
    {
        Body = body;
        using var context = new StellarityContext();
        var comment = context.Entry(this).Entity;
        context.Comments.Update(comment);
        context.SaveChanges();
    }
}