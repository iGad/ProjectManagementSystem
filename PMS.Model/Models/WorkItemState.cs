using Common;

namespace PMS.Model.Models
{
    public enum WorkItemState
    {
        /// <summary>
        /// Элемент создан, но никому не назначен
        /// </summary>
        [LocalizedDescription("StateNew", typeof(Resources))]
        New = 0,
        /// <summary>
        /// Элемент назначен, но еще не взят в работу
        /// </summary>
        [LocalizedDescription("StatePlanned", typeof(Resources))]
        Planned = 1,
        /// <summary>
        /// Работчий элемент завершен
        /// </summary>
        [LocalizedDescription("StateDone", typeof(Resources))]
        Done = 2,
        /// <summary>
        /// Элемент удален
        /// </summary>
        Deleted = 3,
        /// <summary>
        /// Элемент помещен в архив
        /// </summary>
        [LocalizedDescription("StateArchive", typeof(Resources))]
        Archive = 4,
        /// <summary>
        /// Элемент на проверке
        /// </summary>
        [LocalizedDescription("StateReviewing", typeof(Resources))]
        Reviewing = 5,
        /// <summary>
        /// Элемент в работе
        /// </summary>
        [LocalizedDescription("StateAtWork", typeof(Resources))]
        AtWork = 6
    }
}
