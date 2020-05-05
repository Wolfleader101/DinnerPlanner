using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
		}

		private void SaveMeals_Click(object sender, RoutedEventArgs e)
		{

			//need this to update from JSON --- use it for generate meals
			//MenuPlan.ItemsSource = Meal.GetMeals();

			var row = MenuPlan.Items[0];
			Console.WriteLine(row);
			// Update JSON File
			UpdateJson(row);

			
			

		}

		public void UpdateJson(object meals)
		{
			// serialize JSON directly to a file
			using (StreamWriter file = File.CreateText(@"./test.json"))
			{
				JsonSerializer serializer = new JsonSerializer();
				serializer.Serialize(file, meals);
			}
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

		// read JSON directly from a file
		using (StreamReader file = File.OpenText(@"./test.json"))
		using (JsonTextReader reader = new JsonTextReader(file))
		{
			JObject output = (JObject)JToken.ReadFrom(reader);
			string o = output.ToObject<string>();
			var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(o);
			foreach (var kv in dict)
			{
				Console.WriteLine(kv.Key + ":" + kv.Value);
			}

			return dict;
		}

		

		// create new meals object
		//var meals = new ObservableCollection<Meal>();
		//meals.Add(new Meal()
		//{
		//	Mon = "Ali",
		//	Tues = "test",
		//	Wed = "burger",
		//	Thurs = "chips",
		//	Fri = "dog",
		//	Sat = "cat",
		//	Sun = "gjkjf"
		//});
	}

	private static void MakeJson(ObservableCollection<Meal> meals)
	{
		// serialize JSON directly to a file
		using (StreamWriter file = File.CreateText(@"./test.json"))
		{
			JsonSerializer serializer = new JsonSerializer();
			serializer.Serialize(file, meals);
		}
	}

	protected void OnPropertyChanged([CallerMemberName] string name = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}



