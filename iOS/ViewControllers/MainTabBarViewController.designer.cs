// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace mARkIt.iOS
{
    [Register ("MainTabBarViewController")]
    partial class MainTabBarViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem AddMarkButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddMarkButton != null) {
                AddMarkButton.Dispose ();
                AddMarkButton = null;
            }
        }
    }
}