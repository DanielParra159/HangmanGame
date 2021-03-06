using System.Collections.Generic;
using UniRx;

namespace InterfaceAdapters.Controllers
{
    public class InGameViewModel
    {
        // TODO: extract these colors
        public const string DefaultColor = "#FFFFFF";
        public const string IncorrectColor = "#DF3939";
        public const string CorrectColor = "#6FE25D";

        public class KeyButtonViewModel
        {
            public readonly ReactiveProperty<bool> IsEnabled;
            public readonly ReactiveProperty<string> Color;

            public KeyButtonViewModel()
            {
                IsEnabled = new BoolReactiveProperty(true);
                Color = new StringReactiveProperty(DefaultColor);
            }
        }


        public readonly ReactiveProperty<string> CurrentWord;
        public readonly ReactiveCommand<string> OnKeyPressedPressed;
        public readonly Dictionary<string, KeyButtonViewModel> KeyButtonsViewModel;
        public readonly ReactiveProperty<bool> IsVisible;
        public readonly ReactiveProperty<bool> IsEndGameVisible;
        public readonly ReactiveProperty<bool> IsVictoryVisible;
        public readonly ReactiveProperty<bool> IsGameOverVisible;
        public readonly ReactiveCommand OnRestartGamePressed;
        public readonly List<ReactiveProperty<bool>> IsGallowPartVisible;
        public int NextGallowPartToShow { get; set; }


        public InGameViewModel()
        {
            CurrentWord = new StringReactiveProperty();
            OnKeyPressedPressed = new ReactiveCommand<string>();
            KeyButtonsViewModel = new Dictionary<string, KeyButtonViewModel>();
            IsVisible = new BoolReactiveProperty(false);
            IsEndGameVisible = new BoolReactiveProperty(false);
            IsVictoryVisible = new BoolReactiveProperty(false);
            IsGameOverVisible = new BoolReactiveProperty(false);
            OnRestartGamePressed = new ReactiveCommand();
            IsGallowPartVisible = new List<ReactiveProperty<bool>>();
        }


        public KeyButtonViewModel SubscribeKeyButton(string key)
        {
            var keyButtonViewModel = new KeyButtonViewModel();
            KeyButtonsViewModel.Add(key, keyButtonViewModel);
            return keyButtonViewModel;
        }

        public ReactiveProperty<bool> SubscribeGallowImage()
        {
            var boolReactiveProperty = new BoolReactiveProperty(false);
            IsGallowPartVisible.Add(boolReactiveProperty);
            return boolReactiveProperty;
        }
    }
}
