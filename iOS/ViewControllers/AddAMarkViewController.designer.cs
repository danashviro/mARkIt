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
    [Register ("AddAMarkViewController")]
    partial class AddAMarkViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem cancleBarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem doneBarButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox foodCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox generalCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox historyCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel letterCounterLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfRadioGroup markStyleRadioGroup { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView markTextView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox natureCheckBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIBarButtonItem saveMarkButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        Syncfusion.iOS.Buttons.SfCheckBox sportCheckBox { get; set; }

        [Action ("CancleBarButton_Activated:")]
        [GeneratedCode ("iOS Designer", "1.0")]
        partial void CancleBarButton_Activated (UIKit.UIBarButtonItem sender);

        void ReleaseDesignerOutlets ()
        {
            if (cancleBarButton != null) {
                cancleBarButton.Dispose ();
                cancleBarButton = null;
            }

            if (doneBarButton != null) {
                doneBarButton.Dispose ();
                doneBarButton = null;
            }

            if (foodCheckBox != null) {
                foodCheckBox.Dispose ();
                foodCheckBox = null;
            }

            if (generalCheckBox != null) {
                generalCheckBox.Dispose ();
                generalCheckBox = null;
            }

            if (historyCheckBox != null) {
                historyCheckBox.Dispose ();
                historyCheckBox = null;
            }

            if (letterCounterLabel != null) {
                letterCounterLabel.Dispose ();
                letterCounterLabel = null;
            }

            if (markStyleRadioGroup != null) {
                markStyleRadioGroup.Dispose ();
                markStyleRadioGroup = null;
            }

            if (markTextView != null) {
                markTextView.Dispose ();
                markTextView = null;
            }

            if (natureCheckBox != null) {
                natureCheckBox.Dispose ();
                natureCheckBox = null;
            }

            if (saveMarkButton != null) {
                saveMarkButton.Dispose ();
                saveMarkButton = null;
            }

            if (sportCheckBox != null) {
                sportCheckBox.Dispose ();
                sportCheckBox = null;
            }
        }
    }
}