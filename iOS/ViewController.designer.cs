// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace mARkIt.iOS
{
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        UIKit.UIButton Button { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField emailTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton FacebookButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField passwordTextField { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton registerButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton signInButton { get; set; }

        [Action ("SignInButton_TouchUpInside:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void SignInButton_TouchUpInside (UIKit.UIButton sender);

        void ReleaseDesignerOutlets ()
        {
            if (emailTextField != null) {
                emailTextField.Dispose ();
                emailTextField = null;
            }

            if (FacebookButton != null) {
                FacebookButton.Dispose ();
                FacebookButton = null;
            }

            if (passwordTextField != null) {
                passwordTextField.Dispose ();
                passwordTextField = null;
            }

            if (registerButton != null) {
                registerButton.Dispose ();
                registerButton = null;
            }

            if (signInButton != null) {
                signInButton.Dispose ();
                signInButton = null;
            }
        }
    }
}