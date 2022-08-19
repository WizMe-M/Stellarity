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
}