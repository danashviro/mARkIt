using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using mARkIt.Services;
using mARkIt.Models;
using mARkIt.Authentication;

namespace mARkIt.iOS
{
    public partial class MyMarksViewController : UITableViewController
    {
        private List<Mark> m_Marks;
        private Random m_Rand;
        public User ConnectedUser { get; set; }
        private bool m_ViewLoaded = false;


        public MyMarksViewController (IntPtr handle) : base (handle)
        {
            m_Marks = new List<Mark>();
            m_Rand = new Random();
        }


        public override async void ViewDidLoad()
        {
            base.ViewDidLoad();
            m_Marks = await Mark.GetMyMarks(ConnectedUser);
            TableView.ReloadData();
            m_ViewLoaded = true;
        }

        public override async void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            if (m_ViewLoaded)
            {
                m_Marks = await Mark.GetMyMarks(ConnectedUser);
                TableView.ReloadData();
            }
        }

        public override nint RowsInSection(UITableView tableView, nint section)
        {
            return m_Marks.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell("markCell") as MarkTableViewCell;
            var mark = m_Marks[indexPath.Row];
            cell.MessageLabel.Text = mark.Message;
            cell.DateLabel.Text  = mark.createdAt.ToLocalTime().ToLongDateString();
            //cell.coordinatesLabel.Text  = $"{mark.latitude}, {mark.latitude}";
            cell.RatingBar.ItemSize = 10;
            cell.RatingBar.Value = m_Rand.Next(5);
            cell.RatingBar.UserInteractionEnabled = false;

            return cell;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            if(segue.Identifier == "markSegue")
            {
                var selectedRow = TableView.IndexPathForSelectedRow;
                var destenationViewController = segue.DestinationViewController as MarkViewController;
                destenationViewController.ViewMark = m_Marks[selectedRow.Row];
            }
            base.PrepareForSegue(segue, sender);
        }
    }
}