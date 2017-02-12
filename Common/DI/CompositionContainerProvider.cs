using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Compilation;

namespace Common.DI
{
    /// <summary>
    /// Предоставляет контейнер компоновки
    /// </summary>
    public static class CompositionContainerProvider
    {
        /// <summary>
        /// Регулярные выражения для фильтрации загруженных сборок
        /// </summary>
        private static string[] possibleAssemblyRegexes;
        private static List<ICompositionExport> exports;

        #region Загрузка экспортирующих классов

        private static void LoadExports()
        {
            var types = new List<Type>();
            var assemblies = GetLocalAssemblies();
            foreach (var assembly in assemblies)
            {
                AppendProvidersTypes(assembly, types);
            }
            exports = types.Select(t => (ICompositionExport)Activator.CreateInstance(t)).ToList();
        }

        private static void AppendProvidersTypes(Assembly assembly, List<Type> types)
        {
            if (assembly.IsDynamic)
            {
                return;
            }
            var type = typeof(ICompositionExport);
            var exportedTypes = TryGetExportedTypes(assembly);
            types.AddRange(exportedTypes.Where(t => !t.IsAbstract && type.IsAssignableFrom(t)));

        }

        private static Type[] TryGetExportedTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetExportedTypes();
            }
            catch (FileLoadException)
            {
                return new Type[0];
            }
        }

        /// <summary>
        /// Возвращает набор локальных (не внешних и не системных) загруженных сборок 
        /// </summary>
        /// <returns></returns>
        private static ICollection<Assembly> GetLocalAssemblies()
        {
            return BuildManager.GetReferencedAssemblies().OfType<Assembly>().Union(AppDomain.CurrentDomain.GetAssemblies()).Distinct().Where(assembly => possibleAssemblyRegexes.Any(prefix => Regex.IsMatch(assembly.FullName, prefix))).ToList();
        }

        #endregion

        /// <summary>
        /// Создает и инициализирует контейнер компоновки
        /// </summary>
        /// <returns>готовый контейнер</returns>
        public static ICompositionContainer CreateCompositionContainer(string[] possibleRegexes)
        {
            if (exports == null)
            {
                possibleAssemblyRegexes = possibleRegexes;
                LoadExports();
            }
            var container = new NinjectCompositionContainer();
            container.RegisterInstance<ICompositionContainer>(container);
            foreach (var compositionExport in exports)
            {
                compositionExport.RegisterExport(container);
            }

            return container;
        }
    }
}
