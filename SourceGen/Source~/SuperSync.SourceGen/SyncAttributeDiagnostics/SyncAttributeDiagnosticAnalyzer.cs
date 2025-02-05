using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace SuperSync.SourceGen.SyncAttributeDiagnostics;

public static class Logger
{
	public static void Log( string message )
	{
		var p =
			"/Users/gio/dev/projects/gamedev/unity-projects/NetcodeTesting/Assets/Libraries/SuperSync/SourceGen/Source~/SuperSync.SourceGen/test.txt";
#pragma warning disable RS1035
		File.AppendAllText( p, $"{message}\n" );
#pragma warning restore RS1035
	}
}

[DiagnosticAnalyzer( LanguageNames.CSharp )]
public class SyncAttributeDiagnosticAnalyzer : DiagnosticAnalyzer
{
	private static void HandlePropertySymbolAction( SymbolAnalysisContext ctx )
	{
		var propertySymbol = (IPropertySymbol)ctx.Symbol;

		if ( !propertySymbol.HasSyncAttribute() )
			return;

		if ( propertySymbol.ContainingType is not { } containingType )
			return;

		if ( !containingType.CanSupportSyncAttribute() )
		{
			ctx.ReportDiagnostic( Diagnostic.Create( SyncAttributeDiagnosticDescriptors.WarningUnsupportedType,
				propertySymbol.Locations.First(), propertySymbol.Name ) );
			return;
		}

		if ( !propertySymbol.HasGeneratedBackingField() )
		{
			ctx.ReportDiagnostic( Diagnostic.Create( SyncAttributeDiagnosticDescriptors.ErrorPropertyInvalid,
				propertySymbol.Locations.First(), propertySymbol.Name ) );
			return;
		}
	}

	public override void Initialize( AnalysisContext ctx )
	{
		ctx.ConfigureGeneratedCodeAnalysis( GeneratedCodeAnalysisFlags.Analyze );
		ctx.EnableConcurrentExecution();

		ctx.RegisterSymbolAction( HandlePropertySymbolAction, SymbolKind.Property );
	}

	public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics =>
		SyncAttributeDiagnosticDescriptors.SupportedDiagnostics;
}
