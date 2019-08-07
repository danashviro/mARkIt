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
        private User m_User;

        public MyMarksFragment(User i_User)
        {
            m_User = i_User;
        }

        public override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here 
            m_Marks = await AzureService.MobileService.GetTable<Mark>().Where(m => m.UserEmail == m_User.Email).ToListAsync();
            ListAdapter = new MarkAdapter(Context, m_Marks);


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