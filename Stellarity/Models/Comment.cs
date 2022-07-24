using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stellarity.Database;

namespace Stellarity.Models;

public partial class Comment
{
    /// <summary>
    /// Обязательный конструктор. Не удалять!
    /// </summary>
    public Comment()
    {
    }

    public Comment(User commentator, User profile, string text) : this()
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

    public virtual User Author { get; set; } = null!;
    public virtual User Profile { get; set; } = null!;

    public static Comment Send(User author, User profile, string text)
    {
        using var context = new StellarisContext();
        var comment = new Comment(author, profile, text);
        context.Comments.Add(comment);
        context.SaveChanges();
        return comment;
    }
        
    public static Task<Comment> SendAsync(User author, User profile, string text)
    {
        using var context = new StellarisContext();
        var comment = new Comment(author, profile, text);
        context.Comments.Add(comment);
        context.SaveChanges();
        return Task.FromResult(comment);
    }

    public static IEnumerable<Comment> GetAll(User profile)
    {
        using var context = new StellarisContext();
        var comments = context.Comments.Where(comment => comment.ProfileId == profile.Id).ToArray(); 
        return comments.OrderByDescending(comment => comment.CommentDate);
    }
}