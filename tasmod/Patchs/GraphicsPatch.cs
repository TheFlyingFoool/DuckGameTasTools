// Decompiled with JetBrains decompiler
// Type: RunTimeEdit.GraphicsPatch
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using DuckGame;
using System.Diagnostics;

namespace RunTimeEdit
{
    internal class GraphicsPatch
    {
        private static bool Prefixget_inFocus(ref bool __result)
        {
            if (!new StackTrace().GetFrame(2).GetMethod().Name.Contains("RunUpdate") || NotTasInput())
                return true;
            __result = true;
            return false;
        }

        private static bool NotTasInput() => InputProfile.DefaultPlayer1 == null || InputProfile.DefaultPlayer1.GetDevice(typeof(tasDevice)) == null;
    }
}
