using Domain.UseCases.GuessLetter;
using NSubstitute;
using NUnit.Framework;

namespace InterfaceAdapters.Controllers.Tests
{
    [TestFixture]    
    public class KeyboardControllerTest
    {
        [Test]
        public void WhenReceiveCommandOnKeyPressedPressed_CallToGuessLetter()
        {
            var keyboardViewModel = new KeyboardViewModel();
            var guessLetter = Substitute.For<GuessLetter>();
            var keyboardController = new KeyboardController(keyboardViewModel, guessLetter);
            
            keyboardViewModel.OnKeyPressedPressed.Execute("A");

            guessLetter.Received().Guess("A");
        }
    }
}