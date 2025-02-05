using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;

namespace SuperSync.CodeGen.Utils
{
    public static class InstructionExtensions
    {
        public static IEnumerable<Instruction> EnumerateBackwardsUntilAndIncluding(this Instruction instruction,
            Func<Instruction, bool> predicate)
        {
            var current = instruction;
            while (predicate(current))
            {
                yield return current;
                current = current.Previous;
            }

            yield return current;
        }
    }
}