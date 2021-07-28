using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class ProgrammingLanguageDetailViewModel : DetailViewModelBase
    {
        private readonly IProgrammingLanguageRepository programmingLanguageRepository;
        private ProgrammingLanguageWrapper selectedProgrammingLanguage;

        public ObservableCollection<ProgrammingLanguageWrapper> ProgrammingLanguages { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; set; }

        public ProgrammingLanguageDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService, IProgrammingLanguageRepository programmingLanguageRepository)
            : base(eventAggregator, messageDialogService)
        {
            Title = "Programming Languages";
            this.programmingLanguageRepository = programmingLanguageRepository;
            ProgrammingLanguages = new ObservableCollection<ProgrammingLanguageWrapper>();

            AddCommand = new DelegateCommand(OnAddExecute);
            RemoveCommand = new DelegateCommand(OnRemoveExecute, OnRemoveCanExecute);
        }

        public ProgrammingLanguageWrapper SelectedProgrammingLanguage
        {
            get { return selectedProgrammingLanguage; }
            set
            {
                selectedProgrammingLanguage = value;
                OnPropertyChanged();
                ((DelegateCommand)RemoveCommand).RaiseCanExecuteChanged();
            }
        }


        public async override Task LoadAsync(int id)
        {
            Id = id;

            foreach (var wrapper in ProgrammingLanguages)
            {
                wrapper.PropertyChanged -= Wrapper_PropertyChanged;
            }

            ProgrammingLanguages.Clear();
            var languages = await programmingLanguageRepository.GetAllAsync();

            foreach (var model in languages)
            {
                var wrapper = new ProgrammingLanguageWrapper(model);
                wrapper.PropertyChanged += Wrapper_PropertyChanged;
                ProgrammingLanguages.Add(wrapper);
            }
        }

        private void Wrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
                HasChanges = programmingLanguageRepository.HasChanges();

            if (e.PropertyName == nameof(ProgrammingLanguageWrapper.HasErrors))
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        protected override void OnDeleteExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute()
        {
            return HasChanges && ProgrammingLanguages.All(p => !p.HasErrors);
        }

        protected async override void OnSaveExecute()
        {
            try
            {
                await programmingLanguageRepository.SaveAsync();
                HasChanges = programmingLanguageRepository.HasChanges();
                RaiseCollectionSavedEvent();
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                    ex = ex.InnerException;

                MessageDialogService.ShowInfoDialog($"Error while saving the entities, " +
                    $"the data will be reloaded. Details: {ex.Message}");
                await LoadAsync(Id);
            }
        }

        private bool OnRemoveCanExecute()
        {
            return SelectedProgrammingLanguage != null;
        }

        private void OnRemoveExecute()
        {
            SelectedProgrammingLanguage.PropertyChanged -= Wrapper_PropertyChanged;
            programmingLanguageRepository.Remove(SelectedProgrammingLanguage.Model);
            ProgrammingLanguages.Remove(SelectedProgrammingLanguage);
            SelectedProgrammingLanguage = null;
            HasChanges = programmingLanguageRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private void OnAddExecute()
        {
            var wrapper = new ProgrammingLanguageWrapper(new ProgrammingLanguage());
            wrapper.PropertyChanged += Wrapper_PropertyChanged;
            programmingLanguageRepository.Add(wrapper.Model);
            ProgrammingLanguages.Add(wrapper);

            // Trigger validation
            wrapper.Name = string.Empty;
        }
    }
}
