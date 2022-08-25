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

    public static Comment Send(string text, Account profile, Account? sender = null)
    {
        sender ??= profile;
        var comment = new Comment
        {
            Body = text,
            Author = sender, AuthorId = sender.Id,
            Profile = profile, ProfileId = profile.Id
        };
        // TODO: insert into db table
        // context.Comments.Add(comment);
        // var id = comment.Id;
        // comment = context.Comments.First(c => c.Id == id);
        return comment;
    }
}