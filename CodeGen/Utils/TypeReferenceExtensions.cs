using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SuperSync.CodeGen.Utils
{
    public static class TypeReferenceExtensions
    {
        public static IEnumerable<Instruction> GetLoadDefaultInstructions(this TypeReference reference,
            ModuleDefinition module)
        {
            if (!reference.IsValueType)
            {
                // object: null
                yield return Instruction.Create(OpCodes.Ldnull);
                yield break;
            }

            if (reference == module.TypeSystem.Char ||
                reference == module.TypeSystem.SByte ||
                reference == module.TypeSystem.Byte ||
                reference == module.TypeSystem.Int16 ||
                reference == module.TypeSystem.Int32 ||
                reference == module.TypeSystem.UInt16 ||
                reference == module.TypeSystem.UInt32)
            {
                // i4: 0
                yield return Instruction.Create(OpCodes.Ldc_I4_0);
                yield break;
            }

            if (reference == module.TypeSystem.Int64 || reference == module.TypeSystem.UInt64)
            {
                // (i4: 0) as i8
                yield return Instruction.Create(OpCodes.Ldc_I4_0);
                yield return Instruction.Create(OpCodes.Conv_I8);
                yield break;
            }

            if (reference == module.TypeSystem.Single)
            {
                // f4: 0f
                yield return Instruction.Create(OpCodes.Ldc_R4, 0f);
                yield break;
            }

            if (reference == module.TypeSystem.Double)
            {
                // f8: 0f
                yield return Instruction.Create(OpCodes.Ldc_R8, 0f);
                yield break;
            }

            if (reference == module.TypeSystem.IntPtr)
            {
                // (i4: 0) as signed i4
                yield return Instruction.Create(OpCodes.Ldc_I4_0);
                yield return Instruction.Create(OpCodes.Conv_I);
                yield break;
            }

            if (reference == module.TypeSystem.UIntPtr)
            {
                // (i4: 0) as unsigned i4
                yield return Instruction.Create(OpCodes.Ldc_I4_0);
                yield return Instruction.Create(OpCodes.Conv_U);
                yield break;
            }

            // Default to i4: 0
            yield return Instruction.Create(OpCodes.Ldc_I4_0);
        }
    }
}