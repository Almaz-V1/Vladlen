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
using WordDocument = DocumentFormat.OpenXml.Wordprocessing.Document;
using WordBody = DocumentFormat.OpenXml.Wordprocessing.Body;
using WordParagraph = DocumentFormat.OpenXml.Wordprocessing.Paragraph;
using WordRun = DocumentFormat.OpenXml.Wordprocessing.Run;
using WordText = DocumentFormat.OpenXml.Wordprocessing.Text;
using WordRunProperties = DocumentFormat.OpenXml.Wordprocessing.RunProperties;
using WordBold = DocumentFormat.OpenXml.Wordprocessing.Bold;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Microsoft.EntityFrameworkCore;
namespace Clinic_1_.PrisListt
{
    /// <summary>
    /// Логика взаимодействия для DocumentOutput.xaml
    /// </summary>
    public partial class DocumentOutput : Window
    {
        public List<Specialist> GetProfessions()
        {
            using (var context = new MediclcentContext())
            {
                return context.Specialists.ToList(); // Получаем все профессии
            }
        }
        private Emplo satrudneke;

        private Emplo _thisSatrudnik;
        private User us;
        private MediclcentContext dbContext;

        private List<string> sortList = new List<string>
        {
            "Имя по возрастанию",
            "Фамилия по возрастанию",
            "Отчество по возрастанию",
            "Номер телефона по возрастанию",
            "Имя по убыванию",
            "Фамилия по убыванию",
            "Отчество по убыванию",
            "Номер телефона по убыванию"

        };
        public DocumentOutput()
        {
            dbContext = new MediclcentContext();
          
            this.DataContext = TekUser.CurrentUser;

            DisplayUserInfo();
            InitializeComponent();
            filterComboBox.ItemsSource = dbContext.Specialists.Select(e => e.TitleSpecialist).ToList();
            sortComboBox.ItemsSource = sortList;

            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = -1;
        }
        private void DisplayUserInfo()
        {
            var user = TekUser.CurrentUser;
            this.Title = $"{user.lasteNameTekUser} {user.NameTekUser} {user.Patronomick} Администратор";
        }
        private void LoadUsers()
        {
            using (var context = new MediclcentContext()) // Убедитесь, что это ваш контекст базы данных
            {
                var users = context.Emploes.ToList(); // Извлекаем всех пользователей из базы данных
               
            }
            var professions = GetProfessions();
            dbContext.Emploes.Include(r => r.NameIdRolesNavigation).Load();

        }



        private void LotRegkick(object sender, RoutedEventArgs e)
        {

        }

        private void PanelRaz_Click(object sender, RoutedEventArgs e)
        {

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
            UpdateUsersList();

        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();

        }
        private void UpdateUsersList()
        {
            dbContext = new MediclcentContext();

            var staffesList = dbContext.Emploes.Include(e => e.IdProfeshensNavigation).ToList();

            staffesList = ApplyFilter(staffesList);
            staffesList = ApplySort(staffesList);
            staffesList = ApplySearch(staffesList);

            dataGridObject.ItemsSource = null;
            dataGridObject.ItemsSource = staffesList;
        }
        private List<Emplo> ApplyFilter(List<Emplo> staffesList)
        {
            if (filterComboBox.SelectedIndex != -1)
            {
                string nameRole = filterComboBox.SelectedValue.ToString();
                staffesList = staffesList.Where(e => e.IdProfeshensNavigation.TitleSpecialist == nameRole).ToList();
            }

            return staffesList;
        }

        private List<Emplo> ApplySort(List<Emplo> staffesList)
        {
            int sortIndex = sortComboBox.SelectedIndex;

            if (sortComboBox.SelectedIndex != -1)
            {
                switch (sortIndex)
                {
                    case 0:
                        return staffesList.OrderBy(e => e.Name).ToList();


                    case 1:
                        return staffesList.OrderBy(e => e.LasteName).ToList();

                    case 2:
                        return staffesList.OrderBy(e => e.MiddleName).ToList();

                    case 3:
                        return staffesList.OrderBy(e => e.PhoneNumber).ToList();

                    case 4:
                        return staffesList.OrderByDescending(e => e.Name).ToList();

                    case 5:
                        return staffesList.OrderByDescending(e => e.LasteName).ToList();

                    case 6:
                        return staffesList.OrderByDescending(e => e.MiddleName).ToList();

                    case 7:
                        return staffesList.OrderByDescending(e => e.PhoneNumber).ToList();
                }
            }
            return staffesList;
        }

        private List<Emplo> ApplySearch(List<Emplo> staffesList)
        {
            string searchText = searchTextBox.Text.ToLower();
            if (searchText != string.Empty)
            {
                staffesList = staffesList.Where(r => r.Name.ToLower().StartsWith(searchText)
                || r.LasteName.ToLower().StartsWith(searchText)).ToList();
            }
            return staffesList;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var professions = GetProfessions();
            dbContext.Emploes.Include(r => r.NameIdRolesNavigation).Load();
        }

        private void CartListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridObject.SelectedItem != null)
            {
                satrudneke = (Emplo)dataGridObject.SelectedItem;
                LastNameBox.Content =  satrudneke.LasteName;
                NameBox.Content = satrudneke.Name;
                PatronomickBox.Content = satrudneke.MiddleName;
                CabinNumber.Content = satrudneke.CabinetNumber;
                DolchnostBox.Content = satrudneke.IdProfeshens.ToString();
               
            }
        }
    }
}
