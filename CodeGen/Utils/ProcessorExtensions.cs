using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SuperSync.CodeGen.Utils
{
    public static class ProcessorExtensions
    {
        /// <summary>
        /// Copies a group of instructions to the top of a method body
        /// </summary>
        /// <param name="processor">Processor</param>
        /// <param name="target">Instruction to add the new instructions behind</param>
        /// <param name="instructions">Instructions to add</param>
        public static void CopyBefore(this ILProcessor processor, Instruction target,
            IEnumerable<Instruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction == null)
                    continue;
                processor.InsertBefore(target, instruction);
            }
        }

        public static Instruction FindCtorCallInstruction(this ILProcessor processor)
        {
            return processor.Body.Instructions.FirstOrDefault(x =>
                x.OpCode == OpCodes.Call && x.Operand is MethodReference { Name: ".ctor" });
        }
    }
}