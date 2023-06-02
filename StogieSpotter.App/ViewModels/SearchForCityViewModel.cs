using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StogieSpotter.PlacesApi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StogieSpotter.App.ViewModels
{
    public partial class SearchForCityViewModel : ObservableObject
    {
        private GooglePlacesService _placesService;

        [ObservableProperty]
        private ObservableCollection<string> _suggestions = new ObservableCollection<string>();
        [ObservableProperty]
        private string _query;

        private HomeViewModel _homeViewModel;

        public SearchForCityViewModel(GooglePlacesService placesService, HomeViewModel homeViewModel)
        {
            _placesService = placesService;
            _homeViewModel = homeViewModel;
        }

        [RelayCommand]
        public async void FindResults()
        {
            if (Query.Length < 3)
            {
                Suggestions.Clear();
                return;
            }
            var results = await _placesService.GetAutocompleteResults(Query);
            results = results.Take(6).ToList();
            Suggestions = new ObservableCollection<string>();
            results.ForEach(x => Suggestions.Add(x));
        }

        [RelayCommand]
        public void CitySelected(string selectedValue)
        {
            _homeViewModel.CitySelected(selectedValue);
        }
    }
}