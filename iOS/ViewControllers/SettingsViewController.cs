using Foundation;
using mARkIt.Authentication;
using mARkIt.iOS.Helpers;
using mARkIt.Models;
using mARkIt.Utils;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class SettingsViewController : UIViewController
    {
        private bool m_LoggedOut = false;

        public SettingsViewController (IntPtr handle) : base(handle) { }
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            saveButton.Clicked += SaveButton_Clicked;
            logoutButton.TouchUpInside += LogoutButton_TouchUpInside;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (getCategories() != App.ConnectedUser.RelevantCategoriesCode)
            {
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
            saveButton.Enabled = false;
            App.ConnectedUser.RelevantCategoriesCode = getCategories();
            int prevCategoriesCode = App.ConnectedUser.RelevantCategoriesCode;
            bool updated = await User.Update(App.ConnectedUser);
            if(updated)
            {
                Alert.Display("Ok", "Settings updated!", this);
            }
            else
            {
                App.ConnectedUser.RelevantCategoriesCode = prevCategoriesCode;
                Alert.Display("Error", "Their was a problem saving your settings, please try again later", this);
            }

            saveButton.Enabled = true;
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


        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if(segueIdentifier == "logoutSegue")
            {
                return m_LoggedOut;
            }

            return base.ShouldPerformSegue(segueIdentifier, sender);
        }

        private async void LogoutButton_TouchUpInside(object sender, EventArgs e)
        {
            m_LoggedOut = await LoginHelper.Logout();
            if(!m_LoggedOut)
            {
                Alert.Display("Error", "There was a problem logging you out", this);
            }
        }

    }
}