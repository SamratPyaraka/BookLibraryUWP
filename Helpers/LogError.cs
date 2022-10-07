using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibrary1.Helpers
{
    public class LogError
    {
        public LogError()
        {

        }

        public static void TrackError(Exception exception, string pageName, string url = "", ErrorAttachmentLog[] attachments = null)
        {
            try
            {
#if DEBUG
                Console.WriteLine($"{pageName} {exception.Message} {exception.StackTrace} {exception.InnerException}");
#endif
                Dictionary<string, string> generalDetailsDictionary = new Dictionary<string, string>
                {
                    { "Email", AppSettings.IDTokenPayLoad.Email },
                    { "PageName", pageName },
                    { "URL", url }
                };

                if (attachments != null)
                    Crashes.TrackError(exception, generalDetailsDictionary, attachments);
                else
                    Crashes.TrackError(exception, generalDetailsDictionary);
            }
            catch (Exception)
            {
            }
        }
    }
}
