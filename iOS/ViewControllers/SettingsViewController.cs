using CoreGraphics;
using Foundation;
using mARkIt.Authentication;
using mARkIt.iOS.Helpers;
using mARkIt.Models;
using mARkIt.Utils;
using Syncfusion.SfRating.iOS;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class SettingsViewController : UIViewController
    {
        private int? m_CategoriesCode = null;

        public SettingsViewController (IntPtr handle) : base (handle)
        {
        }

        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            saveButton.Clicked += SaveButton_Clicked;
            logoutButton.TouchUpInside += LogoutButton_TouchUpInside;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (m_CategoriesCode == null || m_CategoriesCode.Value != App.ConnectedUser.RelevantCategoriesCode)
            {
                m_CategoriesCode = App.ConnectedUser.RelevantCategoriesCode;
                getCategoriesCheckBoxCheckStatusFromUser();
            }              
        }

        private void getCategoriesCheckBoxCheckStatusFromUser()
        {
            generalCheckBox.IsChecked = (App.ConnectedUser.RelevantCategoriesCode & (int)eCategories.General) != 0;
            foodCheckBox.IsChecked = (App.ConnectedUser.RelevantCategoriesCode & (int)eCategories.Food) != 0;
            sportCheckBox.IsChecked = (App.ConnectedUser.RelevantCategoriesCode & (int)eCategories.Sport) != 0;
            historyCheckBox.IsChecked = (App.ConnectedUser.RelevantCategoriesCode & (int)eCategories.History) != 0;
            natureCheckBox.IsChecked = (App.ConnectedUser.RelevantCategoriesCode & (int)eCategories.Nature) != 0;

        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            App.ConnectedUser.RelevantCategoriesCode = getCategories();
            bool updated = await User.Update(App.ConnectedUser);
            if(updated)
            {
                Alert.Display("Ok", "Settings updated!", this);
            }
            else
            {
                Alert.Display("Error", "Their was a problem saving your settings, please try again later", this);
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


        private async void LogoutButton_TouchUpInside(object sender, EventArgs e)
        {
            await LoginHelper.Logout();
        }

    }
}