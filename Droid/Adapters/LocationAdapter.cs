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

namespace mARkIt.Droid.Adapters
{
    class LocationAdapter : BaseAdapter
    {

        Context context;
        List<Models.Location> m_Locations;
        public LocationAdapter(Context context,List<Models.Location> locations)
        {
            this.context = context;
            m_Locations = locations;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            LocationAdapterViewHolder holder = null;

            if (view != null)
                holder = view.Tag as LocationAdapterViewHolder;

            if (holder == null)
            {
                holder = new LocationAdapterViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
                view = inflater.Inflate(Resource.Layout.MyMarks, parent, false);
                holder.Message = view.FindViewById<TextView>(Resource.Id.LocationMessageTextView);
                holder.Date = view.FindViewById<TextView>(Resource.Id.LocationDateTextView);

                view.Tag = holder;
            }


            //fill in your items
            var location = m_Locations[position];
            holder.Message.Text =location.message;
            holder.Date.Text = location.createdAt.ToLocalTime().ToLongDateString();

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return m_Locations.Count;
            }
        }

    }

    class LocationAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView Message { get; set; }
        public TextView Date { get; set; }

    }
}