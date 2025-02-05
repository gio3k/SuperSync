using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Mono.Cecil;
using Unity.Netcode;

namespace SuperSync.CodeGen.Data
{
	[SuppressMessage( "ReSharper", "InconsistentNaming" )]
	public struct ModuleReferences
	{
		public TypeReference NetworkBehaviourReference;
		public TypeReference NetworkBehaviour;
		public TypeReference NetworkObjectReference;
		public TypeReference NetworkObject;
		public TypeReference NetworkVariableTNone;

		public MethodReference NetworkBehaviourReference_opImplicit_retNetworkBehaviour;
		public MethodReference NetworkBehaviourReference_opImplicit_retNetworkBehaviourReference;

		public MethodReference NetworkObjectReference_opImplicit_retNetworkObject;
		public MethodReference NetworkObjectReference_opImplicit_retNetworkObjectReference;

		public static ModuleReferences Create( ModuleDefinition module )
		{
			var result = new ModuleReferences();

			result.NetworkBehaviourReference = module
				.ImportReference( typeof(NetworkBehaviourReference) );

			foreach ( var methodDefinition in result.NetworkBehaviourReference.Resolve().Methods
				         .Where( methodDefinition => methodDefinition.Name == "op_Implicit" ) )
			{
				if ( methodDefinition.ReturnType.Name == "NetworkBehaviour" )
				{
					result.NetworkBehaviour = module.ImportReference( methodDefinition.ReturnType );
					result.NetworkBehaviourReference_opImplicit_retNetworkBehaviour =
						module.ImportReference( methodDefinition );
				}
				else if ( methodDefinition.ReturnType.Name == "NetworkBehaviourReference" )
				{
					result.NetworkBehaviourReference_opImplicit_retNetworkBehaviourReference =
						module.ImportReference( methodDefinition );
				}
			}

			result.NetworkObjectReference = module
				.ImportReference( typeof(NetworkObjectReference) );

			foreach ( var methodDefinition in result.NetworkObjectReference.Resolve().Methods
				         .Where( methodDefinition => methodDefinition.Name == "op_Implicit" ) )
			{
				if ( methodDefinition.ReturnType.Name == "NetworkObject" )
				{
					result.NetworkObject = module.ImportReference( methodDefinition.ReturnType );
					result.NetworkObjectReference_opImplicit_retNetworkObject =
						module.ImportReference( methodDefinition );
				}
				else if ( methodDefinition.ReturnType.Name == "NetworkObjectReference" )
				{
					result.NetworkObjectReference_opImplicit_retNetworkObjectReference =
						module.ImportReference( methodDefinition );
				}
			}

			result.NetworkVariableTNone = module
				.ImportReference( typeof(NetworkVariable<>) );

			return result;
		}
	}
}
