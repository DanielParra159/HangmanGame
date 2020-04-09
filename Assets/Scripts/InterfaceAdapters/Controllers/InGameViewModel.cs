using System.Collections.Generic;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class InGameViewModel
    {
        // TODO: extract these colors
        public const string WhiteColor = "#FFFFFF";
        public const string IncorrectColor = "#DF3939";
        public const string CorrectColor = "#6FE25D";
        
        public class KeyButtonViewModel
        {
            public readonly ReactiveProperty<bool> IsEnabled;
            public readonly ReactiveProperty<string> Color;

            public KeyButtonViewModel()
            {
                IsEnabled = new BoolReactiveProperty(true);
                
                Color = new StringReactiveProperty(WhiteColor);
            }
        }
        
        public readonly ReactiveProperty<string> CurrentWord;
        public readonly ReactiveCommand<string> OnKeyPressedPressed;
        public readonly Dictionary<string, KeyButtonViewModel> KeyButtonsViewModel;
        public readonly ReactiveProperty<bool> IsVisible;

        public InGameViewModel()
        {
            CurrentWord = new StringReactiveProperty();
            OnKeyPressedPressed = new ReactiveCommand<string>();
            KeyButtonsViewModel = new Dictionary<string, KeyButtonViewModel>();
            IsVisible = new BoolReactiveProperty(false);
        }

        public KeyButtonViewModel SubscribeKeyButton(string key)
        {
            var keyButtonViewModel = new KeyButtonViewModel();
            KeyButtonsViewModel.Add(key, keyButtonViewModel);
            return keyButtonViewModel;
        }
    }
}