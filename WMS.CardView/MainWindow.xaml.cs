using System.Windows;
using System.Diagnostics;

namespace WMS.CardView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public CardView CardViewInstance { get; set; }
        private CardViewModel _cardViewModel;

        public MainWindow()
        {
            InitializeComponent();
 
            //CardViewInstance = new CardView(); // ✅ CardView를 생성하여 바인딩
            _cardViewModel = new CardViewModel();
            this.DataContext = _cardViewModel;

            if (DataContext == null) // ✅ 데이터 컨텍스트 반복 설정 방지
            {
                DataContext = new CardViewModel();
            }

            Debug.WriteLine($"CardViewInstance 설정 확인: {_cardViewModel.CardViewInstance != null}");
            Debug.WriteLine($"DataContext 설정 확인: {_cardViewModel != null}");
            Debug.WriteLine($"카드 개수 final : {_cardViewModel.Cards.Count}");
        }

        private void RefreshCards_Click(object sender, RoutedEventArgs e)
        {
            _cardViewModel.UpdateCardsFromDB(); // ✅ DB에서 최신 데이터 로드
        }
    }
}
