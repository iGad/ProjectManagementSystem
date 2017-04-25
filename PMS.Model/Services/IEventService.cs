using System.Collections.Generic;
using PMS.Model.CommonModels;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public interface IEventService
    {
        WorkEvent AddEvent(WorkEvent workEvent, IEnumerable<string> usersIds);
        void ChangeUserEventState(int workEventId, string userId, EventState newState);
        void ChangeUserEventIsFavorite(int workEventId, string userId, bool isFavorite);
        EventDisplayModel GetEventDisplayModel(EventUserModel eventUserModel, ApplicationUser user);
        string GetEventDescription(WorkEvent workEvent, ApplicationUser forUser);
    }
}