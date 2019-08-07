using CoreGraphics;
using CoreLocation;
using Foundation;
using mARkIt.Models;
using mARkIt.Services;
using mARkIt.Utils;
using Syncfusion.iOS.Buttons;
using System;
using System.Linq;
using UIKit;
using Xamarin.Essentials;

namespace mARkIt.iOS
{
    public partial class NewMarkViewController : UIViewController
    {
        private SfRadioButton m_WoodMarkStyleRadioButton, m_MetalMarkStyleRadioButton, m_SchoolMarkStyleRadioButton;
        private const int m_MaxLettersAllowed = 40;
        public User User { get; set; }

        public NewMarkViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true)));
            doneBarButton.Clicked += DoneBarButton_Clicked;
            addMarkStyleRadioButtons();
            markTextView.Changed += MarkTextView_Changed;
            letterCounterLabel.Text = m_MaxLettersAllowed.ToString();
        }

        private void MarkTextView_Changed(object sender, EventArgs e)
        {
            int remainingLetters = m_MaxLettersAllowed - markTextView.Text.Count<char>();
            letterCounterLabel.Text = (remainingLetters).ToString();
            if (remainingLetters < 0)
            {
                letterCounterLabel.TextColor = UIColor.Red;
            }
            else if (remainingLetters < 10)
            {
                letterCounterLabel.TextColor = UIColor.Orange;
            }
            else
            {
                letterCounterLabel.TextColor = UIColor.White;
            }
        }

        private void addMarkStyleRadioButtons()
        {
            m_WoodMarkStyleRadioButton = new SfRadioButton();
            m_MetalMarkStyleRadioButton = new SfRadioButton();
            m_SchoolMarkStyleRadioButton = new SfRadioButton();
            m_WoodMarkStyleRadioButton.IsChecked = true;
            markStyleRadioGroup.AddArrangedSubview(m_WoodMarkStyleRadioButton);
            markStyleRadioGroup.AddArrangedSubview(m_MetalMarkStyleRadioButton);
            markStyleRadioGroup.AddArrangedSubview(m_SchoolMarkStyleRadioButton);
        }


        private void displayAnAlert(string i_Title, string i_Message, Action<UIAlertAction> i_Action)
        {
            var alertController = UIAlertController.Create(i_Title, i_Message, UIAlertControllerStyle.Alert);
            alertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, i_Action));
            PresentViewController(alertController, true, null);
        }

        private async void uploadMarkAsync()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    Mark mark = new Mark()
                    {
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Message = markTextView.Text,
                        Style = getMarkStyle(),
                        CategoriesCode = getCategories(),
                        UserEmail = User.Email
                };

                    await Mark.Insert(mark);
                    displayAnAlert("Success", "The mARk uploaded", new Action<UIAlertAction>((a) => NavigationController.PopViewController(true)));
                }
                else
                {
                    displayAnAlert("Error", "There was a problem uploading your mARk", null);
                }
            }
            catch (Exception)
            {
                displayAnAlert("Error", "There was a problem uploading your mARk", null);
            }
        }

        private string getMarkStyle()
        {
            string markStyle = null;
            if((bool)m_WoodMarkStyleRadioButton.IsChecked)
            {
                markStyle = "Wood";
            }
            else if ((bool)m_SchoolMarkStyleRadioButton.IsChecked)
            {
                markStyle = "School";
            }
            else if ((bool)m_MetalMarkStyleRadioButton.IsChecked)
            {
                markStyle = "Metal";
            }
            return markStyle;
        }

        private int getCategories()
        {
            int catagories = 0;
            if((bool)generalCheckBox.IsChecked)
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

            return catagories;
        }

        private void DoneBarButton_Clicked(object sender, EventArgs e)
        {
            markTextView.UserInteractionEnabled = false;
            if (markTextView.Text.Count<char>() > m_MaxLettersAllowed)
            {
                displayAnAlert("Error", "There are too many letters!", null);
            }
            else if (string.IsNullOrEmpty(markTextView.Text))
            {
                displayAnAlert("Error", "Please fill mARk text", null);
            }
            else
            {
                uploadMarkAsync();
            }

            markTextView.UserInteractionEnabled = true;
        }

        partial void CancleBarButton_Activated(UIBarButtonItem sender)
        {
            NavigationController.PopViewController(true);
        }
    }
}
