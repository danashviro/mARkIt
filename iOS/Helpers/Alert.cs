using System;
using UIKit;

namespace mARkIt.iOS.Helpers
{
    public class Alert
    {
        public static void DisplayAnAlert(string i_Title, string i_Message, Action<UIAlertAction> i_Action, UIViewController i_ParentViewController)
        {
            var alertController = UIAlertController.Create(i_Title, i_Message, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, i_Action));
            i_ParentViewController.PresentViewController(alertController, true, null);
        }
    }
}
