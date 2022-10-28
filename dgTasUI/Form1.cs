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
            InitializeComponent();
            ReCalculateFrames();
            DrawItems();
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
            foreach (object selected in itemList.SelectedItems)
                yield return getFromItem((ListViewItem)selected);
        }

        public void showEditor(FrameInfo[] frames = null)
        {
            ReCalculateFrames();
            new EditFrameForm(frames == null ? getSelected().ToArray() : frames, frames != null).Show();
        }

        public void DrawItems()
        {
            redrawing = true;
            itemList.Items.Clear();
            int num = 0;
            foreach (FrameInfo frame in Form1.frames)
            {
                ++num;
                ListViewItem listViewItem = frame.getItem(num % 2 == 0);
                itemList.Items.Add(listViewItem);
                if (frame.selected > 0)
                {
                    listViewItem.Selected = true;
                    listViewItem.EnsureVisible();
                }
            }
            redrawing = false;
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
            if (itemList.SelectedItems.Count <= 0)
                return;
            showEditor();
        }

        private void SaveButton_Click(object sender, EventArgs e) => Save(Form1.savePath);

        private void OpenButton_Click(object sender, EventArgs e)
        {
            int num = (int)ofDialogue.ShowDialog();
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
                ReCalculateFrames();
                DrawItems();
            }
        }

        public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
        }

        private void MoveUpButton_Click(object sender, EventArgs e) => move(-1);

        public void move(int by)
        {
            foreach (ListViewItem selectedItem in itemList.SelectedItems)
            {
                int index = getIndex(selectedItem);
                if (index > -1 - by && index < Form1.frames.Count - by)
                {
                    FrameInfo frame = Form1.frames[index];
                    Form1.frames.RemoveAt(index);
                    Form1.frames.Insert(index + by, frame);
                }
            }
            ReCalculateFrames();
            DrawItems();
        }

        private FrameInfo getNew() => new FrameInfo()
        {
            startFrame = 0,
            stopFrame = 1
        };

        private void MoveDownButton_Click(object sender, EventArgs e) => move(1);

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem selectedItem in itemList.SelectedItems)
            {
                int index = getIndex(selectedItem);
                Form1.frames.RemoveAt(index);
            }
            ReCalculateFrames();
            DrawItems();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            FrameInfo frameInfo = getNew();
            Form1.frames.Add(frameInfo);
            showEditor(new FrameInfo[1] { frameInfo });
        }

        public FrameInfo[] Insert(int by)
        {
            List<FrameInfo> frameInfoList = new List<FrameInfo>();
            foreach (ListViewItem selectedItem in itemList.SelectedItems)
            {
                int index = getIndex(selectedItem) + by;
                FrameInfo frameInfo = getNew();
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

        private void InsertAfterButton_Click(object sender, EventArgs e) => showEditor(Insert(1));

        public void Paste()
        {
            int num = itemList.SelectedItems.Count <= 0 ? Form1.frames.Count : getIndex(itemList.SelectedItems[0]);
            for (int index = 0; index < copied.Count; ++index)
            {
                FrameInfo frameInfo = new FrameInfo(copied[index]);
                if (num + index >= Form1.frames.Count)
                    Form1.frames.Add(frameInfo);
                else
                    Form1.frames.Insert(num + index, frameInfo);
            }
            ReCalculateFrames();
            DrawItems();
        }

        public void Copy()
        {
            if (itemList.SelectedItems.Count <= 0)
                return;
            copied.Clear();
            foreach (ListViewItem selectedItem in itemList.SelectedItems)
                copied.Add(getFromItem(selectedItem));
        }

        private void InsertBeforeButton_Click(object sender, EventArgs e) => showEditor(Insert(0));

        private void CopyButton_Click(object sender, EventArgs e) => Copy();

        private void PasteButton_Click(object sender, EventArgs e) => Paste();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Insert:
                    InsertAfterButton.PerformClick();
                    break;
                case Keys.Delete:
                    RemoveButton.PerformClick();
                    break;
            }
            if (e.Control && !e.Shift)
            {
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        MoveUpButton.PerformClick();
                        break;
                    case Keys.Down:
                        MoveDownButton.PerformClick();
                        break;
                    case Keys.A:
                        foreach (FrameInfo frame in Form1.frames)
                            frame.selected = 1;
                        DrawItems();
                        break;
                    case Keys.C:
                        CopyButton.PerformClick();
                        break;
                    case Keys.E:
                        EditButton.PerformClick();
                        break;
                    case Keys.O:
                        OpenButton.PerformClick();
                        break;
                    case Keys.S:
                        SaveButton.PerformClick();
                        break;
                    case Keys.V:
                        PasteButton.PerformClick();
                        break;
                }
            }
            else if (e.Control && e.Shift && e.KeyCode == Keys.S)
                saveAsButton.PerformClick();
            e.Handled = true;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void itemList_ItemSelectionChanged(
          object sender,
          ListViewItemSelectionChangedEventArgs e)
        {
            if (redrawing)
                return;
            foreach (FrameInfo frame in Form1.frames)
                frame.selected = 0;
            try
            {
                foreach (ListViewItem selectedItem in itemList.SelectedItems)
                    getFromItem(selectedItem).selected = 1;
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
                Text = "DG TAS editor: " + Path.GetFileName(Form1.savePath);
                File.WriteAllText("args.txt", Form1.startUpFile);
                Open(Form1.savePath);
            }
            if (!File.Exists(Form1.savePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Form1.savePath));
                File.Create(Form1.savePath);
            }
            ofDialogue.InitialDirectory = Form1.directoryPath;
            saveAsDialogue.InitialDirectory = Form1.directoryPath;
            openTemplateDialogue.InitialDirectory = Form1.directoryPath;
        }

        private void ofDialogue_FileOk(object sender, CancelEventArgs e)
        {
            Open(Path.GetFullPath(ofDialogue.FileName));
            Form1.savePath = ofDialogue.FileName;
            Text = "DG TAS editor: " + Path.GetFileName(Form1.savePath);
        }

        private void saveAsButton_Click(object sender, EventArgs e)
        {
            int num = (int)saveAsDialogue.ShowDialog();
        }

        private void saveAsDialogue_FileOk(object sender, CancelEventArgs e)
        {
            Save(saveAsDialogue.FileName);
            Form1.savePath = saveAsDialogue.FileName;
            Text = "DG TAS editor: " + Path.GetFileName(Form1.savePath);
        }

        private void itemList_MouseDoubleClick(object sender, MouseEventArgs e) => EditButton.PerformClick();

        private void openTemplateButton_Click(object sender, EventArgs e)
        {
            int num = (int)openTemplateDialogue.ShowDialog();
        }

        private void openTemplateDialogue_FileOk(object sender, CancelEventArgs e) => new TemplateForm(openTemplateDialogue.FileName).Show();

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
            itemList = new ListView();
            frameHeader = new ColumnHeader();
            leftHeader = new ColumnHeader();
            rightHeader = new ColumnHeader();
            upHeader = new ColumnHeader();
            donwHeader = new ColumnHeader();
            jumpHeader = new ColumnHeader();
            shootHeader = new ColumnHeader();
            grabHeader = new ColumnHeader();
            quackHeader = new ColumnHeader();
            startHeader = new ColumnHeader();
            strafeHeader = new ColumnHeader();
            ragdollHeader = new ColumnHeader();
            selectHeader = new ColumnHeader();
            rngHeader = new ColumnHeader();
            lTriggerHeader = new ColumnHeader();
            rTriggerHeader = new ColumnHeader();
            toolStrip1 = new ToolStrip();
            SaveButton = new ToolStripButton();
            MoveUpButton = new ToolStripButton();
            MoveDownButton = new ToolStripButton();
            EditButton = new ToolStripButton();
            RemoveButton = new ToolStripButton();
            AddButton = new ToolStripButton();
            InsertAfterButton = new ToolStripButton();
            InsertBeforeButton = new ToolStripButton();
            openTemplateButton = new ToolStripButton();
            CopyButton = new ToolStripButton();
            PasteButton = new ToolStripButton();
            saveAsButton = new ToolStripButton();
            OpenButton = new ToolStripButton();
            ofDialogue = new OpenFileDialog();
            saveAsDialogue = new SaveFileDialog();
            openTemplateDialogue = new OpenFileDialog();
            toolStrip1.SuspendLayout();
            SuspendLayout();
            itemList.Alignment = ListViewAlignment.Left;
            itemList.AutoArrange = false;
            itemList.Columns.AddRange(new ColumnHeader[16]
            {
                frameHeader,
                leftHeader,
                rightHeader,
                upHeader,
                donwHeader,
                jumpHeader,
                shootHeader,
                grabHeader,
                quackHeader,
                startHeader,
                strafeHeader,
                ragdollHeader,
                selectHeader,
                rngHeader,
                lTriggerHeader,
                rTriggerHeader
            });
            itemList.Dock = DockStyle.Fill;
            itemList.FullRowSelect = true;
            itemList.Location = new Point(0, 25);
            itemList.Name = "itemList";
            itemList.Size = new Size(1006, 347);
            itemList.TabIndex = 0;
            itemList.UseCompatibleStateImageBehavior = false;
            itemList.View = View.Details;
            itemList.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(itemList_ItemSelectionChanged);
            itemList.MouseDoubleClick += new MouseEventHandler(itemList_MouseDoubleClick);
            frameHeader.Text = "FRAMES";
            frameHeader.Width = 81;
            leftHeader.Tag = "sdf";
            leftHeader.Text = "LEFT";
            leftHeader.TextAlign = HorizontalAlignment.Center;
            rightHeader.Text = "RIGHT";
            rightHeader.TextAlign = HorizontalAlignment.Center;
            upHeader.Text = "UP";
            upHeader.TextAlign = HorizontalAlignment.Center;
            donwHeader.Text = "DOWN";
            donwHeader.TextAlign = HorizontalAlignment.Center;
            jumpHeader.Text = "JUMP";
            jumpHeader.TextAlign = HorizontalAlignment.Center;
            shootHeader.Text = "SHOOT";
            shootHeader.TextAlign = HorizontalAlignment.Center;
            grabHeader.Text = "GRAB";
            grabHeader.TextAlign = HorizontalAlignment.Center;
            quackHeader.Text = "QUACK";
            quackHeader.TextAlign = HorizontalAlignment.Center;
            startHeader.Text = "START";
            startHeader.TextAlign = HorizontalAlignment.Center;
            strafeHeader.Text = "STRAFE";
            strafeHeader.TextAlign = HorizontalAlignment.Center;
            ragdollHeader.Text = "RAGDOLL";
            ragdollHeader.TextAlign = HorizontalAlignment.Center;
            selectHeader.Text = "SELECT";
            selectHeader.TextAlign = HorizontalAlignment.Center;
            rngHeader.Text = "RNG";
            rngHeader.TextAlign = HorizontalAlignment.Center;
            lTriggerHeader.Text = "LTRIGGER";
            lTriggerHeader.TextAlign = HorizontalAlignment.Center;
            rTriggerHeader.Text = "RTRIGGER";
            rTriggerHeader.TextAlign = HorizontalAlignment.Center;
            toolStrip1.CanOverflow = false;
            toolStrip1.Items.AddRange(new ToolStripItem[13]
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
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1006, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "twww";
            SaveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            SaveButton.Image = (Image)resources.GetObject("SaveButton.Image");
            SaveButton.ImageTransparentColor = Color.Magenta;
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(59, 22);
            SaveButton.Text = "Fast Save";
            SaveButton.ToolTipText = "Ctrl+S";
            SaveButton.Click += new EventHandler(SaveButton_Click);
            MoveUpButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            MoveUpButton.Image = (Image)resources.GetObject("MoveUpButton.Image");
            MoveUpButton.ImageTransparentColor = Color.Magenta;
            MoveUpButton.Name = "MoveUpButton";
            MoveUpButton.Size = new Size(59, 22);
            MoveUpButton.Text = "Move Up";
            MoveUpButton.ToolTipText = "Ctrl+UP";
            MoveUpButton.Click += new EventHandler(MoveUpButton_Click);
            MoveDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            MoveDownButton.Image = (Image)resources.GetObject("MoveDownButton.Image");
            MoveDownButton.ImageTransparentColor = Color.Magenta;
            MoveDownButton.Name = "MoveDownButton";
            MoveDownButton.Size = new Size(75, 22);
            MoveDownButton.Text = "Move Down";
            MoveDownButton.ToolTipText = "Ctrl+DOWN";
            MoveDownButton.Click += new EventHandler(MoveDownButton_Click);
            EditButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            EditButton.Image = (Image)resources.GetObject("EditButton.Image");
            EditButton.ImageTransparentColor = Color.Magenta;
            EditButton.Name = "EditButton";
            EditButton.Size = new Size(31, 22);
            EditButton.Text = "Edit";
            EditButton.ToolTipText = "Ctrl+E or double click";
            EditButton.Click += new EventHandler(button1_Click);
            RemoveButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            RemoveButton.Image = (Image)resources.GetObject("RemoveButton.Image");
            RemoveButton.ImageTransparentColor = Color.Magenta;
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new Size(54, 22);
            RemoveButton.Text = "Remove";
            RemoveButton.ToolTipText = "Delete";
            RemoveButton.Click += new EventHandler(RemoveButton_Click);
            AddButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            AddButton.Image = (Image)resources.GetObject("AddButton.Image");
            AddButton.ImageTransparentColor = Color.Magenta;
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(60, 22);
            AddButton.Text = "Add New";
            AddButton.ToolTipText = "nothin";
            AddButton.Click += new EventHandler(AddButton_Click);
            InsertAfterButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            InsertAfterButton.Image = (Image)resources.GetObject("InsertAfterButton.Image");
            InsertAfterButton.ImageTransparentColor = Color.Magenta;
            InsertAfterButton.Name = "InsertAfterButton";
            InsertAfterButton.Size = new Size(69, 22);
            InsertAfterButton.Text = "Insert After";
            InsertAfterButton.ToolTipText = "Insert";
            InsertAfterButton.Click += new EventHandler(InsertAfterButton_Click);
            InsertBeforeButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            InsertBeforeButton.Image = (Image)resources.GetObject("InsertBeforeButton.Image");
            InsertBeforeButton.ImageTransparentColor = Color.Magenta;
            InsertBeforeButton.Name = "InsertBeforeButton";
            InsertBeforeButton.Size = new Size(77, 22);
            InsertBeforeButton.Text = "Insert Before";
            InsertBeforeButton.ToolTipText = "nothin";
            InsertBeforeButton.Click += new EventHandler(InsertBeforeButton_Click);
            openTemplateButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            openTemplateButton.Image = (Image)resources.GetObject("openTemplateButton.Image");
            openTemplateButton.ImageTransparentColor = Color.Magenta;
            openTemplateButton.Name = "openTemplateButton";
            openTemplateButton.Size = new Size(92, 22);
            openTemplateButton.Text = "Insert Template";
            openTemplateButton.ToolTipText = "Adds an existing tas file to the bottom";
            openTemplateButton.Click += new EventHandler(openTemplateButton_Click);
            CopyButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            CopyButton.Image = (Image)resources.GetObject("CopyButton.Image");
            CopyButton.ImageTransparentColor = Color.Magenta;
            CopyButton.Name = "CopyButton";
            CopyButton.Size = new Size(39, 22);
            CopyButton.Text = "Copy";
            CopyButton.ToolTipText = "Ctrl+C";
            CopyButton.Click += new EventHandler(CopyButton_Click);
            PasteButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            PasteButton.Image = (Image)resources.GetObject("PasteButton.Image");
            PasteButton.ImageTransparentColor = Color.Magenta;
            PasteButton.Name = "PasteButton";
            PasteButton.Size = new Size(39, 22);
            PasteButton.Text = "Paste";
            PasteButton.ToolTipText = "Ctrl+V";
            PasteButton.Click += new EventHandler(PasteButton_Click);
            saveAsButton.Alignment = ToolStripItemAlignment.Right;
            saveAsButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            saveAsButton.Image = (Image)resources.GetObject("saveAsButton.Image");
            saveAsButton.ImageTransparentColor = Color.Magenta;
            saveAsButton.Name = "saveAsButton";
            saveAsButton.Size = new Size(51, 22);
            saveAsButton.Text = "Save As";
            saveAsButton.ToolTipText = "Ctrl+Shift+S";
            saveAsButton.Click += new EventHandler(saveAsButton_Click);
            OpenButton.Alignment = ToolStripItemAlignment.Right;
            OpenButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            OpenButton.Image = (Image)resources.GetObject("OpenButton.Image");
            OpenButton.ImageTransparentColor = Color.Magenta;
            OpenButton.Name = "OpenButton";
            OpenButton.Size = new Size(40, 22);
            OpenButton.Text = "Open";
            OpenButton.ToolTipText = "Ctrl+O";
            OpenButton.Click += new EventHandler(OpenButton_Click);
            ofDialogue.Filter = "tas file|*.dgtas";
            ofDialogue.FileOk += new CancelEventHandler(ofDialogue_FileOk);
            saveAsDialogue.FileName = "mytas.dgtas";
            saveAsDialogue.Filter = "tas file|*.dgtas";
            saveAsDialogue.FileOk += new CancelEventHandler(saveAsDialogue_FileOk);
            openTemplateDialogue.FileName = "myfile.dgtas";
            openTemplateDialogue.Filter = "tas file|*.dgtas";
            openTemplateDialogue.FileOk += new CancelEventHandler(openTemplateDialogue_FileOk);
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1006, 372);
            Controls.Add(itemList);
            Controls.Add(toolStrip1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximumSize = new Size(1022, 4360);
            MinimumSize = new Size(1022, 39);
            Name = "Form1";
            Text = "DG Tas editor";
            Load += new EventHandler(Form1_Load);
            KeyDown += new KeyEventHandler(Form1_KeyDown);
            KeyPress += new KeyPressEventHandler(Form1_KeyPress);
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
