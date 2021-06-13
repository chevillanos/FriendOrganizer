using FriendOrganizer.Model;
using FriendOrganizer.UI.Data.Lookups;
using FriendOrganizer.UI.Data.Repositories;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using FriendOrganizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        private FriendWrapper _friend;
        private readonly IFriendRepository friendRepository;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageDialogService messageDialogService;
        private readonly IProgrammingLanguageLookupDataService programmingLanguageLookupDataService;
        private bool hasChanges;

        public FriendDetailViewModel(IFriendRepository friendRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageLookupDataService programmingLanguageLookupDataService)
        {
            this.friendRepository = friendRepository;
            this.eventAggregator = eventAggregator;
            this.messageDialogService = messageDialogService;
            this.programmingLanguageLookupDataService = programmingLanguageLookupDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync(int? friendId)
        {
            var friend = friendId.HasValue ?
                await friendRepository.GetIdByAsync(friendId.Value)
                : CreateNewFriend();

            InitializeFriend(friend);

            await LoadProgrammingLanguagesLookupAsync();
        }

        private void InitializeFriend(Friend friend)
        {
            Friend = new FriendWrapper(friend);
            Friend.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                    HasChanges = friendRepository.HasChanges();

                if (e.PropertyName == nameof(Friend.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            // Trigger the validation on Create
            if (Friend.Id == 0)
                Friend.FirstName = "";
        }

        private async Task LoadProgrammingLanguagesLookupAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - " });
            var lookup = await programmingLanguageLookupDataService.GetProgrammingLanguageLookupAsync();
            foreach (var lookupItem in lookup)
            {
                ProgrammingLanguages.Add(lookupItem);
            }
        }

        public FriendWrapper Friend
        {
            get
            {
                return _friend;
            }
            private set
            {
                _friend = value;
                OnPropertyChanged();
            }
        }

        public bool HasChanges
        {
            get { return hasChanges; }
            set
            {
                if (hasChanges != value)
                {
                    hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }

        private async void OnSaveExecute()
        {
            await friendRepository.SaveAsync();
            HasChanges = friendRepository.HasChanges();
            eventAggregator.GetEvent<AfterFriendSavedEvent>().Publish
                (new AfterFriendSavedEventArgs
                {
                    Id = Friend.Id,
                    DisplayMember = $"{Friend.FirstName} {Friend.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            return Friend != null && !Friend.HasErrors && HasChanges;
        }
        private async void OnDeleteExecute()
        {
            var result = messageDialogService.ShowOkCancelDialog($"Do you really want to delete the friend " +
                $"{Friend.FirstName} {Friend.LastName}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                friendRepository.Remove(Friend.Model);
                await friendRepository.SaveAsync();
                eventAggregator.GetEvent<AfterFriendDeletedEvent>().Publish(Friend.Id);
            }
        }

        private Friend CreateNewFriend()
        {
            var friend = new Friend();
            friendRepository.Add(friend);
            return friend;
        }
    }
}
