using System;
using Foundation;
using mARkIt.iOS.CoreServices;
using UIKit;
using WikitudeComponent.iOS;
using mARkIt.Utils;

namespace mARkIt.iOS
{
    public partial class ViewController : UIViewController
    {
        bool hasLoggedIn = true;
        private WTAuthorizationRequestManager authorizationRequestManager = new WTAuthorizationRequestManager();

        public ViewController(IntPtr handle) : base(handle)
        {
        }


        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            initFormConfig();
            getWikitudePermissions();
        }

        private void initFormConfig()
        {
            FacebookButton.TouchUpInside += FacebookButton_TouchUpInside;
            emailTextField.ShouldReturn = delegate
            {
                // Changed this slightly to move the text entry to the next field.
                passwordTextField.BecomeFirstResponder();
                return true;
            };

            passwordTextField.ShouldReturn = delegate
            {
                passwordTextField.ResignFirstResponder();
                return true;
            };

            View.AddGestureRecognizer(new UITapGestureRecognizer(() => View.EndEditing(true))); //hide keyboard when tapping on screen
        }

        private void FacebookButton_TouchUpInside(object sender, EventArgs e)
        {

        }

        private void getWikitudePermissions()
        {
            WTFeatures requiredFeatures = WTFeatures.Geo | WTFeatures.WTFeature_InstantTracking;

            ArExperienceAuthorizationController.AuthorizeRestricedAPIAccess(authorizationRequestManager, requiredFeatures, () =>
            {

            }, (UIAlertController alertController) =>
            {
            });
        }
        



        public void onLoginClick()
        {
            //if logged in :
            hasLoggedIn = true;
            PerformSegue("signInSegue", this);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            if(segue.Identifier == "registerSeque")
            {
                var destViewController = segue.DestinationViewController as RegisterViewController;
                destViewController.emailAddress = emailTextField.Text;
            }
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {
            if(segueIdentifier == "signInSegue") //after press login moveOnlyIfLoggedIn 
            {
                return hasLoggedIn;
            }
            return true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        partial void SignInButton_TouchUpInside(UIButton sender)
        {
        }
    }
}
