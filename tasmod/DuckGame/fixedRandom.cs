// Decompiled with JetBrains decompiler
// Type: DuckGame.fixedRandom
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;

namespace DuckGame
{
    internal class fixedRandom : Random
    {
        private float number;
        private double maxnextdouble;

        public fixedRandom(int num)
        {
            this.maxnextdouble = 1.0 / 1.0;
            this.number = num / (float)byte.MaxValue;
        }

        public override int Next(int maxValue)
        {
            --maxValue;
            return (int)(number * (double)maxValue);
        }

        public override int Next(int minValue, int maxValue)
        {
            --maxValue;
            return (int)((maxValue - (double)minValue) * number + minValue);
        }

        public override double NextDouble() => number * this.maxnextdouble;
    }
}
