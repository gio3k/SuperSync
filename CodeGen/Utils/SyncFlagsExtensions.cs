using Unity.Netcode;

namespace SuperSync.CodeGen.Utils
{
    public static class SyncFlagsExtensions
    {
        public static NetworkVariableReadPermission ToReadPermission(this SyncFlags flags)
        {
            if (flags.HasFlag(SyncFlags.Private))
                return NetworkVariableReadPermission.Owner;
            return NetworkVariableReadPermission.Everyone;
        }

        public static NetworkVariableWritePermission ToWritePermission(this SyncFlags flags)
        {
            if (flags.HasFlag(SyncFlags.ServerWritableOnly))
                return NetworkVariableWritePermission.Server;
            return NetworkVariableWritePermission.Owner;
        }
    }
}