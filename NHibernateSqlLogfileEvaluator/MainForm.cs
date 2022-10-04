// Decompiled with JetBrains decompiler
// Type: NHibernateSqlLogfileEvaluator.App.MainForm
// Assembly: NHibernateSqlLogfileEvaluator.App, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AF64C7C9-B58A-48DF-861C-15C0CC87C274
// Assembly location: C:\Users\gevater\OneDrive - adesso Group\Desktop\SQL Parameter Replacer\NHibernateSqlLogfileEvaluator.App.exe

using AstrumIT.Argos.NHibernateSqlLogfileEvaluator;
using PoorMansTSqlFormatterLib;
using PoorMansTSqlFormatterLib.Formatters;
using PoorMansTSqlFormatterLib.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace NHibernateSqlLogfileEvaluator.App
{
    public class MainForm : Form
    {
        private readonly SqlFormattingManager formattingManager;
        private IContainer components;
        private Label label1;
        private Label label2;
        private RichTextBox StatementInputTextBox;
        private RichTextBox ParsedStatementOutputTextBox;
        private CheckBox ParseImmediatelyCheckBox;
        private CheckBox ParseOtherCheckBox;
        private Button ParseButton;

        public MainForm()
        {
            this.InitializeComponent();
            this.formattingManager = MainForm.CreateFormattingManager();
            this.CheckClipboard();
        }

        private void CheckClipboard()
        {
            if (!((IEnumerable<string>)new string[6]
            {
        "select",
        "update",
        "alter",
        "create",
        "insert",
        "delete"
            }).Contains<string>(Clipboard.GetText().ToLower().Split(' ')[0]))
                return;
            this.StatementInputTextBox.Text = Clipboard.GetText();
            this.TryToParse();
        }

        private static SqlFormattingManager CreateFormattingManager() => new SqlFormattingManager((ISqlTreeFormatter)new TSqlStandardFormatter()
        {
            ExpandBooleanExpressions = true,
            TrailingCommas = true,
            ExpandCommaLists = true,
            BreakJoinOnSections = true,
            UppercaseKeywords = true
        });

        private void StatementInputTextBoxTextChanged(object sender, EventArgs e)
       {
            if (!this.ParseImmediatelyCheckBox.Checked)
                return;
            this.TryToParse();
        }

        private void ImmediatelyParsingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.ParseButton.Enabled = !this.ParseImmediatelyCheckBox.Checked;
            if (!this.ParseImmediatelyCheckBox.Checked)
                return;
            this.TryToParse();
        }

        private void ParseButton_Click(object sender, EventArgs e) => this.TryToParse();

        private void TryToParse()
        {
            try
            {
                if (StatementInputTextBox.Text.Contains("',N'"))
                {
                    this.ParsedStatementOutputTextBox.Text = this.formattingManager.Format(StatemenGeneratorSqlProfiler.GenerateStatement(this.StatementInputTextBox.Text).Executable);
                }
                else
                {
                    this.ParsedStatementOutputTextBox.Text = this.formattingManager.Format(StatementGenerator.GenerateStatement(this.StatementInputTextBox.Text).Executable);
                }
                
                this.ParsedStatementOutputTextBox.Focus();
                this.ParsedStatementOutputTextBox.SelectAll();
            }
            catch (Exception ex)
            {
                this.ParsedStatementOutputTextBox.Text = this.StatementInputTextBox.Text;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.label1 = new Label();
            this.label2 = new Label();
            this.StatementInputTextBox = new RichTextBox();
            this.ParsedStatementOutputTextBox = new RichTextBox();
            this.ParseImmediatelyCheckBox = new CheckBox();
            ParseOtherCheckBox = new CheckBox();
            this.ParseButton = new Button();
            this.SuspendLayout();
            this.label1.AutoSize = true;
            this.label1.Location = new Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(150, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Statement with parameters:";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(12, 188);
            this.label2.Name = "label2";
            this.label2.Size = new Size(64, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Statement:";
            this.StatementInputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.StatementInputTextBox.Location = new Point(12, 28);
            this.StatementInputTextBox.Name = "StatementInputTextBox";
            this.StatementInputTextBox.Size = new Size(822, 154);
            this.StatementInputTextBox.TabIndex = 4;
            this.StatementInputTextBox.Text = "";
            this.StatementInputTextBox.TextChanged += new EventHandler(this.StatementInputTextBoxTextChanged);
            this.ParsedStatementOutputTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.ParsedStatementOutputTextBox.Location = new Point(12, 210);
            this.ParsedStatementOutputTextBox.Name = "ParsedStatementOutputTextBox";
            this.ParsedStatementOutputTextBox.Size = new Size(822, 162);
            this.ParsedStatementOutputTextBox.TabIndex = 5;
            this.ParsedStatementOutputTextBox.Text = "";
            this.ParseImmediatelyCheckBox.AutoSize = true;
            this.ParseImmediatelyCheckBox.Checked = true;
            this.ParseImmediatelyCheckBox.CheckState = CheckState.Checked;
            this.ParseImmediatelyCheckBox.Location = new Point(82, 187);
            this.ParseImmediatelyCheckBox.Name = "ParseImmediatelyCheckBox";
            this.ParseImmediatelyCheckBox.Size = new Size(126, 20);
            this.ParseImmediatelyCheckBox.TabIndex = 6;
            this.ParseImmediatelyCheckBox.Text = "Parse Immediately";
            this.ParseImmediatelyCheckBox.UseVisualStyleBackColor = true;
            this.ParseImmediatelyCheckBox.CheckedChanged += new EventHandler(this.ImmediatelyParsingCheckBox_CheckedChanged);
            this.ParseButton.Enabled = false;
            this.ParseButton.Location = new Point(214, 185);
            this.ParseButton.Name = "ParseButton";
            this.ParseButton.Size = new Size(75, 23);
            this.ParseButton.TabIndex = 7;
            this.ParseButton.Text = "Parse";
            this.ParseButton.UseVisualStyleBackColor = true;
            this.ParseButton.Click += new EventHandler(this.ParseButton_Click);
            this.AutoScaleDimensions = new SizeF(6f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(842, 384);
            this.Controls.Add((Control)this.ParseButton);
            this.Controls.Add((Control)this.ParseImmediatelyCheckBox);
            this.Controls.Add((Control)this.ParsedStatementOutputTextBox);
            this.Controls.Add((Control)this.StatementInputTextBox);
            this.Controls.Add((Control)this.label2);
            this.Controls.Add((Control)this.label1);
            this.Font = new Font("Trebuchet MS", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.Margin = new Padding(3, 4, 3, 4);
            this.Name = nameof(MainForm);
            this.Text = "SQL-Parameter Replacer";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
