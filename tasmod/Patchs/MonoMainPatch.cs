// Decompiled with JetBrains decompiler
// Type: RunTimeEdit.GraphicsPatch
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using DuckGame;
using HarmonyInternal;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using static DuckGame.CMD;

namespace RunTimeEdit
{
    public class MonoMainPatch
    {
        public static string Overwrite(string text, int position, string new_text)
        {
            return text.Substring(0, position) + new_text + text.Substring(position + new_text.Length);
        }

        public static string correctilcode(string s)
        {
            char c = char.Parse(".");
            List<int> list = new List<int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == c)
                {
                    list.Add(i + 1);
                }
            }
            foreach (int num in list)
            {
                s = Overwrite(s, num, s[num].ToString().ToUpper());
            }
            s = s.Replace(".", "_");
            s = Overwrite(s, 0, s[0].ToString().ToUpper());
            return s;
        }
        private static MethodInfo Myget_inFocusMethod = typeof(MonoMainPatch).GetMethod("Myget_inFocus", BindingFlags.Static | BindingFlags.NonPublic);

        private static MethodInfo TranPrefixmethod = typeof(MonoMainPatch).GetMethod("TranPrefix", BindingFlags.Static | BindingFlags.NonPublic);

        private static MethodInfo get_inFocusMethod = typeof(Graphics).GetMethod("get_inFocus", BindingFlags.Static | BindingFlags.Public);
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> list = new List<CodeInstruction>(instructions);
            list.Insert(0, new CodeInstruction(OpCodes.Call, TranPrefixmethod));
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].opcode == OpCodes.Call && list[i].operand is MethodInfo && (list[i].operand as MethodInfo) == get_inFocusMethod)
                {
                    list[i].operand = Myget_inFocusMethod;
                }
            }
            return list;
        }
        private static bool dothing;
        private static void TranPrefix()
        {
            if (dothing && updater.current != null)
                updater.current.UpdateBase();
        }
        private static bool Myget_inFocus()
        {
            if (PatchInputProfile.NotTasInput()) // !new StackTrace().GetFrame(2).GetMethod().Name.Contains("RunUpdate") ||
                return Graphics.inFocus;
            return true;
        }
    }
}
