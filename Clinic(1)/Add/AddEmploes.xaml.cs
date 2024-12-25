using Clinic_1_.Model2;
using Clinic_1_.menu;
using Microsoft.EntityFrameworkCore;
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
using System.Runtime.Intrinsics.X86;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace Clinic_1_.Add
{

    public partial class AddEmploes : Window
    {

        public List<Specialist> GetProfessions()
        {
            using (var context = new MediclcentContext())
            {
                return context.Specialists.ToList(); // Получаем все профессии
            }
        }
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
        private MediclcentContext dbContext;
        private Emplo satrudneke;
        private Emplo _thisSatrudnik;
        string idSTEx;
        public AddEmploes()
        {
            InitializeComponent();
            dbContext = new MediclcentContext();
            _thisSatrudnik = satrudneke;
            this.DataContext = TekUser.CurrentUser;
            DisplayUserInfo();
            filterComboBox.ItemsSource = dbContext.Roles.Select(e => e.Title).ToList();
            sortComboBox.ItemsSource = sortList;

            filterComboBox.SelectedIndex = -1;
            sortComboBox.SelectedIndex = -1;
        }

        private void DisplayUserInfo()
        {
            var user = TekUser.CurrentUser;
            this.Title = $"{user.lasteNameTekUser} {user.NameTekUser} {user.Patronomick} Администратор";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            var professions = GetProfessions(); // Получаем профессии из базы
            if (professions != null && professions.Count > 0)
            {
                comboBoxProfessions.ItemsSource = professions; // Устанавливаем источник данных для комбобокса
            }
            else
            {
                MessageBox.Show("Нет доступных профессий.", "Ошибка");
            }
            dbContext.Emploes.Include(r => r.NameIdRolesNavigation).Load();
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


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridObject.SelectedItem != null)
            {
                satrudneke = (Emplo)dataGridObject.SelectedItem;
                NameBox.Text = satrudneke.Name;
                LasteNameBox.Text = satrudneke.LasteName;
                PatronymicBox.Text = satrudneke.MiddleName;
                EmailBox.Text = satrudneke.Emaile;
                PhoneNumberBox.Text = satrudneke.PhoneNumber.ToString();
                IdDojnostBox.Text = satrudneke.NameIdRoles.ToString();
                PasswordBox.Text = satrudneke.Password;
                idSTEx = satrudneke.IdDoctors.ToString();
                PasportSer.Text = satrudneke.SeriaPasport;
                PasportNomer.Text = satrudneke.NomerPasport;
                NumberKabin.Text = satrudneke.CabinetNumber.ToString();
                comboBoxProfessions.SelectedValue = satrudneke.IdProfeshens;
                BirthdayPicker.SelectedDate = new DateTime(satrudneke.Age.Year, satrudneke.Age.Month, satrudneke.Age.Day);
            }

        }
        
        private void searchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateUsersList();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (satrudneke == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления.", "Ошибка");
                return;
            }
            if (satrudneke.IdDoctors == _thisSatrudnik.IdDoctors)
            {
                MessageBox.Show("Вы не можете себя удалить!!!", "Ошибка");
                return;
            }
            MessageBoxResult result = MessageBox.Show("Вы точно хотите удалить данного сотрудника?", "Подверждение удаление", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                dbContext.Emploes.Remove(satrudneke);
                dbContext.SaveChanges();


                dbContext.SaveChanges();
                NameBox.Clear();
                LasteNameBox.Clear();
                PatronymicBox.Clear();
                BirthdayPicker.SelectedDate = null;
                EmailBox.Clear();
                PhoneNumberBox.Clear();
                PasswordBox.Clear();
                PasportNomer.Clear();
                PasportSer.Clear();
                NumberKabin.Clear();
                IdDojnostBox.Clear();
                dataGridObject.Items.Refresh();
                comboBoxProfessions.SelectedIndex = -1;
                satrudneke = null;
                UpdateUsersList();
                MessageBox.Show("Удаление прошло успешно", "Сообщение");

            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string SurnameUser1 = LasteNameBox.Text;
            string NameUser1 = NameBox.Text;
            string PatronymicUser1 = PatronymicBox.Text;
            DateTime BirthdayPicker1 = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            DateOnly birthday = DateOnly.FromDateTime(BirthdayPicker1);
            string NumberUser1 = PhoneNumberBox.Text;
            string PasswordUser1 = PasswordBox.Text;
            string LoginUser1 = EmailBox.Text;
            string PassportUser1 = IdDojnostBox.Text;
            string id = idSTEx;
            string pasportser = PasportSer.Text;
            string pasportnomer = PasportNomer.Text;
            string kabinet = NumberKabin.Text;
            int selectedProfessionId = (int)comboBoxProfessions.SelectedValue;

            AddUserProverka(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);
        }
        private void AddUserProverka(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet, int selectedProfessionId)
        {
            Emplo user1 = App.context.Emploes.ToList().Find(u => u.Emaile == EmailBox.Text);
            Emplo user2 = App.context.Emploes.ToList().Find(u => u.PhoneNumber.ToString() == PhoneNumberBox.Text);
            Emplo pasport = App.context.Emploes.ToList().Find(u => u.NomerPasport == PasportNomer.Text && u.SeriaPasport == PasportSer.Text);
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Имя.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(LasteNameBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Фамилию.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(EmailBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Логин.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(NumberKabin.Text))
            {
                MessageBox.Show("Пожалуйста, номер кабинета.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PhoneNumberBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Номер телефона.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PasportSer.Text))
            {
                MessageBox.Show("Пожалуйста, введите серию распорта.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PasportNomer.Text))
            {
                MessageBox.Show("Пожалуйста, введите номер распорта.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Пароль.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(IdDojnostBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите Номер сотрудника.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (!AreAllValidCharsID(IdDojnostBox.Text))
            {
                MessageBox.Show("В номере сотрудника ошибка(и), сотрудник 1-администратор,сотрудник 2-менеджер " +
                    ".", "Ошибка");
                IdDojnostBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }
            else if (user1 != null)
            {
                MessageBox.Show(" Уже есть пользователь с таким логином", "Ошибка");
                return;
            }
            else if (user2 != null)
            {
                MessageBox.Show(" Уже есть пользователь с таким номерам телефона.", "Ошибка");
                return;
            }
            else if (pasport != null)
            {
                MessageBox.Show(" Уже есть пользователь с такими паспортными данными.", "Ошибка");
                return;
            }

            else if (user1 == null && user2 == null && pasport == null)
            {
                ProvekaSyntax(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);
            }
            else
            {
                MessageBox.Show("Неверно введены данные","Ошибка");
                return;
            }

        }
        private bool AreAllValidCharsKabinet(string text)
        {
            // Проверяем, что текст содержит только цифры
           
                int plusCount = 0;
                // Проверяем, что текст содержит только цифры
           if (IdDojnostBox.Text == "1")
            {
                return true;
            }
            else
            {
                foreach (char c in text)
                {
                    if (!char.IsDigit(c) || NumberKabin.Text.Length > 4)
                    {
                        //Есле не цифра или длина больше 4
                        return false;

                    }
                }
                return true; // Все символы допустимы
            }
         
        }
         
        
        private void LasteNameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text) || (LasteNameBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }

            if (!string.IsNullOrEmpty(e.Text) && LasteNameBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }

        }
        private void NameBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text) || (NameBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }
            if (!string.IsNullOrEmpty(e.Text) && NameBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }
        }
        private void PatronymicBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^а-яА-ЯёЁ]");
            if (regex.IsMatch(e.Text) || (PatronymicBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }
            if (e.Text.Length > 0 && PatronymicBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }
        }
        private void ProvekaSyntax(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet, int selectedProfessionId)
        {


            if (!AreAllValidChars5(PasswordUser1))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars4(LoginUser1))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 25, минримальная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                EmailBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6(SurnameUser1))
            {
                MessageBox.Show("в фамиоии содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                LasteNameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars7(NameUser1))
            {
                MessageBox.Show("в имени содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars8(PatronymicUser1))
            {
                MessageBox.Show("в очестве содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PatronymicBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars12345(pasportser))
            {
                MessageBox.Show("Серия паспорта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasportSer.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6789(pasportnomer))
            {
                MessageBox.Show("Номер паспорта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasportNomer.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsKabinet(kabinet))
            {
                MessageBox.Show("В номере кабинета ошибка. ", "Ошибка");
                NumberKabin.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPhone(NumberUser1))
            {
                MessageBox.Show("В номере телефона содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PhoneNumberBox.Clear(); // Очищаем TextBox
                return;
            }

            else
            {
                AddUser(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);
            }

        }
        private void AddUser(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet,int selectedProfessionId)
        {

            var newSatrudnik = new Emplo
            {
                Name = NameUser1,
                LasteName = SurnameUser1,
                MiddleName = PatronymicUser1,
                Age = birthday,
                Emaile = LoginUser1,
                NameIdRoles = Convert.ToInt32(PassportUser1),
                Password = PasswordUser1,
                PhoneNumber = NumberUser1,
                IdProfeshens = selectedProfessionId,
                SeriaPasport = pasportser,
                NomerPasport = pasportnomer,
                CabinetNumber = kabinet,
                Image = null
            };

            dbContext.Emploes.Add(newSatrudnik);
            dbContext.SaveChanges();
            NameBox.Clear();
            LasteNameBox.Clear();
            PatronymicBox.Clear();
            BirthdayPicker.SelectedDate = null;
            EmailBox.Clear();
            PhoneNumberBox.Clear();
            PasswordBox.Clear();
            PasportNomer.Clear();
            IdDojnostBox.Clear();
            PasportSer.Clear();
            PasportNomer.Clear();
            NumberKabin.Clear();
            comboBoxProfessions.SelectedIndex = -1;
            UpdateUsersList();
            MessageBox.Show("Добавление прошло успешно ", "Сообщение");
        }
        private bool AreAllValidChars6789(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsDigit(c) || PasportNomer.Text.Length > 6)
                {

                    return false;
                }
                if (PasportNomer.Text.Length < 6)
                        {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars12345(string text)
        {
            int plusCount = 0;
            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || PasportSer.Text.Length > 4)
                {
                    //Есле не цифра или длина больше 4
                    return false;

                }
                if (PasportSer.Text.Length < 4)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidCharsID(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || IdDojnostBox.Text.Length > 1)
                {
                    return false; // Найден недопустимый символ
                }
                if (c.ToString() != "1" && c.ToString() != "2")
                {
                    return false; // Найден недопустимый символ
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars8(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsLetter(c) || PatronymicBox.Text.Length > 20)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars7(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsLetter(c) || LasteNameBox.Text.Length > 20)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars6(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsLetter(c) || NameBox.Text.Length > 20)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars5(string text)
        {
            if (PasswordBox.Text.Length > 25)
            {
                return false;
            }
            else
            {
                return true; // Все символы допустимы
            }
        }

        private bool AreAllValidChars4(string text)
        {
            foreach (char c in text)
            {
                if (EmailBox.Text.Length > 25 || EmailBox.Text.Length < 10)
                {
                    return false;
                }
            }

            if (!EmailBox.Text.Contains('@') || !EmailBox.Text.Contains('.')) return false;

            return true; // Все символы допустимы
        }

    

        private bool AreAllValidChars(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || PhoneNumberBox.Text.Length > 11)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars2(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || IdDojnostBox.Text.Length > 4)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidCharsKabin(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || NumberKabin.Text.Length > 4)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            string SurnameUser1 = LasteNameBox.Text;
            string NameUser1 = NameBox.Text;
            string PatronymicUser1 = PatronymicBox.Text;
            DateTime BirthdayPicker1 = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            DateOnly birthday = DateOnly.FromDateTime(BirthdayPicker1);
            string NumberUser1 = PhoneNumberBox.Text;
            string PasswordUser1 = PasswordBox.Text;
            string LoginUser1 = EmailBox.Text;
            string PassportUser1 = IdDojnostBox.Text;
            string id = idSTEx;
            string pasportser = PasportSer.Text;
            string pasportnomer = PasportNomer.Text;
            string kabinet = NumberKabin.Text;
            int selectedProfessionId = (int)comboBoxProfessions.SelectedValue;
            ProverkaTextEditClik(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);
        }
        private void ProverkaTextEditClik(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet,int selectedProfessionId)
        {
            if (string.IsNullOrWhiteSpace(NameUser1))
            {
                MessageBox.Show("Пожалуйста, введите Имя.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(SurnameUser1))
            {
                MessageBox.Show("Пожалуйста, введите Фамилию.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(LoginUser1))
            {
                MessageBox.Show("Пожалуйста, введите Логин." , "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(kabinet))
            {
                MessageBox.Show("Пожалуйста, номер кабинета.");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(NumberUser1))
            {
                MessageBox.Show("Пожалуйста, введите Номер телефона.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(pasportser))
            {
                MessageBox.Show("Пожалуйста, введите серию распорта.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(pasportnomer))
            {
                MessageBox.Show("Пожалуйста, введите номер распорта.", "Ошибка");
                return; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(PasswordUser1))
            {
                MessageBox.Show("Пожалуйста, введите Пароль.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (string.IsNullOrWhiteSpace(PassportUser1))
            {
                MessageBox.Show("Пожалуйста, введите Номер сотрудника.", "Ошибка");
                return; // Exit the method to prevent further processing
            }

            else if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }

            else 
            {
                ProverkaAddChange(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);
            }
        }
        private void ProverkaAddChange(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet, int selectedProfessionId)
        {

            uint TolkoUserId = Convert.ToUInt32(idSTEx); // ID текущего пользователя

            // Проверка на существование пользователя с таким же логином
            bool emailExists = App.context.Emploes.Any(u => u.Emaile == EmailBox.Text && u.IdDoctors != TolkoUserId);

            // Проверка на существование пользователя с таким же номером телефона
            bool phoneExists = App.context.Emploes.Any(u => u.PhoneNumber == PhoneNumberBox.Text && u.IdDoctors != TolkoUserId);

            // Проверка на существование пользователя с такими же паспортными данными
            bool passportExists = App.context.Emploes.Any(u => u.NomerPasport == PasportNomer.Text && u.SeriaPasport == PasportSer.Text && u.IdDoctors != TolkoUserId);

            // Проверка на выбор даты
            if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }

            // Проверка на существование пользователей с такими данными
            if (emailExists)
            {
                MessageBox.Show("Уже есть пользователь с таким логином", "Ошибка");
                return;
            }

            if (phoneExists)
            {
                MessageBox.Show("Уже есть пользователь с таким номерам телефона.", "Ошибка");
                return;
            }

            if (passportExists)
            {
                MessageBox.Show("Уже есть пользователь с такими паспортными данными.", "Ошибка");
                return;
            }

            // Если все проверки пройдены, можно добавлять или изменять данные
            ProverkaCtrlV(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);
        }
       
        private void ProverkaCtrlV(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet, int selectedProfessionId)
        {
            if (!AreAllValidChars5(PasswordUser1))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsID(PassportUser1))
            {
                MessageBox.Show("В номере сотрудника ошибка(и), сотрудник 1-администратор,сотрудник 2-менеджер " +
                    ".", "Ошибка");
                IdDojnostBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }
            else if (!AreAllValidChars4(LoginUser1))
            {
                MessageBox.Show("В почте содержится ошибка(и), попробуйте изменить его,(Максимальная длина 25, минримальная 10, точка и @ обезательно должны присутствовать", "Ошибка");
                EmailBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6(SurnameUser1))
            {
                MessageBox.Show("в фамиоии содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                LasteNameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars7(NameUser1))
            {
                MessageBox.Show("в имени содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                NameBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars8(PatronymicUser1))
            {
                MessageBox.Show("в очестве содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PatronymicBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars12345(pasportser))
            {
                MessageBox.Show("Серия паспорта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasportSer.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6789(pasportnomer))
            {
                MessageBox.Show("Номер паспорта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasportNomer.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsKabinet(kabinet))
            {
                MessageBox.Show("В номере кабинета ошибка. ", "Ошибка");
                NumberKabin.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPhone(NumberUser1))
            {
                MessageBox.Show("В номере телефона содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                PhoneNumberBox.Clear(); // Очищаем TextBox
                return;
            }
            else
            {
                Add(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, PassportUser1, id, pasportser, pasportnomer, kabinet, selectedProfessionId);

            }
        }
        private bool AreAllValidCharsPhone(string text)
        {
            foreach (char c in text)
            {
                if (PhoneNumberBox.Text.Length < 12)
                {

                    return false;

                }
               
            }
            return true; // Все символы допустимы
        }
        private void Add(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string PassportUser1, string id, string pasportser, string pasportnomer, string kabinet, int selectedProfessionId)
        {
            if (satrudneke.IdDoctors == _thisSatrudnik.IdDoctors)
            {

                MessageBoxResult result = MessageBox.Show("Вы точно хотите редактировать свои данные?", "Подверждение редактирование", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    satrudneke.Name = NameUser1;
                    satrudneke.LasteName = SurnameUser1;
                    satrudneke.MiddleName = PatronymicUser1;
                    satrudneke.Age = birthday;
                    satrudneke.Emaile = LoginUser1;
                    satrudneke.Password = PasswordUser1;
                    satrudneke.PhoneNumber = NumberUser1;
                    satrudneke.NameIdRoles = Convert.ToInt32(PassportUser1);
                    satrudneke.NomerPasport = pasportnomer;
                    satrudneke.SeriaPasport = pasportser;
                    satrudneke.CabinetNumber = kabinet;
                    satrudneke.IdProfeshens = selectedProfessionId;
                    dbContext.SaveChanges();
                    NameBox.Clear();
                    LasteNameBox.Clear();
                    PatronymicBox.Clear();
                    BirthdayPicker.SelectedDate = null;
                    EmailBox.Clear();
                    PhoneNumberBox.Clear();
                    PasswordBox.Clear();
                    PasportNomer.Clear();
                    IdDojnostBox.Clear();
                    PasportSer.Clear();
                    PasportNomer.Clear();
                    NumberKabin.Clear();
                    comboBoxProfessions.SelectedIndex = -1;

                    dataGridObject.Items.Refresh();
                    UpdateUsersList();
                    MessageBox.Show("Редактирование прошло успешно", "Сообщение");
                }
                else
                {
                    return;
                }
            }     
            else
            {
                satrudneke.Name = NameUser1;
                satrudneke.LasteName = SurnameUser1;
                satrudneke.MiddleName = PatronymicUser1;
                satrudneke.Age = birthday;
                satrudneke.Emaile = LoginUser1;
                satrudneke.Password = PasswordUser1;
                satrudneke.PhoneNumber = NumberUser1;
                satrudneke.NameIdRoles = Convert.ToInt32(PassportUser1);
                satrudneke.NomerPasport = pasportnomer;
                satrudneke.SeriaPasport = pasportser;
                satrudneke.CabinetNumber = kabinet;
                satrudneke.IdProfeshens = selectedProfessionId;
                dbContext.SaveChanges();


                dbContext.SaveChanges();
                NameBox.Clear();
                LasteNameBox.Clear();
                PatronymicBox.Clear();
                BirthdayPicker.SelectedDate = null;
                EmailBox.Clear();
                PhoneNumberBox.Clear();
                PasswordBox.Clear();
                PasportNomer.Clear();
                IdDojnostBox.Clear();
                PasportSer.Clear();
                PasportNomer.Clear();
                NumberKabin.Clear();
                comboBoxProfessions.SelectedIndex = -1;

                dataGridObject.Items.Refresh();
                UpdateUsersList();
                MessageBox.Show("Редактирование прошло успешно", "Сообщение");
            }
        }

        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            // Получаем ссылку на текстовое поле
            var textBox = sender as TextBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Запретить ввод пробела
                return;
            }
            // Проверка на пробел
            if (e.Key == Key.Back || e.Key == Key.Tab || e.Key == Key.Enter)
            {

                return; // Allow these keys
            }

            if (textBox == NameBox || textBox == LasteNameBox || textBox == PatronymicBox) // Фамилия, Имя, Отчество
            {

                // Проверка длины для имени, фамилии и отчества (20 символов)
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }

            if (textBox == PasswordBox || textBox == EmailBox) // Логин и Пароль
            {
                // Ограничение на 25 символов
                if (textBox.Text.Length > 24 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }

            }

        }
        private void BirthDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BirthdayPicker.SelectedDate.HasValue)
            {
                DateTime specificDate5 = new DateTime(2006, 10, 22);

                DateTime selectedDate = BirthdayPicker.SelectedDate.Value;
                DateTime today = DateTime.Today;

                // Проверяем, что дата не меньше 18 лет назад и не больше 100 лет назад
                if (selectedDate > today.AddYears(-18))
                {
                    MessageBox.Show("Сотрудник должен быть младше 18 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    BirthdayPicker.SelectedDate = specificDate5; // Сбрасываем выбор
                    return;
                }
                else if (selectedDate < today.AddYears(-100))
                {
                    MessageBox.Show("Сотрудник не может быть старше 100 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    BirthdayPicker.SelectedDate = specificDate5; // Сбрасываем выбор
                    return;
                }

            }
        }
        private void TextBox_PreviewTextInput2(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars(e.Text);
        }
        private void TextBox_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars2(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MenuAdmin menu = new MenuAdmin();
            menu.Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dbContext.Dispose();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PhoneNumberBox.Text.Length < 3 || !PhoneNumberBox.Text.StartsWith("+7"))
            {
                PhoneNumberBox.Text = "+7";
                PhoneNumberBox.SelectionStart = PhoneNumberBox.Text.Length; // Place cursor at the end
            }
        }


        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameBox.Text.Length > 0)
            {
                string text = NameBox.Text;
                NameBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                NameBox.CaretIndex = NameBox.Text.Length;
            }
        }

        private void LasteNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (LasteNameBox.Text.Length > 0)
            {
                string text = LasteNameBox.Text;
                LasteNameBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                LasteNameBox.CaretIndex = LasteNameBox.Text.Length;
            }
        }

        private void PatronymicBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PatronymicBox.Text.Length > 0)
            {
                string text = PatronymicBox.Text;
                PatronymicBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                PatronymicBox.CaretIndex = PatronymicBox.Text.Length;
            }
        }


        private void comboBoxProfessions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void NumberKabin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidCharsKabin(e.Text);
        }
        private void TextBox_PreviewTextNomer(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidCharsNomer(e.Text);
        }
        private void TextBox_PreviewTextSer(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidCharsser(e.Text);
        }
        private bool AreAllValidCharsNomer(string text)
        {
           
                // Проверяем, что текст содержит только цифры
                foreach (char c in text)
                {
                    if (!char.IsDigit(c) ||  PasportNomer.Text.Length > 5)
                    {
                        return false;
                    }
                }
            
                return true; // Все символы допустимы
            
        }
        private bool AreAllValidCharsser(string text)
        {
            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || PasportSer.Text.Length > 3)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }

        private void IdDojnostBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(IdDojnostBox.Text, out int idValue))
            {
                // Проверяем условие: если idValue равно определенному числу (например, 18)
                if (idValue == 1)
                {
                    NumberKabin.Text = "-";
                    NumberKabin.IsEnabled = false;
                    comboBoxProfessions.IsEnabled = false; // Блокируем ComboBox
                    comboBoxProfessions.SelectedValue = 5;
                }
                else
                {
                    NumberKabin.IsEnabled = true;
                    comboBoxProfessions.IsEnabled = true; // Разблокируем ComboBox
                    NumberKabin.Text = "";
                }
            }
            else
            {
                NumberKabin.IsEnabled = true;
                comboBoxProfessions.IsEnabled = true; // Разблокируем ComboBox, если ввод некорректен

                NumberKabin.Text = "";
            }
        }

        private void NumberKabin_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }

        private void idS_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasportNomer_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}