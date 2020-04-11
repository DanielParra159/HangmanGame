# HangmanGame

This is just a Unity project for learning purposes, applying <b>TDD</b> and <b>DDD</b> principles, following the architecture proposed by Robert C. Martin in his book <b>Clean Architecture</b>.

![Clean Architecture](https://blog.cleancoder.com/uncle-bob/images/2012-08-13-the-clean-architecture/CleanArchitecture.jpg)

In this architecture the components from an inner layer cannot speak with components in an outer layer, <b>helping to keep our domain testable and decoupled from everything</b>. For this communication the <b>inversion of control principle</b> is used, the outer layers must subscribe to the inner layers if they want to know their results, for this I have used an improvised <i>EventDispatcher</i>.

For the comunication with the view I have used a reactive <b>MVVM pattern</b>, using [UniRx](https://github.com/neuecc/UniRx) to do it reactive. Thanks to this pattern, we can maintain our view logic decoupled of Unity by <b>facilitating testing with unit tests</b>.

For the creation of <b>End to end (E2E) tests</b>, since Unity does not support <b>cucumber</b> or similar, I have improvised some [helper functions](https://github.com/DanielParra159/HangmanGame/blob/master/Assets/Scripts/EndToEndTests/StartGameE2E.cs) for this purpose.

For the server I have used this [hangman api](https://hangman-api.herokuapp.com/api).

## CI
I have created a workflow to [run the tests](https://github.com/DanielParra159/HangmanGame/blob/master/.github/workflows/run_tests.yml) during pull request step and other to [build](https://github.com/DanielParra159/HangmanGame/blob/master/.github/workflows/build.yml) and archive the project when merges to master. To do that I have used [Unity actions](https://github.com/webbertakken/unity-actions) that uses [Unity3D docker](https://gitlab.com/gableroux/unity3d) images from [GabLeRoux](https://github.com/GabLeRoux).

## Next steps

* Improve graphics.
* Introduce game over.

## Workflows

[Unity actions](https://github.com/webbertakken/unity-actions): used to run all the tests during pull request and build the project when push to master.

[Labeler](https://github.com/marketplace/actions/labeler?version=v3-preview): used to assign labels during pull requests depending on the modified files.

[Auto assign action](https://github.com/marketplace/actions/auto-assign-action): used to auto-assign the author of the pull request to this.

[Check critical files](https://github.com/CodelyTV/check-critical-files): used to check critical files/folders and add a commentary in the pull request.

## Unity
Version 2019.3.9f1
