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
using System.Diagnostics;

namespace WMS.CardView
{
    public partial class CardView : UserControl
    {
        public CardView()
        {
            InitializeComponent();
            Debug.WriteLine($"==================================================");
            Debug.WriteLine($"ItemsControl 렌더링 확인: {CardListControl != null}");
            Debug.WriteLine($"==================================================");
            this.DataContext = new CardViewModel(); // ✅ 카드 데이터를 바인딩
        }
    }
}
