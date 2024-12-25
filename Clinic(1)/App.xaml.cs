using Clinic_1_.Model2;
using Clinic_1_.PrisListt;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Clinic_1_
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MediclcentContext context { get; } = new MediclcentContext();

        public static PrisListAdmin Window { get; set; }
    }
}
