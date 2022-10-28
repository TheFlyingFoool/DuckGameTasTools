// Decompiled with JetBrains decompiler
// Type: DuckGame.HelperG
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
    public static class HelperG
    {
        private static readonly Func<MethodInfo, IEnumerable<Type>> ParameterTypeProjection = method => ((IEnumerable<ParameterInfo>)method.GetParameters()).Select(p => p.ParameterType.GetGenericTypeDefinition());
        public static string CollKey = "(0 top, bottom,left,right) (1 top,left,right) (2 top) (3 bottom)(4 one way)";

        public static object GfieldVal(string classname, string Field, object obj = null)
        {
            Type type = CoreMod.coreMod.configuration.assembly.GetType("DuckGame." + classname);
            FieldInfo fieldInfo = !(type == null) ? type.GetField(Field, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentException("type can not be Null", "helperG.GfieldVal");
            return !(fieldInfo == null) ? fieldInfo.GetValue(obj) : throw new ArgumentException("fieldInfo Not found, Null " + Field, "helperG.GfieldVal");
        }

        public static object GfieldVal(Type type, string Field, object obj = null)
        {
            FieldInfo fieldInfo = !(type == null) ? type.GetField(Field, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic) : throw new ArgumentException("type can not be Null", "helperG.GfieldVal");
            return !(fieldInfo == null) ? fieldInfo.GetValue(obj) : throw new ArgumentException("fieldInfo Not found, Null " + Field, "helperG.GfieldVal");
        }

        public static MethodInfo GetGenericMethodTypes(
          this Type type,
          string name,
          BindingFlags bindingAttr,
          params Type[] parameterTypes)
        {
            return ((IEnumerable<MethodInfo>)type.GetMethods()).Where(method => method.Name == name).Where(method => ((IEnumerable<Type>)parameterTypes).SequenceEqual(ParameterTypeProjection(method))).SingleOrDefault();
        }

        public static MethodInfo GetGenericMethod(this Type type, string name, BindingFlags bindingAttr)
        {
            return ((IEnumerable<MethodInfo>)type.GetMethods(bindingAttr)).Where(method => method.Name == name).First();
        }

        public static void CMethod(string classname, string method, object obj = null)
        {
            Type type = CoreMod.coreMod.configuration.assembly.GetType("DuckGame." + classname);
            MethodInfo method1 = type.GetMethod(method, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (!(type != null) || !(method1 != null))
                return;
            method1.Invoke(obj, null);
        }

        public static void SfieldVal(string classname, string Field, object value, object obj = null)
        {
            Type type = CoreMod.coreMod.configuration.assembly.GetType("DuckGame." + classname);
            FieldInfo field = type.GetField(Field, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (!(type != null) || !(field != null))
                return;
            field.SetValue(obj, value);
        }

        public static MethodInfo GMIA(Type type, string Methodname)
        {
            MethodInfo genericMethod = type.GetGenericMethod(Methodname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (type == null)
                throw new ArgumentException("type can not be Null", "helperG.GMI");
            return !(genericMethod == null) ? genericMethod : throw new ArgumentException("MethodInfo Not found, Null " + type.Name + Methodname, "helperG.GMI");
        }

        public static MethodInfo GMI(Type type, string Methodname)
        {
            MethodInfo method = type.GetMethod(Methodname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (type == null)
                throw new ArgumentException("type can not be Null", "helperG.GMI");
            return !(method == null) ? method : throw new ArgumentException("MethodInfo Not found, Null " + type.Name + Methodname, "helperG.GMI");
        }

        public static MethodInfo GMID(string classname, string Methodname)
        {
            Type type = CoreMod.coreMod.configuration.assembly.GetType("DuckGame." + classname);
            MethodInfo method = type.GetMethod(Methodname, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (type == null)
                throw new ArgumentException("type can not be Null " + classname + " " + Methodname, "helperG.GMID");
            return !(method == null) ? method : throw new ArgumentException("MethodInfo Not found, Null " + classname + " " + Methodname, "helperG.GMID");
        }

        public static void SfieldVal(Type type, string Field, object value, object obj = null)
        {
            FieldInfo field = type.GetField(Field, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (!(type != null) || !(field != null))
                return;
            field.SetValue(obj, value);
        }
    }
}
