using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using mARkIt.Droid.Activities;
using mARkIt.Droid.Adapters;
using mARkIt.Models;
using mARkIt.Services;
using Newtonsoft.Json;

namespace mARkIt.Droid.Fragments
{
    public class MyMarksFragment : Android.Support.V4.App.ListFragment
    {
        List<Mark> m_Marks;

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here 
            getMyMarks();
         }

        private async void getMyMarks()
        {
            m_Marks = await Mark.GetMyMarks(App.User);
            ListAdapter = new MarkAdapter(Context, m_Marks);
        }

        public override void OnHiddenChanged(bool hidden)
        {
            base.OnHiddenChanged(hidden);
            if (hidden == false)
                 getMyMarks();

        }

        public override void OnResume()
        {
            base.OnResume();
            if(IsHidden==false)
                 getMyMarks();

        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            base.OnListItemClick(l, v, position, id);
            var selectedMark = m_Marks[position];
            Intent intent = new Intent(Activity, typeof(MarkPresentationActivity));
            string MarkAsJson = JsonConvert.SerializeObject(selectedMark);
            intent.PutExtra("markAsJson", MarkAsJson);
            StartActivity(intent);
            

        }
    }
}