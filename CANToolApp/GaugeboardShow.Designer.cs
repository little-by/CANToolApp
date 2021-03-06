﻿namespace CANToolApp
{
    partial class GaugeboardShow
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
            this.components = new System.ComponentModel.Container();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.gauge1 = new CANToolApp.Gauge();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(94, 286);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "change needle types";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(124, 251);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(75, 21);
            this.textBox1.TabIndex = 17;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // gauge1
            // 
            this.gauge1.BackColor = System.Drawing.SystemColors.Control;
            this.gauge1.BaseArcColor = System.Drawing.Color.LightSlateGray;
            this.gauge1.BaseArcRadius = 80;
            this.gauge1.BaseArcStart = 135;
            this.gauge1.BaseArcSweep = 270;
            this.gauge1.BaseArcWidth = 1;
            this.gauge1.Cap_Idx = ((byte)(1));
            this.gauge1.CapColors = new System.Drawing.Color[] {
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black,
        System.Drawing.Color.Black};
            this.gauge1.CapPosition = new System.Drawing.Point(10, 10);
            this.gauge1.CapsPosition = new System.Drawing.Point[] {
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10),
        new System.Drawing.Point(10, 10)};
            this.gauge1.CapsText = new string[] {
        "",
        "",
        "",
        "",
        ""};
            this.gauge1.CapText = "";
            this.gauge1.Center = new System.Drawing.Point(100, 105);
            this.gauge1.Location = new System.Drawing.Point(62, 28);
            this.gauge1.MaxValue = 400F;
            this.gauge1.MinValue = -100F;
            this.gauge1.Name = "gauge1";
            this.gauge1.NeedleColor1 = CANToolApp.Gauge.NeedleColorEnum.Gray;
            this.gauge1.NeedleColor2 = System.Drawing.Color.DimGray;
            this.gauge1.NeedleRadius = 80;
            this.gauge1.NeedleType = 0;
            this.gauge1.NeedleWidth = 2;
            this.gauge1.Range_Idx = ((byte)(1));
            this.gauge1.RangeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.gauge1.RangeEnabled = true;
            this.gauge1.RangeEndValue = 400F;
            this.gauge1.RangeInnerRadius = 1;
            this.gauge1.RangeOuterRadius = 75;
            this.gauge1.RangesColor = new System.Drawing.Color[] {
        System.Drawing.Color.LightGreen,
        System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128))))),
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.gauge1.RangesEnabled = new bool[] {
        true,
        true,
        false,
        false,
        false};
            this.gauge1.RangesEndValue = new float[] {
        300F,
        400F,
        0F,
        0F,
        0F};
            this.gauge1.RangesInnerRadius = new int[] {
        70,
        1,
        70,
        70,
        70};
            this.gauge1.RangesOuterRadius = new int[] {
        75,
        75,
        80,
        80,
        80};
            this.gauge1.RangesStartValue = new float[] {
        -100F,
        300F,
        0F,
        0F,
        0F};
            this.gauge1.RangeStartValue = 300F;
            this.gauge1.ScaleLinesInterColor = System.Drawing.Color.Black;
            this.gauge1.ScaleLinesInterInnerRadius = 76;
            this.gauge1.ScaleLinesInterOuterRadius = 80;
            this.gauge1.ScaleLinesInterWidth = 1;
            this.gauge1.ScaleLinesMajorColor = System.Drawing.Color.Black;
            this.gauge1.ScaleLinesMajorInnerRadius = 70;
            this.gauge1.ScaleLinesMajorOuterRadius = 80;
            this.gauge1.ScaleLinesMajorStepValue = 50F;
            this.gauge1.ScaleLinesMajorWidth = 2;
            this.gauge1.ScaleLinesMinorColor = System.Drawing.Color.Gray;
            this.gauge1.ScaleLinesMinorInnerRadius = 76;
            this.gauge1.ScaleLinesMinorNumOf = 9;
            this.gauge1.ScaleLinesMinorOuterRadius = 80;
            this.gauge1.ScaleLinesMinorWidth = 1;
            this.gauge1.ScaleNumbersColor = System.Drawing.Color.Black;
            this.gauge1.ScaleNumbersFormat = null;
            this.gauge1.ScaleNumbersRadius = 95;
            this.gauge1.ScaleNumbersRotation = 0;
            this.gauge1.ScaleNumbersStartScaleLine = 0;
            this.gauge1.ScaleNumbersStepScaleLines = 1;
            this.gauge1.Size = new System.Drawing.Size(211, 217);
            this.gauge1.TabIndex = 0;
            this.gauge1.Text = "aGauge1";
            this.gauge1.Value = 50F;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 233);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "label1";
            // 
            // GaugeboardShow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 326);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gauge1);
            this.MaximumSize = new System.Drawing.Size(356, 365);
            this.MinimumSize = new System.Drawing.Size(356, 365);
            this.Name = "GaugeboardShow";
            this.Text = "Gauge Test Window";
            this.Load += new System.EventHandler(this.GaugeboardShow_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Gauge gauge1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}