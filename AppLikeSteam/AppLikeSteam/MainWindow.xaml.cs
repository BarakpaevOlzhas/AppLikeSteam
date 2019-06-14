using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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

namespace AppLikeSteam
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<GameInformation> gameInformations = new List<GameInformation>();

        public MainWindow()
        {
            InitializeComponent();

            WebRequest request = WebRequest.Create("http://steamspy.com/api.php?request=top100in2weeks");

            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);

                string responseFromServer = reader.ReadToEnd();

                string result = SearchId(responseFromServer);

                SetData(result);

                GetImgAndInformations();                
            }            
        }

        private async Task GetImgAndInformations()
        {
            
            await Task.Run(() => {
                foreach (var i in gameInformations)
                {
                    WebRequest request = WebRequest.Create("https://chicken-coop.p.rapidapi.com/games/" + i.Name);
                    request.Headers.Add("X-RapidAPI-Host", "chicken-coop.p.rapidapi.com");
                    request.Headers.Add("X-RapidAPI-Key", "eb5d1de496msh0618c46e96abd90p1fa17cjsncad11ebba8f3");

                    WebResponse response = request.GetResponse();

                    using (Stream dataStream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(dataStream);

                        string responseFromServer = reader.ReadToEnd();

                        Stub information = JsonConvert.DeserializeObject<Stub>(responseFromServer);

                        i.Informations = information.Result;

                        Dispatcher.Invoke(() => {
                            Image image = new Image();
                            image.Width = 130;
                            Thickness thickness = image.Margin;
                            thickness.Left = 10;
                            thickness.Right = 310;
                            image.Margin = thickness;
                            image.Source = new BitmapImage(new Uri(i.Informations.image));

                            Label label = new Label();
                            label.Content = i.Name;
                            Thickness thicknessTwo = label.Margin;
                            thicknessTwo.Left = 140;
                            thicknessTwo.Bottom = 80;                            
                            label.Margin = thicknessTwo;
                            label.FontSize = 25;

                            Label labelTwo = new Label();
                            labelTwo.Content = "Price: " + i.Price;
                            Thickness thicknessThree = labelTwo.Margin;
                            thicknessThree.Left = 370;
                            thicknessThree.Right = 10;
                            thicknessThree.Top = 105;
                            thicknessThree.Bottom = 10;
                            labelTwo.Margin = thicknessThree;
                            labelTwo.Width = 70;                           

                            Grid grid = new Grid();
                            grid.Height = 150;
                            grid.Width = 450;
                            grid.Name = "a" + i.Appid;
                            grid.Children.Add(image);
                            grid.Children.Add(label);
                            grid.Children.Add(labelTwo);
                            grid.MouseUp += GridMouseDown;

                            listBoxForGame.Items.Add(grid);
                        });                        
                    }

                }
            });
        }

        private void SetData(string result)
        {
            foreach (var i in result.Split())
            {
                Thread.Sleep(100);

                WebRequest requestOne = WebRequest.Create("http://steamspy.com/api.php?request=appdetails&appid=" + i);

                WebResponse responseOne = requestOne.GetResponse();

                using (Stream stream = responseOne.GetResponseStream())
                {
                    StreamReader readerOne = new StreamReader(stream);

                    string jsonGameInformation = readerOne.ReadToEnd();

                    GameInformation information = JsonConvert.DeserializeObject<GameInformation>(jsonGameInformation);

                    gameInformations.Add(information);
                }
            }
        }

        private string SearchId(string text)
        {
            string result = "";
            bool isFind = true;           

            for (int i = 0; i < text.Length; i++) 
            {
                if (text[i] == '\n' || i == 0)
                {
                    if (i + 2 < text.Length)
                    isFind = true;
                }

                if (isFind)
                {
                    if (text[i + 2] != '\"') 
                        result += text[i + 2];
                    else
                    {
                        result += " ";
                        isFind = false;
                    }
                }
                
            }

            return result;
        }
        
        private void GridMouseDown(object sender, MouseButtonEventArgs e)
        {
            foreach (var i in gameInformations)
            {
                if ("a" + i.Appid.ToString() == (sender as Grid).Name)
                {
                    lableNameGame.Content = i.Name;
                    try
                    {
                        lableDiscription.Text = "Discription: " + i.Informations.description;
                    }
                    catch (Exception exe)
                    {
                        lableDiscription.Text = "Discription: " + "None";
                    }
                }
            }
        }
    }
}
