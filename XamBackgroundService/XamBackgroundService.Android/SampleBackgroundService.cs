using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamBackgroundService.Droid
{

    [Service]
    public class SampleBackgroundService : Service
    {

        public override IBinder OnBind(Intent intent)
        {
            if (intent != null)
            {
                return null;
            }
            else
            {
                return null;
            }
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            var _isBusy = false;

            var _isConnected = IsConnected();

            Connectivity.ConnectivityChanged += (sender, args) =>
            {
                _isConnected = IsConnected();

                if (!_isConnected)
                {

                    _isBusy = false;
                }
            };

            try
            {
                if (intent != null)
                {
                    RegisterForegroundService();

                    Device.StartTimer(TimeSpan.FromSeconds(5), () =>
                    {
                        ///PLACE YOUR CODE HERE...
                        return true;
                    });

                }
                else
                {
                    _isBusy = false;

                }
            }
            catch (Exception e)
            {
                _isBusy = false;

            }

            return StartCommandResult.Sticky;

        }


        void RegisterForegroundService()
        {
            var intent = new Intent(this, typeof(MainActivity));
            var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            NotificationCompat.Builder builder = new NotificationCompat.Builder(this);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                NotificationChannel notificationChannel = new NotificationChannel("ongoing_channel", "Foreground Service", NotificationImportance.Min);
                builder.SetChannelId("ongoing_channel");
                notificationManager.CreateNotificationChannel(notificationChannel);
            }

            var notification = builder.SetContentIntent(PendingIntent.GetActivity(this, 999, intent, PendingIntentFlags.UpdateCurrent))
                .SetSmallIcon(Resource.Drawable.xamarin)
                .SetTicker("My Background Service")
                .SetContentTitle("My Background Service")
                .SetContentText("This app runs in the background.")
                .SetOngoing(true)
                .SetAutoCancel(false).Build();

            StartForeground(999, notification);
        }

        public static bool IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                return true;
            }

            else if (current == NetworkAccess.ConstrainedInternet)
            {

                return false;
            }

            else if (current == NetworkAccess.Local)
            {
                return false;
            }

            else if (current == NetworkAccess.None)
            {
                return false;
            }

            else if (current == NetworkAccess.Unknown)
            {
                return false;
            }

            else
            {
                return false;
            }
        }
    }
}
