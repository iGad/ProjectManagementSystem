using System;
using System.Collections.Generic;

namespace Common.DI
{
    /// <summary>
    /// Интерфейс DI-контейнера
    /// </summary>
    public interface ICompositionContainer
    {
        /// <summary>
        /// Регистрирует тип TExport как сервис 
        /// </summary>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        void RegisterExport<TExport>()
            where TExport : class;

        /// <summary>
        /// Регистрирует тип TRegister как сервис TExport
        /// </summary>
        /// <typeparam name="TRegister">Регистрируемый тип </typeparam>
        /// <typeparam name="TExport">Тип экспорта</typeparam>
        void RegisterExport<TRegister, TExport>()
            where TRegister : TExport
            where TExport : class;

        /// <summary>
        /// Регистрирует тип TRegister как сервис TExport в скоупе запроса
        /// </summary>
        /// <typeparam name="TRegister">Регистрируемый тип </typeparam>
        /// <typeparam name="TExport">Тип экспорта</typeparam>
        void RegisterExportInRequestScope<TRegister, TExport>()
            where TRegister : TExport
            where TExport : class;

        /// <summary>
        /// Регистрирует экземляр типа TInstance как сервис 
        /// </summary>
        /// <typeparam name="TInstance">Тип сервиса</typeparam>
        /// <param name="instance">Экземпляр типа TInstance</param>
        void RegisterInstance<TInstance>(TInstance instance)
            where TInstance : class;

        /// <summary>
        /// Регистрирует экземляр типа TExport как сервис TExport
        /// </summary>
        /// <typeparam name="TInstance">Тип экземпляра</typeparam>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        /// <param name="instance">Экземпляр типа TInstance</param>
        void RegisterInstance<TInstance, TExport>(TInstance instance)
            where TInstance : TExport
            where TExport : class;

        /// <summary>
        /// Возвращает зарегистрированный сервис (синглтон) типа TExport. 
        /// </summary>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        /// <returns>Экземпляр сервиса (синглтон)</returns>
        /// <exception cref="DependencyInjectionException">Возникает в случае, если не удалось разрешить зависимость</exception>
        TExport ResolveInstance<TExport>()
            where TExport : class;

        /// <summary>
        /// Возвращает объект по заданому типу 
        /// </summary>
        /// <param name="exportType">Тип зарегистрированного объекта</param>
        /// <returns>Экземпляр</returns>
        /// <exception cref="DependencyInjectionException">Возникает в случае, если не удалось разрешить зависимость</exception>
        object ResolveInstance(Type exportType);

        /// <summary>
        /// Возвращает экземепляр зарегистрированного сервиса. Если разрешить невозможно - возвращает null.
        /// </summary>
        /// <param name="exportType">Тип зарегистрированной зависимости</param>
        /// <returns>Экземпляр или null, если зависимость не удалось разрешить</returns>
        object TryResolveInstance(Type exportType);

        /// <summary>
        /// Возвращает список зарегистрированных как TExport объектов. 
        /// </summary>
        /// <typeparam name="TExport">Тип сервиса</typeparam>
        /// <returns>Набор экземпляров</returns>
        IList<TExport> ResolveInstances<TExport>()
            where TExport : class;

        /// <summary>
        /// Возвращает набор объектов по заданому типу 
        /// </summary>
        /// <param name="exportType">Тип зарегистрированного объекта</param>
        /// <returns>Набор экземплюров</returns>
        IList<object> ResolveInstances(Type exportType);
    }
}
