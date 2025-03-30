﻿using Service.Exceptions;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UI.ViewModel.Option;

namespace UI.ViewModel
{
    public class BaseOptionListViewModel : INotifyPropertyChanged
    {
        private OptionViewModel? _selectedOption;

        public string Name { get; set; }

        public string? Description { get; set; }

        protected readonly List<OptionViewModel> _options = [];

        public IReadOnlyList<OptionViewModel> Options => _options.AsReadOnly();

        public OptionViewModel SelectedOption
        {
            get => _selectedOption;
            set
            {
                _selectedOption = value;
                OnPropertyChanged();
            }
        }

        public BaseOptionListViewModel(string name)
        {
            Name = name ?? throw new EmptyListNameException(nameof(BaseOptionListViewModel));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
