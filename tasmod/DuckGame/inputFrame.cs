// Decompiled with JetBrains decompiler
// Type: DuckGame.inputFrame
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
    public class inputFrame
    {
        public float lTrigger;
        public float rTrigger;
        public int[] inputs;
        public int rng;
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
        public static bool oldway = true;

        public inputFrame(byte[] bytes)
        {
            this.inputs = ((IEnumerable<byte>)bytes).Take(12).Select<byte, int>(x => x).ToArray();
            this.rng = bytes[12];
            this.lTrigger = BitConverter.ToSingle(bytes, 13);
            this.rTrigger = BitConverter.ToSingle(bytes, 17);
        }

        public byte[] write()
        {
            List<byte> byteList = new List<byte>();
            byteList.AddRange(((IEnumerable<int>)this.inputs).Select(x => (byte)x).ToArray());
            byteList.Add((byte)this.rng);
            byteList.AddRange(BitConverter.GetBytes(this.lTrigger));
            byteList.AddRange(BitConverter.GetBytes(this.rTrigger));
            return byteList.ToArray();
        }

        private byte[] toByte(object o)
        {
            byte[] numArray;
            switch (o)
            {
                case float num:
                    numArray = BitConverter.GetBytes(num);
                    break;
                case bool flag:
                    numArray = new byte[1]
                    {
            flag ? (byte) 1 : (byte) 0
                    };
                    break;
                default:
                    numArray = new byte[0];
                    break;
            }
            return numArray;
        }

        public byte[] write2()
        {
            List<byte> byteList = new List<byte>();
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
            return byteList.ToArray();
        }

        public inputFrame(InputProfile inputProfile)
        {
            InputDevice lastActiveDevice = inputProfile.lastActiveDevice;
            AnalogGamePad analogGamePad = null;
            if (lastActiveDevice is GenericController)
                analogGamePad = (lastActiveDevice as GenericController).device;
            if (oldway)
            {
                this.LEFT = inputProfile.Down(nameof(LEFT));
                this.RIGHT = inputProfile.Down(nameof(RIGHT));
                this.UP = inputProfile.Down(nameof(UP));
                this.DOWN = inputProfile.Down(nameof(DOWN));
                this.JUMP = inputProfile.Down(nameof(JUMP));
                this.SHOOT = inputProfile.Down(nameof(SHOOT));
                this.GRAB = inputProfile.Down(nameof(GRAB));
                this.QUACK = inputProfile.Down(nameof(QUACK));
                this.START = inputProfile.Down(nameof(START));
                this.STRAFE = inputProfile.Down(nameof(STRAFE));
                this.RAGDOLL = inputProfile.Down(nameof(RAGDOLL));
                this.SELECT = inputProfile.Down(nameof(SELECT));
            }
            else if (analogGamePad != null)
            {
                PadState padState = (PadState)HelperG.GfieldVal(typeof(AnalogGamePad), "_state", analogGamePad);
                DevConsole.Log(padState.sticks.left.x.ToString() + " " + padState.sticks.left.y.ToString());
                this.LEFT = padState.sticks.left.x < -0.600000023841858 || padState.IsButtonDown(PadButton.LeftThumbstickLeft) || padState.IsButtonDown(PadButton.DPadLeft);
                this.RIGHT = padState.sticks.left.x > 0.600000023841858 || padState.IsButtonDown(PadButton.LeftThumbstickRight) || padState.IsButtonDown(PadButton.DPadRight);
                this.UP = padState.sticks.left.y > 0.600000023841858 || padState.IsButtonDown(PadButton.LeftThumbstickUp) || padState.IsButtonDown(PadButton.DPadUp);
                this.DOWN = padState.sticks.left.y < -0.600000023841858 || padState.IsButtonDown(PadButton.LeftThumbstickDown) || padState.IsButtonDown(PadButton.DPadDown);
                this.JUMP = padState.IsButtonDown(PadButton.A);
                this.SHOOT = padState.IsButtonDown(PadButton.X);
                this.GRAB = padState.IsButtonDown(PadButton.Y);
                this.QUACK = padState.IsButtonDown(PadButton.B);
                this.START = padState.IsButtonDown(PadButton.Start);
                this.STRAFE = padState.IsButtonDown(PadButton.LeftShoulder);
                this.RAGDOLL = padState.IsButtonDown(PadButton.RightShoulder);
                this.SELECT = padState.IsButtonDown(PadButton.Start);
            }
            this.RNG = 0;
            this.LTRIGGER = 0.0f;
            this.RTRIGGER = 0.0f;
        }
    }
}
