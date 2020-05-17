using System;
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
			// set Datagrids item source
			MenuPlan.ItemsSource = Meal.GetMeals();
			//clear any rubbish data in added meal box
			MealBox.Items.Clear();
			// get latest mealbox data
			GetMealBox();

			// on closing run	 OnClosing
			Closing += OnClosing;
		}
		private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
		{
			//ask if they want to close
			MessageBoxResult result = MessageBox.Show("Do you really want to close the app?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if (result == MessageBoxResult.No)
			{
				//dont close
				cancelEventArgs.Cancel = true;
			}
		}

		//save DataGrid data to xml
		private void SaveDataToXml()
		{
			// get var for items
			var meals = MenuPlan.Items;
			//convert to list
			List<DinnerPlanner.Meal> NewMeals = meals.OfType<DinnerPlanner.Meal>().ToList();
			//convert list to ObservableCollection
			var oc = new ObservableCollection<Meal>(NewMeals);
			//serialize to XML
			XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Meal>));
			using (StreamWriter wr = new StreamWriter("Meals.xml"))
			{
				//write to file
				xs.Serialize(wr, oc);
			}
		}
		private void UpdateMenuPlan(ObservableCollection<Meal> oc)
		{
			// update datagrid values
			MenuPlan.ItemsSource = oc;
		}
		private void UpdateMealBox()
		{
			//get mealbox items
			var meals = MealBox.Items;
			//convert to itemcollection to list
			List<string> NewMeals = meals.OfType<string>().ToList();
			// serialize to xml
			XmlSerializer xs = new XmlSerializer(typeof(List<string>));
			using (StreamWriter wr = new StreamWriter("AddedMeals.xml"))
			{
				//save to xml
				xs.Serialize(wr, NewMeals);
			}
		}

		private void GetMealBox()
		{
			// set empty list
			var meals = new List<string>();
			//set path
			string MealPath = @".\AddedMeals.xml";
			if (File.Exists(MealPath))
			{
				//serializer xml, with typeof list
				XmlSerializer xs = new XmlSerializer(typeof(List<string>));

				using (StreamReader rd = new StreamReader("AddedMeals.xml"))
				{
					//read xml and convert to list
					meals = xs.Deserialize(rd) as List<string>;
					//for each item in list, add to mealbox
					meals.ForEach(delegate (string s) { MealBox.Items.Add(s); });
				}

			}
			else // if no file exsists
			{
				// xml serializer
				XmlSerializer xs = new XmlSerializer(typeof(List<string>));
				using (StreamWriter wr = new StreamWriter("AddedMeals.xml"))
				{
					//write empty list to file
					xs.Serialize(wr, meals);
				}
			}
		}

		//same as last method, but returns a list
		private List<string> GetLocalMeals()
		{
			// set empty list
			var meals = new List<string>();
			//set path
			string MealPath = @".\AddedMeals.xml";
			if (File.Exists(MealPath))
			{
				//xml serializer
				XmlSerializer xs = new XmlSerializer(typeof(List<string>));

				using (StreamReader rd = new StreamReader("AddedMeals.xml"))
				{
					// read file, convert to list
					meals = xs.Deserialize(rd) as List<string>;
				}
			}
			else // if no file
			{
				XmlSerializer xs = new XmlSerializer(typeof(List<string>));
				using (StreamWriter wr = new StreamWriter("AddedMeals.xml"))
				{
					//write empty list
					xs.Serialize(wr, meals);
				}
			}
			return meals;
		}
		// on random button click event
		private void random_Click(object sender, RoutedEventArgs e)
		{
			// get list of meals
			var _meals = GetLocalMeals();
			var newMeals = new List<string>();
			// if atleast 7 meals (1 for each day)
			if (_meals.Count >= 7)
			{
				// random generator
				Random rand = new Random();
				//index
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
				// new ObservableCollection var
				var ocMeals = new ObservableCollection<Meal>();
				// add random meals to each day
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
				//update xml and datagrid
				UpdateMenuPlan(ocMeals);
				SaveDataToXml();
			}
			else // must have atleast 7 meals otherwise it wont work
			{
				MessageBox.Show("You must have atleast 7 meals for this", "Error - Cannot Randomly Generate", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}

		// add button click event
		private void Add_Meal(object sender, RoutedEventArgs e)
		{
			// get text from textbox
			var _text = mealText.Text;
			if (_text != "") // if not empty string
			{
				// add value to mealbox and save to xml
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
			if (MealBox.SelectedItem != null) // check if they selected an item
			{
				// remove item and update xml
				MealBox.Items.RemoveAt(MealBox.Items.IndexOf(MealBox.SelectedItem));
				UpdateMealBox();
			}
			else
			{
				MessageBox.Show("Please select a meal to delete", "Error - Cannot delete meal", MessageBoxButton.OK, MessageBoxImage.Error);
			}

		}
	}

	// meal class
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

		// getters and setters for each day
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
		// method for getting meals
		public static ObservableCollection<Meal> GetMeals()
		{
			// empty ObservableCollection, type meal
			var meals = new ObservableCollection<Meal>();
			string MealPath = @".\Meals.xml";
			if (File.Exists(MealPath))
			{
				// serializer
				XmlSerializer xs2 = new XmlSerializer(typeof(ObservableCollection<Meal>));

				using (StreamReader rd = new StreamReader("Meals.xml"))
				{
					// read this file and convert to ObservableCollection
					meals = xs2.Deserialize(rd) as ObservableCollection<Meal>;
				}

			}
			else //if not file
			{
				//create example menu
				meals.Add(new Meal()
				{
					Mon = "Fish",
					Tues = "Spaghetti",
					Wed = "Burgers",
					Thurs = "Chips",
					Fri = "Sausages",
					Sat = "Mash potato",
					Sun = "Fried Rice"
				});

				// then save this meal to xml
				XmlSerializer xs = new XmlSerializer(typeof(ObservableCollection<Meal>));
				using (StreamWriter wr = new StreamWriter("Meals.xml"))
				{
					xs.Serialize(wr, meals);
				}
			}
			// return meals ObservableCollection
			return meals;
		}
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}






