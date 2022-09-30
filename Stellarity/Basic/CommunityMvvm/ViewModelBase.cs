using System;
using System.Collections;
using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveValidation;

namespace Stellarity.Basic.CommunityMvvm;

public class ViewModelBase : ObservableObject, IValidatableObject, IViewModelBase
{
    public Guid Id = new();
    private IObjectValidator? _validator;

    public IObjectValidator? Validator
    {
        get => _validator;
        set
        {
            _validator?.Dispose();
            _validator = value;
            _validator?.Revalidate();
        }
    }

    public bool HasErrors => Validator?.IsValid == true || Validator?.HasWarnings == false;

    public IEnumerable GetErrors(string? propertyName)
    {
        if (Validator == null)
            return Array.Empty<ValidationMessage>();

        return string.IsNullOrEmpty(propertyName)
            ? Validator.ValidationMessages
            : Validator.GetMessages(propertyName);
    }

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public void OnPropertyMessagesChanged(string propertyName) =>
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
}