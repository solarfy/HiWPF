using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ComputerDatingWizard
{
    public class Vitals
    {
        public string Name;
        public string Home;
        public string Gender;
        public string FavoriteOS;
        public string Directory;
        public string MomsMaidenName;
        public string Pet;
        public string Income;

        public static RadioButton GetCheckedRadioButton(GroupBox grpbox)
        {
            Panel pnl = grpbox.Content as Panel;

            if (pnl != null)
            {
                foreach (UIElement el in pnl.Children)
                {
                    RadioButton radio = el as RadioButton;
                    if (radio != null && (bool)radio.IsChecked)
                        return radio;
                }
            }

            return null;
        }
    }
}
