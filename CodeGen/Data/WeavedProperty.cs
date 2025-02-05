using Mono.Cecil;
using Mono.Cecil.Cil;

namespace SuperSync.CodeGen.Data
{
	public struct WeavedProperty
	{
		public PropertyWithSyncAttribute PropertyWithSyncAttribute;

		public NetworkVariableReferences NetworkVariableReferences;

		/// <summary>
		/// The backing field / NetworkVariable we created
		/// </summary>
		public FieldDefinition WeavedBackingField;

		/// <summary>
		/// The backing field created by the compiler
		/// </summary>
		public FieldReference NativeBackingField;

		/// <summary>
		/// The instructions setting the native backing field in the constructor.
		/// This is used to set the default value of the property usually - we need to transform these
		/// </summary>
		public Instruction[] NativeBackingFieldCtorBody;
	}
}
