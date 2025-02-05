using System.Linq;
using Unity.Netcode;

namespace SuperSync.Serialization
{
    public static class NetworkObjectSerializationExtensions
    {
        public static void ReadValueSafe(this FastBufferReader reader, out NetworkObject networkObject)
        {
            reader.ReadValueSafe(out ulong networkObjectId);

            if (networkObjectId == ulong.MaxValue)
            {
                networkObject = null;
                return;
            }

            NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(networkObjectId, out networkObject);
        }

        public static void WriteValueSafe(this FastBufferWriter writer, in NetworkObject networkObject)
        {
            writer.WriteValueSafe(networkObject?.NetworkObjectId ?? ulong.MaxValue);
        }

        public static void SerializeValue<TReaderWriter>(this BufferSerializer<TReaderWriter> serializer,
            ref NetworkObject networkObject) where TReaderWriter : IReaderWriter
        {
            if (serializer.IsReader)
            {
                serializer.GetFastBufferReader().ReadValueSafe(out NetworkObject found);
                networkObject = found;
            }
            else
                serializer.GetFastBufferWriter().WriteValueSafe(networkObject);
        }

        public static void ReadValueSafe(this FastBufferReader reader, out NetworkBehaviour networkBehaviour)
        {
            reader.ReadValueSafe(out NetworkObject networkObject);
            reader.ReadValueSafe(out ushort networkBehaviourId);

            if (networkObject == null || networkBehaviourId == ushort.MaxValue)
            {
                networkBehaviour = null;
                return;
            }

            networkBehaviour = networkObject.GetComponents<NetworkBehaviour>()
                .FirstOrDefault(v => v.NetworkBehaviourId == networkBehaviourId);
        }

        public static void WriteValueSafe(this FastBufferWriter writer, in NetworkBehaviour networkBehaviour)
        {
            writer.WriteValueSafe(networkBehaviour.NetworkObject);
            writer.WriteValueSafe(networkBehaviour.NetworkBehaviourId);
        }

        public static void SerializeValue<TReaderWriter>(this BufferSerializer<TReaderWriter> serializer,
            ref NetworkBehaviour networkBehaviour) where TReaderWriter : IReaderWriter
        {
            if (serializer.IsReader)
            {
                serializer.GetFastBufferReader().ReadValueSafe(out NetworkBehaviour found);
                networkBehaviour = found;
            }
            else
                serializer.GetFastBufferWriter().WriteValueSafe(networkBehaviour);
        }
    }
}