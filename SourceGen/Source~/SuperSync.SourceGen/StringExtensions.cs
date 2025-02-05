using System.Text.RegularExpressions;

namespace SuperSync.SourceGen;

public static class StringExtensions
{
	public static string ToSafeGeneratedName( this string name ) => Regex.Replace( name.Trim(), "[^A-Za-z0-9_]", "_" );
}
