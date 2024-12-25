using Clinic_1_.Model2;
using Clinic_1_.PrisListt;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

namespace Clinic_1_.Add
{
    /// <summary>
    /// Логика взаимодействия для AddUserAwtoComplected.xaml
    /// </summary>
    public partial class AddUserAwtoComplected : Window

    {
        private Emplo satrudneke;
        private User _thisSatrudnik;
        private User user;
        private MediclcentContext dbContext;
        int idUs;
        public AddUserAwtoComplected()
        {
            dbContext = new MediclcentContext();
            _thisSatrudnik = user;
            InitializeComponent();

        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {

            PrisListAdmin prisListAdmin = new PrisListAdmin();
            prisListAdmin.Show();
            Close();
        }

        private void RegButton_Click(object sender, RoutedEventArgs e)
        {

            string SurnameUser1 = LasteNameBox.Text;
            string NameUser1 = NameBox.Text;
            string PatronymicUser1 = PatronomickBox.Text;
            DateTime BirthdayPicker1 = BirthdayPicker.SelectedDate ?? DateTime.MinValue;
            DateOnly birthday = DateOnly.FromDateTime(BirthdayPicker1);
            string NumberUser1 = Phone.Text;
            string PasswordUser1 = PasswordBox.Text;
            string LoginUser1 = LoginNext.Text;
            string pasportser = SerPasport.Text;
            string pasportnomer = NomerPasport.Text;
            ProverkaTextEditClik(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, pasportser, pasportnomer);
        }
        private void ProverkaTextEditClik(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string pasportser, string pasportnomer)
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
                MessageBox.Show("Пожалуйста, введите Логин.", "Ошибка");
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

            else if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }

            else
            {
                ProverkaAddChange(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, pasportser, pasportnomer);
            }
        }
        private void ProverkaAddChange(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string pasportser, string pasportnomer)
        {

            // Проверка на существование пользователя с таким же логином
            User emailExists = App.context.Users.ToList().Find(u => u.Login == LoginUser1);

            Emplo emplo = App.context.Emploes.ToList().Find(u => u.Emaile == LoginUser1);


            // Проверка на существование пользователя с таким же номером телефона
            User phoneExists = App.context.Users.ToList().Find(u => u.PhoneNumber == NumberUser1);

            // Проверка на существование пользователя с такими же паспортными данными
            User passportExists = App.context.Users.ToList().Find(u => u.Password == pasportnomer && u.PasswordSeries == pasportser);

            // Проверка на выбор даты
            if (BirthdayPicker.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату.", "Ошибка");
                return; // Выход из метода, если дата не выбрана
            }

            // Проверка на существование пользователей с такими данными
            if (emailExists != null || emplo != null)
            {
                MessageBox.Show("Уже есть пользователь с таким логином", "Ошибка");
                return;
            }

            if (phoneExists != null)
            {
                MessageBox.Show("Уже есть пользователь с таким номерам телефона.", "Ошибка");
                return;
            }

            if (passportExists != null)
            {
                MessageBox.Show("Уже есть пользователь с такими паспортными данными.", "Ошибка");
                return;
            }

            // Если все проверки пройдены, можно добавлять или изменять данные
            ProverkaCtrlV(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, pasportser, pasportnomer);
        }
        private void ProverkaCtrlV(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string pasportser, string pasportnomer)
        {
            if (!AreAllValidChars5(PasswordUser1))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox.Clear(); // Очищаем TextBox
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
                LoginNext.Clear(); // Очищаем TextBox
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
                PatronomickBox.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars12345(pasportser))
            {
                MessageBox.Show("Серия паспорта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                SerPasport.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidChars6789(pasportnomer))
            {
                MessageBox.Show("Номер паспорта содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                NomerPasport.Clear(); // Очищаем TextBox
                return;
            }
            else if (!AreAllValidCharsPhone(NumberUser1))
            {
                MessageBox.Show("В номере телефона содержится ошибка(и), попробуйте изменить его.", "Ошибка");
                Phone.Clear(); // Очищаем TextBox
                return;
            }
            else
            {
                Add(SurnameUser1, NameUser1, PatronymicUser1, birthday, NumberUser1, PasswordUser1, LoginUser1, pasportser, pasportnomer);

            }
        }
        private void Add(string SurnameUser1, string NameUser1, string PatronymicUser1, DateOnly birthday, string NumberUser1, string PasswordUser1, string LoginUser1, string pasportser, string pasportnomer)
        {
            var newUser = new User
            {
                PhoneNumber = NumberUser1,
                Firstname = NameUser1,
                Lastname = SurnameUser1,
                Partominyc = PatronymicUser1,
                Login = LoginUser1,
                IdRole = 3,
                PasswordSeries = pasportser,
                PasswordNumber = PasswordUser1,
                Password = pasportnomer,
                Age = birthday
            };
            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
            MessageBox.Show("Дабавление просло успешно", "Сообщение");
            PrisListAdmin prisListAdmin = new PrisListAdmin();
            prisListAdmin.Show();
            Close();

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

            if (textBox == NameBox || textBox == LasteNameBox || textBox == PatronomickBox) // Фамилия, Имя, Отчество
            {

                // Проверка длины для имени, фамилии и отчества (20 символов)
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }

            if (textBox == PasswordBox || textBox == LoginNext) // Логин и Пароль
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
                    MessageBox.Show("Пользователь должен быть младше 18 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    BirthdayPicker.SelectedDate = specificDate5; // Сбрасываем выбор
                    return;
                }
                else if (selectedDate < today.AddYears(-100))
                {
                    MessageBox.Show("Пользователь не может быть старше 100 лет.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    BirthdayPicker.SelectedDate = specificDate5; // Сбрасываем выбор
                    return;
                }

            }
        }
        private void TextBox_PreviewTextInput2(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars(e.Text);
        }
        private bool AreAllValidChars6789(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsDigit(c) || NomerPasport.Text.Length > 6)
                {

                    return false;
                }
                if (NomerPasport.Text.Length < 6)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars1313(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || SerPasport.Text.Length > 3)
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
                if (!char.IsDigit(c) || SerPasport.Text.Length > 4)
                {
                    //Есле не цифра или длина больше 4
                    return false;

                }
                if (SerPasport.Text.Length < 4)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidChars8(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsLetter(c) || PatronomickBox.Text.Length > 20)
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
                if (LoginNext.Text.Length > 25 || LoginNext.Text.Length < 10)
                {
                    return false;
                }
            }

            if (!LoginNext.Text.Contains('@') || !LoginNext.Text.Contains('.')) return false;

            return true; // Все символы допустимы
        }



        private bool AreAllValidChars(string text)
        {

            // Проверяем, что текст содержит только цифры
            foreach (char c in text)
            {
                if (!char.IsDigit(c) || Phone.Text.Length > 11)
                {
                    return false;
                }
            }
            return true; // Все символы допустимы
        }
        private bool AreAllValidCharsPhone(string text)
        {
            foreach (char c in text)
            {
                if (Phone.Text.Length < 12)
                {

                    return false;

                }

            }
            return true; // Все символы допустимы
        }

        private void Phone_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars(e.Text);
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
            if (regex.IsMatch(e.Text) || (PatronomickBox.Text.Length + e.Text.Length > 20))
            {
                e.Handled = true;
            }
            if (e.Text.Length > 0 && PatronomickBox.Text.Length > 0 && char.IsUpper(e.Text[0]))
            {
                e.Handled = true;
            }
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Phone.Text.Length < 3 || !Phone.Text.StartsWith("+7"))
            {
                Phone.Text = "+7";
                Phone.SelectionStart = Phone.Text.Length; // Place cursor at the end
            }
        }
        private void LasteNameBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (LasteNameBox.Text.Length > 0)
            {
                string text = LasteNameBox.Text;
                LasteNameBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                LasteNameBox.CaretIndex = LasteNameBox.Text.Length;
            }
        }

        private void NameBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            if (NameBox.Text.Length > 0)
            {
                string text = NameBox.Text;
                NameBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                NameBox.CaretIndex = NameBox.Text.Length;
            }
        }

        private void PatronomickBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (PatronomickBox.Text.Length > 0)
            {
                string text = PatronomickBox.Text;
                PatronomickBox.Text = char.ToUpper(text[0]) + text.Substring(1);
                PatronomickBox.CaretIndex = PatronomickBox.Text.Length;
            }
        }

        private void LoginNext_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void NomerPasport_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars789(e.Text);
        }

        private void SerPasport_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidChars1313(e.Text);
        }

        private void PasswordBox_PreviewTextInput(Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }
        private bool AreAllValidChars789(string text)
        {

            foreach (char c in text)
            {
                if (!char.IsDigit(c) || NomerPasport.Text.Length > 5)
                {

                    return false;
                }
            }
            return true; // Все символы допустимы
        }

        private void NomerPasport_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LoginNext_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void SerPasport_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PasswordBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
