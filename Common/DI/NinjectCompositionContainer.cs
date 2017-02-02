using System;
using System.Collections.Generic;
using System.Linq;
using Ninject;
using Ninject.Web.Common;

namespace Common.DI
{
    /// <summary>
    /// Обертка для DI-контейнера на базе Ninject
    /// </summary>
    public class NinjectCompositionContainer : IDisposable, ICompositionContainer
    {
        /// <summary>
        /// Сам Ninject контейнер
        /// </summary>
        public IKernel Container { get; private set; }

        /// <summary>
        /// Создает и инициализирует контейнер
        /// </summary>
        public NinjectCompositionContainer()
        {
            Container = new StandardKernel();
        }

        /// <summary>
        /// Регистрирует public тип TExport как сервис 
        /// </summary>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        public virtual void RegisterExport<TExport>()
            where TExport : class
        {
            Container.Bind<TExport>().ToSelf();
        }
        

        /// <summary>
        /// Регистрирует public тип TRegister как сервис TExport
        /// </summary>
        /// <typeparam name="TRegister">Регистрируемый тип </typeparam>
        /// <typeparam name="TExport">Тип экспорта</typeparam>
        public virtual void RegisterExport<TRegister, TExport>()
            where TRegister : TExport
            where TExport : class
        {
            Container.Bind<TExport>().To<TRegister>();
        }

        /// <summary>
        /// Регистрирует экземляр типа public TInstance как сервис 
        /// </summary>
        /// <typeparam name="TInstance">Тип сервиса</typeparam>
        /// <param name="instance">Экземпляр типа TInstance</param>
        public virtual void RegisterInstance<TInstance>(TInstance instance)
            where TInstance : class
        {
            Container.Bind<TInstance>().ToConstant(instance);
        }

        /// <summary>
        /// Регистрирует экземляр типа public TExport как сервис TExport
        /// </summary>
        /// <typeparam name="TInstance">Тип экземпляра</typeparam>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        /// <param name="instance">Экземпляр типа TInstance</param>
        public virtual void RegisterInstance<TInstance, TExport>(TInstance instance)
            where TInstance : TExport
            where TExport : class
        {
            Container.Bind<TExport>().ToConstant(instance);
        }

        /// <summary>
        /// Возвращает зарегистрированный как TExport. 
        /// </summary>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        /// <returns>Экземпляр</returns>
        public TExport ResolveInstance<TExport>()
            where TExport : class
        {
            try
            {
                return Container.Get<TExport>();
            }
            catch (ActivationException e)
            {
                throw new DependencyInjectionException("Error while resolving instance", e);
            }
        }

        /// <summary>
        /// Возвращает объект по заданому типу 
        /// </summary>
        /// <param name="exportType">Тип зарегистрированного объекта</param>
        /// <returns>Экземпляр</returns>
        public object ResolveInstance(Type exportType)
        {
            try
            {
                return Container.Get(exportType);
            }
            catch (ActivationException e)
            {
                throw new DependencyInjectionException("Error while resolving instance", e);
            }
        }

        /// <summary>
        /// Возвращает список зарегистрированных как TExport объектов. 
        /// </summary>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        /// <returns>Набор экземпляров</returns>
        public IList<TExport> ResolveInstances<TExport>()
            where TExport : class
        {
            try
            {
                return Container.GetAll<TExport>().ToArray();
            }
            catch (ActivationException e)
            {
                throw new DependencyInjectionException("Error while resolving instances", e);
            }
        }

        /// <summary>
        /// Возвращает набор объектов по заданому типу 
        /// </summary>
        /// <param name="exportType">Тип зарегистрированного объекта</param>
        /// <returns>Набор экземплюров</returns>
        public IList<object> ResolveInstances(Type exportType)
        {
            try
            {
                return Container.GetAll(exportType).ToArray();
            }
            catch (ActivationException e)
            {
                throw new DependencyInjectionException("Error while resolving instances", e);
            }
        }

        /// <summary>
        /// Освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if (Container != null)
            {
                Container.Dispose();
                Container = null;
            }
        }

        /// <summary>
        /// Возвращает экземепляр зарегистрированного сервиса. Если разрешить невозможно - возвращает null.
        /// </summary>
        /// <param name="exportType">Тип зарегистрированной зависимости</param>
        /// <returns>Экземпляр или null, если зависимость не удалось разрешить</returns>
        public object TryResolveInstance(Type exportType)
        {
            return Container.TryGet(exportType);
        }

        /// <summary>
        /// Регистрирует тип TRegister как сервис TExport в скоупе запроса
        /// </summary>
        /// <typeparam name="TRegister">Регистрируемый тип </typeparam>
        /// <typeparam name="TExport">Тип экспорта</typeparam>
        public void RegisterExportInRequestScope<TRegister, TExport>()
            where TRegister : TExport
            where TExport : class
        {
            Container.Bind<TExport>().To<TRegister>().InRequestScope();
        }
    }
}
