using FluentValidation;
using ftrip.io.framework.Globalization;
using ftrip.io.notification_service.NotificationTypes;
using System;
using System.Linq;

namespace ftrip.io.notification_service.UserNotificationTypes.UseCases.Configure
{
    public class ConfigureRequestValidator : AbstractValidator<ConfigureRequest>
    {
        public ConfigureRequestValidator(IStringManager stringManager)
        {
            RuleFor(request => request.Codes)
                .NotNull()
                .WithMessage(stringManager.Format("Common_Validation_FieldIsRequired", "Codes"));

            RuleForEach(request => request.Codes)
                .Must(NotificationTypeCodes.All.Contains)
                .WithMessage((_, code) => stringManager.Format("UserNotificationTypes_Validation_CodeNotSupported", code));
        }
    }
}