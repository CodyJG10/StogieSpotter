using CommunityToolkit.Maui.Views;
using StogieSpotter.App.ViewModels;

namespace StogieSpotter.App.Views;

public partial class SearchForCity : Popup
{
    public SearchForCity(SearchForCityViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        (BindingContext as SearchForCityViewModel).FindResultsCommand.Execute(null);
    }

    private void suggestionsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var selectedCity = suggestionsListView.SelectedItem as string;
        var viewModel = BindingContext as SearchForCityViewModel;
        viewModel.CitySelectedCommand.Execute(selectedCity);
        Close();
    }
}