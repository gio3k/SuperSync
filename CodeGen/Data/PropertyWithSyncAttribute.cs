using System.Linq;
using Mono.Cecil;
using SuperSync.CodeGen.Utils;

namespace SuperSync.CodeGen.Data
{
    /// <summary>
    /// Something that has the SyncAttribute on it
    /// </summary>
    public class PropertyWithSyncAttribute
    {
        public readonly PropertyDefinition Property;

        public readonly SyncFlags SyncFlags = 0;

        public readonly bool IsDerivedFromNetworkBehaviour;
        public readonly bool IsDerivedFromNetworkObject;

        public readonly bool ShouldCreateAnticipatedVariable = false;

        public bool RequiresWrapper => IsDerivedFromNetworkObject || IsDerivedFromNetworkBehaviour;

        public bool IsValueType => Property.PropertyType.IsValueType;

        public PropertyWithSyncAttribute(PropertyDefinition property, CustomAttribute attribute)
        {
            Property = property;

            var resolvedPropertyType = property.PropertyType.Resolve();

            IsDerivedFromNetworkBehaviour = resolvedPropertyType.IsDerivedFromOrIs("NetworkBehaviour");
            IsDerivedFromNetworkObject = resolvedPropertyType.IsDerivedFromOrIs("NetworkObject");

            // Check for sibling attributes
            if (property.CustomAttributes.FirstOrDefault(v =>
                    v.AttributeType.FullName == SymbolNames.SuperSync_AnticipateAttribute) is not null)
            {
                ShouldCreateAnticipatedVariable = true;
            }

            // Handle attribute ctor arguments
            if (attribute.HasConstructorArguments)
            {
                SyncFlags = (SyncFlags)(int)attribute.ConstructorArguments.First().Value;
            }

            // Handle attribute properties
            foreach (var customAttributeNamedArgument in attribute.Properties.Where(customAttributeNamedArgument =>
                         customAttributeNamedArgument.Name == "Flags"))
            {
                SyncFlags = (SyncFlags)(int)customAttributeNamedArgument.Argument.Value;
            }
        }

        public static bool TryCreate(PropertyDefinition property,
            out PropertyWithSyncAttribute propertyWithSyncAttribute)
        {
            if (!property.HasCustomAttributes)
            {
                propertyWithSyncAttribute = null;
                return false;
            }

            if (property.CustomAttributes.FirstOrDefault(v =>
                    v.AttributeType.FullName == SymbolNames.SuperSync_SyncAttribute) is not { } syncAttribute)
            {
                propertyWithSyncAttribute = null;
                return false;
            }

            propertyWithSyncAttribute =
                new PropertyWithSyncAttribute(property, syncAttribute);
            return true;
        }

        public MethodReference GetWrapperConversionFunctionReturningPropertyType(ModuleReferences moduleReferences)
        {
            if (IsDerivedFromNetworkBehaviour)
                return moduleReferences.NetworkBehaviourReference_opImplicit_retNetworkBehaviour;
            if (IsDerivedFromNetworkObject)
                return moduleReferences.NetworkObjectReference_opImplicit_retNetworkObject;
            return null;
        }

        public MethodReference GetWrapperConversionFunctionReturningWrapperType(ModuleReferences moduleReferences)
        {
            if (IsDerivedFromNetworkBehaviour)
                return moduleReferences.NetworkBehaviourReference_opImplicit_retNetworkBehaviourReference;
            if (IsDerivedFromNetworkObject)
                return moduleReferences.NetworkObjectReference_opImplicit_retNetworkObjectReference;
            return null;
        }

        public TypeReference GetWrapperType(ModuleReferences moduleReferences)
        {
            if (IsDerivedFromNetworkBehaviour)
                return moduleReferences.NetworkBehaviourReference;
            if (IsDerivedFromNetworkObject)
                return moduleReferences.NetworkObjectReference;
            return null;
        }
    }
}