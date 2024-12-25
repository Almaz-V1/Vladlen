using Clinic_1_.Add;
using Clinic_1_.Model2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace Clinic_1_.PrisListt
{
    public class UserViewModel : INotifyPropertyChanged
    {
        private string _searchTerm;
        private UserViewModel _viewModel;
        private ObservableCollection<User> _users;
        private ObservableCollection<User> _filteredUsers;
        private string _selectedSortOption;

        public event PropertyChangedEventHandler PropertyChanged;
        private string _selectedFilterOption;
       
        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
                UpdateFilteredUsers();
            }
        }

        public ObservableCollection<User> FilteredUsers
        {
            get => _filteredUsers;
            private set
            {
                _filteredUsers = value;
                OnPropertyChanged(nameof(FilteredUsers));

            }
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
                UpdateFilteredUsers();
            }
        }

        public string SelectedSortOption
        {
            get => _selectedSortOption;
            set
            {
                _selectedSortOption = value;
                OnPropertyChanged(nameof(SelectedSortOption));

                // Сначала обновляем отфильтрованные пользователи, чтобы получить актуальный список
                UpdateFilteredUsers(); // Это обновит FilteredUsers на основе текущих данных

                // Теперь вызываем сортировку с актуальными данными
                SortUsers(FilteredUsers.AsQueryable()); // Передаем отфильтрованный список
            }
        }

        public ObservableCollection<string> SortOptions { get; } = new ObservableCollection<string>
{  
    "Имя по возрастанию",
    "Фамилия по возрастанию",
    "Номер телефона по возрастанию",
    "Имя по убыванию",
    "Фамилия по убыванию",
    "Номер телефона по убыванию",
     "id по возрастанию"
};

        public UserViewModel()
        {
            Users = new ObservableCollection<User>();
            FilteredUsers = new ObservableCollection<User>();
        }

        public string SelectedFilterOption //выюранный вариарнт фильтрацим
        {
            get => _selectedFilterOption;
            set
            {
                _selectedFilterOption = value;
                OnPropertyChanged(nameof(SelectedFilterOption));
                UpdateFilteredUsers(); // Обновляем пользователей при изменении фильтра
            }
        }
        public void FilterFrisListe()
        {
            App.Window.UpdateViewModel();

        }
        public void UpdateFilteredUsers()
        {

            Users.Clear();
            if (Users == null) return; // Проверка на null

            var filtered = Users.AsQueryable();
            using (var dbContext = new MediclcentContext())
            {
                var usersFromDb = dbContext.Users.ToList(); // Получаем пользователей из базы данных
                
                foreach (var user in usersFromDb)
                {
                    Users.Add(user); // Добавляем пользователей в коллекцию ViewModel
                }
            }
            // Фильтрация по строке поиска
            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                filtered = filtered.Where(u =>
                    u.Firstname.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.Lastname.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    u.PhoneNumber.Contains(SearchTerm));
            }

            // Фильтрация по возрасту
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            switch (SelectedFilterOption)
            {
                case "18":
                    filtered = filtered.Where(u =>
                        (today.Year - u.Age.Year >= 18) && (today.Year - u.Age.Year <= 30));
                    break;
                case "30-52 года":
                    filtered = filtered.Where(u =>
                        (today.Year - u.Age.Year > 30) && (today.Year - u.Age.Year <= 52));
                    break;
                case "52 года и старше":
                    filtered = filtered.Where(u =>
                        (today.Year - u.Age.Year > 52));
                    break;
                default:
                    break; // Все пользователи
            }

            // Применяем сортировку
            SortUsers(filtered); // Передаем отфильтрованный список
            
            FilteredUsers = new ObservableCollection<User>(filtered.ToList());
        }

        private void SortUsers(IQueryable<User> users)
        {
            switch (SelectedSortOption)
            {
                case "Имя по возрастанию":
                    users = users.OrderBy(u => u.Firstname);
                    break;
                case "Фамилия по возрастанию":
                    users = users.OrderBy(u => u.Lastname);
                    break;
                case "Номер телефона по возрастанию":
                    users = users.OrderBy(u => u.PhoneNumber);
                    break;
                case "Имя по убыванию":
                    users = users.OrderByDescending(u => u.Firstname);
                    break;
                case "Фамилия по убыванию":
                    users = users.OrderByDescending(u => u.Lastname);
                    break;
                case "Номер телефона по убыванию":
                    users = users.OrderByDescending(u => u.PhoneNumber);
                    break;
                case "id по возрастанию":
                    users = users.OrderBy(u => u.IdUsers);
                    break;
                default:
                    break;
            }

            
            FilteredUsers = new ObservableCollection<User>(users.ToList());
        }
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class PrisListAdmin : Window
    {

        private UserViewModel _viewModel;
      
        private Emplo _thisSatrudnik;




        public PrisListAdmin()
        {

            App.Window = this;
            InitializeComponent(); 
            this.DataContext = TekUser.CurrentUser;
            DisplayUserInfo();

            UpdateViewModel();
        }

        public void UpdateViewModel()
        {
            var userControl = new UsControl();

            _viewModel = new UserViewModel();

       
            // Установка DataContext
            DataContext = _viewModel;

            LoadUsers(); // Загрузка пользователей

            filterComboBox.SelectedIndex = 0; // Установка начального значения для фильтрации
            sortComboBox.SelectedIndex = 6;
        }



        private void LoadUsers()
        {
            using (var dbContext = new MediclcentContext())
            {
                var usersFromDb = dbContext.Users.ToList(); // Получаем пользователей из базы данных
                foreach (var user in usersFromDb)
                {
                    _viewModel.Users.Add(user); // Добавляем пользователей в коллекцию ViewModel
                }
            }

            // Обновляем фильтрованные пользователи
            _viewModel.UpdateFilteredUsers();
        }
       
        private void DisplayUserInfo()

        {

            var user = TekUser.CurrentUser;
            this.Title = $"{user.lasteNameTekUser} {user.NameTekUser} {user.Patronomick} Администратор";
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
            
            filterComboBox.SelectedIndex = 0;
            sortComboBox.SelectedIndex = 6;
            searchTextBox.Text = "";
         
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (filterComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
              

                if (_viewModel != null)
                {
                    _viewModel.SelectedFilterOption = selectedItem.Content.ToString();
                }
                else
                {
                }
            }
        }



        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void searchTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text) || (searchTextBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }
            if (e.Text.Length > 0 && searchTextBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }
        }

        private void Addbutton_Click(object sender, RoutedEventArgs e)
        {
            AddUserAwtoComplected addUserAwtoComplected = new AddUserAwtoComplected();
            addUserAwtoComplected.Show();
            Hide();
        }
    }
}
