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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Clinic_1_.Add;
using Clinic_1_.PrisListt;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
namespace Clinic_1_
{
    /// <summary>
    /// Логика взаимодействия для MenuAdmin.xaml
    /// </summary>
    public partial class MenuAdmin : Window
    {

        private Emplo _thisSatrudnik;
        private MediclcentContext dbContext;
        private User avtorez;
        
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
    
        public MenuAdmin()
        {


            
            InitializeComponent();
   

            dbContext = new MediclcentContext();
            DisplayUserInfo();
            this.DataContext = TekUser.CurrentUser;

            filterComboBox.ItemsSource = dbContext.Roles.Select(e => e.Title).ToList();
            sortComboBox.ItemsSource = sortList;

            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = -1;
        }
        private void DisplayUserInfo()
        {
           
            
            NumberRole.Content = "Администратор";
        }
        private void AddEmploes_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbContext.Emploes.Include(r => r.NameIdRolesNavigation).Load();
            UpdateUsersList();
        }

        private void cartButton_Click(object sender, RoutedEventArgs e)
        {
            
            AddEmploes addEmploes = new AddEmploes();
            addEmploes.Show();
            Hide();
        }


        private void exitButton_Click(object sender, RoutedEventArgs e)

        {
          
            Authorization authorization = new Authorization();

            Close();
            authorization.Show();
        }

        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = -1;
            searchTextBox.Text = string.Empty;

            UpdateUsersList();
        }

        private void filterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void sortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void UpdateUsersList()
        {
            dbContext = new MediclcentContext();

            var staffesList = dbContext.Emploes.Include(e => e.NameIdRolesNavigation).ToList();

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
                staffesList = staffesList.Where(e => e.NameIdRolesNavigation.Title == nameRole).ToList();
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
                        return staffesList.OrderByDescending(e => e.PhoneNumber).ToList();                }
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

        private void updateButton_Click(object sender, RoutedEventArgs e)
        {

            // Создание нового окна с передачей текущего сотрудника
            PrisListAdmin proverkaFm = new PrisListAdmin();
            proverkaFm.Show();
            Hide();
        
            
        }

        private void PanelRaz_Click(object sender, RoutedEventArgs e)
        {
            DocumentOutput documentOutput = new DocumentOutput();
            documentOutput.Show();
            Hide();

        }
    }
}