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
            InitializeComponent();
            frames = selected.ToList();
            adding = add;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e) => rngLabel.Text = "RNG: " + trackBar1.Value;

        private bool commonValue(string fieldName)
        {
            FieldInfo field = getField(fieldName);
            object obj = field.GetValue(frames[0]);
            foreach (FrameInfo frame in frames)
            {
                if (field.GetValue(frame) != obj)
                    return false;
            }
            return true;
        }

        private bool commonDuration()
        {
            foreach (FrameInfo frame in frames)
            {
                if (frame.stopFrame - frame.startFrame != frames[0].stopFrame - frames[0].startFrame)
                    return false;
            }
            return true;
        }

        private void set(string fieldName, object value)
        {
            FieldInfo field = getField(fieldName);
            foreach (FrameInfo frame in frames)
                field.SetValue(frame, value);
        }

        private void Save()
        {
            set("LEFT", checkedListBox1.GetItemChecked(0));
            set("RIGHT", checkedListBox1.GetItemChecked(1));
            set("UP", checkedListBox1.GetItemChecked(2));
            set("DOWN", checkedListBox1.GetItemChecked(3));
            set("JUMP", checkedListBox1.GetItemChecked(4));
            set("SHOOT", checkedListBox1.GetItemChecked(5));
            set("GRAB", checkedListBox1.GetItemChecked(6));
            set("QUACK", checkedListBox1.GetItemChecked(7));
            set("START", checkedListBox1.GetItemChecked(8));
            set("STRAFE", checkedListBox1.GetItemChecked(9));
            set("RAGDOLL", checkedListBox1.GetItemChecked(10));
            set("SELECT", checkedListBox1.GetItemChecked(11));
            set("RNG", (byte)trackBar1.Value);
            set("RTRIGGER", (float)(trackBar3.Value / 100.0));
            set("LTRIGGER", (float)(trackBar2.Value / 100.0));
            Form1.current.clearSelection();
            if (commonDuration())
            {
                foreach (FrameInfo frame in frames)
                {
                    frame.stopFrame = frame.startFrame + (int)numericUpDown1.Value;
                    frame.selected = 1;
                }
            }
            Form1.current.ReCalculateFrames();
            Form1.current.DrawItems();
        }

        private FieldInfo getField(string name) => typeof(FrameInfo).GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        private void EditFrameForm_Load(object sender, EventArgs e)
        {
            checkedListBox1.SetItemChecked(0, frames[0].LEFT);
            checkedListBox1.SetItemChecked(1, frames[0].RIGHT);
            checkedListBox1.SetItemChecked(2, frames[0].UP);
            checkedListBox1.SetItemChecked(3, frames[0].DOWN);
            checkedListBox1.SetItemChecked(4, frames[0].JUMP);
            checkedListBox1.SetItemChecked(5, frames[0].SHOOT);
            checkedListBox1.SetItemChecked(6, frames[0].GRAB);
            checkedListBox1.SetItemChecked(7, frames[0].QUACK);
            checkedListBox1.SetItemChecked(8, frames[0].START);
            checkedListBox1.SetItemChecked(9, frames[0].STRAFE);
            checkedListBox1.SetItemChecked(10, frames[0].RAGDOLL);
            checkedListBox1.SetItemChecked(11, frames[0].SELECT);
            rngLabel.Text = "RNG: " + (int)frames[0].RNG;
            trackBar1.Value = frames[0].RNG;
            trackBar2.Value = (int)(frames[0].LTRIGGER * 100.0);
            trackBar3.Value = (int)(frames[0].RTRIGGER * 100.0);
            if (!commonDuration())
                numericUpDown1.Enabled = false;
            else
                numericUpDown1.Value = frames[0].stopFrame - frames[0].startFrame;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Save();
            Hide();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (adding)
            {
                foreach (FrameInfo frame in frames)
                    Form1.RemoveFrame(frame);
            }
            Form1.current.ReCalculateFrames();
            Form1.current.DrawItems();
            Hide();
        }

        private void EditFrameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Return)
                button1.PerformClick();
            if (e.KeyData != Keys.Escape)
                return;
            button2.PerformClick();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            checkedListBox1 = new CheckedListBox();
            button1 = new Button();
            trackBar1 = new TrackBar();
            rngLabel = new Label();
            trackBar2 = new TrackBar();
            trackBar3 = new TrackBar();
            label1 = new Label();
            label2 = new Label();
            numericUpDown1 = new NumericUpDown();
            label3 = new Label();
            button2 = new Button();
            trackBar1.BeginInit();
            trackBar2.BeginInit();
            trackBar3.BeginInit();
            numericUpDown1.BeginInit();
            SuspendLayout();
            checkedListBox1.CheckOnClick = true;
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Items.AddRange(new object[12]
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
            checkedListBox1.Location = new Point(12, 9);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new Size(120, 184);
            checkedListBox1.TabIndex = 0;
            button1.Location = new Point(138, 170);
            button1.Name = "button1";
            button1.Size = new Size(278, 23);
            button1.TabIndex = 1;
            button1.Text = "SAVE";
            button1.UseVisualStyleBackColor = true;
            button1.Click += new EventHandler(button1_Click);
            trackBar1.Location = new Point(312, 12);
            trackBar1.Maximum = byte.MaxValue;
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(104, 45);
            trackBar1.TabIndex = 2;
            trackBar1.ValueChanged += new EventHandler(trackBar1_ValueChanged);
            rngLabel.AutoSize = true;
            rngLabel.Location = new Point(344, 44);
            rngLabel.Name = "rngLabel";
            rngLabel.Size = new Size(37, 13);
            rngLabel.TabIndex = 3;
            rngLabel.Text = "RNG: ";
            trackBar2.Location = new Point(138, 103);
            trackBar2.Maximum = 100;
            trackBar2.Name = "trackBar2";
            trackBar2.Size = new Size(104, 45);
            trackBar2.TabIndex = 4;
            trackBar3.Location = new Point(312, 103);
            trackBar3.Maximum = 100;
            trackBar3.Name = "trackBar3";
            trackBar3.Size = new Size(104, 45);
            trackBar3.TabIndex = 5;
            label1.AutoSize = true;
            label1.Location = new Point(330, 84);
            label1.Name = "label1";
            label1.Size = new Size(64, 13);
            label1.TabIndex = 6;
            label1.Text = "RTRIGGER";
            label2.AutoSize = true;
            label2.Location = new Point(155, 84);
            label2.Name = "label2";
            label2.Size = new Size(62, 13);
            label2.TabIndex = 7;
            label2.Text = "LTRIGGER";
            label2.Click += new EventHandler(label2_Click);
            numericUpDown1.Location = new Point(158, 12);
            numericUpDown1.Maximum = new Decimal(new int[4]
            {
        1000,
        0,
        0,
        0
            });
            numericUpDown1.Minimum = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(68, 20);
            numericUpDown1.TabIndex = 8;
            numericUpDown1.Value = new Decimal(new int[4]
            {
        1,
        0,
        0,
        0
            });
            label3.AutoSize = true;
            label3.Location = new Point(172, 35);
            label3.Name = "label3";
            label3.Size = new Size(45, 13);
            label3.TabIndex = 9;
            label3.Text = "duration";
            button2.Location = new Point(138, 141);
            button2.Name = "button2";
            button2.Size = new Size(278, 23);
            button2.TabIndex = 10;
            button2.Text = "CANCEL";
            button2.UseVisualStyleBackColor = true;
            button2.Click += new EventHandler(button2_Click);
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(428, 205);
            ControlBox = false;
            Controls.Add(button2);
            Controls.Add(label3);
            Controls.Add(numericUpDown1);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(trackBar3);
            Controls.Add(trackBar2);
            Controls.Add(rngLabel);
            Controls.Add(trackBar1);
            Controls.Add(button1);
            Controls.Add(checkedListBox1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Name = "EditFrameForm";
            Text = "EditFrameForm";
            Load += new EventHandler(EditFrameForm_Load);
            KeyDown += new KeyEventHandler(EditFrameForm_KeyDown);
            trackBar1.EndInit();
            trackBar2.EndInit();
            trackBar3.EndInit();
            numericUpDown1.EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
