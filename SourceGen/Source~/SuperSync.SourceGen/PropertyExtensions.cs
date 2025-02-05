using System.Linq;
using Microsoft.CodeAnalysis;

namespace SuperSync.SourceGen;

public static class PropertyExtensions
{
	public static bool HasGeneratedBackingField( this IPropertySymbol propertySymbol )
	{
		return propertySymbol.ContainingType.GetMembers().Any( v =>
			v.Kind == SymbolKind.Field && v is IFieldSymbol fieldSymbol &&
			SymbolEqualityComparer.Default.Equals( fieldSymbol.AssociatedSymbol, propertySymbol ) );
	}
}
