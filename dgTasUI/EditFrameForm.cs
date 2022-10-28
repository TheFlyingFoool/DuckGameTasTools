using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace dgTasUI
{
    public class EditFrameForm : Form
    {
        private List<FrameInfo> frames = new List<FrameInfo>();
        private bool adding = false;
        private IContainer components = null;
        private CheckedListBox checkedListBox1;
        private Button button1;
        private TrackBar trackBar1;
        private Label rngLabel;
        private TrackBar trackBar2;
        private TrackBar trackBar3;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown numericUpDown1;
        private Button button2;

        public EditFrameForm(FrameInfo[] selected, bool add)
        {
            this.InitializeComponent();
            this.frames = selected.ToList();
            this.adding = add;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e) => this.rngLabel.Text = "RNG: " + trackBar1.Value;

        private bool commonValue(string fieldName)
        {
            FieldInfo field = this.getField(fieldName);
            object obj = field.GetValue(this.frames[0]);
            foreach (FrameInfo frame in this.frames)
            {
                if (field.GetValue(frame) != obj)
                    return false;
            }
            return true;
        }

        private bool commonDuration()
        {
            foreach (FrameInfo frame in this.frames)
            {
                if (frame.stopFrame - frame.startFrame != this.frames[0].stopFrame - this.frames[0].startFrame)
                    return false;
            }
            return true;
        }

        private void set(string fieldName, object value)
        {
            FieldInfo field = this.getField(fieldName);
            foreach (FrameInfo frame in this.frames)
                field.SetValue(frame, value);
        }

        private void Save()
        {
            this.set("LEFT", this.checkedListBox1.GetItemChecked(0));
            this.set("RIGHT", this.checkedListBox1.GetItemChecked(1));
            this.set("UP", this.checkedListBox1.GetItemChecked(2));
            this.set("DOWN", this.checkedListBox1.GetItemChecked(3));
            this.set("JUMP", this.checkedListBox1.GetItemChecked(4));
            this.set("SHOOT", this.checkedListBox1.GetItemChecked(5));
            this.set("GRAB", this.checkedListBox1.GetItemChecked(6));
            this.set("QUACK", this.checkedListBox1.GetItemChecked(7));
            this.set("START", this.checkedListBox1.GetItemChecked(8));
            this.set("STRAFE", this.checkedListBox1.GetItemChecked(9));
            this.set("RAGDOLL", this.checkedListBox1.GetItemChecked(10));
            this.set("SELECT", this.checkedListBox1.GetItemChecked(11));
            this.set("RNG", (byte)this.trackBar1.Value);
            this.set("RTRIGGER", (float)(trackBar3.Value / 100.0));
            this.set("LTRIGGER", (float)(trackBar2.Value / 100.0));
            Form1.current.clearSelection();
            if (this.commonDuration())
            {
                foreach (FrameInfo frame in this.frames)
                {
                    frame.stopFrame = frame.startFrame + (int)this.numericUpDown1.Value;
                    frame.selected = 1;
                }
            }
            Form1.current.ReCalculateFrames();
            Form1.current.DrawItems();
        }

        private FieldInfo getField(string name) => typeof(FrameInfo).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private void EditFrameForm_Load(object sender, EventArgs e)
        {
            this.checkedListBox1.SetItemChecked(0, this.frames[0].LEFT);
            this.checkedListBox1.SetItemChecked(1, this.frames[0].RIGHT);
            this.checkedListBox1.SetItemChecked(2, this.frames[0].UP);
            this.checkedListBox1.SetItemChecked(3, this.frames[0].DOWN);
            this.checkedListBox1.SetItemChecked(4, this.frames[0].JUMP);
            this.checkedListBox1.SetItemChecked(5, this.frames[0].SHOOT);
            this.checkedListBox1.SetItemChecked(6, this.frames[0].GRAB);
            this.checkedListBox1.SetItemChecked(7, this.frames[0].QUACK);
            this.checkedListBox1.SetItemChecked(8, this.frames[0].START);
            this.checkedListBox1.SetItemChecked(9, this.frames[0].STRAFE);
            this.checkedListBox1.SetItemChecked(10, this.frames[0].RAGDOLL);
            this.checkedListBox1.SetItemChecked(11, this.frames[0].SELECT);
            this.rngLabel.Text = "RNG: " + (int)this.frames[0].RNG;
            this.trackBar1.Value = frames[0].RNG;
            this.trackBar2.Value = (int)(frames[0].LTRIGGER * 100.0);
            this.trackBar3.Value = (int)(frames[0].RTRIGGER * 100.0);
            if (!this.commonDuration())
                this.numericUpDown1.Enabled = false;
            else
                this.numericUpDown1.Value = this.frames[0].stopFrame - this.frames[0].startFrame;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Save();
            this.Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (this.adding)
            {
                foreach (FrameInfo frame in this.frames)
                    Form1.RemoveFrame(frame);
            }
            Form1.current.ReCalculateFrames();
            Form1.current.DrawItems();
            this.Hide();
        }

        private void EditFrameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
                this.button1.PerformClick();
            if (e.KeyData != Keys.Escape)
                return;
            this.button2.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.checkedListBox1 = new CheckedListBox();
            this.button1 = new Button();
            this.trackBar1 = new TrackBar();
            this.rngLabel = new Label();
            this.trackBar2 = new TrackBar();
            this.trackBar3 = new TrackBar();
            this.label1 = new Label();
            this.label2 = new Label();
            this.numericUpDown1 = new NumericUpDown();
            this.label3 = new Label();
            this.button2 = new Button();
            this.trackBar1.BeginInit();
            this.trackBar2.BeginInit();
            this.trackBar3.BeginInit();
            this.numericUpDown1.BeginInit();
            this.SuspendLayout();
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[12]
            {
         "LEFT",
         "RIGHT",
         "UP",
         "DOWN",
         "JUMP",
         "SHOOT",
         "GRAB",
         "QUACK",
         "START",
         "STRAFE",
         "RAGDOLL",
         "SELECT"
            });
            this.checkedListBox1.Location = new Point(12, 9);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new Size(120, 184);
            this.checkedListBox1.TabIndex = 0;
            this.button1.Location = new Point(138, 170);
            this.button1.Name = "button1";
            this.button1.Size = new Size(278, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "SAVE";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.trackBar1.Location = new Point(312, 12);
            this.trackBar1.Maximum = byte.MaxValue;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new Size(104, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.ValueChanged += new EventHandler(this.trackBar1_ValueChanged);
            this.rngLabel.AutoSize = true;
            this.rngLabel.Location = new Point(344, 44);
            this.rngLabel.Name = "rngLabel";
            this.rngLabel.Size = new Size(37, 13);
            this.rngLabel.TabIndex = 3;
            this.rngLabel.Text = "RNG: ";
            this.trackBar2.Location = new Point(138, 103);
            this.trackBar2.Maximum = 100;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new Size(104, 45);
            this.trackBar2.TabIndex = 4;
            this.trackBar3.Location = new Point(312, 103);
            this.trackBar3.Maximum = 100;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new Size(104, 45);
            this.trackBar3.TabIndex = 5;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(330, 84);
            this.label1.Name = "label1";
            this.label1.Size = new Size(64, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "RTRIGGER";
            this.label2.AutoSize = true;
            this.label2.Location = new Point(155, 84);
            this.label2.Name = "label2";
            this.label2.Size = new Size(62, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "LTRIGGER";
            this.label2.Click += new EventHandler(this.label2_Click);
            this.numericUpDown1.Location = new Point(158, 12);
            this.numericUpDown1.Maximum = new Decimal(new int[4]
            {
        1000,
        0,
        0,
        0
            });
            this.numericUpDown1.Minimum = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new Size(68, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            this.label3.AutoSize = true;
            this.label3.Location = new Point(172, 35);
            this.label3.Name = "label3";
            this.label3.Size = new Size(45, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "duration";
            this.button2.Location = new Point(138, 141);
            this.button2.Name = "button2";
            this.button2.Size = new Size(278, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(428, 205);
            this.ControlBox = false;
            this.Controls.Add(button2);
            this.Controls.Add(label3);
            this.Controls.Add(numericUpDown1);
            this.Controls.Add(label2);
            this.Controls.Add(label1);
            this.Controls.Add(trackBar3);
            this.Controls.Add(trackBar2);
            this.Controls.Add(rngLabel);
            this.Controls.Add(trackBar1);
            this.Controls.Add(button1);
            this.Controls.Add(checkedListBox1);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "EditFrameForm";
            this.Text = "EditFrameForm";
            this.Load += new EventHandler(this.EditFrameForm_Load);
            this.KeyDown += new KeyEventHandler(this.EditFrameForm_KeyDown);
            this.trackBar1.EndInit();
            this.trackBar2.EndInit();
            this.trackBar3.EndInit();
            this.numericUpDown1.EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
