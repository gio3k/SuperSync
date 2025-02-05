using System.Linq;
using Microsoft.CodeAnalysis;

namespace SuperSync.SourceGen.SyncAttributeDiagnostics;

public static class SyncAttributeExtensions
{
	public static bool HasSyncAttribute( this ISymbol fieldSymbol )
	{
		return fieldSymbol.GetAttributes()
			.Any( v => v.AttributeClass?.ToString() == SymbolNames.SuperSync_SyncAttribute );
	}

	public static bool CanSupportSyncAttribute( this INamedTypeSymbol namedTypeSymbol )
	{
		return namedTypeSymbol.GetBaseTypesAndThis()
			.Any( v => v.ToString() is SymbolNames.UNGO_NetworkBehaviour or SymbolNames.UNGO_NetworkObject );
	}
}
