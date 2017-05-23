using PMS.Model.Models;

namespace PMS.Model.Services
{
    /// <summary>
    /// Базовый класс для получения описания на ЕЯ для события
    /// </summary>
    public abstract class EventDescriber
    {
        protected bool IsCurrentUser { get; private set; }
        protected bool IsUserAuthor { get; private set; }
        /// <summary>
        /// Проверка на то, может ли конкретный класс описать событие определенного типа
        /// </summary>
        /// <param name="eventType">Тип события</param>
        /// <returns></returns>
        public abstract bool CanDescribeEventType(EventType eventType);
        protected ApplicationUser CurrentUser { get; private set; }
        /// <summary>
        /// Получить описание события
        /// </summary>
        /// <param name="workEvent">Событие</param>
        /// <param name="currentUser">Текущий пользователь</param>
        /// <returns></returns>
        public string DescribeEvent(WorkEvent workEvent, ApplicationUser forUser, ApplicationUser currentUser)
        {
            if (!CanDescribeEventType(workEvent.Type))
                throw new PmsException($"Can not describe event with type {workEvent.Type}");
            IsCurrentUser = currentUser.Id == forUser.Id;
            IsUserAuthor = forUser.Id == workEvent.UserId;
            CurrentUser = currentUser;
            return GetDescription(workEvent, forUser);
        }

        /// <summary>
        /// Внутренний метод получения описания события
        /// </summary>
        /// <param name="workEvent">Событие</param>
        /// <param name="forUser">Пользователь, для которого создается описание</param>
        /// <returns></returns>
        protected abstract string GetDescription(WorkEvent workEvent, ApplicationUser forUser);
        /// <summary>
        /// Получить текст Пользователь "ИО" или Вы в зависимости от того, какому пользователю создается описание
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected string GetStartText(ApplicationUser user)
        {
            return IsUserAuthor ? NotificationResources.You : (NotificationResources.User + " " + user.GetUserIdentityText());
        }
    }
}
