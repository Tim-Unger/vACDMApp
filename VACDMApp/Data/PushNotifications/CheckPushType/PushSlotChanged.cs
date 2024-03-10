using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VacdmApp.Data;
using static VacdmApp.Data.PushNotifications.PushNotificationHandler;

namespace VacdmApp.Data.PushNotifications
{
    internal partial class PushType
    {
        internal static async Task PushSlotChangedAsync(VacdmPilot pilot, bool isOwnFlight, TimeOnly pushTime)
        {
            var vacdm = pilot.Vacdm;

            if (!IsAllowedToSendPush(NotificationType.SlotChanged, isOwnFlight))
            {
                return;
            }

            //The changed Slot Push for this Slot revision has already been sent
            if (
                PushedNotifications
                    .Where(x => x.callsign == pilot.Callsign)
                    .Where(x => x.notificationType == NotificationType.SlotChanged)
                    .Any(x => x.pushedTsat == vacdm.Tsat)
            )
            {
                return;
            }

            PushedNotifications.Add(
                new(pilot.Callsign, NotificationType.SlotChanged, pushTime, vacdm.Tsat)
            );

            var removeIndex = LastTsats.FindIndex(x => x.callsign == pilot.Callsign);

            LastTsats.RemoveAt(removeIndex);

            LastTsats.Add(new(pilot.Callsign, vacdm.Tsat));

            await PushSender.SendPushNotificationAsync(pilot, NotificationType.SlotChanged);

            return;
        }
    }
}
