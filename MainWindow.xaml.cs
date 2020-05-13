﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Xml.Serialization;

namespace DinnerPlanner
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			MenuPlan.ItemsSource = Meal.GetMeals();
			MealBox.Items.Clear();
			GetMealBox();

			Closing += OnClosing;
		}
		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			MessageBoxResult result = MessageBox.Show("Do you really want to close the app?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
			{
				cancelEventArgs.Cancel = true;
			}
			Console.WriteLine("Application closing..");
		}

		private void SaveDataToXml()
		{

			var meals = MenuPlan.Items;
			List<DinnerPlanner.Meal> NewMeals = meals.OfType<DinnerPlanner.Meal>().ToList();
			var oc = new ObservableCollection<Meal>(NewMeals);
			XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Meal>));
			using (StreamWriter wr = new StreamWriter("Meals.xml"))
			{
				xs.Serialize(wr, oc);
			}
		}
		private void UpdateMenuPlan(ObservableCollection<Meal> oc)
		{
			MenuPlan.ItemsSource = oc;
		}
		private void UpdateMealBox()
		{
			var meals = MealBox.Items;
			List<string> NewMeals = meals.OfType<string>().ToList();
			XmlSerializer xs = new XmlSerializer(typeof(List<string>));
			using (StreamWriter wr = new StreamWriter("AddedMeals.xml"))
			{
				xs.Serialize(wr, NewMeals);
			}
		}

		private void GetMealBox()
		{
			var meals = new List<string>();
			string MealPath = @".\AddedMeals.xml";
			if (File.Exists(MealPath))
			{
				XmlSerializer xs2 = new XmlSerializer(typeof(List<string>));

				using (StreamReader rd = new StreamReader("AddedMeals.xml"))
				{
					meals = xs2.Deserialize(rd) as List<string>;
					meals.ForEach(delegate (string s) { MealBox.Items.Add(s); });
				}

			}
			else
			{
				XmlSerializer xs = new XmlSerializer(typeof(List<string>));
				using (StreamWriter wr = new StreamWriter("AddedMeals.xml"))
				{
					xs.Serialize(wr, meals);
				}
			}
		}
		private List<string> GetLocalMeals()
		{
			var meals = new List<string>();
			string MealPath = @".\AddedMeals.xml";
			if (File.Exists(MealPath))
			{
				XmlSerializer xs2 = new XmlSerializer(typeof(List<string>));

				using (StreamReader rd = new StreamReader("AddedMeals.xml"))
				{
					meals = xs2.Deserialize(rd) as List<string>;
				}
			}
			else
			{
				XmlSerializer xs = new XmlSerializer(typeof(List<string>));
				using (StreamWriter wr = new StreamWriter("AddedMeals.xml"))
				{
					xs.Serialize(wr, meals);
				}
			}
			return meals;
		}
		private void random_Click(object sender, RoutedEventArgs e)
		{
			var _meals = GetLocalMeals();
			var newMeals = new List<string>();
			// if atleast 7 meals (1 for each day)
			if (_meals.Count >= 7)
			{
				Random rand = new Random();
				int RandIndex;
				_meals.ForEach(delegate (string s)
				{
					do
					{
						// get random index
						RandIndex = rand.Next(_meals.Count);
						
					} while (newMeals.Contains(_meals[RandIndex])); // make sure new array doesnt already contain that
					// Add random meal to newMeal
					newMeals.Add(_meals[RandIndex]);		
				});
				
			}
			else
			{
				MessageBox.Show("You must have atleast 7 meals for this", "Error - Cannot Randomly Generate", MessageBoxButton.OK, MessageBoxImage.Error);
				Console.WriteLine(_meals.Count);
			}
			//newMeals;
			var ocMeals = new ObservableCollection<Meal>();
			ocMeals.Add(new Meal()
			{
				Mon = newMeals[0],
				Tues = newMeals[1],
				Wed = newMeals[2],
				Thurs = newMeals[3],
				Fri = newMeals[4],
				Sat = newMeals[5],
				Sun = newMeals[6]
			});
			UpdateMenuPlan(ocMeals);
			SaveDataToXml();

		}

		private void Add_Meal(object sender, RoutedEventArgs e)
		{
			var _text = mealText.Text;
			if (_text != "")
			{
				MealBox.Items.Add(_text);
				UpdateMealBox();
			}
			else
			{
				MessageBox.Show("Please enter a valid meal \n it cannot be empty", "Error - Cannot add meal", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}

		private void Delete_Meal(object sender, RoutedEventArgs e)
		{
			if (MealBox.SelectedItem != null)
			{
				MealBox.Items.RemoveAt(MealBox.Items.IndexOf(MealBox.SelectedItem));
				UpdateMealBox();
			}
			else
			{
				MessageBox.Show("Please select a meal to delete", "Error - Cannot delete meal", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}
	}

	public class Meal : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		private string mon;
		private string tues;
		private string wed;
		private string thurs;
		private string fri;
		private string sat;
		private string sun;

		public string Mon
		{
			get { return mon; }
			set
			{
				mon = value;
				OnPropertyChanged();
			}
		}
		public string Tues
		{
			get { return tues; }
			set
			{
				tues = value;
				OnPropertyChanged();
			}
		}
		public string Wed
		{
			get { return wed; }
			set
			{
				wed = value;
				OnPropertyChanged();
			}
		}
		public string Thurs
		{
			get { return thurs; }
			set
			{
				thurs = value;
				OnPropertyChanged();
			}
		}
		public string Fri
		{
			get { return fri; }
			set
			{
				fri = value;
				OnPropertyChanged();
			}
		}
		public string Sat
		{
			get { return sat; }
			set
			{
				sat = value;
				OnPropertyChanged();
			}
		}
		public string Sun
		{
			get { return sun; }
			set
			{
				sun = value;
				OnPropertyChanged();
			}
		}
		public static ObservableCollection<Meal> GetMeals()
		{
			var meals = new ObservableCollection<Meal>();
			string MealPath = @".\Meals.xml";
			if (File.Exists(MealPath))
			{
				//https://stackoverflow.com/questions/1194931/what-is-the-easiest-way-to-save-an-
				XmlSerializer xs2 = new XmlSerializer(typeof(ObservableCollection<Meal>));

				using (StreamReader rd = new StreamReader("Meals.xml"))
				{
					meals = xs2.Deserialize(rd) as ObservableCollection<Meal>;
				}

			}
			else
			{
				meals.Add(new Meal()
				{
					Mon = "Ali",
					Tues = "test",
					Wed = "burger",
					Thurs = "chips",
					Fri = "dog",
					Sat = "cat",
					Sun = "gjkjf"
				});

				XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Meal>));
				using (StreamWriter wr = new StreamWriter("Meals.xml"))
				{
					xs.Serialize(wr, meals);
				}
			}

			return meals;
		}
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}






