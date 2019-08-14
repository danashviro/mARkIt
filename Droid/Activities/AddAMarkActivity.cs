
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
using mARkIt.Droid.Helpers;
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
        private RadioButton m_GeneralRadioButton;
        private RadioButton m_FoodRadioButton;
        private RadioButton m_SportRadioButton;
        private RadioButton m_HistoryRadioButton;
        private RadioButton m_NatureRadioButton;
        private const int k_MaxLetters = 40;
        private int m_LettersCount = 0;

        public void AfterTextChanged(IEditable s)
        {
        }

        public void BeforeTextChanged(ICharSequence s, int start, int count, int after)
        {
        }

        public void OnTextChanged(ICharSequence s, int start, int before, int count)
        {
            m_LettersCount = count;
            int remainingLetters = k_MaxLetters - m_LettersCount;
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
            m_NumOfLettersTextView.Text = "Letters: " + k_MaxLetters;
            m_MessageEditText.AddTextChangedListener(this);
        }

        private async void SaveButton_Click(object sender, EventArgs e)
        {
            bool inputIsValid = validateInput();

            if (inputIsValid)
            {
                try
                {
                    var location = await Geolocation.GetLocationAsync();

                    Mark mark = new Mark()
                    {
                        Message = m_MessageEditText.Text,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        CategoriesCode = getCategoriesCode(),
                    };
                    bool uploadSuccessful = await Mark.Insert(mark);

                    if (uploadSuccessful)
                    {
                        Alert.Show("Success", "Upload successfull.", this, Finish);
                    }
                    else
                    {
                        Alert.Show("Faliure", "Upload failed.", this);
                    }
                }
                catch (FeatureNotEnabledException ex)
                {
                    Toast.MakeText(this, "Please activate location services.", ToastLength.Long).Show();
                }
            }
        }

        private bool validateInput()
        {
            bool isInputValid = false;

            if (m_LettersCount == 0)
            {
                Toast.MakeText(this, "Please insert a message.", ToastLength.Long).Show();
            }
            else if (m_LettersCount > k_MaxLetters)
            {
                Toast.MakeText(this, "Please delete some letters.", ToastLength.Long).Show();
            }
            else if (!atLeastOneRadioButton())
            {
                Toast.MakeText(this, "Please check a category.", ToastLength.Long).Show();
            }
            else
            {
                isInputValid = true;
            }

            return isInputValid;
        }

        private int getCategoriesCode()
        {
            int catagories = 0;
            if (m_GeneralRadioButton.Checked)
            {
                catagories |= (int)eCategories.General;
            }
            else if (m_FoodRadioButton.Checked)
            {
                catagories |= (int)eCategories.Food;
            }
            else if (m_SportRadioButton.Checked)
            {
                catagories |= (int)eCategories.Sport;
            }
            else if (m_HistoryRadioButton.Checked)
            {
                catagories |= (int)eCategories.History;
            }
            else if (m_NatureRadioButton.Checked)
            {
                catagories |= (int)eCategories.Nature;
            }
            return catagories;
        }

        private bool atLeastOneRadioButton()
        {
            return m_GeneralRadioButton.Checked || m_FoodRadioButton.Checked || m_SportRadioButton.Checked || m_HistoryRadioButton.Checked || m_NatureRadioButton.Checked;
        }

        private void findComponents()
        {
            m_MessageEditText = FindViewById<EditText>(Resource.Id.MarkMessageEditText);
            m_NumOfLettersTextView = FindViewById<TextView>(Resource.Id.LettersTextView);
            m_GeneralRadioButton = FindViewById<RadioButton>(Resource.Id.GeneralRadioButton);
            m_FoodRadioButton = FindViewById<RadioButton>(Resource.Id.FoodRadioButton);
            m_HistoryRadioButton = FindViewById<RadioButton>(Resource.Id.HistoryRadioButton);
            m_SportRadioButton = FindViewById<RadioButton>(Resource.Id.SportRadioButton);
            m_NatureRadioButton = FindViewById<RadioButton>(Resource.Id.NatureRadioButton);
        }
    }
}
