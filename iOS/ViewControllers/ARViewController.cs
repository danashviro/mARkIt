using System;
using UIKit;
using Foundation;
using CoreGraphics;
using AVFoundation;
using WikitudeComponent.iOS;

namespace mARkIt.iOS
{
    public partial class ARViewController : UIViewController, IUIGestureRecognizerDelegate
    {

        protected class ArchitectDelegate : WTArchitectViewDelegate
        {
            [Weak]
            protected ARViewController arExperienceViewController;

            public ArchitectDelegate(ARViewController arExperienceViewController)
            {
                this.arExperienceViewController = arExperienceViewController;
            } 

            public override void DidFinishLoadNavigation(WTArchitectView architectView, WTNavigation navigation)
            {
                Console.WriteLine("Finished loading Architect World");
                arExperienceViewController.ArchitectWorldFinishedLoading(navigation);
            }

            public override void DidFailToLoadNavigation(WTArchitectView architectView, WTNavigation navigation, NSError error)
            {
                string errorMessage = error.LocalizedDescription + " ('" + navigation.OriginalURL + "')";
                UIAlertController failedToLoadArchitectWorldAlertController = UIAlertController.Create("Failed to load Architect World", errorMessage, UIAlertControllerStyle.Alert);
                failedToLoadArchitectWorldAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));

                arExperienceViewController.PresentViewController(failedToLoadArchitectWorldAlertController, true, null);
            }

            public override UIViewController PresentingViewControllerForViewControllerPresentationInArchitectView(WTArchitectView architectView)
            {
                return arExperienceViewController;
            }
        }

        protected class NavigationControllerDelegate : UINavigationControllerDelegate
        {
            [Weak]
            protected WTArchitectView architectView = null;

            public NavigationControllerDelegate(WTArchitectView architectView)
            {
                this.architectView = architectView;
            }

      
        }


        protected WTArchitectView architectView;
        protected ArchitectDelegate delegateObject;

        [Weak]
        protected ArExperience currentArExperience;
        protected WTNavigation loadedArExperienceNavigation = null;
        protected WTNavigation loadingArExperienceNavigation = null;

        protected NSObject applicationWillResignActiveObserver;
        protected NSObject applicationDidBecomeActiveObserver;

        protected AVCaptureDevicePosition currentCaptureDevicePosition;

        protected bool isRunning;


        public ARViewController() : base("ARViewController", null)
        {
            currentCaptureDevicePosition = AVCaptureDevicePosition.Unspecified;
        }

        public ARViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            architectView = new WTArchitectView();
            architectView.SetLicenseKey(mARkIt.Utils.Keys.WikitudeLicense);
            delegateObject = new ArchitectDelegate(this);
            architectView.Delegate = delegateObject;
            architectView.TranslatesAutoresizingMaskIntoConstraints = false;
            Add(architectView);

            architectView.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor).Active = true;
            architectView.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor).Active = true;
            architectView.WidthAnchor.ConstraintEqualTo(View.WidthAnchor).Active = true;
            architectView.HeightAnchor.ConstraintEqualTo(View.HeightAnchor).Active = true;

            NavigationController.Delegate = new NavigationControllerDelegate(architectView);

            EdgesForExtendedLayout = UIRectEdge.None;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationItem.Title = "AR";

            NavigationController.InteractivePopGestureRecognizer.Delegate = this;

            LoadArExperienceIfRequired();
            StartArchitectViewRendering();

            UIInterfaceOrientation currentOrientation = UIApplication.SharedApplication.StatusBarOrientation;
            architectView.SetShouldRotateToInterfaceOrientation(true, currentOrientation);

            applicationWillResignActiveObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.WillResignActiveNotification, ApplicationWillResignActive);
            applicationDidBecomeActiveObserver = NSNotificationCenter.DefaultCenter.AddObserver(UIApplication.DidBecomeActiveNotification, ApplicationDidBecomeActive);
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            NavigationController.InteractivePopGestureRecognizer.Delegate = null;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            StopArchitectViewRendering();

            NSNotificationCenter.DefaultCenter.RemoveObserver(applicationWillResignActiveObserver);
            NSNotificationCenter.DefaultCenter.RemoveObserver(applicationDidBecomeActiveObserver);
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewWillTransitionToSize(CGSize toSize, IUIViewControllerTransitionCoordinator coordinator)
        {
            if (coordinator != null)
            {
                coordinator.AnimateAlongsideTransition((IUIViewControllerTransitionCoordinatorContext context) =>
                {
                    UIInterfaceOrientation newInterfaceOrientation = UIApplication.SharedApplication.StatusBarOrientation;
                    architectView.SetShouldRotateToInterfaceOrientation(true, newInterfaceOrientation);
                }, null);
            }
            else
            {
                UIInterfaceOrientation newInterfaceOrientation = UIApplication.SharedApplication.StatusBarOrientation;
                architectView.SetShouldRotateToInterfaceOrientation(true, newInterfaceOrientation);
            }

            base.ViewWillTransitionToSize(toSize, coordinator);
        }

        public void ArchitectWorldFinishedLoading(WTNavigation navigation)
        {
            if (loadingArExperienceNavigation.Equals(navigation))
            {
                loadedArExperienceNavigation = navigation;
            }
        }

        #region Delegation
        public bool ShouldRecognizeSimultaneously(UIGestureRecognizer gestureRecognizer, UIGestureRecognizer otherGestureRecognizer)
        {
            return true;
        }
        #endregion

        #region Notifications
        private void ApplicationWillResignActive(NSNotification notification)
        {
            StopArchitectViewRendering();
        }

        private void ApplicationDidBecomeActive(NSNotification notification)
        {
            StartArchitectViewRendering();
        }
        #endregion

        #region Private Methods
        private void LoadArExperienceIfRequired()
        {
            NSUrl fullArExperienceURL = NSBundle.MainBundle.GetUrlForResource("index", "html", "ARPages/ServerPage");

            if (loadedArExperienceNavigation == null || (loadedArExperienceNavigation != null && !loadedArExperienceNavigation.OriginalURL.Equals(fullArExperienceURL)))
            {
                loadingArExperienceNavigation = architectView.LoadArchitectWorldFromURL(fullArExperienceURL);
            }
        }

        private void StartArchitectViewRendering()
        {
            if (!architectView.IsRunning)
            {
                architectView.Start((WTArchitectStartupConfiguration architectStartupConfiguration) =>
                {
                    if (currentCaptureDevicePosition != AVCaptureDevicePosition.Unspecified)
                    {
                        architectStartupConfiguration.CaptureDevicePosition = currentCaptureDevicePosition;
                    }
                    else
                    {
                        architectStartupConfiguration.CaptureDevicePosition = AVCaptureDevicePosition.Back; 
                    }
                    architectStartupConfiguration.CaptureDeviceResolution = WTCaptureDeviceResolution.WTCaptureDeviceResolution_AUTO;
                    architectStartupConfiguration.CaptureDeviceFocusMode = AVCaptureFocusMode.ContinuousAutoFocus; 
                }, (bool success, NSError error) =>
                {
                    isRunning = success;
                });
            }
        }

        private void StopArchitectViewRendering()
        {
            if (isRunning)
            {
                architectView.Stop();
            }
        }
        #endregion

    }
}