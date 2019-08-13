using Foundation;
using mARkIt.Models;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class RateViewController : UIViewController
    {
        public string MarkId { get; set; }
        public RateViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            //markRating.Value
            saveButton.Clicked += SaveButton_Clicked;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (await User.RateMark(App.ConnectedUser.Email, MarkId, (float)userRating.Value))
            {
                Helpers.Alert.DisplayAnAlert("Success", "The mARk uploaded", this, new Action<UIAlertAction>((a) => NavigationController.PopViewController(true)));
            }
            //NavigationController.PopViewController(true);
        }
    }
}