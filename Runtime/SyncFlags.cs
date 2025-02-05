using System;

namespace SuperSync
{
	[Flags]
	public enum SyncFlags
	{
		None = 0,

		/// <summary>
		/// Only the owner and server can read the value
		/// </summary>
		Private = 1 << 2,

		/// <summary>
		/// Only the server can write the value, not the owner
		/// </summary>
		ServerWritableOnly = 1 << 3,

		/// <summary>
		/// Don't call OnChange{property name}() when the value is
		/// </summary>
		NoChangeEvents = 1 << 4,
	}
}
