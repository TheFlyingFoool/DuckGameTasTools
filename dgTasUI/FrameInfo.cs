// Decompiled with JetBrains decompiler
// Type: dgTasUI.FrameInfo
// Assembly: dgTasUI, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3D420AD3-C250-4777-B857-6AA7DF3EA832
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\DGTas\dgTasUI.exe

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace dgTasUI
{
    public class FrameInfo
    {
        public int selected = 0;
        public bool LEFT;
        public bool RIGHT;
        public bool UP;
        public bool DOWN;
        public bool JUMP;
        public bool SHOOT;
        public bool GRAB;
        public bool QUACK;
        public bool RAGDOLL;
        public bool START;
        public bool STRAFE;
        public bool SELECT;
        public byte RNG;
        public float LTRIGGER;
        public float RTRIGGER;
        public int startFrame;
        public int stopFrame;

        public FrameInfo()
        {
        }

        public FrameInfo(FrameInfo copyFrom)
        {
            foreach (FieldInfo fieldInfo in typeof(FrameInfo).GetFields().Where(f => f.FieldType == typeof(bool)))
                fieldInfo.SetValue(this, fieldInfo.GetValue(copyFrom));
            this.RNG = copyFrom.RNG;
            this.startFrame = copyFrom.startFrame;
            this.stopFrame = copyFrom.stopFrame;
            this.LTRIGGER = copyFrom.LTRIGGER;
            this.RTRIGGER = copyFrom.RTRIGGER;
        }

        public override int GetHashCode() => base.GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is FrameInfo frameInfo))
                return false;
            foreach (FieldInfo fieldInfo in typeof(FrameInfo).GetFields().Where(f => f.FieldType == typeof(bool)))
            {
                if ((bool)fieldInfo.GetValue(obj) != (bool)fieldInfo.GetValue(this))
                    return false;
            }
            return RNG == frameInfo.RNG;
        }

        public static FrameInfo Load(byte[] bytes) => new FrameInfo()
        {
            LEFT = bytes[0] > 0,
            RIGHT = bytes[1] > 0,
            UP = bytes[2] > 0,
            DOWN = bytes[3] > 0,
            JUMP = bytes[4] > 0,
            SHOOT = bytes[5] > 0,
            GRAB = bytes[6] > 0,
            QUACK = bytes[7] > 0,
            START = bytes[8] > 0,
            STRAFE = bytes[9] > 0,
            RAGDOLL = bytes[10] > 0,
            SELECT = bytes[11] > 0,
            RNG = bytes[12],
            LTRIGGER = BitConverter.ToSingle(bytes, 13),
            RTRIGGER = BitConverter.ToSingle(bytes, 17)
        };

        public string frameString => (this.startFrame + 1).ToString() + "-" + this.stopFrame.ToString();

        private byte[] toByte(object o)
        {
            switch (o)
            {
                case float num:
                    return BitConverter.GetBytes(num);
                case bool flag:
                    return new byte[1] { flag ? (byte)1 : (byte)0 };
                default:
                    return new byte[0];
            }
        }

        public ListViewItem getItem(bool grey)
        {
            ListViewItem listViewItem = new ListViewItem(this.frameString);
            listViewItem.BackColor = grey ? Color.LightGray : Color.Gray;
            listViewItem.UseItemStyleForSubItems = false;
            listViewItem.SubItems.Add(this.LEFT.ToString());
            listViewItem.SubItems.Add(this.RIGHT.ToString());
            listViewItem.SubItems.Add(this.UP.ToString());
            listViewItem.SubItems.Add(this.DOWN.ToString());
            listViewItem.SubItems.Add(this.JUMP.ToString());
            listViewItem.SubItems.Add(this.SHOOT.ToString());
            listViewItem.SubItems.Add(this.GRAB.ToString());
            listViewItem.SubItems.Add(this.QUACK.ToString());
            listViewItem.SubItems.Add(this.START.ToString());
            listViewItem.SubItems.Add(this.STRAFE.ToString());
            listViewItem.SubItems.Add(this.RAGDOLL.ToString());
            listViewItem.SubItems.Add(this.SELECT.ToString());
            listViewItem.SubItems.Add(((int)this.RNG).ToString());
            listViewItem.SubItems.Add(this.LTRIGGER.ToString());
            listViewItem.SubItems.Add(this.RTRIGGER.ToString());
            foreach (object subItem in listViewItem.SubItems)
            {
                ListViewItem.ListViewSubItem listViewSubItem = subItem as ListViewItem.ListViewSubItem;
                Color color = grey ? Color.LightGray : Color.Gray;
                if (listViewSubItem.Text == "True")
                    color = grey ? Color.LightGreen : Color.Green;
                if (listViewSubItem.Text == "False")
                    color = grey ? Color.LightSalmon : Color.Salmon;
                listViewSubItem.BackColor = color;
            }
            return listViewItem;
        }

        public byte[] write()
        {
            List<byte> byteList = new List<byte>();
            for (int index = 0; index < this.stopFrame - this.startFrame; ++index)
            {
                byteList.Add(this.LEFT ? (byte)1 : (byte)0);
                byteList.Add(this.RIGHT ? (byte)1 : (byte)0);
                byteList.Add(this.UP ? (byte)1 : (byte)0);
                byteList.Add(this.DOWN ? (byte)1 : (byte)0);
                byteList.Add(this.JUMP ? (byte)1 : (byte)0);
                byteList.Add(this.SHOOT ? (byte)1 : (byte)0);
                byteList.Add(this.GRAB ? (byte)1 : (byte)0);
                byteList.Add(this.QUACK ? (byte)1 : (byte)0);
                byteList.Add(this.START ? (byte)1 : (byte)0);
                byteList.Add(this.STRAFE ? (byte)1 : (byte)0);
                byteList.Add(this.RAGDOLL ? (byte)1 : (byte)0);
                byteList.Add(this.SELECT ? (byte)1 : (byte)0);
                byteList.Add(this.RNG);
                byteList.AddRange(this.toByte(LTRIGGER));
                byteList.AddRange(this.toByte(RTRIGGER));
            }
            return byteList.ToArray();
        }
    }
}
