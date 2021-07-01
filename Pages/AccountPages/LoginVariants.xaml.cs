using ModernWpf.Media.Animation;
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

namespace Fluetta.Pages.AccountPages
{
    /// <summary>
    /// Логика взаимодействия для LoginVariants.xaml
    /// </summary>
    public partial class LoginVariants : Page
    {
        public LoginVariants()
        {
            InitializeComponent();
        }

        private void MojangLogin_Click(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo _transitionInfo = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(MojangLogin), null, _transitionInfo);
        }

        private void OfflineLogin_Click(object sender, RoutedEventArgs e)
        {
            SlideNavigationTransitionInfo _transitionInfo = new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight };
            ((Account)Window.GetWindow(this)).ContentFrame.Navigate(typeof(OfflineLogin), null, _transitionInfo);
        }
    }
}
