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
        public EventsApiService(IEventRepository repository, IUserRepository userRepository, ICurrentUsernameProvider currentUsernameProvider, EventDescriber[] describers) 
            : base(repository, userRepository, currentUsernameProvider, describers)
        {
        }
        
        public void ChangeEventIsFavorite(int eventId, bool isFavorite)
        {
            var user = GetCurrentUser();
            ChangeUserEventIsFavorite(eventId, user.Id, isFavorite);
        }

        public void ChangeEventState(int eventId, EventState state)
        {
            ChangeUserEventState(eventId, GetCurrentUser().Id, state);
        }
    }
}