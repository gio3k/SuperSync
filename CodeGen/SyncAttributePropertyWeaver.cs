using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Mono.Cecil;
using Mono.Cecil.Cil;
using SuperSync.CodeGen.Data;
using Unity.CompilationPipeline.Common.ILPostProcessing;

namespace SuperSync.CodeGen
{
    public class SyncAttributePropertyWeaver : ILPostProcessor
    {
        public override ILPostProcessor GetInstance() => this;

        public override bool WillProcess(ICompiledAssembly compiledAssembly)
        {
            return !compiledAssembly.Name.StartsWith("Unity") &&
                   compiledAssembly.References.Any(v => v.EndsWith("SuperSync.Runtime.dll")) &&
                   compiledAssembly.References.Any(v => v.EndsWith("Unity.Netcode.Runtime.dll"));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ILPostProcessResult ProcessUnsafe(ICompiledAssembly compiledAssembly)
        {
            // Prepare our assembly resolver
            var resolver = new DefaultAssemblyResolver();
            foreach (var reference in compiledAssembly.References)
            {
                var directoryName = Path.GetDirectoryName(reference);
                if (resolver.GetSearchDirectories().Contains(directoryName))
                    continue;

                resolver.AddSearchDirectory(directoryName);
            }

            // Load the assembly
            var assemblyDefinition =
                AssemblyDefinition.ReadAssembly(
                    new MemoryStream(compiledAssembly.InMemoryAssembly.PeData),
                    new ReaderParameters { AssemblyResolver = resolver });

            foreach (var module in assemblyDefinition.Modules)
            {
                var moduleReferences = ModuleReferences.Create(module);

                foreach (var type in module.Types)
                {
                    var weavedProperties = new List<WeavedProperty>();

                    // 'Weave' (emit IL for) properties first
                    foreach (var property in type.Properties)
                    {
                        if (!PropertyWithSyncAttribute.TryCreate(property, out var propertyWithSyncAttribute))
                            continue;

                        weavedProperties.Add(CodeWeaver.WeaveProperty(type, propertyWithSyncAttribute, module,
                            moduleReferences));
                    }

                    // Weave constructor now that we have all the properties weaved
                    if (weavedProperties.Any())
                        CodeWeaver.WeaveConstructor(type, weavedProperties, module, moduleReferences);

                    // Weave replaceable types (like SyncUtils.GetVariable)
                    CodeWeaver.WeaveAllReplaceableCalls(type, weavedProperties, module, moduleReferences);
                }
            }

            // Save modified assembly
            using var peOutputStream = new MemoryStream();
            using var pdbOutputStream = new MemoryStream();

            var writerParameters = new WriterParameters
            {
                SymbolWriterProvider = new PortablePdbWriterProvider(),
                SymbolStream = pdbOutputStream,
                WriteSymbols = true
            };

            assemblyDefinition.Write(peOutputStream, writerParameters);

            return new ILPostProcessResult(new InMemoryAssembly(peOutputStream.ToArray(),
                pdbOutputStream.ToArray()));
        }

        public override ILPostProcessResult Process(ICompiledAssembly compiledAssembly)
        {
            try
            {
                return ProcessUnsafe(compiledAssembly);
            }
            catch (Exception e)
            {
                Logger.Log($"Failed to process assembly: {e}");
                return new ILPostProcessResult(compiledAssembly.InMemoryAssembly);
            }
        }
    }
}