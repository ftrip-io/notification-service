using ftrip.io.framework.ExceptionHandling.Exceptions;
using ftrip.io.notification_service.Notifications;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace ftrip.io.notification_service.Attributes
{
    public class UserSpecificNotificationAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var notificationIdFromRoute = GetNotificationIdFromRoute(context);
            var notificationRepository = GetService<INotificationRepository>(context);
            var notification = await notificationRepository.Read(notificationIdFromRoute);

            var userIdFromRoute = GetUserIdFromRoute(context);
            if (userIdFromRoute != notification.UserId)
            {
                throw new ForbiddenException("Sorry, you are not allowed to perform this action!");
            }

            await next();
        }

        private string GetUserIdFromRoute(ActionExecutingContext context)
        {
            return GetParamFromRoute(context, "userId");
        }

        private string GetNotificationIdFromRoute(ActionExecutingContext context)
        {
            return GetParamFromRoute(context, "notificationId");
        }

        private string GetParamFromRoute(ActionExecutingContext context, string param)
        {
            return context.HttpContext.Request.RouteValues[param]?.ToString() ?? null;
        }

        private T GetService<T>(ActionExecutingContext context) where T : class => context.HttpContext.RequestServices.GetService(typeof(T)) as T;
    }
}