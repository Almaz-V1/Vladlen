using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Clinic_1_.menu;
using Clinic_1_.Model2;

namespace Clinic_1_
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        
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

        public Authorization()
        {
            InitializeComponent();
        }
        private User Authenticate(string login, string password)
        {
            using (var context = new MediclcentContext()) // Замените на ваш контекст
            {
                return context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            }
        }
        private Emplo Authenticate2(string login, string password)
        {
            using (var context = new MediclcentContext()) // Замените на ваш контекст
            {
                return context.Emploes.FirstOrDefault(u => u.Emaile == login && u.Password == password);
            }
        }

        private void AutButton_Click(object sender, RoutedEventArgs e)
        {

            string log = loginText.Text;
            string pastext = PasswordTextBox.Text;
            string pass = PasswordBox1.Password;
            
            if (Personalisation(log, pastext, pass) == true && ProvetkaTextBox(log, pastext, pass) == true)
            {
                var (satrudneke, user) = Authtorization(log, pastext, pass);
                if (satrudneke != null)
                {
                    if (satrudneke.NameIdRoles == 1)
                    {
                        TekUser.CurrentUser.NameTekUser = satrudneke.Name;
                        TekUser.CurrentUser.lasteNameTekUser = satrudneke.LasteName;
                        TekUser.CurrentUser.Patronomick = satrudneke.MiddleName;
                        
                        Emplo authenticatedUser2 = Authenticate2(loginText.Text, PasswordBox1.Password);
                        MessageBox.Show($"Добро пожаловать вы вошли как,{" Администратор"} {satrudneke.Name} {satrudneke.LasteName} {satrudneke.MiddleName} !");
                        MenuAdmin menuAdmin = new MenuAdmin();
                        menuAdmin.Show();
                    }
                    else
                    {
                        TekUser.CurrentUser.NameTekUser = satrudneke.Name;
                        TekUser.CurrentUser.lasteNameTekUser = satrudneke.LasteName;
                        TekUser.CurrentUser.Patronomick = satrudneke.MiddleName;
                        MessageBox.Show($"Добро пожаловать вы вошли как, {"Врач"} {satrudneke.Name} {satrudneke.LasteName} {satrudneke.MiddleName} !");
                        MenuDoctors menuDoctors = new MenuDoctors();
                        menuDoctors.Show();
                    }
                    this.Close();
                }
                else if (user != null)
                {
                    TekUser.CurrentUser.NameTekUser = satrudneke.Name;
                    TekUser.CurrentUser.lasteNameTekUser = satrudneke.LasteName;
                    TekUser.CurrentUser.Patronomick = satrudneke.MiddleName;
                    MessageBox.Show($"Рады вас видеть вы вошли как, {"Пациент"}{user.Lastname} {user.Firstname} {user.Partominyc} !");
                    MenuDoctors doctors = new MenuDoctors();
                    doctors.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Пользователь не найден");
                }
            }
          

           
        }
    
        private bool Personalisation(string login, string passwordText, string password)
        {
            if (string.IsNullOrWhiteSpace(password) && string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Заполните поля", "Ошибка.");
                return false;
            }
            else if (string.IsNullOrWhiteSpace(login))
            {
                MessageBox.Show("Пожалуйста, введите логин.");
                return false; // Exit the method to prevent further processing
            }
            else if (string.IsNullOrWhiteSpace(password))
            {

                MessageBox.Show("Пожалуйста, введите пароль.");
                return false; // Exit the method to prevent further processing
            }

            else
            {
                return true;
                
            }
        }
        private bool ProvetkaTextBox(string login, string passwordText, string password)
        {
            if (!AreAllValidChars1(login) && !AreAllValidChars1(passwordText) && !AreAllValidChars2(password)){
                MessageBox.Show("Введены неверные значения", "Ошибка");
                loginText.Clear();
                PasswordTextBox.Clear();
                PasswordBox1.Clear();
                return false;
            }
            else if(!AreAllValidChars1(login))
            {
                MessageBox.Show("Логин содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                loginText.Clear(); // Очищаем TextBox
                return false;
            }
            else if (!AreAllValidChars1(passwordText))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordTextBox.Clear(); // Очищаем TextBox
                return false;
            }
            else if (!AreAllValidChars2(password))
            {
                MessageBox.Show("Пароль содержит ошибку(и), попробуйте изменить его.", "Ошибка");
                PasswordBox1.Clear(); // Очищаем TextBox
                return false;
            }
            else
            {
    
                return true;
            }
        }
        private (Emplo satrudneke, User user) Authtorization(string login, string passwordText, string password)
        {
            Emplo satrudneke = App.context.Emploes.ToList().Find(u => u.Password == password && u.Emaile == login);

            User user = App.context.Users.ToList().Find(u => u.Password == password && u.Login == login);
            return (satrudneke, user);
           
        }
        private bool AreAllValidChars1(string text)
        {

            foreach (char c in text)
            {
                if (loginText.Text.Length > 20)
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
                if (PasswordBox1.Password.Length > 20)
                {

                    return false;

                }
            }
            return true; // Все символы допустимы
        }
        private void ViewPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Показываем пароль в TextBox и скрываем PasswordBox
            PasswordTextBox.Text = PasswordBox1.Password;
            PasswordTextBox.Visibility = Visibility.Visible;
            PasswordBox1.Visibility = Visibility.Hidden;
        }
        private void PasswordTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            // Обновляем пароль в PasswordBox при изменении текста в TextBox
            if (ViewPasswordCheckBox.IsChecked == true)
            {
                PasswordBox1.Password = PasswordTextBox.Text;
            }
        }

        private void ViewPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Скрываем TextBox и показываем PasswordBox
            PasswordBox1.Password = PasswordTextBox.Text; // Сохраняем введенный текст в PasswordBox
            PasswordTextBox.Visibility = Visibility.Hidden;
            PasswordBox1.Visibility = Visibility.Visible;
        }

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {
            RegistrWindow registr = new RegistrWindow();
            registr.Show();
            Hide();
        }


        private void loginText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void MainTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            var pas = sender as PasswordBox;
            if (e.Key == Key.Space)
            {
                e.Handled = true; // Запретить ввод пробела
                return;
            }
            if (textBox == loginText)
            {
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }
            else if (textBox == PasswordTextBox)
            {
                if (textBox.Text.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }
            else if (pas == PasswordBox1)
            {
                if (pas.Password.Length >= 20 && e.Key != Key.Back)
                {
                    e.Handled = true; // Запретить ввод, если превышена длина
                }
            }

        }


    }
}