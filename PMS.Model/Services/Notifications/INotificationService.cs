using PMS.Model.Models;

namespace PMS.Model.Services.Notifications
{
    public interface INotificationService
    {
        /// <summary>
        /// �������� � ������� (������� ��� ��������� �����������)
        /// </summary>
        /// <param name="event">������������ �������</param>
        void SendEventNotifications(WorkEvent @event);
    }
}