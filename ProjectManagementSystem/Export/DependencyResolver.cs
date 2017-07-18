using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Common.DI;

namespace ProjectManagementSystem.Export
{
    /// <summary>
    /// Свой DependencyResolver вместо стандартного. Позволяет использовать зависимости в конструкторе контроллера
    /// </summary>
    public class PmsDependencyResolver : IDependencyResolver
    {
        private readonly ICompositionContainer _container;

        public PmsDependencyResolver()
        {
            _container = CompositionContainerProvider.CreateCompositionContainer(new [] {"PMS", "Common", "ProjectManagementSystem"});
        }

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        /// <param name="serviceType">The type of the requested service or object.</param>
        public object GetService(Type serviceType)
        {
            try
            {
                return _container.ResolveInstance(serviceType);
            }
            catch (DependencyInjectionException e)
            {
                Debugger.Log(1000, "DI", e.ToString());
                return null;
            }
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <returns>
        /// The requested services.
        /// </returns>
        /// <param name="serviceType">The type of the requested services.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _container.ResolveInstances(serviceType);
            }
            catch (DependencyInjectionException e)
            {
                Debugger.Log(1000, "DI", e.ToString());
                return null;
            }
        }
    }
}