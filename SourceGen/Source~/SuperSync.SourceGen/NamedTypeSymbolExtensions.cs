using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace SuperSync.SourceGen;

public static class NamedTypeSymbolExtensions
{
	public static IEnumerable<INamedTypeSymbol> GetBaseTypesAndThis( this INamedTypeSymbol type )
	{
		// https://github.com/dotnet/roslyn-analyzers/blob/3746107f6c9e0a2f1cae7e37a78a9d9de249857a/src/Utilities/Compiler/Extensions/INamedTypeSymbolExtensions.cs#L22
		var current = type;
		while ( current != null )
		{
			yield return current;
			current = current.BaseType;
		}
	}

	public static IEnumerable<INamedTypeSymbol> GetBaseTypes( this INamedTypeSymbol type )
	{
		// https://github.com/dotnet/roslyn-analyzers/blob/3746107f6c9e0a2f1cae7e37a78a9d9de249857a/src/Utilities/Compiler/Extensions/INamedTypeSymbolExtensions.cs#L22
		var current = type.BaseType;
		while ( current != null )
		{
			yield return current;
			current = current.BaseType;
		}
	}
}
