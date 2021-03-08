namespace ImageConverter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public interface ITypeLoader
    {
        List<Type> LoadAllTypes();
    }
    
    public class TypeLoader : ITypeLoader
    {
        public List<Type> LoadAllTypes()
        {
            LoadUnusedAssemblies();
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(ass => ass.GetTypes())
                .ToList();
        }

        private void LoadUnusedAssemblies()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            var loadedPaths = loadedAssemblies.Select(a => a.Location).ToArray();

            var referencedPaths = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll");
            var toLoad = referencedPaths
                .Where(r => !loadedPaths.Contains(r, StringComparer.InvariantCultureIgnoreCase))
                .ToList();
            
            foreach (var path in toLoad)
            {
                var assembly = AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(path));
                loadedAssemblies.Add(assembly);
            }
        }
    }
}