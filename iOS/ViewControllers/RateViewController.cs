using Foundation;
using mARkIt.iOS.Helpers;
using mARkIt.Models;
using Syncfusion.SfRating.iOS;
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
            markRating.UserInteractionEnabled = false;
            markRating.ItemSize = 33;
            markRating.Precision = SFRatingPrecision.Exact;
            userRating.ItemSize = 33;
            userRating.Precision = SFRatingPrecision.Exact;
            markRating.Value = (await Mark.GetById(MarkId)).Rating;
            var userRatingVal = await User.GetUserRatingForMark(MarkId);
            userRating.Value = userRatingVal != null ? userRatingVal.Value : 0;
            saveButton.Clicked += SaveButton_Clicked;
            backButton.Clicked += BackButton_Clicked;
            base.ViewDidLoad();

        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            NavigationController.PopViewController(true);
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            if (await User.RateMark(MarkId, (float)userRating.Value))
            {
                markRating.Value = (await Mark.GetById(MarkId)).Rating;
                Alert.Display("Success", "The mARk rateted", this);
            }
            else
            {
                Alert.Display("Error", "Their was a problem rate this mARk", this);
            }
        }
    }
}