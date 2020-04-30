using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

		return meals;
	}

	protected void OnPropertyChanged([CallerMemberName] string name = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}



