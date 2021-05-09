using FriendOrganizer.Model;
using FriendOrganizer.UI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriendOrganizer.UI.ViewModel
{
    public class FriendDetailViewModel : ViewModelBase, IFriendDetailViewModel
    {
        public Friend Friend
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
        private Friend _friend;

        private readonly IFriendDataService dataService;

        public FriendDetailViewModel(IFriendDataService dataService)
        {
            this.dataService = dataService;
        }

        public async Task LoadAsync(int friendId)
        {
            Friend = await dataService.GetIdByAsync(friendId);
        }
    }
}
