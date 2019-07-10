using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Com.Wikitude.Architect;
using Android;
using Com.Wikitude.Common.Permission;
using System;
using Android.Support.V7.App;

namespace mARkIt.Droid
{
    [Activity(Label = "mARk-it", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IPermissionManagerPermissionManagerCallback
    {
       // Button button;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Main);
     
            {
                //button = FindViewById(Resource.Id.myButton) as Button;
                //button.Click += Button_Click;
            }

            m_Tabs = new Intent(this, typeof(TabsActivity));

            //asking for AR permissions
            string[] permissions = { Manifest.Permission.Camera, Manifest.Permission.AccessFineLocation };
            ArchitectView.PermissionManager.CheckPermissions(this, permissions, PermissionManager.WikitudePermissionRequest, this);

        }


        Intent m_Tabs;
        //private void Button_Click(object sender, System.EventArgs e)
        //{
        //   // ar = new Intent(this, typeof(ArActivity));

        //}


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            int[] results = new int[grantResults.Length];
            for (int i = 0; i < grantResults.Length; i++)
            {
                results[i] = (int)grantResults[i];
            }
            ArchitectView.PermissionManager.OnRequestPermissionsResult(requestCode, permissions, results);
        }



        public void PermissionsDenied(string[] deniedPermissions)
        {

        }

        public void PermissionsGranted(int responseCode)
        {
            StartActivity(m_Tabs);
        }

        public void ShowPermissionRationale(int requestCode, string[] permissions)
        {

        }
    }
}

