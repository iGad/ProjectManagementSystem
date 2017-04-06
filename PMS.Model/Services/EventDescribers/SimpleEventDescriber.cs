using PMS.Model.Models;

namespace PMS.Model.Services.EventDescribers
{
    /// <summary>
    /// Базовый класс для описателей с одинаковым форматом события "Пользователь И О {тест действия} описание элемента"
    /// </summary>
    public abstract class SimpleEventDescriber : EventDescriber
    {
        private readonly string actionString, currentUserActionString;
        private readonly WorkItemService workItemService;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="workItemService"></param>
        /// <param name="actionString">Строка действия для обычного пользователя</param>
        /// <param name="currentUserActionString">Строка действия для текущего пользователя</param>
        protected SimpleEventDescriber(WorkItemService workItemService)
        {
            this.workItemService = workItemService;
        }

        /// <summary>
        /// Внутренний метод получения описания события
        /// </summary>
        /// <param name="workEvent">Событие</param>
        /// <param name="forUser">Пользователь, для которого создается описание</param>
        /// <returns></returns>
        protected override string GetDescription(WorkEvent workEvent, ApplicationUser forUser)
        {
            var item = this.workItemService.Get(workEvent.ObjectId.Value);
            return $"{GetStartText(forUser)} {GetActionString()} {LexicalHelper.GetWorkItemTypeInCase(item.Type, "a")} {item.GetWorkItemIdentityText()}.";
        }

        protected abstract string GetActionString();
    }
}
