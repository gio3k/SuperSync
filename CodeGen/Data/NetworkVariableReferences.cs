using System.Linq;
using Mono.Cecil;
using SuperSync.CodeGen.Utils;

namespace SuperSync.CodeGen.Data
{
	public struct NetworkVariableReferences
	{
		public readonly MethodReference Constructor;
		public readonly MethodReference Getter;
		public readonly MethodReference Setter;

		public readonly GenericInstanceType NetworkVariableWithT1;
		public readonly TypeReference NetworkVariableWithT1Reference;
		public readonly TypeReference T1TypeReference;

		public NetworkVariableReferences(
			ModuleDefinition module,
			ModuleReferences moduleReferences,
			TypeReference t1 )
		{
			// Create a type reference to NetworkVariable<> and resolve it
			T1TypeReference = module.ImportReference( t1 );

			// Create a type reference to NetworkVariable<T>
			NetworkVariableWithT1 = new GenericInstanceType( moduleReferences.NetworkVariableTNone );
			NetworkVariableWithT1.GenericArguments.Add( T1TypeReference );

			var typeWithT1Resolved = NetworkVariableWithT1.Resolve();

			Getter = typeWithT1Resolved
				.Methods.FirstOrDefault( v => v.Name == "get_Value" );
			Getter!.DeclaringType = NetworkVariableWithT1;
			Getter = NetworkVariableWithT1.MakeGenericMethod( Getter.ReturnType, Getter );

			Setter = typeWithT1Resolved.Methods.FirstOrDefault(
				v => v.Name == "set_Value" );
			Setter!.DeclaringType = NetworkVariableWithT1;
			Setter = NetworkVariableWithT1.MakeGenericMethod( module.TypeSystem.Void, Setter );

			Constructor = typeWithT1Resolved.Methods.FirstOrDefault(
				v => v.Name == ".ctor" );
			Constructor!.DeclaringType = NetworkVariableWithT1;
			Constructor = NetworkVariableWithT1.MakeGenericMethod( module.TypeSystem.Void, Constructor );

			NetworkVariableWithT1Reference = module.ImportReference( NetworkVariableWithT1 );
		}
	}
}
