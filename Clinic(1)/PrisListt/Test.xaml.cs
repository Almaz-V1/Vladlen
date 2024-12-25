using Clinic_1_.Make;
using Clinic_1_.Model2;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Clinic_1_.PrisListt
{
    /// <summary>
    /// Логика взаимодействия для Test.xaml
    /// </summary>
    public partial class Test : UserControl
    {
        private UserViewModel _viewModel;
        public event Action UserDeleted;
        private MediclcentContext dbContext;
        private Emplo user;
        private Emplo _thisSatrudnik;
        private PrisListAdmin PrisListadmiN;
         private ObservableCollection<Emplo> _filteredUsers;

        // DependencyProperty для передачи объекта User
        public static readonly DependencyProperty UserProperty =
            DependencyProperty.Register("Emploes", typeof(Emplo), typeof(Test), new PropertyMetadata(null, OnUserChanged));

        public Emplo Emploes
        {
            get => (Emplo)GetValue(UserProperty);
            set => SetValue(UserProperty, value);
        }


        // Конструктор без параметров
        public Test()
        {

            InitializeComponent();
            _viewModel = new UserViewModel();
            dbContext = new MediclcentContext(); // Инициализация контекста базы данных
        }


        private static void OnUserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Test control && e.NewValue is Emplo user)
            {
                control.ControUserPack(user); // Обновляем данные при изменении пользователя
            }
        }

        public void ControUserPack(Emplo user)
        {
            this.user = user;
            if (user == null) return; // Проверка на null

            IdUs.Content = $"Пациент № : {user.IdDoctors}";
            FirstNameUs.Content = $"Имя: {user.Name}"; // Имя
           
        }

        private void Redact_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Remuv_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}