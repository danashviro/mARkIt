using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace mARkIt.Droid.Helpers
{
    class Alert
    {
        public static void Show(string i_Title, string i_Message, Context i_Context, Action i_Action = null)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(i_Context);
            dialog.SetTitle(i_Title);
            dialog.SetMessage(i_Message);
            dialog.SetPositiveButton("OK", (sender, eventArgs) => i_Action?.Invoke());
            dialog.Show();
        }
    }
}