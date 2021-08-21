using System;
using System.Collections.Generic;
using System.Text;

namespace AppCarnesDF.Helpers.Azure
{
    public static class AzureConstants
    {
        //Notification Push
        public static string NotificationChannelName { get; set; } = "XamarinNotifyChannel";
        public static string NotificationHubName { get; set; } = "appcarnesDFN";
        public static string ListenConnectionString { get; set; } = "Endpoint=sb://momentosdonfernando.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=64I+DgyhU6e/MFunawGN2fnw+28IzY3yEsxs2bm9eUM=";
        public static string DebugTag { get; set; } = "XamarinNotify";
        public static string[] SubscriptionTags { get; set; } = { "default" };
        public static string FCMTemplateBody { get; set; } = "{\"data\":{\"message\":\"$(messageParam)\"}}";
        public static string APNTemplateBody { get; set; } = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";


        //Table Storage
        public static string StorageAccountConnectionString { get; set; } = "DefaultEndpointsProtocol=https;AccountName=appmomentosdf;AccountKey=jxxkCEhAjwvDBda+0sF4cy/Etul9DY1LM3cGMv5Bp8ACrYPhY3yNYpwI77IYVnB4T/7gqbQ3/eFA0049VhPIuw==;EndpointSuffix=core.windows.net";
        public static string UserTableName { get; set; } = "Users";
        public static string CompartirTableName { get; set; } = "Compartir";

    }
}
