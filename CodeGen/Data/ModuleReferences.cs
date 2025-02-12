using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mono.Cecil;
using Unity.Netcode;

namespace SuperSync.CodeGen.Data
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct ModuleReferences
    {
        public TypeReference NetworkBehaviourReference;
        public TypeReference NetworkBehaviour;
        public TypeReference NetworkObjectReference;
        public TypeReference NetworkObject;
        public TypeReference NetworkVariableTNone;
        public TypeReference AnticipatedNetworkVariableTNone;
        public TypeReference SyncUtils;

        public MethodReference NetworkBehaviourReference_opImplicit_retNetworkBehaviour;
        public MethodReference NetworkBehaviourReference_opImplicit_retNetworkBehaviourReference;

        public MethodReference NetworkObjectReference_opImplicit_retNetworkObject;
        public MethodReference NetworkObjectReference_opImplicit_retNetworkObjectReference;

        public MethodReference SyncUtils_GetVariableT1_retNetworkVariableT1;
        public MethodReference SyncUtils_GetAnticipatedVariableT1_retAnticipatedNetworkVariableT1;

        public static ModuleReferences Create(ModuleDefinition module)
        {
            var result = new ModuleReferences
            {
                NetworkBehaviourReference = module
                    .ImportReference(typeof(NetworkBehaviourReference)),
                NetworkObjectReference = module
                    .ImportReference(typeof(NetworkObjectReference)),
                NetworkVariableTNone = module
                    .ImportReference(typeof(NetworkVariable<>)),
                AnticipatedNetworkVariableTNone = module
                    .ImportReference(typeof(AnticipatedNetworkVariable<>)),
                SyncUtils = module
                    .ImportReference(typeof(SyncUtils))
            };

            result.SyncUtils_GetVariableT1_retNetworkVariableT1 = result
                .SyncUtils.Resolve()
                .Methods
                .FirstOrDefault(v => v.Name == "GetVariable");

            result.SyncUtils_GetAnticipatedVariableT1_retAnticipatedNetworkVariableT1 = result
                .SyncUtils.Resolve()
                .Methods
                .FirstOrDefault(v => v.Name == "GetAnticipatedVariable");

            foreach (var methodDefinition in result.NetworkBehaviourReference.Resolve().Methods
                         .Where(methodDefinition => methodDefinition.Name == "op_Implicit"))
            {
                if (methodDefinition.ReturnType.FullName == SymbolNames.UNGO_NetworkBehaviour)
                {
                    result.NetworkBehaviour = module.ImportReference(methodDefinition.ReturnType);
                    result.NetworkBehaviourReference_opImplicit_retNetworkBehaviour =
                        module.ImportReference(methodDefinition);
                }
                else if (methodDefinition.ReturnType.FullName == SymbolNames.UNGO_NetworkBehaviourReference)
                {
                    result.NetworkBehaviourReference_opImplicit_retNetworkBehaviourReference =
                        module.ImportReference(methodDefinition);
                }
            }

            foreach (var methodDefinition in result.NetworkObjectReference.Resolve().Methods
                         .Where(methodDefinition => methodDefinition.Name == "op_Implicit"))
            {
                if (methodDefinition.ReturnType.FullName == SymbolNames.UNGO_NetworkObject)
                {
                    result.NetworkObject = module.ImportReference(methodDefinition.ReturnType);
                    result.NetworkObjectReference_opImplicit_retNetworkObject =
                        module.ImportReference(methodDefinition);
                }
                else if (methodDefinition.ReturnType.FullName == SymbolNames.UNGO_NetworkObjectReference)
                {
                    result.NetworkObjectReference_opImplicit_retNetworkObjectReference =
                        module.ImportReference(methodDefinition);
                }
            }

            return result;
        }
    }
}