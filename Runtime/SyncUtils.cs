using System;
using System.Runtime.CompilerServices;
using Unity.Netcode;

namespace SuperSync
{
    public static class SyncUtils
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static NetworkVariable<T> GetVariable<T>(T property)
        {
            throw new InvalidOperationException("GetVariable was used in an invalid way.");
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static AnticipatedNetworkVariable<T> GetAnticipatedVariable<T>(T property)
        {
            throw new InvalidOperationException("GetAnticipatedVariable was used in an invalid way.");
        }
    }
}