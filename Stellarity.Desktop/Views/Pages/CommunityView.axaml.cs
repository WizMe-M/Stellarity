﻿using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using Stellarity.Desktop.ViewModels.Pages;
using Stellarity.Navigation.Event;

namespace Stellarity.Desktop.Views.Pages;

public partial class CommunityView : ReactiveUserControl<CommunityViewModel>, IContentPage
{
    public CommunityView()
    {
        AvaloniaXamlLoader.Load(this);
        this.WhenActivated(async d => await ViewModel!.LoadAsync());
    }
}