using System;

namespace SuperSync
{
	[AttributeUsage( AttributeTargets.Property )]
	public class SyncAttribute : Attribute
	{
		public SyncAttribute()
		{
		}

		public SyncAttribute( SyncFlags flags )
		{
			Flags = flags;
		}

		public SyncFlags Flags { get; set; }
	}
}
