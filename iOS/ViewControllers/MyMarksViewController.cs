using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using mARkIt.Services;
using mARkIt.Models;

namespace mARkIt.iOS
{
    public partial class MyMarksViewController : UITableViewController
    {
        List<Location> m_Marks;
        Random m_Rand;

        public MyMarksViewController (IntPtr handle) : base (handle)
        {
            m_Marks = new List<Location>();
            m_Rand = new Random();
        }

        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            //change to: get my marks
            var marks =await LocationService.Instance().GetLocations();
            m_Marks = new List<Location>(marks);
            TableView.ReloadData();
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return m_Marks.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("markCell") as MarkTableViewCell;
            var mark = m_Marks[indexPath.Row];
            cell.MessageLabel.Text = mark.message;
            cell.DateLabel.Text  = mark.createdAt.ToLocalTime().ToLongDateString();
            //cell.coordinatesLabel.Text  = $"{mark.latitude}, {mark.latitude}";
            cell.RatingBar.ItemSize = 10;
            cell.RatingBar.Value = m_Rand.Next(5);
            cell.RatingBar.UserInteractionEnabled = false;
            return cell;
        }
    }
}