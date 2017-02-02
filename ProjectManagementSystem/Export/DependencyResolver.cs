using System;
using System.Collections.Generic;
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
        private readonly ICompositionContainer container;

        public PmsDependencyResolver()
        {
            this.container = CompositionContainerProvider.CreateCompositionContainer(new [] {"PMS", "Common", "Project"});
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
            return this.container.TryResolveInstance(serviceType);
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
            return this.container.ResolveInstances(serviceType).ToList();
        }
    }
}