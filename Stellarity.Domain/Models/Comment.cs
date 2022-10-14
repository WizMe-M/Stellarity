using Stellarity.Database.Entities;
using Stellarity.Domain.Abstractions;

namespace Stellarity.Domain.Models;

public class Comment : DomainModel<CommentEntity>
{
    public Comment(CommentEntity entity) : base(entity)
    {
        Body = Entity.Body;
        CommentDate = Entity.CommentDate;
        Author = new Account(Entity.Author);
        Profile = new Account(Entity.Profile);
    }

    private Comment(CommentEntity entity, Account author) : base(entity)
    {
        Body = Entity.Body;
        CommentDate = Entity.CommentDate;
        Author = author;
        Profile = author;
    }

    private Comment(CommentEntity entity, Account profile, Account author) : base(entity)
    {
        Body = Entity.Body;
        CommentDate = Entity.CommentDate;
        Author = author;
        Profile = profile;
    }

    public string Body { get; private set; }

    public DateTime CommentDate { get; }

    public Account Author { get; }
    public Account Profile { get; }

    public static Comment SendOnMyProfile(string text, Account profile)
    {
        var entity = CommentEntity.Add(text, profile.Entity);
        var comment = new Comment(entity, profile);
        return comment;
    }

    public static Comment SendOnOtherProfile(string text, Account profile, Account sender)
    {
        var entity = CommentEntity.Add(text, profile.Entity, sender.Entity);
        var comment = new Comment(entity, profile, sender);
        return comment;
    }

    public bool IsOwner(Account author) => Entity.AuthorId == author.Entity.Id;

    public void Edit(string body)
    {
        Entity.UpdateBody(body);
        Body = Entity.Body;
    }
}