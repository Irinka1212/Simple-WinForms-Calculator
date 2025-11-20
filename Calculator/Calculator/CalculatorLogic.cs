// Author: Irina Tsvetanova Hristova
// Faculty Number: F124021
// Project: Simple WinForms Calculator
// Description: Core calculator logic. Parses simple expressions and evaluates
// them left-to-right. Supports negative numbers and decimals.
// The logic is separated from the UI to demonstrate separation of concerns

using System;
using System.Collections.Generic;

namespace Calculator
{
    /// <summary>
    /// Static class that contains parsing and evaluation routines for simple
    /// arithmetic expressions used by the CalculatorForm.
    /// 
    /// Design notes:
    /// - Separation of concerns: UI code (CalculatorForm) does not perform parsing/evaluation.
    /// - The parser implements a simple tokenization that supports negative numbers
    ///   (leading '-' or '-' immediately after an operator) and decimal numbers.
    /// - Evaluation is strictly left-to-right (immediate execution behavior), which
    ///   matches the running-total behaviour of many basic calculators.
    /// - Operators supported: + - * /
    /// </summary>
    public static class CalculatorLogic
    {
        /// <summary>
        /// Evaluates a simple arithmetic expression represented as a string.
        /// The expression should contain numbers and operators only.
        /// Example accepted expressions: "5+3", "-2*10", "12.5/2"
        /// 
        /// The evaluation strategy:
        /// 1. Tokenize the expression into number/operator tokens.
        /// 2. Evaluate tokens left-to-right applying each operator to the running result.
        /// 
        /// The method throws exceptions for invalid formats or division by zero.
        /// </summary>
        /// <param name="expression">Expression string to evaluate.</param>
        /// <returns>Double result of the evaluated expression.</returns>
        public static double EvaluateExpression(string expression)
        {
            if (expression == null) throw new ArgumentNullException("expr");
            expression = expression.Replace(" ", "");
            if (string.IsNullOrEmpty(expression))
                return 0;

            // Tokenize (numbers and operators)
            List<string> tokens = ParseExpressionToTokens(expression);
            if (tokens.Count == 0)
                return 0;

            // The first token must be a number
            double result;
            if (!double.TryParse(tokens[0], out result))
                throw new FormatException("Expression must start with a number.");

            // Evaluate left-to-right: token sequence is number, operator, number, operator, ...
            for (int i = 1; i < tokens.Count; i += 2)
            {
                string op = tokens[i];

                if (i + 1 >= tokens.Count)
                    throw new FormatException("Expression ends with an operator.");

                double nextNum;
                if (!double.TryParse(tokens[i + 1], out nextNum))
                    throw new FormatException("Expected a number after operator.");

                switch (op)
                {
                    case "+":
                        result += nextNum;
                        break;
                    case "-":
                        result -= nextNum;
                        break;
                    case "*":
                        result *= nextNum;
                        break;
                    case "/":
                        if (nextNum == 0.0)
                            throw new DivideByZeroException("Division by zero.");
                        result /= nextNum;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown operator: " + op);
                }
            }

            return result;
        }

        /// <summary>
        /// Parses the expression into tokens. A token is either:
        /// - a number (with optional leading '-' for negative numbers and optional decimal point)
        /// - an operator: + - * /
        /// 
        /// Rules:
        /// - A '-' is considered part of a number if it occurs at the start of the expression
        ///   or immediately after another operator.
        /// - Decimal point is allowed (one per number).
        /// </summary>
        /// <param name="expression">Expression string without whitespace.</param>
        /// <returns>List of tokens in order.</returns>
        private static List<string> ParseExpressionToTokens(string expression)
        {
            var tokens = new List<string>();
            int i = 0;
            while (i < expression.Length)
            {
                bool isNegative = false;
                // Determine if a '-' should be treated as a negative sign for the number.
                if (expression[i] == '-' && (i == 0 || "+-*/".Contains(expression[i - 1].ToString())))
                {
                    isNegative = true;
                    i++;
                }

                // Parse the number (digits and optional single decimal point)
                int start = i;
                bool seenDecimal = false;
                while (i < expression.Length && (char.IsDigit(expression[i]) || expression[i] == '.'))
                {
                    if (expression[i] == '.')
                    {
                        if (seenDecimal)
                            throw new FormatException("Invalid number format: multiple decimal points.");
                        seenDecimal = true;
                    }
                    i++;
                }

                if (start != i)
                {
                    string number = expression.Substring(start, i - start);
                    if (isNegative)
                        number = "-" + number;
                    tokens.Add(number);
                }

                // If an operator char remains at position i, add it as token.
                if (i < expression.Length)
                {
                    char c = expression[i];
                    // Only + - * / are valid operators here; '-' used as operator when it wasn't consumed as a sign.
                    if ("+-*/".Contains(c.ToString()))
                    {
                        tokens.Add(c.ToString());
                        i++;
                    }
                    else
                    {
                        // Any other character is invalid
                        throw new FormatException("Invalid character in expression: " + c);
                    }
                }
            }

            return tokens;
        }
    }
}
