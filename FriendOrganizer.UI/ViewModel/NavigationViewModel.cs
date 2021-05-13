using FriendOrganizer.UI.Data;
using FriendOrganizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IFriendLookupDataService friendLookupService;
        private readonly IEventAggregator eventAggregator;
        private NavigationItemViewModel selectedFriend;

        public ObservableCollection<NavigationItemViewModel> Friends { get; }
        public NavigationItemViewModel SelectedFriend
        {
            get { return selectedFriend; }
            set
            {
                selectedFriend = value;
                OnPropertyChanged();

                if (selectedFriend != null)
                    eventAggregator.GetEvent<OpenFriendDetailViewEvent>()
                        .Publish(selectedFriend.Id);
            }
        }

        public NavigationViewModel(IFriendLookupDataService friendLookupService,
            IEventAggregator eventAggregator)
        {
            this.friendLookupService = friendLookupService;
            this.eventAggregator = eventAggregator;
            Friends = new ObservableCollection<NavigationItemViewModel>();
            eventAggregator.GetEvent<AfterFriendSavedEvent>().Subscribe
                (AfterFriendSaved);
        }

        public async Task LoadAsync()
        {
            var lookup = await friendLookupService.GetFriendLookupAsync();
            Friends.Clear();
            foreach (var item in lookup)
            {
                Friends.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
            }
        }

        private void AfterFriendSaved(AfterFriendSavedEventArgs obj)
        {
            var lookupItem = Friends.Single(l => l.Id == obj.Id);
            lookupItem.DisplayMember = obj.DisplayMember;
        }


    }
}
