// Decompiled with JetBrains decompiler
// Type: dgTasUI.Form1
// Assembly: dgTasUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3D420AD3-C250-4777-B857-6AA7DF3EA832
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\DGTas\dgTasUI.exe

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace dgTasUI
{
    public class Form1 : Form
    {
        private static string savePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\DuckGame\\TAS\\debug.dgtas";
        private static string directoryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\DuckGame\\TAS\\";
        public static string startUpFile = "";
        private bool redrawing = false;
        public static List<FrameInfo> frames = new List<FrameInfo>();
        private List<FrameInfo> copied = new List<FrameInfo>();
        public static Form1 current;
        private IContainer components = null;
        private ColumnHeader leftHeader;
        private ColumnHeader frameHeader;
        private ColumnHeader rightHeader;
        private ColumnHeader upHeader;
        private ColumnHeader donwHeader;
        private ColumnHeader jumpHeader;
        private ColumnHeader shootHeader;
        private ColumnHeader grabHeader;
        private ColumnHeader quackHeader;
        private ColumnHeader startHeader;
        private ColumnHeader ragdollHeader;
        private ColumnHeader selectHeader;
        private ColumnHeader rngHeader;
        private ColumnHeader lTriggerHeader;
        private ColumnHeader rTriggerHeader;
        private ColumnHeader strafeHeader;
        public ListView itemList;
        private ToolStrip toolStrip1;
        private ToolStripButton SaveButton;
        private ToolStripButton OpenButton;
        private ToolStripButton MoveUpButton;
        private ToolStripButton MoveDownButton;
        private ToolStripButton EditButton;
        private ToolStripButton AddButton;
        private ToolStripButton InsertAfterButton;
        private ToolStripButton InsertBeforeButton;
        private ToolStripButton CopyButton;
        private ToolStripButton PasteButton;
        private ToolStripButton RemoveButton;
        private OpenFileDialog ofDialogue;
        private ToolStripButton saveAsButton;
        private SaveFileDialog saveAsDialogue;
        private ToolStripButton openTemplateButton;
        private OpenFileDialog openTemplateDialogue;

        public Form1()
        {
            Form1.current = this;
            this.InitializeComponent();
            this.ReCalculateFrames();
            this.DrawItems();
        }

        public static void RemoveFrame(FrameInfo f)
        {
            for (int index = 0; index < Form1.frames.Count; ++index)
            {
                if (Form1.frames[index].startFrame == f.startFrame)
                {
                    Form1.frames.RemoveAt(index);
                    break;
                }
            }
        }

        public void Save(string path)
        {
            if (Form1.frames.Count == 0)
                return;
            List<byte> byteList = new List<byte>();
            foreach (FrameInfo frame in Form1.frames)
                byteList.AddRange(frame.write());
            File.WriteAllBytes(path, byteList.ToArray());
        }

        public FrameInfo getFromItem(ListViewItem item)
        {
            foreach (FrameInfo frame in Form1.frames)
            {
                if (frame.frameString == item.Text)
                    return frame;
            }
            return null;
        }

        public void ReCalculateFrames()
        {
            int num1 = 1;
            foreach (FrameInfo frame in Form1.frames)
            {
                int num2 = frame.stopFrame - frame.startFrame;
                frame.startFrame = num1 - 1;
                frame.stopFrame = num1 + num2 - 1;
                num1 += num2;
            }
        }

        public IEnumerable<FrameInfo> getSelected()
        {
            foreach (object selected in this.itemList.SelectedItems)
                yield return this.getFromItem((ListViewItem)selected);
        }

        public void showEditor(FrameInfo[] frames = null)
        {
            this.ReCalculateFrames();
            new EditFrameForm(frames == null ? this.getSelected().ToArray() : frames, frames != null).Show();
        }

        public void DrawItems()
        {
            this.redrawing = true;
            this.itemList.Items.Clear();
            int num = 0;
            foreach (FrameInfo frame in Form1.frames)
            {
                ++num;
                ListViewItem listViewItem = frame.getItem(num % 2 == 0);
                this.itemList.Items.Add(listViewItem);
                if (frame.selected > 0)
                {
                    listViewItem.Selected = true;
                    listViewItem.EnsureVisible();
                }
            }
            this.redrawing = false;
        }

        public void clearSelection()
        {
            foreach (FrameInfo frame in Form1.frames)
                frame.selected = 0;
        }

        public int getIndex(ListViewItem item)
        {
            for (int index = 0; index < Form1.frames.Count; ++index)
            {
                if (Form1.frames[index].frameString == item.Text)
                    return index;
            }
            return -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.itemList.SelectedItems.Count <= 0)
                return;
            this.showEditor();
        }

        private void SaveButton_Click(object sender, EventArgs e) => this.Save(Form1.savePath);

        private void OpenButton_Click(object sender, EventArgs e)
        {
            int num = (int)this.ofDialogue.ShowDialog();
        }

        public void Open(string filepath, bool add = false)
        {
            byte[] source = File.ReadAllBytes(filepath);
            if (source.Length % 21 != 0)
            {
                int num = (int)MessageBox.Show("Invalid file", "dont open that pls", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                List<FrameInfo> list = Form1.splitList<byte>(source.ToList(), 21).Select(x => FrameInfo.Load(x.ToArray())).ToList();
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
                if (add)
                    Form1.frames.AddRange(list);
                else
                    Form1.frames = list;
                this.ReCalculateFrames();
                this.DrawItems();
            }
        }

        public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
        }

        private void MoveUpButton_Click(object sender, EventArgs e) => this.move(-1);

        public void move(int by)
        {
            foreach (ListViewItem selectedItem in this.itemList.SelectedItems)
            {
                int index = this.getIndex(selectedItem);
                if (index > -1 - by && index < Form1.frames.Count - by)
                {
                    FrameInfo frame = Form1.frames[index];
                    Form1.frames.RemoveAt(index);
                    Form1.frames.Insert(index + by, frame);
                }
            }
            this.ReCalculateFrames();
            this.DrawItems();
        }

        private FrameInfo getNew() => new FrameInfo()
        {
            startFrame = 0,
            stopFrame = 1
        };

        private void MoveDownButton_Click(object sender, EventArgs e) => this.move(1);

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in this.itemList.SelectedItems)
            {
                int index = this.getIndex(selectedItem);
                Form1.frames.RemoveAt(index);
            }
            this.ReCalculateFrames();
            this.DrawItems();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            FrameInfo frameInfo = this.getNew();
            Form1.frames.Add(frameInfo);
            this.showEditor(new FrameInfo[1] { frameInfo });
        }

        public FrameInfo[] Insert(int by)
        {
            List<FrameInfo> frameInfoList = new List<FrameInfo>();
            foreach (ListViewItem selectedItem in this.itemList.SelectedItems)
            {
                int index = this.getIndex(selectedItem) + by;
                FrameInfo frameInfo = this.getNew();
                frameInfoList.Add(frameInfo);
                if (index >= Form1.frames.Count)
                    Form1.frames.Add(frameInfo);
                else if (index < 0)
                    Form1.frames.Insert(0, frameInfo);
                else
                    Form1.frames.Insert(index, frameInfo);
            }
            return frameInfoList.ToArray();
        }

        private void InsertAfterButton_Click(object sender, EventArgs e) => this.showEditor(this.Insert(1));

        public void Paste()
        {
            int num = this.itemList.SelectedItems.Count <= 0 ? Form1.frames.Count : this.getIndex(this.itemList.SelectedItems[0]);
            for (int index = 0; index < this.copied.Count; ++index)
            {
                FrameInfo frameInfo = new FrameInfo(this.copied[index]);
                if (num + index >= Form1.frames.Count)
                    Form1.frames.Add(frameInfo);
                else
                    Form1.frames.Insert(num + index, frameInfo);
            }
            this.ReCalculateFrames();
            this.DrawItems();
        }

        public void Copy()
        {
            if (this.itemList.SelectedItems.Count <= 0)
                return;
            this.copied.Clear();
            foreach (ListViewItem selectedItem in this.itemList.SelectedItems)
                this.copied.Add(this.getFromItem(selectedItem));
        }

        private void InsertBeforeButton_Click(object sender, EventArgs e) => this.showEditor(this.Insert(0));

        private void CopyButton_Click(object sender, EventArgs e) => this.Copy();

        private void PasteButton_Click(object sender, EventArgs e) => this.Paste();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Insert:
                    this.InsertAfterButton.PerformClick();
                    break;
                case Keys.Delete:
                    this.RemoveButton.PerformClick();
                    break;
            }
            if (e.Control && !e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        this.MoveUpButton.PerformClick();
                        break;
                    case Keys.Down:
                        this.MoveDownButton.PerformClick();
                        break;
                    case Keys.A:
                        foreach (FrameInfo frame in Form1.frames)
                            frame.selected = 1;
                        this.DrawItems();
                        break;
                    case Keys.C:
                        this.CopyButton.PerformClick();
                        break;
                    case Keys.E:
                        this.EditButton.PerformClick();
                        break;
                    case Keys.O:
                        this.OpenButton.PerformClick();
                        break;
                    case Keys.S:
                        this.SaveButton.PerformClick();
                        break;
                    case Keys.V:
                        this.PasteButton.PerformClick();
                        break;
                }
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
                this.saveAsButton.PerformClick();
            e.Handled = true;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void itemList_ItemSelectionChanged(
          object sender,
          ListViewItemSelectionChangedEventArgs e)
        {
            if (this.redrawing)
                return;
            foreach (FrameInfo frame in Form1.frames)
                frame.selected = 0;
            try
            {
                foreach (ListViewItem selectedItem in this.itemList.SelectedItems)
                    this.getFromItem(selectedItem).selected = 1;
            }
            catch
            {
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Environment.GetCommandLineArgs().Length > 1)
                Form1.startUpFile = Environment.GetCommandLineArgs()[1];
            if (Form1.startUpFile != "")
            {
                Form1.savePath = Form1.startUpFile;
                this.Text = "DG TAS editor: " + Path.GetFileName(Form1.savePath);
                File.WriteAllText("args.txt", Form1.startUpFile);
                this.Open(Form1.savePath);
            }
            if (!File.Exists(Form1.savePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Form1.savePath));
                File.Create(Form1.savePath);
            }
            this.ofDialogue.InitialDirectory = Form1.directoryPath;
            this.saveAsDialogue.InitialDirectory = Form1.directoryPath;
            this.openTemplateDialogue.InitialDirectory = Form1.directoryPath;
        }

        private void ofDialogue_FileOk(object sender, CancelEventArgs e)
        {
            this.Open(Path.GetFullPath(this.ofDialogue.FileName));
            Form1.savePath = this.ofDialogue.FileName;
            this.Text = "DG TAS editor: " + Path.GetFileName(Form1.savePath);
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            int num = (int)this.saveAsDialogue.ShowDialog();
        }

        private void saveAsDialogue_FileOk(object sender, CancelEventArgs e)
        {
            this.Save(this.saveAsDialogue.FileName);
            Form1.savePath = this.saveAsDialogue.FileName;
            this.Text = "DG TAS editor: " + Path.GetFileName(Form1.savePath);
        }

        private void itemList_MouseDoubleClick(object sender, MouseEventArgs e) => this.EditButton.PerformClick();

        private void openTemplateButton_Click(object sender, EventArgs e)
        {
            int num = (int)this.openTemplateDialogue.ShowDialog();
        }

        private void openTemplateDialogue_FileOk(object sender, CancelEventArgs e) => new TemplateForm(this.openTemplateDialogue.FileName).Show();

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.components != null)
                this.components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            this.itemList = new ListView();
            this.frameHeader = new ColumnHeader();
            this.leftHeader = new ColumnHeader();
            this.rightHeader = new ColumnHeader();
            this.upHeader = new ColumnHeader();
            this.donwHeader = new ColumnHeader();
            this.jumpHeader = new ColumnHeader();
            this.shootHeader = new ColumnHeader();
            this.grabHeader = new ColumnHeader();
            this.quackHeader = new ColumnHeader();
            this.startHeader = new ColumnHeader();
            this.strafeHeader = new ColumnHeader();
            this.ragdollHeader = new ColumnHeader();
            this.selectHeader = new ColumnHeader();
            this.rngHeader = new ColumnHeader();
            this.lTriggerHeader = new ColumnHeader();
            this.rTriggerHeader = new ColumnHeader();
            this.toolStrip1 = new ToolStrip();
            this.SaveButton = new ToolStripButton();
            this.MoveUpButton = new ToolStripButton();
            this.MoveDownButton = new ToolStripButton();
            this.EditButton = new ToolStripButton();
            this.RemoveButton = new ToolStripButton();
            this.AddButton = new ToolStripButton();
            this.InsertAfterButton = new ToolStripButton();
            this.InsertBeforeButton = new ToolStripButton();
            this.openTemplateButton = new ToolStripButton();
            this.CopyButton = new ToolStripButton();
            this.PasteButton = new ToolStripButton();
            this.saveAsButton = new ToolStripButton();
            this.OpenButton = new ToolStripButton();
            this.ofDialogue = new OpenFileDialog();
            this.saveAsDialogue = new SaveFileDialog();
            this.openTemplateDialogue = new OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            this.itemList.Alignment = ListViewAlignment.Left;
            this.itemList.AutoArrange = false;
            this.itemList.Columns.AddRange(new ColumnHeader[16]
            {
                this.frameHeader,
                this.leftHeader,
                this.rightHeader,
                this.upHeader,
                this.donwHeader,
                this.jumpHeader,
                this.shootHeader,
                this.grabHeader,
                this.quackHeader,
                this.startHeader,
                this.strafeHeader,
                this.ragdollHeader,
                this.selectHeader,
                this.rngHeader,
                this.lTriggerHeader,
                this.rTriggerHeader
            });
            this.itemList.Dock = DockStyle.Fill;
            this.itemList.FullRowSelect = true;
            this.itemList.Location = new Point(0, 25);
            this.itemList.Name = "itemList";
            this.itemList.Size = new Size(1006, 347);
            this.itemList.TabIndex = 0;
            this.itemList.UseCompatibleStateImageBehavior = false;
            this.itemList.View = View.Details;
            this.itemList.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.itemList_ItemSelectionChanged);
            this.itemList.MouseDoubleClick += new MouseEventHandler(this.itemList_MouseDoubleClick);
            this.frameHeader.Text = "FRAMES";
            this.frameHeader.Width = 81;
            this.leftHeader.Tag = "sdf";
            this.leftHeader.Text = "LEFT";
            this.leftHeader.TextAlign = HorizontalAlignment.Center;
            this.rightHeader.Text = "RIGHT";
            this.rightHeader.TextAlign = HorizontalAlignment.Center;
            this.upHeader.Text = "UP";
            this.upHeader.TextAlign = HorizontalAlignment.Center;
            this.donwHeader.Text = "DOWN";
            this.donwHeader.TextAlign = HorizontalAlignment.Center;
            this.jumpHeader.Text = "JUMP";
            this.jumpHeader.TextAlign = HorizontalAlignment.Center;
            this.shootHeader.Text = "SHOOT";
            this.shootHeader.TextAlign = HorizontalAlignment.Center;
            this.grabHeader.Text = "GRAB";
            this.grabHeader.TextAlign = HorizontalAlignment.Center;
            this.quackHeader.Text = "QUACK";
            this.quackHeader.TextAlign = HorizontalAlignment.Center;
            this.startHeader.Text = "START";
            this.startHeader.TextAlign = HorizontalAlignment.Center;
            this.strafeHeader.Text = "STRAFE";
            this.strafeHeader.TextAlign = HorizontalAlignment.Center;
            this.ragdollHeader.Text = "RAGDOLL";
            this.ragdollHeader.TextAlign = HorizontalAlignment.Center;
            this.selectHeader.Text = "SELECT";
            this.selectHeader.TextAlign = HorizontalAlignment.Center;
            this.rngHeader.Text = "RNG";
            this.rngHeader.TextAlign = HorizontalAlignment.Center;
            this.lTriggerHeader.Text = "LTRIGGER";
            this.lTriggerHeader.TextAlign = HorizontalAlignment.Center;
            this.rTriggerHeader.Text = "RTRIGGER";
            this.rTriggerHeader.TextAlign = HorizontalAlignment.Center;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Items.AddRange(new ToolStripItem[13]
            {
                SaveButton,
                MoveUpButton,
                MoveDownButton,
                EditButton,
                RemoveButton,
                AddButton,
                InsertAfterButton,
                InsertBeforeButton,
                openTemplateButton,
                CopyButton,
                PasteButton,
                saveAsButton,
                OpenButton
            });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(1006, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "twww";
            this.SaveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.SaveButton.Image = (Image)resources.GetObject("SaveButton.Image");
            this.SaveButton.ImageTransparentColor = Color.Magenta;
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new Size(59, 22);
            this.SaveButton.Text = "Fast Save";
            this.SaveButton.ToolTipText = "Ctrl+S";
            this.SaveButton.Click += new EventHandler(this.SaveButton_Click);
            this.MoveUpButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.MoveUpButton.Image = (Image)resources.GetObject("MoveUpButton.Image");
            this.MoveUpButton.ImageTransparentColor = Color.Magenta;
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new Size(59, 22);
            this.MoveUpButton.Text = "Move Up";
            this.MoveUpButton.ToolTipText = "Ctrl+UP";
            this.MoveUpButton.Click += new EventHandler(this.MoveUpButton_Click);
            this.MoveDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.MoveDownButton.Image = (Image)resources.GetObject("MoveDownButton.Image");
            this.MoveDownButton.ImageTransparentColor = Color.Magenta;
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new Size(75, 22);
            this.MoveDownButton.Text = "Move Down";
            this.MoveDownButton.ToolTipText = "Ctrl+DOWN";
            this.MoveDownButton.Click += new EventHandler(this.MoveDownButton_Click);
            this.EditButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.EditButton.Image = (Image)resources.GetObject("EditButton.Image");
            this.EditButton.ImageTransparentColor = Color.Magenta;
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new Size(31, 22);
            this.EditButton.Text = "Edit";
            this.EditButton.ToolTipText = "Ctrl+E or double click";
            this.EditButton.Click += new EventHandler(this.button1_Click);
            this.RemoveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.RemoveButton.Image = (Image)resources.GetObject("RemoveButton.Image");
            this.RemoveButton.ImageTransparentColor = Color.Magenta;
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new Size(54, 22);
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.ToolTipText = "Delete";
            this.RemoveButton.Click += new EventHandler(this.RemoveButton_Click);
            this.AddButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.AddButton.Image = (Image)resources.GetObject("AddButton.Image");
            this.AddButton.ImageTransparentColor = Color.Magenta;
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new Size(60, 22);
            this.AddButton.Text = "Add New";
            this.AddButton.ToolTipText = "nothin";
            this.AddButton.Click += new EventHandler(this.AddButton_Click);
            this.InsertAfterButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.InsertAfterButton.Image = (Image)resources.GetObject("InsertAfterButton.Image");
            this.InsertAfterButton.ImageTransparentColor = Color.Magenta;
            this.InsertAfterButton.Name = "InsertAfterButton";
            this.InsertAfterButton.Size = new Size(69, 22);
            this.InsertAfterButton.Text = "Insert After";
            this.InsertAfterButton.ToolTipText = "Insert";
            this.InsertAfterButton.Click += new EventHandler(this.InsertAfterButton_Click);
            this.InsertBeforeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.InsertBeforeButton.Image = (Image)resources.GetObject("InsertBeforeButton.Image");
            this.InsertBeforeButton.ImageTransparentColor = Color.Magenta;
            this.InsertBeforeButton.Name = "InsertBeforeButton";
            this.InsertBeforeButton.Size = new Size(77, 22);
            this.InsertBeforeButton.Text = "Insert Before";
            this.InsertBeforeButton.ToolTipText = "nothin";
            this.InsertBeforeButton.Click += new EventHandler(this.InsertBeforeButton_Click);
            this.openTemplateButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.openTemplateButton.Image = (Image)resources.GetObject("openTemplateButton.Image");
            this.openTemplateButton.ImageTransparentColor = Color.Magenta;
            this.openTemplateButton.Name = "openTemplateButton";
            this.openTemplateButton.Size = new Size(92, 22);
            this.openTemplateButton.Text = "Insert Template";
            this.openTemplateButton.ToolTipText = "Adds an existing tas file to the bottom";
            this.openTemplateButton.Click += new EventHandler(this.openTemplateButton_Click);
            this.CopyButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.CopyButton.Image = (Image)resources.GetObject("CopyButton.Image");
            this.CopyButton.ImageTransparentColor = Color.Magenta;
            this.CopyButton.Name = "CopyButton";
            this.CopyButton.Size = new Size(39, 22);
            this.CopyButton.Text = "Copy";
            this.CopyButton.ToolTipText = "Ctrl+C";
            this.CopyButton.Click += new EventHandler(this.CopyButton_Click);
            this.PasteButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.PasteButton.Image = (Image)resources.GetObject("PasteButton.Image");
            this.PasteButton.ImageTransparentColor = Color.Magenta;
            this.PasteButton.Name = "PasteButton";
            this.PasteButton.Size = new Size(39, 22);
            this.PasteButton.Text = "Paste";
            this.PasteButton.ToolTipText = "Ctrl+V";
            this.PasteButton.Click += new EventHandler(this.PasteButton_Click);
            this.saveAsButton.Alignment = ToolStripItemAlignment.Right;
            this.saveAsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.saveAsButton.Image = (Image)resources.GetObject("saveAsButton.Image");
            this.saveAsButton.ImageTransparentColor = Color.Magenta;
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new Size(51, 22);
            this.saveAsButton.Text = "Save As";
            this.saveAsButton.ToolTipText = "Ctrl+Shift+S";
            this.saveAsButton.Click += new EventHandler(this.saveAsButton_Click);
            this.OpenButton.Alignment = ToolStripItemAlignment.Right;
            this.OpenButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.OpenButton.Image = (Image)resources.GetObject("OpenButton.Image");
            this.OpenButton.ImageTransparentColor = Color.Magenta;
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new Size(40, 22);
            this.OpenButton.Text = "Open";
            this.OpenButton.ToolTipText = "Ctrl+O";
            this.OpenButton.Click += new EventHandler(this.OpenButton_Click);
            this.ofDialogue.Filter = "tas file|*.dgtas";
            this.ofDialogue.FileOk += new CancelEventHandler(this.ofDialogue_FileOk);
            this.saveAsDialogue.FileName = "mytas.dgtas";
            this.saveAsDialogue.Filter = "tas file|*.dgtas";
            this.saveAsDialogue.FileOk += new CancelEventHandler(this.saveAsDialogue_FileOk);
            this.openTemplateDialogue.FileName = "myfile.dgtas";
            this.openTemplateDialogue.Filter = "tas file|*.dgtas";
            this.openTemplateDialogue.FileOk += new CancelEventHandler(this.openTemplateDialogue_FileOk);
            this.AutoScaleDimensions = new SizeF(6f, 13f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1006, 372);
            this.Controls.Add(itemList);
            this.Controls.Add(toolStrip1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.KeyPreview = true;
            this.MaximumSize = new Size(1022, 4360);
            this.MinimumSize = new Size(1022, 39);
            this.Name = "Form1";
            this.Text = "DG Tas editor";
            this.Load += new EventHandler(this.Form1_Load);
            this.KeyDown += new KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new KeyPressEventHandler(this.Form1_KeyPress);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
