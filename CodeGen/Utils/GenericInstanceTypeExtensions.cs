using Mono.Cecil;

namespace SuperSync.CodeGen.Utils
{
	public static class GenericInstanceTypeExtensions
	{
		public static MethodReference MakeGenericMethod( this GenericInstanceType self, TypeReference returnType,
			MethodReference methodToCopy )
		{
			var result = new MethodReference( methodToCopy.Name,
				returnType, self )
			{
				DeclaringType = self,
				CallingConvention = methodToCopy.CallingConvention,
				HasThis = methodToCopy.HasThis,
				ExplicitThis = methodToCopy.ExplicitThis,
			};
			foreach ( var genericDefinition in methodToCopy.GenericParameters )
			{
				result.GenericParameters.Add( genericDefinition );
			}

			foreach ( var parameterDefinition in methodToCopy.Parameters )
			{
				if ( !parameterDefinition.ParameterType.IsGenericParameter )
					parameterDefinition.ParameterType =
						self.Module.ImportReference( parameterDefinition.ParameterType );
				result.Parameters.Add( parameterDefinition );
			}

			return result;
		}
	}
}
