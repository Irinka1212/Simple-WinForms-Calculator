// Author: Irina Tsvetanova Hristova
// Faculty Number: F124021
// Project: Simple WinForms Calculator
// Description: Minimal designer partial class. The UI is created in code
// in CalculatorForm.InitializeUI() to keep the Designer file minimal and
// avoid storing layout in the .resx for this educational example.

namespace Calculator
{
    partial class CalculatorForm
    {
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Dispose pattern for the form.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Minimal InitializeComponent method. The heavy UI creation happens
        /// in InitializeUI() inside CalculatorForm.cs so we keep designer minimal.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "CalculatorForm";
        }
    }
}
