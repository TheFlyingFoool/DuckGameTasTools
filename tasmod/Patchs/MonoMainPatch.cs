// Decompiled with JetBrains decompiler
// Type: RunTimeEdit.MonoMainPatch
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using DuckGame;

namespace RunTimeEdit
{
    internal class MonoMainPatch
    {
        private static bool dothing;

        private static bool Prefix()
        {
            if (dothing && updater.current != null)
                updater.current.UpdateBase();
            return true;
        }
    }
}
