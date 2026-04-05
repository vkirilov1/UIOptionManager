using Control.Others.Commands;
using Control.Others.Constants;
using Control.ViewModel.OptionList;
using Service.Model.Users;
using Service.Others.Identifiers.Constants;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Service.Service;

namespace Control.ViewModel.JobApplicationVM
{
    public class JobApplicationViewModel : INotifyPropertyChanged
    {
        private string _position;
        private string _coverLetter;

        public ObservableCollection<BaseOptionListViewModel> OptionLists { get; set; }

        public string Position
        {
            get => _position;
            set { _position = value; OnPropertyChanged(); }
        }

        public string CoverLetter
        {
            get => _coverLetter;
            set { _coverLetter = value; OnPropertyChanged(); }
        }

        public ICommand SubmitCommand { get; }
        public User User { get; set; }

        private JobApplicationService _service = new();

        public JobApplicationViewModel(User user)
        {
            User = user;
            OptionLists = new ObservableCollection<BaseOptionListViewModel>();
            LoadOptionLists(user.Id);
            SubmitCommand = new DelegateCommandNoParam(Submit);
        }

        private void LoadOptionLists(int userId)
        {
            OptionLists.Clear();
            OptionLists.Add(new SystemOptionListViewModel<SystemIdConstants.EmploymentType>(OptionListIdentifier.EmploymentType, userId));
            OptionLists.Add(new MixedOptionListViewModel<SystemIdConstants.WorkLocation>(OptionListIdentifier.WorkLocations, userId));
        }

        private void Submit()
        {
            var employmentTypeVM = OptionLists.FirstOrDefault(x => x.ListIdentifier == OptionListIdentifier.EmploymentType);
            var workLocationVM = OptionLists.FirstOrDefault(x => x.ListIdentifier == OptionListIdentifier.WorkLocations);

            string selectedEmployment = employmentTypeVM?.SelectedOption?.Value;
            string selectedLocation = workLocationVM?.SelectedOption?.Value;

            if (string.IsNullOrWhiteSpace(Position) || string.IsNullOrEmpty(CoverLetter) || string.IsNullOrWhiteSpace(selectedEmployment) || string.IsNullOrEmpty(selectedLocation))
            {
                System.Windows.MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (CoverLetter.Length < 200)
            {
                System.Windows.MessageBox.Show("Cover Letter must be at least 200 characters.");
                return;
            }

            _service.Apply(User, Position, CoverLetter, selectedEmployment, selectedLocation);

            System.Windows.MessageBox.Show($"Application for {Position} submitted!");

            ClearFields();
        }

        private void ClearFields()
        {
            Position = string.Empty;
            CoverLetter = string.Empty;

            foreach (var list in OptionLists)
            {
                list.SelectedOption = null;
                list.FilterText = string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}