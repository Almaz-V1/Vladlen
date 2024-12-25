using Clinic_1_.Model2;
using DocumentFormat.OpenXml.Bibliography;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Clinic_1_.PrisListt
{
    /// <summary>
    /// Логика взаимодействия для NewUserControlTest.xaml
    /// </summary>
    public partial class NewUserControlTest : UserControl
    {
        private UserViewModel _viewModel;
        public event Action UserDeleted;
        private MediclcentContext dbContext;
        private User user;
        public static readonly DependencyProperty NewUsContol = DependencyProperty.Register("UserType2", typeof(User), typeof(NewUserControlTest), new PropertyMetadata(null, OnChnge));

        public User UserType2
        {
            get => (User)GetValue(NewUsContol);
            set => SetValue(NewUsContol, value);
            }
        public NewUserControlTest()
        {
            InitializeComponent();
            _viewModel = new UserViewModel();
            dbContext = new MediclcentContext();
        }
        private static void OnChnge(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is NewUserControlTest control && e.NewValue is User user )
            {
                control.ControUserPack2(user);
            }
        }
        private void ControUserPack2(User user) 
        {
            this .user = user;
            LasteNameBox.Text = user.Lastname;
            NameBox.Text = user.Firstname;
            PatronomickBox.Text = user.Partominyc;
            Phone.Text = user.PhoneNumber;

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatronomickBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LasteNameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void NameBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
