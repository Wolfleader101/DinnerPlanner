﻿using System;
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
    private string food;

    public string Food
    {
        get { return food; }
        set
        {
            food = value;
            OnPropertyChanged();
        }
    }

    public static ObservableCollection<Meal> GetMeals()
    {
        var meals = new ObservableCollection<Meal>();

        meals.Add(new Meal()
        {
            Food = "Ali"
        });

        meals.Add(new Meal()
        {
            Food = "Ahmed",
        });

        meals.Add(new Meal()
        {
            Food = "Amjad",
        });

        meals.Add(new Meal()
        {
            Food = "Waqas",
        });

        meals.Add(new Meal()
        {
            Food = "Bilal",
        });

        meals.Add(new Meal()
        {
            Food = "Waqar",
        });

        return meals;
    }

    protected void OnPropertyChanged([CallerMemberName] string name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}



