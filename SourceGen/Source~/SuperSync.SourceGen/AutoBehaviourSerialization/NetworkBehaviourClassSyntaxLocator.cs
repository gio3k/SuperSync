using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SuperSync.SourceGen.AutoBehaviourSerialization;

internal class NetworkBehaviourClassSyntaxLocator : ISyntaxReceiver
{
	public readonly List<ClassDeclarationSyntax> Classes = [];

	public void OnVisitSyntaxNode( SyntaxNode syntaxNode )
	{
		if ( syntaxNode is not ClassDeclarationSyntax classDeclarationSyntax )
			return;

		if ( classDeclarationSyntax.BaseList is null )
			return;

		// Don't allow template classes
		if ( classDeclarationSyntax.TypeParameterList != null )
			return;

		Classes.Add( classDeclarationSyntax );
	}
}
