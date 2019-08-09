using CoreGraphics;
using Foundation;
using mARkIt.Models;
using mARkIt.Utils;
using Syncfusion.SfRating.iOS;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class SettingsViewController : UIViewController
    {
        private bool m_ViewLoaded = false;

        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }

        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            saveButton.Clicked += SaveButton_Clicked;
            getCategoriesCheckBoxCheckStatusFromUser();
            m_ViewLoaded = true;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (m_ViewLoaded)
            {
                getCategoriesCheckBoxCheckStatusFromUser();
            }              
        }

        private void getCategoriesCheckBoxCheckStatusFromUser()
        {
            generalCheckBox.IsChecked = (App.User.RelevantCategoriesCode & (int)eCategories.General) != 0;
            foodCheckBox.IsChecked = (App.User.RelevantCategoriesCode & (int)eCategories.Food) != 0;
            sportCheckBox.IsChecked = (App.User.RelevantCategoriesCode & (int)eCategories.Sport) != 0;
            historyCheckBox.IsChecked = (App.User.RelevantCategoriesCode & (int)eCategories.History) != 0;
            natureCheckBox.IsChecked = (App.User.RelevantCategoriesCode & (int)eCategories.Nature) != 0;

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            App.User.RelevantCategoriesCode = getCategories();
            bool updated = await User.Update(App.User);
            if(updated)
            {
                Helpers.Alert.DisplayAnAlert("Ok", "Settings updated!", null, this);
            }
            else
            {
                Helpers.Alert.DisplayAnAlert("Error", "Settings updated!", null, this);
            }
        }


        private int getCategories()
        {
            int catagories = 0;
            if ((bool)generalCheckBox.IsChecked)
            {
                catagories |= (int)eCategories.General;
            }
            if ((bool)foodCheckBox.IsChecked)
            {
                catagories |= (int)eCategories.Food;
            }
            if ((bool)sportCheckBox.IsChecked)
            {
                catagories |= (int)eCategories.Sport;
            }
            if ((bool)historyCheckBox.IsChecked)
            {
                catagories |= (int)eCategories.History;
            }
            if ((bool)natureCheckBox.IsChecked)
            {
                catagories |= (int)eCategories.Nature;
            }

            return catagories;
        }


        partial void LogoutButton_TouchUpInside(UIButton sender)
        {
            Xamarin.Essentials.SecureStorage.RemoveAll();
        }

       
    }
}