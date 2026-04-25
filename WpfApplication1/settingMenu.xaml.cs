using System;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WpfApplication1
{
    /// <summary>
    /// settingMenu.xaml の相互作用ロジック
    /// </summary>
    public partial class settingMenu : Window
    {
        internal static String name;
        internal static bool textChanged = false;
        internal static bool saved = false;

        public settingMenu()
        {
            InitializeComponent();

            if (MainWindow.shortcutEditFlag)
            {
                XDocument xml = XDocument.Load(@"D:\special.xml");
                XElement table = xml.Element("table");
                var rows = table.Elements("shortcut");

                foreach (XElement row in rows)
                {
                    XElement moji = row.Element("moji");
                    XElement ID = row.Element("ID");
                    
                    textBox.Text += (moji.Value + "=" + ID.Value + "\r\n");

                }
            }

            textChanged = false;

            DataContext = new
            {
                xmlName = name
            };
        }
        
        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            textChanged = true;
            DataContext = new
            {
                xmlName = name + '*'
            };
        }
        protected virtual void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // テキストに変更があり、なおかつセーブしていないのなら表示
            if (textChanged　&& !saved) {
                if (MessageBoxResult.Yes != MessageBox.Show("保存しないでウィンドウを閉じます。よろしいですか？", "確認", MessageBoxButton.YesNo, MessageBoxImage.Information))
                {
                    e.Cancel = true;
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            saved = true;
            DataContext = new
            {
                xmlName = name
            };
            switch (name) {

                case "special.xml(定型文)":

                    break;
            }
        }
    }
}
