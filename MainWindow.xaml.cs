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
    public partial class MainWindow : Window
    {
        List<string> sizes = new List<string>() { "2x2", "4x4", "6x6" };
        List<string> games = new List<string>() { "számok", "emojik" };

        int gridmeret = 0;
        string jatekTipus = "számok"; // Default game type

        private Button elsoKattintas = null;
        private bool blokkolva = false;
        private int probalkozasok = 0;

        // Emoji list for emoji mode
        private List<string> emojis = new List<string>()
        {
            "❤", "🙌", "🤳", "✨", "😁", "✔", "🎁", "👍", "🐨", "😎",
            "🐱‍🐉", "👀", "🌹", "🐸", "🐵", "🐔", "🐧", "🐦"
        };

        public MainWindow()
        {
            InitializeComponent();
            lbox_meret.ItemsSource = sizes;
            lbox_type.ItemsSource = games;
        }

        private void Kivalasztasmeret(object sender, SelectionChangedEventArgs e)
        {
            if (lbox_meret.SelectedItem != null)
            {
                string[] kivalasztott = lbox_meret.SelectedItem.ToString().Split("x");
                gridmeret = Convert.ToInt32(kivalasztott[0]);
                tb_kiirat.Text = "A választott méret: " + lbox_meret.SelectedItem;
            }
            else
            {
                tb_kiirat.Text = "Nincs kiválasztott méret";
            }
        }

        private void Kivalasztas2(object sender, SelectionChangedEventArgs e)
        {
            if (lbox_type.SelectedItem != null)
            {
                jatekTipus = lbox_type.SelectedItem.ToString();
                tb_kiirat2.Text = "A választott játéktípus: " + jatekTipus;
            }
            else
            {
                tb_kiirat2.Text = "Nincs kiválasztott játéktípus";
            }
        }

        private void starter_Click(object sender, RoutedEventArgs e)
        {
            if (gridmeret == 0)
            {
                MessageBox.Show("Kérlek válassz méretet a játék indítása előtt!", "Figyelmeztetés");
                return;
            }

            Button btn = (Button)sender;
            
            // Reset counter and clear any existing board
            probalkozasok = 0;
            tb_kiirat.Text = "Próbálkozások száma: 0";
            ResetGrid();

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

            int pairCount = (gridmeret * gridmeret) / 2;
            List<string> elemek = new List<string>();

            // Generate pairs based on selected game mode
            if (jatekTipus == "emojik")
            {
                for (int i = 0; i < pairCount; i++)
                {
                    elemek.Add(emojis[i % emojis.Count]);
                    elemek.Add(emojis[i % emojis.Count]);
                }
            }
            else // "számok" default mode
            {
                for (int i = 1; i <= pairCount; i++)
                {
                    elemek.Add(i.ToString());
                    elemek.Add(i.ToString());
                }
            }

            // Shuffle the elements
            elemek = elemek.Shuffle().ToList();
            int index = 0;

            for (int i = 0; i < gridmeret; i++)
            {
                for (int j = 0; j < gridmeret; j++)
                {
                    string rejtettErtek = elemek[index++];

                    Button btn = new Button
                    {
                        Content = "?",
                        Tag = rejtettErtek, 
                        FontSize = 20,
                        FontWeight = FontWeights.Bold,
                        Margin = new Thickness(3)
                    };

                    btn.Click += button_cl;

                    Grid.SetRow(btn, i);
                    Grid.SetColumn(btn, j);

                    btn_grid.Children.Add(btn);
                }
            }
        }

        private async void button_cl(object sender, RoutedEventArgs e)
        {
            if (blokkolva) return;

            Button btn = (Button)sender;

            // If the button is already flipped, do nothing
            if (btn.Content.ToString() != "?") return;

            // Reveal hidden content (number or emoji)
            btn.Content = btn.Tag;

            if (elsoKattintas == null)
            {
                elsoKattintas = btn;
            }
            else
            {
                // Second card clicked -> increment try counter
                Button masodikKattintas = btn;
                probalkozasok++;
                tb_kiirat.Text = "Próbálkozások száma: " + probalkozasok;

                // Compare Tag values (works for both numbers and emojis)
                if (elsoKattintas.Tag.ToString() == masodikKattintas.Tag.ToString())
                {
                    // MATCH: Leave them revealed
                    
                    elsoKattintas.IsEnabled =false;
                    masodikKattintas.IsEnabled = false;
                    elsoKattintas = null;
                    szamlalo();
                }
                else
                {
                    // NO MATCH: Wait briefly, then flip both back
                    blokkolva = true;

                    await Task.Delay(700);

                    elsoKattintas.Content = "?";
                    masodikKattintas.Content = "?";

                    elsoKattintas = null;
                    blokkolva = false;
                    szamlalo();
                }
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            ResetGrid();
            starter.IsEnabled = true;
            txtB_counter.Text = "Próbálkozások száma: 0";
        }

        private void ResetGrid()
        {
            btn_grid.Children.Clear();
            btn_grid.RowDefinitions.Clear();
            btn_grid.ColumnDefinitions.Clear();

            elsoKattintas = null;
            blokkolva = false;
            probalkozasok = 0;
        }
        private void szamlalo()
        {
            probalkozasok++;
            txtB_counter.Text = "Próbálkozások száma:" + probalkozasok/2;
        }
    }
}