using Newtonsoft.Json;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace To_Do_Liste
{
    class LabelCheckboxData
    {
        public string aufgabe;
        public bool erledigt;

        public LabelCheckboxData(string aufgabe)
        {
            this.aufgabe = aufgabe;
            this.erledigt = false;
        }
    }
    public partial class MainWindow : Window
    {
        List<String> list = new List<String>();
        List<LabelCheckboxData> labels = new List<LabelCheckboxData>();

        String path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "Daten" + "Liste.json");

        public MainWindow()
        {
            InitializeComponent();
        }

        private void GridHinzufügen(String aufgabe, bool erledig)
        {
            Label neuesLabel = new Label();
            CheckBox checkBox = new CheckBox();
            Grid myGrid = new Grid();
            bool erledigt = erledig;
            HorizontalAlignment alignment = HorizontalAlignment.Right;
            checkBox.HorizontalAlignment = alignment;
            neuesLabel.Content = aufgabe;
            myGrid.Children.Add(neuesLabel);
            myGrid.Children.Add(checkBox);

            if (erledigt)
            {
                myGrid.Background = Brushes.Green;
                checkBox.IsChecked = true;
            }

            checkBox.Checked += abhaken;
            checkBox.Unchecked += hakenEntfernen;

            MeinStackPanel.Children.Add(myGrid);
            SpeichernButton.Content = "Speichern";
        }
        private void ButtonKlick(object sender, RoutedEventArgs e)
        {
            if (MeinStackPanel.Children.Count > 0)
            {
                var json = JsonConvert.SerializeObject(labels);
                File.WriteAllText(path, json);
                InputBox.Text = "Json gespeichert in " + path;
            }
            else
            {
                String jsonstring = File.ReadAllText(path);

                labels = JsonConvert.DeserializeObject<List<LabelCheckboxData>>(jsonstring);

                foreach (LabelCheckboxData labelCheckboxData in labels)
                {
                    CheckBox checkBox = new CheckBox();
                    GridHinzufügen(labelCheckboxData.aufgabe, labelCheckboxData.erledigt);
                }
            }
        }
        private void Button2Klick(object sender, RoutedEventArgs e)
        {
            String input = InputBox.Text;

            GridHinzufügen(input, false);

            labels.Add(new LabelCheckboxData(input));
            list.Add(input);
        }

        private void abhaken(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            Grid grid = VisualTreeHelper.GetParent(checkBox) as Grid;

            grid.Background = Brushes.Green;

            erledigtAktualisieren(grid, true);
        }

        private void hakenEntfernen(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            Grid grid = VisualTreeHelper.GetParent(checkBox) as Grid;

            grid.Background = Brushes.White;

            erledigtAktualisieren(grid, false);
        }

        private void erledigtAktualisieren(Grid grid, bool erledigt)
        {
            foreach (Control control in grid.Children)
            {
                if (control is Label label)
                {
                    String aufgabeX = (string?)label.Content;
                    foreach (LabelCheckboxData labelCheckboxData in labels)
                    {
                        if (labelCheckboxData.aufgabe.Equals(aufgabeX))
                        {
                            labelCheckboxData.erledigt = erledigt;
                        }
                    }
                }
            }
        }
    }
}