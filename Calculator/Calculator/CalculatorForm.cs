// Author: Irina Tsvetanova Hristova
// Faculty Number: F124021
// Project: Simple WinForms Calculator
// Description: Windows Forms UI for the calculator. The form creates its UI
// elements at runtime (TextBox + Buttons) and handles user interaction.
// The form uses CalculatorLogic for expression evaluation to demonstrate
// separation of UI and business logic.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calculator
{
    /// <summary>
    /// Main calculator form. Builds the UI dynamically and handles events.
    /// Key behavior:
    /// - Running total / immediate execution (pressing operator evaluates previous operator)
    /// - Supports negative numbers and decimals
    /// - Percent (%) converts the current displayed number to num/100
    /// - Limits number entry to MaxDigits to avoid precision issues
    /// - Displays "NaN" or "NaN: Div By Zero" on errors
    /// </summary>
    public partial class CalculatorForm : Form
    {
        private const int buttonSize = 60;   // Size of square buttons
        private const int spacing = 10;      // Spacing between controls
        private const int maxDigits = 15;    // Maximum digits per number entry

        private TextBox TextDisplay;          // Display textbox (read-only)
        private double runningTotal = 0;     // Running total used for chaining operations
        private string lastOperator = "";    // Last operator clicked by the user
        private bool isNewNumber = true;     // True when next digit should start a new number

        /// <summary>
        /// Default constructor: initialize form components and UI controls.
        /// </summary>
        public CalculatorForm()
        {
            InitializeComponent();
            InitializeUI();
        }

        /// <summary>
        /// Builds the UI: display textbox and a grid of buttons (digits, operators).
        /// Buttons are created dynamically to keep Designer code minimal and
        /// demonstrate UI creation in code.
        /// </summary>
        private void InitializeUI()
        {
            this.Text = "C# Calculator";
            this.ClientSize = new Size(4 * buttonSize + 5 * spacing, 6 * buttonSize + 7 *spacing);
            this.BackColor = Color.FromArgb(45, 45, 48);
            this.Font = new Font("Segoe UI", 12F, FontStyle.Regular);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Display TextBox (readonly). Font size set large for readability.
            TextDisplay = new TextBox
            {
                Location = new Point(spacing, spacing),
                Size = new Size(this.ClientSize.Width - 2 * spacing, buttonSize - 10),
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                TextAlign = HorizontalAlignment.Right,
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle,
                ReadOnly = true,
                TabIndex = 0,
                Text = "0"
            };
            this.Controls.Add(TextDisplay);

            // Button labels (C, %, /, *, digits, -, +, =, 0, dot)
            string[] buttonTexts = {
                "C", "%", "/", "*",
                "7", "8", "9", "-",
                "4", "5", "6", "+",
                "1", "2", "3", "=",
                "0", "."
            };

            // Create buttons in a 4-column grid
            for (int i = 0; i < buttonTexts.Length; i++)
            {
                Button btn = new Button
                {
                    Text = buttonTexts[i],
                    Size = new Size(buttonSize, buttonSize),
                    BackColor = Color.FromArgb(75, 75, 75),
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };
                btn.FlatAppearance.BorderSize = 0;

                int row = i / 4;
                int col = i % 4;
                btn.Location = new Point(
                    spacing + col * (buttonSize + spacing),
                    TextDisplay.Bottom + spacing + row * (buttonSize + spacing)
                );

                // Hook events: equals, clear, and general input
                if (btn.Text == "=")
                    btn.Click += ClickEqualsButton;
                else if (btn.Text == "C")
                    btn.Click += ClickClearButton;
                else
                    btn.Click += ClickInputButton;

                this.Controls.Add(btn);
            }
        }

        /// <summary>
        /// Generic input handler for numeric buttons, operator buttons ("+-*/"),
        /// percent ("%") and decimal point.
        /// This method implements the input rules:
        /// - If starting a new number, numeric input replaces the display.
        /// - Operator input triggers HandleOperator (which updates running total).
        /// - Percent applies to current number (divides by 100).
        /// - Decimal point allowed only once per current number.
        /// - Digit entry limited by MaxDigits per current number.
        /// </summary>
        private void ClickInputButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string input = button.Text;
            bool isOperator = "+-*/".Contains(input);

            // If a new number should start (after pressing operator or equals),
            // only clear the display when next input is a digit or dot or percent.
            if (isNewNumber)
            {
                if (!isOperator && input != "%")
                    TextDisplay.Text = "";
                isNewNumber = false;
            }

            if (isOperator)
                HandleOperator(input);
            else if (input == "%")
                HandlePercent();
            else
            {
                // Numeric or decimal input path
                string currentNumber = GetCurrentNumber(TextDisplay.Text);

                // Enforce digit limit (only count digits, not sign or decimal point)
                if (char.IsDigit(input[0]) && currentNumber.Length >= maxDigits)
                    return;

                // Prevent multiple decimals in the same number
                if (input == "." && currentNumber.Contains("."))
                    return;

                // Append new char to display
                TextDisplay.Text += input;
            }
        }

        /// <summary>
        /// Handles operator clicks (+ - * /).
        /// Implements running-total chaining:
        /// - If there is a previously stored operator, evaluate runningTotal (lastOperator) currentNumber.
        /// - Otherwise set runningTotal = currentNumber.
        /// - Store the clicked operator as lastOperator and mark next input as new number.
        /// Error handling: catches divide-by-zero or parse errors and displays "NaN".
        /// </summary>
        /// <param name="op">Operator string like "+", "-", "*", "/".</param>
        private void HandleOperator(string op)
        {
            double currentNumber;
            if (!double.TryParse(TextDisplay.Text, out currentNumber))
            {
                TextDisplay.Text = "NaN";
                isNewNumber = true;
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(lastOperator))
                {
                    // Evaluate runningTotal <lastOperator> currentNumber using CalculatorLogic
                    runningTotal = CalculatorLogic.EvaluateExpression(runningTotal.ToString() + lastOperator + currentNumber);
                    TextDisplay.Text = runningTotal.ToString();
                }
                else
                {
                    runningTotal = currentNumber;
                }

                lastOperator = op;
                isNewNumber = true;
            }
            catch (DivideByZeroException)
            {
                TextDisplay.Text = "NaN: Div By Zero";
                lastOperator = "";
                isNewNumber = true;
            }
            catch
            {
                TextDisplay.Text = "NaN";
                lastOperator = "";
                isNewNumber = true;
            }
        }

        /// <summary>
        /// Percent handler: converts the currently displayed number to a percentage
        /// by dividing it by 100. Example: "50" -> "%" -> "0.5".
        /// Sets isNewNumber to true so next numeric entry starts fresh.
        /// </summary>
        private void HandlePercent()
        {
            double currentNumber;
            if (!double.TryParse(TextDisplay.Text, out currentNumber))
            {
                TextDisplay.Text = "NaN";
                isNewNumber = true;
                return;
            }

            currentNumber /= 100.0;
            TextDisplay.Text = currentNumber.ToString();
            isNewNumber = true;
        }

        /// <summary>
        /// Equals button: finalizes the current operation by evaluating
        /// runningTotal (lastOperator) currentNumber if a lastOperator exists.
        /// Displays "NaN" messages on failures.
        /// </summary>
        private void ClickEqualsButton(object sender, EventArgs e)
        {
            double currentNumber;
            if (!double.TryParse(TextDisplay.Text, out currentNumber))
            {
                TextDisplay.Text = "NaN";
                isNewNumber = true;
                return;
            }

            if (!string.IsNullOrEmpty(lastOperator))
            {
                try
                {
                    runningTotal = CalculatorLogic.EvaluateExpression(runningTotal.ToString() + lastOperator + currentNumber);
                    TextDisplay.Text = runningTotal.ToString();
                }
                catch (DivideByZeroException)
                {
                    TextDisplay.Text = "NaN: Div By Zero";
                }
                catch
                {
                    TextDisplay.Text = "NaN";
                }
                finally
                {
                    lastOperator = "";
                    isNewNumber = true;
                }
            }
        }

        /// <summary>
        /// Clear button: resets display and internal state.
        /// </summary>
        private void ClickClearButton(object sender, EventArgs e)
        {
            TextDisplay.Text = "0";
            runningTotal = 0;
            lastOperator = "";
            isNewNumber = true;
        }

        /// <summary>
        /// Returns the substring representing the number currently being entered.
        /// This method scans backwards from the end of the display text until it
        /// finds an operator character; everything after that is considered the
        /// current number (including a leading '-' sign if present).
        /// </summary>
        /// <param name="text">Full display text.</param>
        /// <returns>Current number string.</returns>
        private string GetCurrentNumber(string text)
        {
            int i = text.Length - 1;
            while (i >= 0 && !"+-*/".Contains(text[i].ToString()))
                --i;
            return text.Substring(i + 1);
        }
    }
}
