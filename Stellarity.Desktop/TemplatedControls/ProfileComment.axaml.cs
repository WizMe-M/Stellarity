using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Stellarity.Desktop.TemplatedControls;

[PseudoClasses(MyOwn)]
public class ProfileComment : TemplatedControl
{
    private const string MyOwn = "my_own";

    public ProfileComment()
    {
        PseudoClasses.Set(MyOwn, false);
        PropertyChanged += OnIsMyOwnChanged;
    }

    private void OnIsMyOwnChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property != IsMyOwnProperty) return;
        if (e.NewValue is not bool myOwn) return;
        PseudoClasses.Set(MyOwn, myOwn);
        if (myOwn) Classes.Add(MyOwn);
        else Classes.Remove(MyOwn);
    }

    public static readonly StyledProperty<string> AuthorProperty =
        AvaloniaProperty.Register<ProfileComment, string>(nameof(Author));

    public static readonly StyledProperty<string> CommentBodyProperty =
        AvaloniaProperty.Register<ProfileComment, string>(nameof(CommentBody));

    public static readonly StyledProperty<string> CommentDateProperty =
        AvaloniaProperty.Register<ProfileComment, string>(nameof(CommentDate));

    public static readonly StyledProperty<bool> IsMyOwnProperty =
        AvaloniaProperty.Register<ProfileComment, bool>(nameof(IsMyOwn));

    public string Author
    {
        get => GetValue(AuthorProperty);
        set => SetValue(AuthorProperty, value);
    }

    public string CommentBody
    {
        get => GetValue(CommentBodyProperty);
        set => SetValue(CommentBodyProperty, value);
    }

    public string CommentDate
    {
        get => GetValue(CommentDateProperty);
        set => SetValue(CommentDateProperty, value);
    }

    public bool IsMyOwn
    {
        get => GetValue(IsMyOwnProperty);
        set => SetValue(IsMyOwnProperty, value);
    }
}