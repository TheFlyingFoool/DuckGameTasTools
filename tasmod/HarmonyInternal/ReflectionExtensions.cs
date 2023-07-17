// Decompiled with JetBrains decompiler
// Type: ReflectionExtensions
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public static class ReflectionExtensions
{
    public static object GetMemberValue(this object obj, string memberName)
    {
        MemberInfo memberInfo = GetMemberInfo(obj, memberName);
        if (memberInfo == null)
            throw new Exception("memberName " + memberName);
        if ((object)(memberInfo as PropertyInfo) != null)
            return memberInfo.As<PropertyInfo>().GetValue(obj, null);
        return (object)(memberInfo as FieldInfo) != null ? memberInfo.As<FieldInfo>().GetValue(obj) : throw new Exception();
    }

    public static object SetMemberValue(this object obj, string memberName, object newValue)
    {
        MemberInfo memberInfo = GetMemberInfo(obj, memberName);
        if (memberInfo == null)
            throw new Exception(nameof(memberName));
        object memberValue = obj.GetMemberValue(memberName);
        if ((object)(memberInfo as PropertyInfo) != null)
        {
            memberInfo.As<PropertyInfo>().SetValue(obj, newValue, null);
            return memberValue;
        }
        if ((object)(memberInfo as FieldInfo) == null)
            throw new Exception();
        memberInfo.As<FieldInfo>().SetValue(obj, newValue);
        return memberValue;
    }

    private static MemberInfo GetMemberInfo(object obj, string memberName)
    {
        List<PropertyInfo> list1 = new List<PropertyInfo>()
    {
      obj.GetType().GetProperty(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
    }.Where(i => i != null).ToList();
        if (list1.Count != 0)
            return list1[0];
        List<FieldInfo> list2 = new List<FieldInfo>()
    {
      obj.GetType().GetField(memberName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
    }.Where(i => i != null).ToList();
        return list2.Count != 0 ? list2[0] : null;
    }

    [DebuggerHidden]
    private static T As<T>(this object obj) => (T)obj;
}
