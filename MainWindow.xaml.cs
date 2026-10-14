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


namespace memoria_játék
{
    


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> sizes = new List<string>() { "2x2", "4x4", "6x6", };
        List<string> games = new List<string>() { "szinek", "számok", "emojik" };
        int gridmeret = 0; //globális változó
        public MainWindow()
        {
            InitializeComponent();
            lbox_meret.ItemsSource = sizes;
            lbox_type.ItemsSource = games;


        }

        private void Kivalasztasmeret(object sender, SelectionChangedEventArgs e)
        {
            string[] kivalasztott = lbox_meret.SelectedItem.ToString().Split("x");
            gridmeret = Convert.ToInt32(""+kivalasztott[0]); 
            //mi van akkor, ha nincs semmi kiválasztva
            if (kivalasztott != null)
            {
                tb_kiirat.Text = "A választott méret " + kivalasztott;

            }
            else
            {
                tb_kiirat.Text = "Nincs kiválasztott elem";
            }

        }

        private void Kivalasztas2(object sender, SelectionChangedEventArgs e)
        {
            string kivalasztott2 = "" + lbox_type.SelectedItem;
            //mi van akkor, ha nincs semmi kiválasztva
            if (kivalasztott2 != null)
            {
                tb_kiirat2.Text = "A választott játéktipus " + kivalasztott2;
            }
            else
            {
                tb_kiirat2.Text = "Nincs kiválasztott elem";
            }
        }

        private void starter_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            GombokElhelyezese();
            btn.IsEnabled = false;
        }
        private void GombokElhelyezese()

        {

            for (int i = 0; i < gridmeret; i++)

            {

                btn_grid.RowDefinitions.Add(new RowDefinition());

                btn_grid.ColumnDefinitions.Add(new ColumnDefinition());

            }

            List<int> szamok = new List<int>();

            for (int i = 1; i <= gridmeret * gridmeret / 2; i++)

            {

                szamok.Add(i);

                szamok.Add(i);

            }


            szamok = szamok.Shuffle().ToList(); 
            int index = 0;

            for (int i = 0; i < gridmeret; i++)

            {

                for (int j = 0; j < gridmeret; j++)

                {

                    Button btn = new Button
                    {

                        Content = "?",

                        FontSize = 20,

                        FontWeight = FontWeights.Bold,

                        Margin = new Thickness(3),

                    };
                    btn.Click += button_cl;

                    Grid.SetRow(btn, i);

                    Grid.SetColumn(btn, j);


                    btn_grid.Children.Add(btn);

                }

            }

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button_cl(object sender, RoutedEventArgs e)
        {


        }
    }
}