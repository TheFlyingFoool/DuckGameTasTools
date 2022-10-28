// Decompiled with JetBrains decompiler
// Type: DuckGame.MPatch
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System.Reflection;

namespace DuckGame
{
    public class MPatch
    {
        public MethodInfo prefix;
        public MethodInfo postfix;
        public MethodInfo transpiler;
        public MethodBase original;

        public MPatch(
          MethodBase _original = null,
          MethodInfo _prefix = null,
          MethodInfo _postfix = null,
          MethodInfo _transpiler = null)
        {
            original = _original;
            prefix = _prefix;
            postfix = _postfix;
            transpiler = _transpiler;
        }
    }
}
