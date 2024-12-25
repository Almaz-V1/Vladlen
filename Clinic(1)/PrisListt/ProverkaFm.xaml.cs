using Clinic_1_.Model2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Clinic_1_.PrisListt
{
    public class Scol : INotifyPropertyChanged
    {

        private ObservableCollection<Appointment> _appointment;
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<Appointment> Filtere
        {
            get => _appointment;
            private set
            {
                _appointment = value;
                OnPropertyChanged(nameof(Filtere));

            }
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }


    /// <summary>
    /// Логика взаимодействия для ProverkaFm.xaml
    /// </summary>
    public partial class ProverkaFm : Window
    {
       
        public ProverkaFm()
        {
            InitializeComponent();
            LoadUsers(); // Загружаем пользователей из базы данных
        }

        private void LoadUsers()
        {
            using (var context = new MediclcentContext()) // Убедитесь, что это ваш контекст базы данных
            {
                var users = context.Users.ToList(); // Извлекаем всех пользователей из базы данных
                userControlList.ItemsSource = users; // Устанавливаем источник данных для ItemsControl
            }
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            MenuAdmin menuAdmin = new MenuAdmin();
            menuAdmin.Show();
            Close();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}