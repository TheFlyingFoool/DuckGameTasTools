// Decompiled with JetBrains decompiler
// Type: HarmonyInternal.CodeInstruction
// Assembly: tasmod, Version=69.420.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F552643C-C31D-4CFF-B7DA-3A8581E06C76
// Assembly location: C:\Users\daniel\Documents\DuckGame\76561198124539558\Mods\tasmod\tasmod.dll

using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace HarmonyInternal
{
    public class CodeInstruction
    {
        public OpCode opcode;
        public object operand;
        public List<Label> labels = new List<Label>();

        public CodeInstruction(OpCode opcode, object operand = null)
        {
            this.opcode = opcode;
            this.operand = operand;
        }

        public CodeInstruction(CodeInstruction instruction)
        {
            opcode = instruction.opcode;
            operand = instruction.operand;
            labels = ((IEnumerable<Label>)instruction.labels.ToArray()).ToList();
        }

        public CodeInstruction Clone() => new CodeInstruction(this)
        {
            labels = new List<Label>()
        };

        public CodeInstruction Clone(OpCode opcode) => new CodeInstruction(this)
        {
            labels = new List<Label>(),
            opcode = opcode
        };

        public CodeInstruction Clone(OpCode opcode, object operand) => new CodeInstruction(this)
        {
            labels = new List<Label>(),
            opcode = opcode,
            operand = operand
        };

        public override string ToString() => "";
    }
}
