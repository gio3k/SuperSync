using System.Collections.Immutable;
using Microsoft.CodeAnalysis;

namespace SuperSync.SourceGen.SyncAttributeDiagnostics;

public static class SyncAttributeDiagnosticDescriptors
{
	public static readonly DiagnosticDescriptor WarningUnsupportedType = new(
		"SPSC0010",
		"Type is not networkable",
		"[Sync] property '{0}' in non-networkable type",
		"SuperSync",
		DiagnosticSeverity.Warning,
		isEnabledByDefault: true,
		description:
		"The type containing this property doesn't support NetworkVariables. Please refer to Unity Netcode documentation for more details.");

	public static readonly DiagnosticDescriptor ErrorPropertyInvalid = new(
		"SPSC0020",
		"Property {0} is invalid",
		"[Sync] property '{0}' can't have a getter or setter",
		"SuperSync",
		DiagnosticSeverity.Error,
		isEnabledByDefault: true,
		description:
		"Properties with the [Sync] attribute can't have a getter or setter.");

	public static ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =
		ImmutableArray.Create( WarningUnsupportedType, ErrorPropertyInvalid );
}
