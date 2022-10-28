// Decompiled with JetBrains decompiler
// Type: dgTasUI.TemplateForm
// Assembly: dgTasUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3D420AD3-C250-4777-B857-6AA7DF3EA832
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\DGTas\dgTasUI.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace dgTasUI
{
    public class TemplateForm : Form
    {
        public int LEFT = -1;
        public int RIGHT = -1;
        public int UP = -1;
        public int DOWN = -1;
        public int JUMP = -1;
        public int SHOOT = -1;
        public int GRAB = -1;
        public int QUACK = -1;
        public int RAGDOLL = -1;
        public int START = -1;
        public int STRAFE = -1;
        public int SELECT = -1;
        public List<FrameInfo> frames;
        private IContainer components = null;
        private Button button1;
        private Button button2;

        private void button2_Click(object sender, EventArgs e) => this.Hide();

        private void button1_Click(object sender, EventArgs e)
        {
            this.Save();
            this.Hide();
        }

        public void load(string file)
        {
            byte[] source = File.ReadAllBytes(file);
            if (source.Length % 21 != 0)
            {
                int num = (int)MessageBox.Show("Invalid file", "dont open that pls", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                List<FrameInfo> list = Form1.splitList(source.ToList(), 21).Select(x => FrameInfo.Load(x.ToArray())).ToList();
                for (int index = 0; index < list.Count() - 1; ++index)
                {
                    if (list[index].Equals(list[index + 1]))
                    {
                        ++list[index].stopFrame;
                        list.RemoveAt(index + 1);
                        --index;
                    }
                }
                foreach (FrameInfo frameInfo in list)
                    ++frameInfo.stopFrame;
                this.frames = list;
            }
        }

        public void Save()
        {
            IEnumerable<FieldInfo> fieldInfos1 = typeof(TemplateForm).GetFields().Where(x => x.FieldType == typeof(int));
            IEnumerable<FieldInfo> fieldInfos2 = typeof(FrameInfo).GetFields().Where(x => x.FieldType == typeof(bool));
            foreach (FieldInfo fieldInfo1 in fieldInfos1)
            {
                foreach (FieldInfo fieldInfo2 in fieldInfos2)
                {
                    if (fieldInfo2.Name == fieldInfo1.Name)
                    {
                        int num = (int)fieldInfo1.GetValue(this);
                        if (num >= 0)
                        {
                            using (List<FrameInfo>.Enumerator enumerator = this.frames.GetEnumerator())
                            {
                                while (enumerator.MoveNext())
                                {
                                    FrameInfo current = enumerator.Current;
                                    fieldInfo2.SetValue(current, num == 0);
                                }
                                break;
                            }
                        }
                        else
                            break;
                    }
                }
            }
            Form1.frames.AddRange(frames);
            Form1.current.ReCalculateFrames();
            Form1.current.DrawItems();
        }

        public TemplateForm(string fileName)
        {
            this.InitializeComponent();
            this.load(fileName);
        }

        public GroupBox generateGroupBox(string text, int y)
        {
            GroupBox groupBox = new GroupBox();
            RadioButton radioButton1 = new RadioButton();
            RadioButton radioButton2 = new RadioButton();
            RadioButton radioButton3 = new RadioButton();
            groupBox.SuspendLayout();
            this.SuspendLayout();
            groupBox.Controls.Add(radioButton3);
            groupBox.Controls.Add(radioButton2);
            groupBox.Controls.Add(radioButton1);
            groupBox.Font = new Font("Microsoft Sans Serif", 8.25f);
            groupBox.Location = new Point(1, y);
            groupBox.Name = "groupBox1";
            groupBox.Size = new Size(230, 44);
            groupBox.TabIndex = 0;
            groupBox.TabStop = false;
            groupBox.Text = text;
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(7, 17);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(86, 17);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "KEEP SAME";
            radioButton1.Checked = true;
            radioButton1.UseVisualStyleBackColor = true;
            radioButton1.CheckedChanged += new EventHandler(this.checkchanged);
            radioButton2.AutoSize = true;
            radioButton2.Location = new Point(98, 17);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new Size(55, 17);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "TRUE";
            radioButton2.UseVisualStyleBackColor = true;
            radioButton2.CheckedChanged += new EventHandler(this.checkchanged);
            radioButton3.AutoSize = true;
            radioButton3.Location = new Point(159, 17);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new Size(58, 17);
            radioButton3.TabIndex = 2;
            radioButton3.TabStop = true;
            radioButton3.Text = "FALSE";
            radioButton3.UseVisualStyleBackColor = true;
            radioButton3.CheckedChanged += new EventHandler(this.checkchanged);
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            return groupBox;
        }

        private void checkchanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            if (!radioButton.Checked)
                return;
            int num = radioButton.TabIndex - 1;
            typeof(TemplateForm).GetField(radioButton.Parent.Text).SetValue(this, num);
        }

        private void TemplateForm_Load(object sender, EventArgs e)
        {
            List<string> stringList = new List<string>();
            foreach (FieldInfo fieldInfo in typeof(TemplateForm).GetFields().Where(x => x.FieldType == typeof(int)))
                stringList.Add(fieldInfo.Name);
            int index = 0;
            for (int y = 13; y < stringList.Count * 45 + 13; y += 45)
            {
                this.Controls.Add(this.generateGroupBox(stringList[index], y));
                ++index;
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
            this.button1 = new Button();
            this.button2 = new Button();
            this.SuspendLayout();
            this.button1.Location = new Point(12, 588);
            this.button1.Name = "button1";
            this.button1.Size = new Size(208, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "IMPORT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Location = new Point(12, 559);
            this.button2.Name = "button2";
            this.button2.Size = new Size(208, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "CANCEL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(232, 623);
            this.ControlBox = false;
            this.Controls.Add(button2);
            this.Controls.Add(button1);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Name = "TemplateForm";
            this.Text = "Import Template";
            this.Load += new EventHandler(this.TemplateForm_Load);
            this.ResumeLayout(false);
        }
    }
}
