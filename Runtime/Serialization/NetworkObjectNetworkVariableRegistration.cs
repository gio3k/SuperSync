using Unity.Netcode;
using UnityEngine;

namespace SuperSync.Serialization
{
	public class NetworkObjectNetworkVariableRegistration
	{
		[RuntimeInitializeOnLoadMethod]
		public static void OnRuntimeMethodLoad()
		{
			UserNetworkVariableSerialization<NetworkObject>.WriteValue =
				NetworkObjectSerializationExtensions.WriteValueSafe;
			UserNetworkVariableSerialization<NetworkObject>.ReadValue =
				NetworkObjectSerializationExtensions.ReadValueSafe;
			UserNetworkVariableSerialization<NetworkBehaviour>.WriteValue =
				NetworkObjectSerializationExtensions.WriteValueSafe;
			UserNetworkVariableSerialization<NetworkBehaviour>.ReadValue =
				NetworkObjectSerializationExtensions.ReadValueSafe;
		}
	}
}
