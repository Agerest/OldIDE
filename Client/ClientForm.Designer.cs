﻿using System;

namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.CodeTextBox = new System.Windows.Forms.TextBox();
            this.IPTextBox = new System.Windows.Forms.TextBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.CompileButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.ProjectNameTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // CodeTextBox
            // 
            this.CodeTextBox.Location = new System.Drawing.Point(2, 45);
            this.CodeTextBox.Multiline = true;
            this.CodeTextBox.Name = "CodeTextBox";
            this.CodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.CodeTextBox.Size = new System.Drawing.Size(786, 393);
            this.CodeTextBox.TabIndex = 0;
            this.CodeTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyUpCodeTextBox);
            // 
            // IPTextBox
            // 
            this.IPTextBox.Location = new System.Drawing.Point(55, 10);
            this.IPTextBox.Name = "IPTextBox";
            this.IPTextBox.Size = new System.Drawing.Size(100, 20);
            this.IPTextBox.TabIndex = 1;
            this.IPTextBox.Text = "127.0.0.1";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(161, 10);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 20);
            this.ConnectButton.TabIndex = 2;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // CompileButton
            // 
            this.CompileButton.Location = new System.Drawing.Point(370, 9);
            this.CompileButton.Name = "CompileButton";
            this.CompileButton.Size = new System.Drawing.Size(75, 20);
            this.CompileButton.TabIndex = 3;
            this.CompileButton.Text = "Compile";
            this.CompileButton.UseVisualStyleBackColor = true;
            this.CompileButton.Click += new System.EventHandler(this.СompileButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(12, 13);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(37, 13);
            this.StatusLabel.TabIndex = 4;
            this.StatusLabel.Text = "Offline";
            // 
            // ProjectNameTextBox
            // 
            this.ProjectNameTextBox.Location = new System.Drawing.Point(253, 10);
            this.ProjectNameTextBox.Name = "ProjectNameTextBox";
            this.ProjectNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.ProjectNameTextBox.TabIndex = 5;
            this.ProjectNameTextBox.Text = "HelloWorld";
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ProjectNameTextBox);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.CompileButton);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.IPTextBox);
            this.Controls.Add(this.CodeTextBox);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Exit);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CodeTextBox;
        private System.Windows.Forms.TextBox IPTextBox;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button CompileButton;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.TextBox ProjectNameTextBox;
    }
}

