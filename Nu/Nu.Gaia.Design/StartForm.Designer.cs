﻿namespace Nu.Gaia.Design
{
    partial class StartForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.binaryFilePathText = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.selectExecutable = new System.Windows.Forms.Button();
            this.customButton = new System.Windows.Forms.Button();
            this.defaultButton = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.useImperativeExecutionCheckBox = new System.Windows.Forms.CheckBox();
            this.modeComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // binaryFilePathText
            // 
            this.binaryFilePathText.Location = new System.Drawing.Point(14, 45);
            this.binaryFilePathText.Margin = new System.Windows.Forms.Padding(2);
            this.binaryFilePathText.Name = "binaryFilePathText";
            this.binaryFilePathText.Size = new System.Drawing.Size(411, 20);
            this.binaryFilePathText.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 18);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select game\'s executable for editing.";
            // 
            // selectExecutable
            // 
            this.selectExecutable.Location = new System.Drawing.Point(429, 45);
            this.selectExecutable.Margin = new System.Windows.Forms.Padding(2);
            this.selectExecutable.Name = "selectExecutable";
            this.selectExecutable.Size = new System.Drawing.Size(24, 20);
            this.selectExecutable.TabIndex = 2;
            this.selectExecutable.Text = "...";
            this.selectExecutable.UseVisualStyleBackColor = true;
            this.selectExecutable.Click += new System.EventHandler(this.button1_Click);
            // 
            // customButton
            // 
            this.customButton.Location = new System.Drawing.Point(262, 141);
            this.customButton.Name = "customButton";
            this.customButton.Size = new System.Drawing.Size(96, 23);
            this.customButton.TabIndex = 6;
            this.customButton.Text = "Use Settings";
            this.customButton.UseVisualStyleBackColor = true;
            this.customButton.Click += new System.EventHandler(this.customButton_Click);
            // 
            // defaultButton
            // 
            this.defaultButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.defaultButton.Location = new System.Drawing.Point(364, 141);
            this.defaultButton.Name = "defaultButton";
            this.defaultButton.Size = new System.Drawing.Size(91, 23);
            this.defaultButton.TabIndex = 7;
            this.defaultButton.Text = "Use Defaults";
            this.defaultButton.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "EXE files|*.exe|All files|*.*";
            // 
            // useImperativeExecutionCheckBox
            // 
            this.useImperativeExecutionCheckBox.AutoSize = true;
            this.useImperativeExecutionCheckBox.Location = new System.Drawing.Point(14, 109);
            this.useImperativeExecutionCheckBox.Name = "useImperativeExecutionCheckBox";
            this.useImperativeExecutionCheckBox.Size = new System.Drawing.Size(284, 17);
            this.useImperativeExecutionCheckBox.TabIndex = 5;
            this.useImperativeExecutionCheckBox.Text = "Use Imperative Execution (faster, but no Undo / Redo)";
            this.useImperativeExecutionCheckBox.UseVisualStyleBackColor = true;
            // 
            // modeComboBox
            // 
            this.modeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeComboBox.Enabled = false;
            this.modeComboBox.FormattingEnabled = true;
            this.modeComboBox.Location = new System.Drawing.Point(56, 77);
            this.modeComboBox.Name = "modeComboBox";
            this.modeComboBox.Size = new System.Drawing.Size(399, 21);
            this.modeComboBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mode:";
            // 
            // StartForm
            // 
            this.AcceptButton = this.customButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.defaultButton;
            this.ClientSize = new System.Drawing.Size(467, 179);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.modeComboBox);
            this.Controls.Add(this.useImperativeExecutionCheckBox);
            this.Controls.Add(this.defaultButton);
            this.Controls.Add(this.customButton);
            this.Controls.Add(this.selectExecutable);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.binaryFilePathText);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StartForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Editor Start Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectExecutable;
        public System.Windows.Forms.Button customButton;
        public System.Windows.Forms.Button defaultButton;
        public System.Windows.Forms.TextBox binaryFilePathText;
        public System.Windows.Forms.OpenFileDialog openFileDialog;
        public System.Windows.Forms.CheckBox useImperativeExecutionCheckBox;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.ComboBox modeComboBox;
	}
}