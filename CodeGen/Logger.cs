using System.IO;

namespace SuperSync.CodeGen
{
	public static class Logger
	{
		public static void Log( string message )
		{
			// note: can't really log to a console in unity ILPP codegen, so I just log to a file
			const string p = "/Users/gio/dev/projects/gamedev/unity-projects/NetcodeTesting/log.txt";
			File.AppendAllText( p, $"{message}\n" );
		}
	}
}
