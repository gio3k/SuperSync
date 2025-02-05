using System.Diagnostics.CodeAnalysis;

namespace SuperSync.SourceGen;

[SuppressMessage( "ReSharper", "InconsistentNaming" )]
public static class SymbolNames
{
	public const string SuperSync_SyncAttribute = "SuperSync.SyncAttribute";
	public const string SuperSync_AnticipateAttribute = "SuperSync.AnticipateAttribute";
	public const string UNGO_NetworkBehaviour = "Unity.Netcode.NetworkBehaviour";
	public const string UNGO_NetworkObject = "Unity.Netcode.NetworkObject";
	public const string UNGO_NetworkBehaviourReference = "Unity.Netcode.NetworkBehaviourReference";
	public const string UNGO_NetworkObjectReference = "Unity.Netcode.NetworkObjectReference";
	public const string UNGO_NetworkVariableT1 = "Unity.Netcode.NetworkVariable";
	public const string UNGO_AnticipatedNetworkVariableT1 = "Unity.Netcode.AnticipatedNetworkVariable";
}
