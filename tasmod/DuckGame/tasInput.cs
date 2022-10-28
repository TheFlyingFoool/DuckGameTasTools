// Decompiled with JetBrains decompiler
// Type: DuckGame.tasInput
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
    internal class tasInput
    {
		public static Dictionary<int, int> Keys = new Dictionary<int, int>
		{
			{
				4,
				0
			},
			{
				8,
				1
			},
			{
				1,
				2
			},
			{
				2,
				3
			},
			{
				4096,
				4
			},
			{
				16384,
				5
			},
			{
				32768,
				6
			},
			{
				8192,
				7
			},
			{
				16,
				8
			},
			{
				256,
				9
			},
			{
				512,
				10
			}
		};

		public static inputFrame[] read(string file)
        {
            byte[] bytes = new byte[0];
            if (System.IO.File.Exists(file))
                bytes = System.IO.File.ReadAllBytes(file);
            return read(bytes);
        }

		public static inputFrame[] read(byte[] bytes)
		{
			bool flag = bytes.Length % 21 != 0;
			inputFrame[] result;
			if (flag)
			{
				result = new inputFrame[1]
				{
					new inputFrame(new byte[21])
				};
			}
			else
			{
				List<inputFrame> list = new List<inputFrame>();
				IEnumerable<List<byte>> enumerable = tasInput.splitList<byte>(bytes.ToList<byte>(), 21);
				foreach (List<byte> list2 in enumerable)
				{
					list.Add(new inputFrame(list2.ToArray()));
				}
				result = list.ToArray();
			}
			return result;
		}

		public static IEnumerable<List<T>> splitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
        }
    }
}
