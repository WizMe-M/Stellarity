using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;

namespace Stellarity.Basic;

/// <summary>
/// Base for Styling without generic parameter
/// </summary>
public abstract class ExtUserControl : UserControl
{
}

/// <summary>
/// User control that has ViewModel property and method to initialize ViewModel asynchronously
/// </summary>
/// <typeparam name="TViewModel">Type of your ViewModel for this View</typeparam>
/// <remarks>ViewModel and DataContext depend on each other, so ViewModel is a generic-typed DataContext</remarks>
public class ExtUserControl<TViewModel> : ExtUserControl
    where TViewModel : class
{
    public static readonly StyledProperty<TViewModel?> ViewModelProperty = AvaloniaProperty
        .Register<ExtUserControl<TViewModel>, TViewModel?>(nameof(ViewModel));

    /// <summary>
    /// Initializes a new instance of the <see cref="ExtUserControl{TViewModel}"/> class.
    /// </summary>
    public ExtUserControl()
    {
        this.GetObservable(ViewModelProperty).Subscribe(OnViewModelChanged);
    }

    /// <summary>
    /// The ViewModel.
    /// </summary>
    public TViewModel? ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);
        ViewModel = DataContext as TViewModel;
    }

    private void OnViewModelChanged(object? value)
    {
        if (value == null)
        {
            ClearValue(DataContextProperty);
        }
        else if (DataContext != value)
        {
            DataContext = value;
        }
    }

    public virtual Task InitializeViewModelAsync(TViewModel viewModel)
    {
        ViewModel = viewModel;
        return Task.CompletedTask;
    }
}