# Quiz Game
The goal of the Quiz Game application was to create a user-friendly trivia game player.
This app was built using C# with WPF for the user interface. The application works by loading `.QUIZ` files. Once a quiz is loaded
the user can play the quiz and upon completion save a detailed report.

# Requirements
- Microsoft Visual Studio 2022
- .NET Core 3.1

# Running in Visual Studio 2022
1. Open `./PIIIProject.sln` using Microsoft Visual Studio 2022
2. Press run button

# Sample quiz files
Samples quiz files can be found in: `./Sample Quizzes`

# Quiz file format
```
[Quiz Title]
[Quiz Description]
[Question #1],[Option #1]:[true|false],[Option #2]:[true|false],[Option #3]:[true|false],[Option #4]:[true|false]
[Question #2],[Option #1]:[true|false],[Option #2]:[true|false],[Option #3]:[true|false],[Option #4]:[true|false]
```
- There must be 4 options per question.
- There is no limit on the number of questions.

# Binaries
Binaries can be found in `PIIIProject\bin\Debug\netcoreapp3.1` or `PIIIProject\bin\Release\netcoreapp3.1`.

# Authors
- Aidan Grant-Ushinsky
- Jonathan-Joseph Roy