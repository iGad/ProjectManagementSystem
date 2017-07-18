using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Services;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;

namespace ProjectManagementSystem.Services
{
    public class EventsApiService : EventService
    {
        public EventsApiService(IEventRepository repository, IUsersService userRepository, EventDescriber[] describers) 
            : base(repository, userRepository, describers)
        {
        }
        
        public void ChangeEventIsFavorite(int eventId, bool isFavorite)
        {
            var user = GetCurrentUser();
            ChangeUserEventIsFavorite(eventId, user.Id, isFavorite);
        }

        public void ChangeEventsState(int[] eventIds, EventState state)
        {
            foreach (var eventId in eventIds)
            {
                ChangeUserEventState(eventId, GetCurrentUser().Id, state);
            }
        }
    }
}