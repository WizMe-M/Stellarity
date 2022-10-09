using System;

namespace Stellarity.Database.Entities;

public partial class Comment
{
    /// <summary>
    /// Обязательный конструктор. Не удалять!
    /// </summary>
    public Comment()
    {
    }

    public Comment(Account commentator, Account profile, string text) : this()
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

    public virtual Account Author { get; set; } = null!;
    public virtual Account Profile { get; set; } = null!;

    public override bool Equals(object? obj) => obj is Comment comment && comment.Id == Id;

    public static Comment Add(string text, Account profile, Account? sender = null)
    {
        sender ??= profile;
        var comment = new Comment
        {
            Body = text,
            Author = sender, AuthorId = sender.Id,
            Profile = profile, ProfileId = profile.Id
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