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
		public inputFrame(byte[] bytes)
		{
			inputs = ((IEnumerable<byte>)bytes).Take(12).Select<byte, int>(x => x).ToArray();
			rng = bytes[12];
			lTrigger = BitConverter.ToSingle(bytes, 13);
			rTrigger = BitConverter.ToSingle(bytes, 17);
		}

		public byte[] write()
		{
			List<byte> byteList = new List<byte>();
			byteList.AddRange(((IEnumerable<int>)inputs).Select(x => (byte)x).ToArray());
			byteList.Add((byte)rng);
			byteList.AddRange(BitConverter.GetBytes(lTrigger));
			byteList.AddRange(BitConverter.GetBytes(rTrigger));
			return byteList.ToArray();
		}

		private byte[] toByte(object o)
		{
			byte[] result;
			if (o is float)
			{
				result = BitConverter.GetBytes((float)o);
			}
			else if (o is bool)
			{
				result = new byte[]
				{
					(byte)(((bool)o) ? 1 : 0)
				};
			}
			else
			{
				result = new byte[0];
			}
			return result;
		}

		public byte[] write2()
		{
			List<byte> list = new List<byte>();
			list.Add((byte)(LEFT ? 1 : 0));
			list.Add((byte)(RIGHT ? 1 : 0));
			list.Add((byte)(UP ? 1 : 0));
			list.Add((byte)(DOWN ? 1 : 0));
			list.Add((byte)(JUMP ? 1 : 0));
			list.Add((byte)(SHOOT ? 1 : 0));
			list.Add((byte)(GRAB ? 1 : 0));
			list.Add((byte)(QUACK ? 1 : 0));
			list.Add((byte)(START ? 1 : 0));
			list.Add((byte)(STRAFE ? 1 : 0));
			list.Add((byte)(RAGDOLL ? 1 : 0));
			list.Add((byte)(SELECT ? 1 : 0));
			list.Add(RNG);
			list.AddRange(toByte(LTRIGGER));
			list.AddRange(toByte(RTRIGGER));
			return list.ToArray();
		}

		public inputFrame(InputProfile inputProfile)
		{
			InputDevice lastActiveDevice = inputProfile.lastActiveDevice;
			AnalogGamePad analogGamePad = null;
			if (lastActiveDevice is GenericController)
			{
				analogGamePad = (lastActiveDevice as GenericController).device;
			}
			if (inputFrame.oldway)
			{
				LEFT = inputProfile.Down("LEFT");
				RIGHT = inputProfile.Down("RIGHT");
				UP = inputProfile.Down("UP");
				DOWN = inputProfile.Down("DOWN");
				JUMP = inputProfile.Down("JUMP");
				SHOOT = inputProfile.Down("SHOOT");
				GRAB = inputProfile.Down("GRAB");
				QUACK = inputProfile.Down("QUACK");
				START = inputProfile.Down("START");
				STRAFE = inputProfile.Down("STRAFE");
				RAGDOLL = inputProfile.Down("RAGDOLL");
				SELECT = inputProfile.Down("SELECT");
			}
			else if (analogGamePad != null)
			{
				PadState padState = (PadState)HelperG.GfieldVal(typeof(AnalogGamePad), "_state", analogGamePad);
				DevConsole.Log(padState.sticks.left.x.ToString() + " " + padState.sticks.left.y.ToString());
				LEFT = padState.sticks.left.x < -0.6f || padState.IsButtonDown(PadButton.LeftThumbstickLeft) || padState.IsButtonDown(PadButton.DPadLeft);
				RIGHT = padState.sticks.left.x > 0.6f || padState.IsButtonDown(PadButton.LeftThumbstickRight) || padState.IsButtonDown(PadButton.DPadRight);
				UP = padState.sticks.left.y > 0.6f || padState.IsButtonDown(PadButton.LeftThumbstickUp) || padState.IsButtonDown(PadButton.DPadUp);
				DOWN = padState.sticks.left.y < -0.6f || padState.IsButtonDown(PadButton.LeftThumbstickDown) || padState.IsButtonDown(PadButton.DPadDown);
				JUMP = padState.IsButtonDown(PadButton.A);
				SHOOT = padState.IsButtonDown(PadButton.X);
				GRAB = padState.IsButtonDown(PadButton.Y);
				QUACK = padState.IsButtonDown(PadButton.B);
				START = padState.IsButtonDown(PadButton.Start);
				STRAFE = padState.IsButtonDown(PadButton.LeftShoulder);
				RAGDOLL = padState.IsButtonDown(PadButton.RightShoulder);
				SELECT = padState.IsButtonDown(PadButton.Start);
			}
			RNG = 0;
			LTRIGGER = 0f;
			RTRIGGER = 0f;
		}

		static inputFrame()
		{
		}

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
	}
}


	
