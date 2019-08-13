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

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            markRating.Value = (await Mark.GetById(MarkId)).Rating;
            var userRatingVal = await User.GetUserRatingForMark(App.ConnectedUser.Email, MarkId);
            userRating.Value = userRatingVal != null ? userRatingVal.Value : 0;
            saveButton.Clicked += SaveButton_Clicked;
            backButton.Clicked += BackButton_Clicked;
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            NavigationController.PopViewController(true);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (await User.RateMark(App.ConnectedUser.Email, MarkId, (float)userRating.Value))
            {
                Helpers.Alert.DisplayAnAlert("Success", "The mARk rateted", this);
            }
            else
            {
                Helpers.Alert.DisplayAnAlert("Error", "Thir was a problem rate this mARk", this);
            }
        }
    }
}