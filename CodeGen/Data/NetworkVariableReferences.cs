using System.Linq;
using Mono.Cecil;
using SuperSync.CodeGen.Utils;

namespace SuperSync.CodeGen.Data
{
    public struct NetworkVariableReferences
    {
        public readonly TypeReference T1;

        public readonly GenericInstanceType NetworkVariableT1GenericType;
        public readonly TypeReference NetworkVariableT1;
        public readonly MethodReference NetworkVariableT1_Constructor;
        public readonly MethodReference NetworkVariableT1_Getter;
        public readonly MethodReference NetworkVariableT1_Setter;

        public readonly MethodReference AnticipatedNetworkVariableT1_Anticipate;

        public NetworkVariableReferences(
            ModuleDefinition module,
            ModuleReferences moduleReferences,
            TypeReference t1, bool createAnticipatedNetworkVariable)
        {
            // Create a type reference to NetworkVariable<> and resolve it
            T1 = module.ImportReference(t1);

            // Create a type reference to NetworkVariable<T> or AnticipatedNetworkVariable<T>)
            NetworkVariableT1GenericType = new GenericInstanceType(createAnticipatedNetworkVariable
                ? moduleReferences.AnticipatedNetworkVariableTNone
                : moduleReferences.NetworkVariableTNone);
            NetworkVariableT1GenericType.GenericArguments.Add(T1);

            var typeWithT1Resolved = NetworkVariableT1GenericType.Resolve();

            NetworkVariableT1_Getter = typeWithT1Resolved
                .Methods.FirstOrDefault(v => v.Name == "get_Value");
            NetworkVariableT1_Getter!.DeclaringType = NetworkVariableT1GenericType;
            NetworkVariableT1_Getter =
                NetworkVariableT1GenericType.MakeGenericMethod(NetworkVariableT1_Getter.ReturnType,
                    NetworkVariableT1_Getter);

            if (!createAnticipatedNetworkVariable)
            {
                NetworkVariableT1_Setter = typeWithT1Resolved.Methods.FirstOrDefault(
                    v => v.Name == "set_Value");
                NetworkVariableT1_Setter!.DeclaringType = NetworkVariableT1GenericType;
                NetworkVariableT1_Setter =
                    NetworkVariableT1GenericType.MakeGenericMethod(module.TypeSystem.Void, NetworkVariableT1_Setter);
            }
            else
            {
                NetworkVariableT1_Setter = null;
            }

            NetworkVariableT1_Constructor = typeWithT1Resolved.Methods.FirstOrDefault(
                v => v.Name == ".ctor");
            NetworkVariableT1_Constructor!.DeclaringType = NetworkVariableT1GenericType;
            NetworkVariableT1_Constructor =
                NetworkVariableT1GenericType.MakeGenericMethod(module.TypeSystem.Void, NetworkVariableT1_Constructor);

            // Find a few extra methods for AnticipatedNetworkVariable<T>
            if (createAnticipatedNetworkVariable)
            {
                AnticipatedNetworkVariableT1_Anticipate =
                    typeWithT1Resolved.Methods.FirstOrDefault(
                        v => v.Name == "Anticipate");
                AnticipatedNetworkVariableT1_Anticipate!.DeclaringType = NetworkVariableT1GenericType;
                AnticipatedNetworkVariableT1_Anticipate =
                    NetworkVariableT1GenericType.MakeGenericMethod(AnticipatedNetworkVariableT1_Anticipate.ReturnType,
                        AnticipatedNetworkVariableT1_Anticipate);
            }
            else
            {
                AnticipatedNetworkVariableT1_Anticipate = null;
            }

            NetworkVariableT1 = module.ImportReference(NetworkVariableT1GenericType);
        }
    }
}