using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace Grapher
{
    public partial class MainWindow : Window
    {
        public static List<BMAP> type = new List<BMAP>();

        public MainWindow()
        {
            InitializeComponent();

            //Plottable.MinWidth = BMAP.width;
            //Plottable.MinHeight = BMAP.height;

            BMAP.Ids.Add(0);
            BMAP.Ids.Add(1);

            type.Add(new BMAP(0, "Prime distribution"));
            type.Add(new BMAP(1, "Pi digit distribution"));

            PlotSourceBox.DisplayMemberPath = "Value";
            PlotSourceBox.SelectedValuePath = "Id";
            PlotSourceBox.ItemsSource = type;
        }

        static int i;
        private void PlotButton_Click(object sender, RoutedEventArgs e)
        {
            BMAP.width = BMAP.height = Int32.Parse(PlotIterationBox.Text);
            PlotStatLabel.Content = BMAP.width * BMAP.height + " iterations.";
            i = (int)PlotSourceBox.SelectedValue;
            ImageSource imageSource = BMAP.GetMapImage((int)PlotSourceBox.SelectedValue);
            Plottable.Source = imageSource;
        }

        private void PlotSaveButton_Click(object sender, RoutedEventArgs e)
        {
            BMAP.width = BMAP.height = Int32.Parse(PlotIterationBox.Text);
            PlotStatLabel.Content = BMAP.width * BMAP.height + " iterations.";
            Bitmap bmp = BMAP.GetMap(i);
            ImageSource imageSource = BMAP.BitmapImageFromBitmap(bmp);
            Plottable.Source = imageSource;
            bmp.Save(PlotSourceBox.SelectedItem + ".png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void PlotIterationBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+");
            return !regex.IsMatch(text);
        }

        private void PlotIterationBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(String)))
            {
                String text = (String)e.DataObject.GetData(typeof(String));
                if (!IsTextAllowed(text))
                {
                    e.CancelCommand();
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
    }
}
