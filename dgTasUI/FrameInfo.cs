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
            RNG = copyFrom.RNG;
            startFrame = copyFrom.startFrame;
            stopFrame = copyFrom.stopFrame;
            LTRIGGER = copyFrom.LTRIGGER;
            RTRIGGER = copyFrom.RTRIGGER;
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

        public string frameString => (startFrame + 1).ToString() + "-" + stopFrame.ToString();

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
            ListViewItem listViewItem = new ListViewItem(frameString);
            listViewItem.BackColor = grey ? Color.LightGray : Color.Gray;
            listViewItem.UseItemStyleForSubItems = false;
            listViewItem.SubItems.Add(LEFT.ToString());
            listViewItem.SubItems.Add(RIGHT.ToString());
            listViewItem.SubItems.Add(UP.ToString());
            listViewItem.SubItems.Add(DOWN.ToString());
            listViewItem.SubItems.Add(JUMP.ToString());
            listViewItem.SubItems.Add(SHOOT.ToString());
            listViewItem.SubItems.Add(GRAB.ToString());
            listViewItem.SubItems.Add(QUACK.ToString());
            listViewItem.SubItems.Add(START.ToString());
            listViewItem.SubItems.Add(STRAFE.ToString());
            listViewItem.SubItems.Add(RAGDOLL.ToString());
            listViewItem.SubItems.Add(SELECT.ToString());
            listViewItem.SubItems.Add(((int)RNG).ToString());
            listViewItem.SubItems.Add(LTRIGGER.ToString());
            listViewItem.SubItems.Add(RTRIGGER.ToString());
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
            for (int index = 0; index < stopFrame - startFrame; ++index)
            {
                byteList.Add(LEFT ? (byte)1 : (byte)0);
                byteList.Add(RIGHT ? (byte)1 : (byte)0);
                byteList.Add(UP ? (byte)1 : (byte)0);
                byteList.Add(DOWN ? (byte)1 : (byte)0);
                byteList.Add(JUMP ? (byte)1 : (byte)0);
                byteList.Add(SHOOT ? (byte)1 : (byte)0);
                byteList.Add(GRAB ? (byte)1 : (byte)0);
                byteList.Add(QUACK ? (byte)1 : (byte)0);
                byteList.Add(START ? (byte)1 : (byte)0);
                byteList.Add(STRAFE ? (byte)1 : (byte)0);
                byteList.Add(RAGDOLL ? (byte)1 : (byte)0);
                byteList.Add(SELECT ? (byte)1 : (byte)0);
                byteList.Add(RNG);
                byteList.AddRange(toByte(LTRIGGER));
                byteList.AddRange(toByte(RTRIGGER));
            }
            return byteList.ToArray();
        }
    }
}
