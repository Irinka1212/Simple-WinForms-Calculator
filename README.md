# Simple WinForms Calculator

**Author:** Irina Tsvetanova Hristova  
**Faculty Number:** F124021

# Recordings with explanation
https://drive.google.com/drive/folders/1I9R6uLlfQJxt1E2FrNdMbRZsc6GSf3vh?usp=drive_link
- Video 1: Demo of the calculator (its functionalities, buttons, and error handling).
-  Video 2: Explanation of the code (project structure, classes, logic, and key OOP principles).

## Project description
This is a simple Windows Forms calculator demonstrating basic object-oriented programming concepts from the first part of the course:
- Separation of UI and logic (CalculatorForm vs CalculatorLogic)
- Encapsulation and single responsibility
- Event-driven programming
- Error handling

Features:
- Decimal numbers ('.')
- Negative numbers
- Percent (%) button (converts current number to number/100)
- Running total behavior (immediate execution / chaining operations)
- 15-digit input limit per number to mitigate precision issues
- Error display: `NaN` and `NaN: Div By Zero`

## Files
- `Program.cs` — program entry point
- `CalculatorForm.cs` — form UI and event handlers (heavily commented)
- `CalculatorLogic.cs` — parsing and evaluation logic (heavily commented)
- `CalculatorForm.Designer.cs` — minimal designer partial class
