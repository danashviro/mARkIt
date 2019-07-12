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
            WTFeatures requiredFeatures = WTFeatures.Geo | WTFeatures.WTFeature_InstantTracking;

            ArExperienceAuthorizationController.AuthorizeRestricedAPIAccess(authorizationRequestManager, requiredFeatures, () =>
            {

            }, (UIAlertController alertController) =>
            {
            });
            FacebookButton.TouchUpInside += FacebookButton_TouchUpInside;
        }

        private void FacebookButton_TouchUpInside(object sender, EventArgs e)
        {

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
