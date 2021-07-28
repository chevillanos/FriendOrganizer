using Autofac.Features.Indexed;
using FriendOrganizer.UI.Event;
using FriendOrganizer.UI.View.Services;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FriendOrganizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IIndex<string, IDetailViewModel> detailViewModelCreator;
        private readonly IEventAggregator eventAggregator;
        private readonly IMessageDialogService messageDialogService;
        private IDetailViewModel selectedDetailViewModel;


        public MainViewModel(INavigationViewModel navigationViewModel,
            IIndex<string, IDetailViewModel> detailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            this.eventAggregator = eventAggregator;
            this.messageDialogService = messageDialogService;
            NavigationViewModel = navigationViewModel;

            DetailViewModels = new ObservableCollection<IDetailViewModel>();

            eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Subscribe(OnOpenDetailView);
            eventAggregator.GetEvent<AfterDetailDeletedEvent>()
                .Subscribe(AfterDetailDeleted);
            eventAggregator.GetEvent<AfterDetailClosedEvent>()
                .Subscribe(AfterDetailClosed);

            CreateNewDetailCommand = new DelegateCommand<Type>(OnCreateNewDetailExecute);
            OpenSingleDetailViewCommand = new DelegateCommand<Type>(OnOpenSingleDetailViewExecute);

            this.detailViewModelCreator = detailViewModelCreator;
        }

        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public ICommand CreateNewDetailCommand { get; }
        public ICommand OpenSingleDetailViewCommand { get; }
        public INavigationViewModel NavigationViewModel { get; }
        public ObservableCollection<IDetailViewModel> DetailViewModels { get; }
        public IDetailViewModel SelectedDetailViewModel
        {
            get { return selectedDetailViewModel; }
            set
            {
                selectedDetailViewModel = value;
                OnPropertyChanged();
            }
        }

        private async void OnOpenDetailView(OpenDetailViewEventArgs args)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id &&
                vm.GetType().Name == args.ViewModelName);

            if (detailViewModel == null)
            {
                detailViewModel = detailViewModelCreator[args.ViewModelName];
                await detailViewModel.LoadAsync(args.Id);
                DetailViewModels.Add(detailViewModel);
            }

            SelectedDetailViewModel = detailViewModel;
        }

        private int nextNewItemId = 0;
        private void OnCreateNewDetailExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            {
                Id = nextNewItemId--,
                ViewModelName = viewModelType.Name
            });
        }

        private void OnOpenSingleDetailViewExecute(Type viewModelType)
        {
            OnOpenDetailView(new OpenDetailViewEventArgs
            {
                Id = -1,
                ViewModelName = viewModelType.Name
            });
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            var detailViewModel = DetailViewModels
                .SingleOrDefault(vm => vm.Id == args.Id &&
                vm.GetType().Name == args.ViewModelName);

            if (detailViewModel != null)
                DetailViewModels.Remove(detailViewModel);
        }

        private void RemoveDetailViewModel(int id, string viewModelName)
        {
            var detailViewModel = DetailViewModels
                            .SingleOrDefault(vm => vm.Id == id
                            && vm.GetType().Name == viewModelName);

            if (detailViewModel != null)
                DetailViewModels.Remove(detailViewModel);
        }

        private void AfterDetailClosed(AfterDetailClosedEventArgs args)
        {
            RemoveDetailViewModel(args.Id, args.ViewModelName);
        }
    }
}
