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
    [Register ("RateViewController")]
    partial class RateViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.SfRating.iOS.SfRating markRating { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.SfRating.iOS.SfRating userRating { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (markRating != null) {
                markRating.Dispose ();
                markRating = null;
            }

            if (userRating != null) {
                userRating.Dispose ();
                userRating = null;
            }
        }
    }
}