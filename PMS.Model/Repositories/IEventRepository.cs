using System;
using System.Collections.Generic;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<WorkEvent> GetEvents(Func<WorkEvent, bool> filter);
        WorkEvent AddEvent(WorkEvent workEvent);
        WorkEvent Get(int id);
        IEnumerable<Tuple<WorkEvent, WorkEventUserRelation>> GetEventsForUser(string userId);
        IEnumerable<EventUserModel> GetEventsForUser(string userId, EventFilterModel filterModel);
        int GetTotalEventsForUserCount(string userId, EventFilterModel filterModel);
        void AddWorkEventRelation(WorkEventUserRelation relation);
        WorkEventUserRelation GetRelation(int eventId, string userId);
        int SaveChanges();
    }
}