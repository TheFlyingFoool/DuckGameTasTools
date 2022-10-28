// Decompiled with JetBrains decompiler
// Type: HarmonyInternal.AccessTools
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Reflection;

namespace HarmonyInternal
{
    public static class AccessTools
    {
        public static BindingFlags all = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty;

        public static FieldInfo Field(Type type, string name) => (type == null ? 1 : (name == null ? 1 : 0)) == 0 ? FindRecursive(type, t => t.GetField(name, all)) : null;

        public static T FindRecursive<T>(Type type, Func<Type, T> action)
        {
            T recursive;
            while (true)
            {
                recursive = action(type);
                if (recursive == null)
                {
                    if (!(type == typeof(object)))
                        type = type.BaseType;
                    else
                        goto label_4;
                }
                else
                    break;
            }
            return recursive;
        label_4:
            return default(T);
        }
    }
}
