using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace SquirrelStash.ViewModel
{
    public partial class ItemCardViewModel : ObservableObject
    {
        [ObservableProperty]
        private string name = "Test name";

        [ObservableProperty]
        private int quantity = 0;


        [RelayCommand]
        private void IncreaseQuantity()
        {
            Quantity++;
        }

        [RelayCommand]
        private void DecreaseQuantity()
        {
            if (Quantity > 0)
                Quantity--;
        }
    }
}
