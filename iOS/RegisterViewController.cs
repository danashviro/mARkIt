using Foundation;
using System;
using UIKit;

namespace mARkIt.iOS
{
    public partial class RegisterViewController : UIViewController
    {

        public string emailAddress;

        public RegisterViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            emailTextField.Text = emailAddress;
        }
    }
}