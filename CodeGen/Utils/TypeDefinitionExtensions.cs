using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;

namespace SuperSync.CodeGen.Utils
{
    public static class TypeDefinitionExtensions
    {
        public static IEnumerable<TypeDefinition> GetBaseTypes(this TypeDefinition typeDefinition)
        {
            var current = typeDefinition.BaseType?.Resolve();
            while (current != null)
            {
                yield return current;
                current = current.BaseType?.Resolve();
            }
        }

        public static bool IsDerivedFromOrIs(this TypeDefinition typeDefinition, string name)
        {
            if (typeDefinition.Name == name)
                return true;

            return typeDefinition.GetBaseTypes().Any(t => t.Name == name);
        }

        public static MethodDefinition GetOrCreateConstructor(this TypeDefinition typeDefinition)
        {
            if (typeDefinition.Methods.FirstOrDefault(m => m.Name == ".ctor") is not { } constructor)
            {
                constructor = new MethodDefinition(".ctor", MethodAttributes.Public,
                    typeDefinition.Module.TypeSystem.Void);
                typeDefinition.Methods.Add(constructor);
            }

            return constructor;
        }
    }
}