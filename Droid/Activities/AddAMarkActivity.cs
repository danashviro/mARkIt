
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Text;
using Android.Views;
using Android.Widget;
using Java.Lang;
using mARkIt.Models;
using mARkIt.Utils;
using Xamarin.Essentials;

namespace mARkIt.Droid
{
    [Activity(Label = "AddAMarkActivity")]
    public class AddAMarkActivity : AppCompatActivity,ITextWatcher
    {
        private EditText m_MessageEditText;
        private TextView m_NumOfLettersTextView;
        private CheckBox m_GeneralCheckBox;
        private CheckBox m_FoodCheckBox;
        private CheckBox m_SportCheckBox;
        private CheckBox m_HistoryCheckBox;
        private CheckBox m_NatureCheckBox;
        private const int m_MaxLetters = 40;
        private int m_Count = 0;
        private string m_UserEmail;

        public void AfterTextChanged(IEditable s)
        {
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            m_Count = count;
            int remainingLetters = m_MaxLetters - m_Count;
            m_NumOfLettersTextView.SetTextColor(remainingLetters < 0?Android.Graphics.Color.Red: Android.Graphics.Color.Black);
            m_NumOfLettersTextView.Text = "Letters: " + remainingLetters;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Create your application here
            SetContentView(Resource.Layout.AddAMark);
            Button saveButton = FindViewById<Button>(Resource.Id.SaveButton);
            saveButton.Click += SaveButton_Click;
            findComponents();
            m_NumOfLettersTextView.Text = "Letters: " + m_MaxLetters;
            m_MessageEditText.AddTextChangedListener(this);
            m_UserEmail = Intent.GetStringExtra("userEmail");
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            if(!atLeastOneCheckBox())
            {
                Toast.MakeText(this, "Please check a check box.", ToastLength.Long).Show();
            }
            else if(m_Count==0)
            {
                Toast.MakeText(this, "Please insert a message.", ToastLength.Long).Show();
            }
            else if(m_Count<= m_MaxLetters)
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                Mark mark = new Mark()
                {
                    Message = m_MessageEditText.Text,
                    Latitude = location.Latitude,
                    Longitude = location.Longitude,
                    UserEmail = m_UserEmail,
                    CategoriesCode = getCategoriesCode(),
                };
                await Mark.Insert(mark);
                Toast.MakeText(this, "Upload successfull.", ToastLength.Long).Show();
            }
            else 
            {
                Toast.MakeText(this, "Please delete some letters.", ToastLength.Long).Show();
            }
        }

        private int getCategoriesCode()
        {
            int catagories = 0;
            if (m_GeneralCheckBox.Checked)
            {
                catagories |= (int)eCategories.General;
            }
            if (m_FoodCheckBox.Checked)
            {
                catagories |= (int)eCategories.Food;
            }
            if (m_SportCheckBox.Checked)
            {
                catagories |= (int)eCategories.Sport;
            }
            if (m_HistoryCheckBox.Checked)
            {
                catagories |= (int)eCategories.History;
            }
            if (m_NatureCheckBox.Checked)
            {
                catagories |= (int)eCategories.Nature;
            }
            return catagories;
        }

        private bool atLeastOneCheckBox()
        {
            return m_GeneralCheckBox.Checked || m_FoodCheckBox.Checked || m_SportCheckBox.Checked || m_HistoryCheckBox.Checked || m_NatureCheckBox.Checked;
        }

        private void findComponents()
        {
            m_MessageEditText = FindViewById<EditText>(Resource.Id.MarkMessageEditText);
            m_NumOfLettersTextView = FindViewById<TextView>(Resource.Id.LettersTextView);
            m_GeneralCheckBox = FindViewById<CheckBox>(Resource.Id.GeneralCheckBox);
            m_FoodCheckBox = FindViewById<CheckBox>(Resource.Id.FoodCheckBox);
            m_HistoryCheckBox = FindViewById<CheckBox>(Resource.Id.HistoryCheckBox);
            m_SportCheckBox = FindViewById<CheckBox>(Resource.Id.SportCheckBox);
            m_NatureCheckBox = FindViewById<CheckBox>(Resource.Id.NatureCheckBox);
        }
    }
}
