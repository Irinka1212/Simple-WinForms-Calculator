// Author: Irina Tsvetanova Hristova
// Faculty Number: F124021
// Project: Simple WinForms Calculator
// Description: Program entry point for the Calculator application.
// Notes: Minimal file that starts the Windows Forms message loop.

using System;
using System.Windows.Forms;

namespace Calculator
{
    static class Program
    {
        /// <summary>
        /// Application entry point. Initializes visual styles and runs the main form.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new CalculatorForm());
        }
    }
}
