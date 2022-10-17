using System;
using Stellarity.Desktop.Basic;
using Stellarity.Domain.Models;

namespace Stellarity.Desktop.ViewModels.Wraps;

public class CommentViewModel : ViewModelBase
{
    private readonly Comment _comment;
    public CommentViewModel(Comment comment, Account viewer)
    {
        _comment = comment;
        Viewer = viewer;
    }

    public Account Viewer { get; }
    public Account Author => _comment.Author;
    public DateTime CommentDate => _comment.CommentDate;
    public string CommentBody => _comment.Body;
}