using Android.Gms.Common.Util;
using Java.Nio.Channels;
using Java.Security;
using JetBrains.Annotations;
using Microsoft.Maui.Controls.Shapes;
using VacdmApp.Data;

namespace VacdmApp;

public partial class WaypointlePage : ContentPage
{
    private string _waypoint;

    private int _tryCount = 0;

    private int _currentLetterInRow = 0;

    private readonly SolidColorBrush _incorrectColor = new(Color.FromArgb("#232323"));

    private readonly SolidColorBrush _correctLetterColor = new(Colors.Gold);

    private readonly SolidColorBrush _correctPositionColor = new(Colors.Green);

    private enum GuessResult
    {
        LetterIncorrect,
        LetterCorrect,
        LetterAndPositionCorrect
    }

	public WaypointlePage()
	{
		InitializeComponent();
        _waypoint = GetCurrentWaypoint();
	}

    private void ContentPage_Loaded(object sender, EventArgs e)
    {

    }

    private string GetCurrentWaypoint()
    {
        var baseDate = new DateOnly(2023, 12, 27);

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        //Get the day amounts between today and the reference (first day with a waypoint and a possible game)
        var difference = today.DayNumber - baseDate.DayNumber;

        var waypoints = Data.Data.Waypoints;

        if (difference > waypoints.Count)
        {
            //TODO Out of Range Message
            return waypoints[^1];
        }

        return waypoints[difference];
    }

    private void KeyButton_Clicked(object sender, EventArgs e)
    {
        var button = (Button)sender;

        var buttonParentGrid = (Grid)button.Parent;

        var buttonText = ((Label)buttonParentGrid.Children[1]).Text;

        if(buttonText == "ENTER")
        {
            MakeGuess();
            return;
        }

        if(buttonText == "BACK")
        {
            BackKey();
            return;
        }

        EnterKey(buttonText);
    }

    private void EnterKey(string key)
    {
        var currentGrid = GetCurrentGrid(_tryCount);

        var currentLetterGrid = (Grid)currentGrid.Children[_currentLetterInRow];

        ((Label)currentLetterGrid.Children[1]).Text = key;

        if(_currentLetterInRow == 4)
        {
            return;
        }

        _currentLetterInRow++;
    }

    private void BackKey()
    {
        var currentGrid = GetCurrentGrid(_tryCount);

        var currentLetterGrid = (Grid)currentGrid.Children[_currentLetterInRow];

        ((Label)currentLetterGrid.Children[1]).Text = string.Empty;

        if(_currentLetterInRow == 0)
        {
            return;
        }

        _currentLetterInRow--;
    }


    private void MakeGuess()
    {
        if(_tryCount > 5)
        {
            //TODO Fail
            return;
        }

        if(_currentLetterInRow != 4)
        {
            return;
        }

        var currentGrid = GetCurrentGrid(_tryCount);

        var children = currentGrid.Children;
        var guessList = new List<char>(5);

        foreach (var child in children.Cast<Grid>()) 
        {
            var guessString = ((Label)child.Children[1]).Text;

            var guessChar = guessString.ToCharArray()[0];

            guessList.Add(guessChar);
        }

        var waypointLetterList = _waypoint.ToCharArray().ToList();

        var correctLetterCount = 0;

        var iterator = 0;

        foreach(var guessLetter in guessList)
        {
            //Check if letter and position is correct
            if (waypointLetterList[iterator] == guessLetter)
            {
                SetGuessColor(iterator, guessLetter, GuessResult.LetterAndPositionCorrect);
                correctLetterCount++;
                iterator++;
                continue;
            }

            //Letter is correct but position is incorrect
            if(waypointLetterList.Any(x => x == guessLetter))
            {
                SetGuessColor(iterator, guessLetter, GuessResult.LetterCorrect);
                iterator++;
                continue;
            }

            SetGuessColor(iterator, guessLetter, GuessResult.LetterIncorrect);
            iterator++;
        }

        if(correctLetterCount == 5)
        {
            //TODO Win
            return;
        }

        _tryCount++;
        _currentLetterInRow = 0;
        SetNextRowColors();
    }

    private void SetGuessColor(int index, char letter, GuessResult guessResult)
    {
        var currentGrid = GetCurrentGrid(_tryCount);

        var currentLetterGrid = (Grid)currentGrid.Children[index];

        var color = guessResult switch
        {
            GuessResult.LetterIncorrect => _incorrectColor,
            GuessResult.LetterCorrect => _correctLetterColor,
            GuessResult.LetterAndPositionCorrect => _correctPositionColor
        };

        ((Rectangle)currentLetterGrid.Children[0]).Background = color;

        SetKeyboardLetterBackground(letter, color);
    }

    private Grid GetCurrentGrid(int count) =>  count switch
    {
        0 => FirstGuessGrid,
        1 => SecondGuessGrid,
        2 => ThirdGuessGrid,
        3 => FourthGuessGrid,
        4 => FifthGuessGrid,
        5 => SixthGuessGrid,
        _ => throw new ArgumentOutOfRangeException(nameof(_tryCount))
    };

    private void SetKeyboardLetterBackground(char letter, SolidColorBrush color)
    {
        var firstRowLetters = new[] { 'Q', 'W', 'E', 'R', 'T', 'Z', 'U', 'I', 'O', 'P' };

        if(firstRowLetters.Any(x => x == letter))
        {
            var firstLetterIndex = firstRowLetters.ToList().IndexOf(letter);

            var firstChildGrid = (Grid)FirstKeyRowGrid.Children[firstLetterIndex];

            var firstKeyButton = (Button)firstChildGrid.Children[0];

            firstKeyButton.Background = color;

            return;
        }

        var secondRowLetters = new[] { 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L' };

        if (secondRowLetters.Any(x => x == letter))
        {
            var secondLetterIndex = secondRowLetters.ToList().IndexOf(letter);

            var secondChildGrid = (Grid)SecondKeyRowGrid.Children[secondLetterIndex];

            var secondKeyButton = (Button)secondChildGrid.Children[0];

            secondKeyButton.Background = color;

            return;
        }

        var thirdRowLetters = new[] { 'Y', 'X', 'C', 'V', 'B', 'N', 'M'};

        var letterIndex = thirdRowLetters.ToList().IndexOf(letter);

        var childGrid = (Grid)ThirdKeyRowGrid.Children[letterIndex];

        var keyButton = (Button)childGrid.Children[0];

        keyButton.Background = color;
    }

    private void SetNextRowColors()
    {
        var currentGrid = GetCurrentGrid(_tryCount);

        var children = currentGrid.Children.Cast<Grid>();

        foreach(var child in children)
        {
            var rectangle = (Rectangle)child.Children[0];

            rectangle.Background = _incorrectColor;
        }
    }
}