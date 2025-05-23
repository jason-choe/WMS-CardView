using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using WMS.CardView;

public class CardViewModel : INotifyPropertyChanged
{

    private UserControl _cardViewInstance;

    public UserControl CardViewInstance
    {
        get => _cardViewInstance;
        set
        {
            _cardViewInstance = value;
            OnPropertyChanged(nameof(CardViewInstance)); // ✅ 속성 변경 시 UI 업데이트
        }
    }

    public ObservableCollection<CardModel> Cards { get; set; }

    public CardViewModel()
    {
        Cards = new ObservableCollection<CardModel>();
        //LoadCardsFromDB(); // ✅ DB에서 카드 데이터 가져오기
        CardViewInstance = new CardView(); // ✅ 인스턴스 생성하여 UI에 정상적으로 표시
    }

    // ✅ INotifyPropertyChanged 구현
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void RefreshUI()
    {
        Debug.WriteLine($"RefreshUI---------------Start");
        Application.Current.Dispatcher.Invoke(() =>
        {
            OnPropertyChanged(nameof(Cards)); // ✅ UI 강제 업데이트
            Application.Current.MainWindow.UpdateLayout(); // ✅ UI 강제 새로고침
        });
        Debug.WriteLine($"RefreshUI---------------Ends");
    }

    private void LoadCardsFromDB()
    {
        Debug.WriteLine($"LoadCardsFromDB---------------Start");
        string connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString; // ✅ MS SQL 연결
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id as 'Id', rack_name as 'Title', rack_type*3 + bullet_type AS 'ImageIndex', visible AS 'IsVisible' FROM RackState"; // ✅ SQL 조회
                using (var command = new SqlCommand(query, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        long id = reader.GetInt64(0); // ✅ `bigint` → `long`
                        string title = reader.IsDBNull(1) ? "Untitled" : reader.GetString(1); // ✅ `nullable varchar`
                        int imageIndex = reader.IsDBNull(2) ? 0 : reader.GetInt32(2); // ✅ `nullable smallint` → `int`
                        bool isVisible = reader.GetBoolean(3); // ✅ `bit` → `bool`

                        Cards.Add(new CardModel
                        {
                            Id = (int)id,
                            Title = title,
                            ImageIndex = imageIndex,
                            IsVisible = isVisible
                        });
                    }
                }
            }
            Debug.WriteLine($"카드 개수: {Cards.Count}"); // ✅ 데이터가 정상적으로 추가되었는지 확인
/*            foreach (var card in Cards)
            {
                Debug.WriteLine($"카드 ID: {card.Id}, 제목: {card.Title}, 이미지: {card.ImageIndex}, 표시 여부: {card.IsVisible}");
            }*/
        }
        catch (SqlException sqlEx)
        {
            Debug.WriteLine($"SQL 오류 발생: {sqlEx.Message}"); // ✅ SQL 예외 처리
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"데이터 불러오기 실패: {ex.Message}"); // ✅ 일반 예외 처리
        }

        //RefreshUI();
        Debug.WriteLine($"LoadCardsFromDB---------------Ends");
    }

    public void UpdateCardsFromDB()
    {
        Debug.WriteLine($"기존 데이터 초기화 후 다시 로드");
        Cards.Clear(); // ✅ 기존 데이터 초기화 후 다시 로드
        LoadCardsFromDB();
    }
}

public class CardModel : INotifyPropertyChanged
{
    private int _imageIndex;
    private bool _isVisible = true;

    public int Id { get; set; } // ✅ 카드 ID (DB Key)
    public string? Title { get; set; } // ✅ 카드 제목
    //public string ImageSource { get; set; }

    public int ImageIndex
    {
        get => _imageIndex;
        set
        {
            if (_imageIndex != value)
            {
                _imageIndex = value;
                OnPropertyChanged(nameof(ImageIndex));
                OnPropertyChanged(nameof(ImageSource)); // ✅ 이미지 변경 시 업데이트
            }
        }
    }

    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                OnPropertyChanged(nameof(IsVisible));
                OnPropertyChanged(nameof(CardVisibility)); // ✅ Visibility 속성도 변경될 경우 업데이트
            }
        }
    }

    public Visibility CardVisibility
    {
        get => IsVisible ? Visibility.Visible : Visibility.Collapsed;
    }
    public string ImageSource => $"Images/image_{ImageIndex}.png"; // ✅ 이미지 동적 설정 (0~5)

#pragma warning disable CS8612 // 형식에 있는 참조 형식 Null 허용 여부가 암시적으로 구현된 멤버와 일치하지 않습니다.
    public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS8612 // 형식에 있는 참조 형식 Null 허용 여부가 암시적으로 구현된 멤버와 일치하지 않습니다.
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); // ✅ 올바르게 이벤트 호출
}
