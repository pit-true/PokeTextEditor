using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Linq.Expressions;

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        internal static String processingBinaryText = ""; // Changeクラスで変換されたバイナリを保存しておく用の変数
        internal static String displayingText = ""; // 最終結果、左テキストボックスに表示する用
        internal static String displayingBinaryText = ""; // 最終結果、右テキストボックスに表示する用
        internal static String fileName;
        internal static String nameData; // ROM名
        internal static bool brilleFlag = false; // 点字用xmlファイルを読むか否かのフラグ
        internal static bool specialFlag = false; // F9 XXのxmlファイルを読むか否かのフラグ
        internal static bool numberFlag = false; // 点字用xmlファイルを読み込み中に数符が来たときにオンになる
        internal static bool shortcutEditFlag = false; // ショートカット編集用　未使用
        internal static bool leaveFlag = false; // <>で囲ったbyteをそのまま出力する用フラグ leave(そのままにする)
        internal static int currentLocation = 0; // 現在オフセット
        internal static byte[] buf; // buf[0]~buf[fileSize]までバイナリが入っている
        internal static byte[] bin;
        internal static int fileSize;
        //internal static String[] currentBinaryData = new string[10000]; 多分どっからも参照されていないはず

        public class MyCommands
        {
            public static RoutedUICommand shortcutF1 = new RoutedUICommand("shortcutF1", "shortcutF1", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F1) });
            public static RoutedUICommand shortcutF2 = new RoutedUICommand("shortcutF2", "shortcutF2", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F2) });
            public static RoutedUICommand shortcutF3 = new RoutedUICommand("shortcutF3", "shortcutF3", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F3) });
            public static RoutedUICommand shortcutF4 = new RoutedUICommand("shortcutF4", "shortcutF4", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F4) });
            public static RoutedUICommand shortcutF5 = new RoutedUICommand("shortcutF5", "shortcutF5", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F5) });
            public static RoutedUICommand shortcutF6 = new RoutedUICommand("shortcutF6", "shortcutF6", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F6) });
            public static RoutedUICommand shortcutF7 = new RoutedUICommand("shortcutF7", "shortcutF7", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F7) });
            public static RoutedUICommand shortcutF8 = new RoutedUICommand("shortcutF8", "shortcutF8", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F8) });
            public static RoutedUICommand shortcutF9 = new RoutedUICommand("shortcutF9", "shortcutF9", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F9) });
            public static RoutedUICommand shortcutF10 = new RoutedUICommand("shortcutF10", "shortcutF10", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F10) });
            public static RoutedUICommand shortcutF11 = new RoutedUICommand("shortcutF11", "shortcutF11", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F11) });
            public static RoutedUICommand shortcutF12 = new RoutedUICommand("shortcutF12", "shortcutF12", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.F12) });

            // Ctrl + Nで\nを挿入しつつ改行する
            public static RoutedUICommand addNewLine = new RoutedUICommand("addNewLine", "addNewLine", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.N, ModifierKeys.Control) });
            // Ctrl + Pで\pを挿入しつつ改行する
            public static RoutedUICommand addNewPage = new RoutedUICommand("addNewPage", "addNewPage", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.P, ModifierKeys.Control) });
            // Ctrl + Mで\mを挿入しつつ改行する
            public static RoutedUICommand addContinueLine = new RoutedUICommand("addContinueLine", "addContinueLine", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.M, ModifierKeys.Control) });
            // Ctrl + OでΩを挿入する
            public static RoutedUICommand addOmega = new RoutedUICommand("addContinueLine", "addContinueLine", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.O, ModifierKeys.Control) });            // Ctrl + OでΩを挿入する
            // Ctrl + RでROMの再読み込み
            public static RoutedUICommand reloadROM = new RoutedUICommand("reloadROM", "reloadROM", typeof(MyCommands), new InputGestureCollection { new KeyGesture(Key.R, ModifierKeys.Control) });

        }

        public MainWindow()
        {
            InitializeComponent();

            // 定型文名前取得用の処理

            // DataBinding用変数
            String[] st = new String[12];

            for (int i = 0; i < 12; i++)
            {
                var xelmSP = XElement.Load(@"setting\special.xml");
                var emp = (
                    from p in xelmSP.Elements("shortcut")
                    where p.Element("number").Value == (i + 1).ToString()
                    select p).SingleOrDefault();

                st[i] = emp.Element("name").Value;
            }

            this.DataContext = new
            {
                s1 = st[0],
                s2 = st[1],
                s3 = st[2],
                s4 = st[3],
                s5 = st[4],
                s6 = st[5],
                s7 = st[6],
                s8 = st[7],
                s9 = st[8],
                s10 = st[9],
                s11 = st[10],
                s12 = st[11],
            };

            // ショートカットキーで呼び出した処理
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF1, delegate { addMessage(1); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF2, delegate { addMessage(2); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF3, delegate { addMessage(3); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF4, delegate { addMessage(4); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF5, delegate { addMessage(5); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF6, delegate { addMessage(6); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF7, delegate { addMessage(7); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF8, delegate { addMessage(8); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF9, delegate { addMessage(9); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF10, delegate { addMessage(10); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF11, delegate { addMessage(11); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.shortcutF12, delegate { addMessage(12); }));

            // 改行キー呼び出しなどのショートカット
            this.CommandBindings.Add(new CommandBinding(MyCommands.addNewLine, delegate { addKey('N'); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.addNewPage, delegate { addKey('P'); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.addContinueLine, delegate { addKey('M'); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.addOmega, delegate { addKey('O'); }));
            this.CommandBindings.Add(new CommandBinding(MyCommands.reloadROM, delegate { addKey('R'); }));
        }


        // MainWindowを呼び出すためにおく
        public MainWindow MainWindowPointer;

        // 定型文をテキストに追記する処理
        public void addMessage(int i)
        {

            MainWindow win = new MainWindow();
            win.MainWindowPointer = this;

            this.textBox1.Text += LoadXML.shortcutMoji(i);

            if (this.textBox2.Text == "")
            {
                this.textBox2.Text = LoadXML.shortcutID(i);
            }
            else
            {
                this.textBox2.Text += " " + LoadXML.shortcutID(i);
            }

            this.textBox1.Select(this.textBox1.Text.Length, 0);
        }

        // 改行コードなどをテキストに追記する処理
        public void addKey(char c)
        {

            MainWindow win = new MainWindow();
            win.MainWindowPointer = this;

            // キャレット位置に挿入

            int charpos = this.textBox1.SelectionStart;

            switch (c)
            {
                case 'N':
                    if (this.textBox1.Text.Length == 0)
                    {
                        this.textBox1.Text += "\\n\r\n";
                    }
                    else
                    {
                        this.textBox1.Text = textBox1.Text.Insert(charpos, "\\n\r\n");
                    }
                    break;

                case 'P':
                    if (this.textBox1.Text.Length == 0)
                    {
                        this.textBox1.Text += "\\p\r\n";
                    }
                    else
                    {
                        this.textBox1.Text = textBox1.Text.Insert(charpos, "\\p\r\n");
                    }
                    break;

                case 'M':
                    if (this.textBox1.Text.Length == 0)
                    {
                        this.textBox1.Text += "\\m\r\n";
                    }
                    else
                    {
                        this.textBox1.Text = textBox1.Text.Insert(charpos, "\\m\r\n");
                    }
                    break;

                case 'O':
                    if (this.textBox1.Text.Length == 0)
                    {
                        String omega = LoadXML.IDtoMoji("ff");
                        this.textBox1.Text += omega;
                    }
                    else
                    {
                        String omega = LoadXML.IDtoMoji("ff");
                        this.textBox1.Text = textBox1.Text.Insert(charpos, omega);
                    }
                    break;

                case 'R':
                    if (fileName != null) // 空じゃなければ読み込みへ
                    {

                        FileStream fs = new FileStream(
                            fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                        try // 例外処理
                        {
                            StreamReader sr = new StreamReader(fs);

                            try
                            {

                                fileSize = (int)fs.Length;
                                buf = new byte[fileSize];

                                int readSize;
                                int remain = fileSize;
                                int busPos = 0;

                                while (remain > 0)
                                {
                                    // 1024byteずつ読み込む
                                    readSize = fs.Read(buf, busPos, Math.Min(1024, remain));

                                    busPos += readSize;
                                    remain -= readSize;
                                }
                            }
                            finally
                            {
                                if (sr != null)
                                {
                                    sr.Dispose();
                                }
                            }
                        }
                        finally
                        {
                            if (fs != null)
                            {
                                fs.Dispose();
                            }
                        }
                    }
                    break;
            }

        }

        public void button1_Click(object sender, RoutedEventArgs e)
        {
            textBox2.Text = Change.mojiToBinary(textBox1.Text);
            textBox1.Text = displayingText;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if(textBox2.Text.StartsWith(".byte"))
            {
                textBox2.Text = textBox2.Text.Replace(".byte","").Replace("0x","");
            } 
            textBox1.Text = Change.binaryToMoji(textBox2.Text);
            textBox2.Text = displayingBinaryText;
        }
        private void checkBox1_Checked(object sender, RoutedEventArgs e)
        {
            brilleFlag = true;
        }
        private void checkBox1_Unchecked(object sender, RoutedEventArgs e)
        {
            brilleFlag = false;
        }
        private void checkBox2_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }
        private void checkBox2_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }
        private void leftSaveROM_Click(object sender, RoutedEventArgs e)
        {
            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                if (textBox1.Text != "")
                {
                    if (offset.Text != "")
                    {
                        // 16進数に変換して指定アドレスに書き込む
                        String t = Change.mojiToBinary(textBox1.Text);
                        try
                        {
                            Change.binary(offset.Text, t);
                        }
                        catch (Exception)
                        {
                            System.Media.SystemSounds.Hand.Play();
                            System.Windows.Forms.MessageBox.Show("多分他のツールによって共有違反が発生しています\n思い当たるツールを閉じてみてください", "エラー");
                        }
                    }
                    else
                    {
                        System.Media.SystemSounds.Hand.Play();
                        System.Windows.Forms.MessageBox.Show("オフセットに何も書かれていません", "エラー");
                    }
                }
                else
                {
                    System.Media.SystemSounds.Hand.Play();
                    System.Windows.Forms.MessageBox.Show("テキストボックスに何も書かれていません", "エラー");
                }
            }
        }

        private void RightSaveROM_Click(object sender, RoutedEventArgs e)
        {
            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                if (textBox2.Text != "")
                {
                    if (offset.Text != "")
                    {
                        try
                        {
                            Change.binary(offset.Text, textBox2.Text);
                        }
                        catch (Exception)
                        {
                            System.Media.SystemSounds.Hand.Play();
                            System.Windows.Forms.MessageBox.Show("多分他のツールによって共有違反が発生しています\n思い当たるツールを閉じてみてください", "エラー");
                        }
                    }
                    else
                    {
                        System.Media.SystemSounds.Hand.Play();
                        System.Windows.Forms.MessageBox.Show("オフセットに何も書かれていません", "エラー");
                    }
                }
                else
                {
                    System.Media.SystemSounds.Hand.Play();
                    System.Windows.Forms.MessageBox.Show("テキストボックスに何も書かれていません", "エラー");
                }
            }
        }

        private void clipboard_copy_Click(object sender, RoutedEventArgs e)
        {
            String copyText = "";

            if (textBox2.Text != "") {
                copyText = textBox2.Text;
                copyText = ".byte 0x" + copyText.Replace(" ",", 0x");
                System.Windows.Clipboard.SetData(System.Windows.Forms.DataFormats.Text, copyText);
                System.Windows.Forms.MessageBox.Show("クリップボードにコピーしました", "システムメッセージ");
            }
                
        }

            private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OpenROM_Click(object sender, RoutedEventArgs e)
        {

            //OpenFileDialogクラスのインスタンスを作成
            OpenFileDialog ofd = new OpenFileDialog();

            //[ファイルの種類]に表示される選択肢を指定する
            //指定しないとすべてのファイルが表示される
            ofd.Filter = "ROMファイル(*.gba)|*.gba";

            //[ファイルの種類]ではじめに選択されるものを指定する
            ofd.FilterIndex = 1;

            //タイトルを設定する
            ofd.Title = "開くファイルを選択してください";

            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            ofd.RestoreDirectory = true;

            //存在しないファイルの名前が指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            //ofd.CheckFileExists = true;

            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            //ofd.CheckPathExists = true;

            //オープンファイルダイアログを表示する
            DialogResult result = ofd.ShowDialog();

            if (ofd.FileName != "") // 空じゃなければ読み込みへ
            {
                /* if (result == System.Windows.Forms.DialogResult.OK)
                {

                }*/

                FileStream fs = new FileStream(
                    ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                try // 例外処理
                {
                    StreamReader sr = new StreamReader(fs);

                    try
                    {
                        fileName = ofd.FileName;

                        fileSize = (int)fs.Length;
                        buf = new byte[fileSize];

                        int readSize;
                        int remain = fileSize;
                        int busPos = 0;

                        while (remain > 0)
                        {
                            // 1024byteずつ読み込む
                            readSize = fs.Read(buf, busPos, Math.Min(1024, remain));

                            busPos += readSize;
                            remain -= readSize;
                        }

                        // a8からあるROM名取得 10進数にしてnumに代入
                        int num = Int32.Parse("a8", System.Globalization.NumberStyles.HexNumber);

                        String romName = "";
                        byte[] bytesData = new byte[8];

                        for (int i = 0; i < 8; i++)
                        {
                            bytesData[i] = buf[num + i];

                        }

                        romName = System.Text.Encoding.UTF8.GetString(bytesData) ?? "ERROR";

                        if (romName == "FIREBPRJ")
                        {
                            romName = "ファイアレッド(J)";
                        }
                        else if (romName == " LEAFBPGJ")
                        {
                            romName = "リーフグリーン(J)";
                        }
                        else if (romName == "RUBYAXVJ")
                        {
                            romName = "ルビー(J)";
                        }
                        else if (romName == "SAPPAXPJ")
                        {
                            romName = "サファイア(J)";
                        }
                        else if (romName == "EMERBPEJ")
                        {
                            romName = "エメラルド(J)";
                        }
                        else
                        {
                            romName = "?????";
                        }

                        if (romName != "ERROR") // ERRORの場合はnullなので何もしない
                        {
                            NameROM.Text = romName;
                            nameData = romName;
                        }

                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Dispose();
                        }
                    }
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Dispose();
                    }
                }
            }
        }

        private void NameROM_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void offset_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void loadMessage_Click(object sender, RoutedEventArgs e)
        {
            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                textBox2.Text = function.loadMessage(offset.Text);
                textBox1.Text = Change.binaryToMoji(textBox2.Text);
            }
        }

        private void loadFirstMessage_Click(object sender, RoutedEventArgs e)
        {
            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                if (function.searchFirst(offset.Text) == null)
                {

                }
                else
                {
                    offset.Text = function.searchFirst(offset.Text);
                    textBox2.Text = function.loadMessage(offset.Text);
                    textBox1.Text = Change.binaryToMoji(textBox2.Text);
                }
            }
        }
        private void searchMessage_Click(object sender, RoutedEventArgs e)
        {
            listBox.Items.Clear();

            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {

                int hitCnt = 0; // オフセットのヒット数
                int j = 0; // オフセット(String型配列, 表示用)のカウント用

                // テキストボックスに入るオフセット offsetTextTemp[0]で1個目にヒットしたやつ、offsetTextTemp[i-1]でi番目にヒットしたやつ
                String[] offsetTextTemp = new String[10000];


                for (int i = 0; i < buf.Length; i++)
                {
                    if (j == 10000)
                    {
                        break;
                    }
                    else
                    {
                        // 0f 00 @@ @@ @@ @@ 09 まで読みに行く
                        // 67 @@ @@ @@ @@ 09　　も読みに行く
                        if ((buf[i] == 0x0f && buf[i + 1] == 0x00 && buf[i + 6] == 0x09) || (buf[i] == 0x67 && buf[i + 5] == 0x09))
                        {

                            // 08****** or 09******の形式のアドレスか確かめる(09******は拡張用)
                            if ((buf[i] == 0x0f && buf[i + 5] == 0x08) || (buf[i] == 0x0f && buf[i + 5] == 0x09)
                                || (buf[i] == 0x67 && buf[i + 4] == 0x08) || (buf[i] == 0x67 && buf[i + 4] == 0x09))
                            {
                                // 一致しているため3byte読み込み
                                for (int n = 0; n < 3; n++)
                                {
                                    if (String.Format("{0:X}", buf[i + 4 - n]).Length == 1)
                                    {   // 0aなどがaとして認識されてしまうので0を付け加える
                                        offsetTextTemp[j] += 0.ToString() + String.Format("{0:X}", buf[i + 4 - n]);
                                    }
                                    else
                                    {
                                        offsetTextTemp[j] += String.Format("{0:X}", buf[i + 4 - n]);
                                    }
                                }

                                // @@ @@ @@ 09は01@@@@@@を読み込みにいくので先頭に01を付与
                                if ((buf[i] == 0x0f && buf[i + 5] == 0x09) || (buf[i] == 0x67 && buf[i + 4] == 0x09))
                                {
                                    offsetTextTemp[j] = "01" + offsetTextTemp[j];
                                }

                                // 大文字を小文字に
                                offsetTextTemp[j] = offsetTextTemp[j].Replace('A', 'a');
                                offsetTextTemp[j] = offsetTextTemp[j].Replace('B', 'b');
                                offsetTextTemp[j] = offsetTextTemp[j].Replace('C', 'c');
                                offsetTextTemp[j] = offsetTextTemp[j].Replace('D', 'd');
                                offsetTextTemp[j] = offsetTextTemp[j].Replace('E', 'e');
                                offsetTextTemp[j] = offsetTextTemp[j].Replace('F', 'f');

                                j++;
                                hitCnt++;
                            }

                        }
                    }
                }

                for (int i = 0; i < hitCnt; i++) // 見つけたオフセットの数だけ追加する
                {
                    listBox.Items.Add(offsetTextTemp[i]);
                }

                for (int i = 0; i < listBox.Items.Count - 1; i++)
                {
                    for (int k = i + 1; k < listBox.Items.Count; k++)
                    {
                        if (listBox.Items[i].Equals(listBox.Items[k])) // 被りがある(true)なら削除
                        {
                            listBox.Items.RemoveAt(k);
                        }
                    }
                }
            }
        }


        private void listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            offset.Text = listBox.SelectedItem.ToString();
        }

        private void searchOffset_Click(object sender, RoutedEventArgs e)
        {

            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                try
                {
                    if (textBox1.Text == "")
                    {
                        System.Media.SystemSounds.Hand.Play();
                        System.Windows.Forms.MessageBox.Show("何も入力されていません", "エラー");
                    }
                    else
                    {

                        listBox.Items.Clear();
                        function.CastHex(Change.mojiToBinary(textBox1.Text));
                        int foundIndex = 0;
                        String offset = "";
                        bool loopEnd = false;

                        
                        for (int i = 0; i < 100; i++)
                        {
                            // 対象の配列、探す配列、開始位置
                            foundIndex = function.SearchBytes(MainWindow.buf, MainWindow.bin, foundIndex + 1);

                            // オフセットが見つかっていれば小文字にしてリストへ代入
                            offset = String.Format("{0:X}", foundIndex);
                            offset = offset.Replace('A', 'a');
                            offset = offset.Replace('B', 'b');
                            offset = offset.Replace('C', 'c');
                            offset = offset.Replace('D', 'd');
                            offset = offset.Replace('E', 'e');
                            offset = offset.Replace('F', 'f');

                            if (!loopEnd)
                            {
                                listBox.Items.Add(offset);
                                loopEnd = true;
                            }
                            else
                            {
                                if (offset == (String)listBox.Items[0])
                                {
                                    // もし存在したら一巡したということなので終わる
                                    break;
                                }
                                else
                                {
                                    listBox.Items.Add(offset);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { }

                try
                {
                    if (listBox.Items[listBox.Items.Count - 1].Equals("ffffffff"))
                    {
                        // return -1;なので除外
                        listBox.Items.RemoveAt(listBox.Items.Count - 1);
                        if (listBox.Items.Count == 0)
                        {
                            System.Media.SystemSounds.Hand.Play();
                            System.Windows.Forms.MessageBox.Show("一致するオフセットは見つかりませんでした", "エラー");
                        }
                    }
                } catch (Exception ex) { }   
            }
        }

    
        

        private void searchOffset2_Click(object sender, RoutedEventArgs e)
        {

            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                if (textBox2.Text == "")
                {
                    System.Media.SystemSounds.Hand.Play();
                    System.Windows.Forms.MessageBox.Show("何も入力されていません", "エラー");
                }
                else
                {
                    listBox.Items.Clear();

                    bool braiileCheckFlag = false;

                    if (brilleFlag)
                    {
                        // 点字にチェック付いてるとオフセット検索の挙動おかしくなるので一旦オフ
                        braiileCheckFlag = true;
                        brilleFlag = false;
                    }

                    function.CastHex(Change.mojiToBinary(Change.binaryToMoji(textBox2.Text)));

                    int foundIndex = 0;
                    String offset = "";
                    bool flag = false;

                    for (int i = 0; i < 100; i++)
                    {
                        // 対象の配列、探す配列、開始位置  
                        foundIndex = function.SearchBytes(MainWindow.buf, MainWindow.bin, foundIndex + 1);

                        // オフセットが見つかっていれば小文字にしてリストへ代入
                        offset = String.Format("{0:X}", foundIndex);
                        offset = offset.Replace('A', 'a');
                        offset = offset.Replace('B', 'b');
                        offset = offset.Replace('C', 'c');
                        offset = offset.Replace('D', 'd');
                        offset = offset.Replace('E', 'e');
                        offset = offset.Replace('F', 'f');

                        if (!flag)
                        {
                            listBox.Items.Add(offset);
                            flag = true;
                        }
                        else
                        {
                            if (offset == (String)listBox.Items[0])
                            {
                                // もし存在したら一巡したということなので終わる
                                break;
                            }
                            else
                            {
                                listBox.Items.Add(offset);
                            }
                        }
                    }

                    if (listBox.Items[listBox.Items.Count - 1].Equals("ffffffff"))
                    {
                        // return -1;なので除外
                        listBox.Items.RemoveAt(listBox.Items.Count - 1);
                    }

                    if (braiileCheckFlag)
                    {   //戻す
                        brilleFlag = true;
                    }

                    if (listBox.Items.Count == 0)
                    {
                        System.Media.SystemSounds.Hand.Play();
                        System.Windows.Forms.MessageBox.Show("一致するオフセットは見つかりませんでした", "エラー");
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            addMessage(1);
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            addMessage(2);
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            addMessage(3);
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            addMessage(4);
        }

        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            addMessage(5);
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            addMessage(6);
        }

        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            addMessage(7);
        }

        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            addMessage(8);
        }

        private void MenuItem_Click_9(object sender, RoutedEventArgs e)
        {
            addMessage(9);
        }

        private void MenuItem_Click_10(object sender, RoutedEventArgs e)
        {
            addMessage(10);
        }

        private void MenuItem_Click_11(object sender, RoutedEventArgs e)
        {
            addMessage(11);
        }

        private void MenuItem_Click_12(object sender, RoutedEventArgs e)
        {
            addMessage(12);
        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem2_Click_1(object sender, RoutedEventArgs e)
        {
            settingMenu.name = "moji.xml";
            var win = new settingMenu();
            win.ShowDialog();
        }
        private void MenuItem2_Click_2(object sender, RoutedEventArgs e)
        {
            settingMenu.name = "braille.xml";
            var win = new settingMenu();
            win.ShowDialog();
        }
        private void MenuItem2_Click_3(object sender, RoutedEventArgs e)
        {
            settingMenu.name = "special.xml";
            var win = new settingMenu();
            win.ShowDialog();
        }

        private void MenuItem2_Click_4(object sender, RoutedEventArgs e)
        {
            settingMenu.name = "special.xml(定型文)";
            shortcutEditFlag = true;
            var win = new settingMenu();
            win.ShowDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {       //ROMの再読み込み
            if (NameROM.Text == "ROMファイル")
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.Forms.MessageBox.Show("ROMが読み込まれていません", "エラー");
            }
            else
            {
                if (fileName != "") // 空じゃなければ読み込みへ
                {
                    /* if (result == System.Windows.Forms.DialogResult.OK)
                    {

                    }*/

                    FileStream fs = new FileStream(
                        fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                    try // 例外処理
                    {
                        StreamReader sr = new StreamReader(fs);

                        try
                        {

                            fileSize = (int)fs.Length;
                            buf = new byte[fileSize];

                            int readSize;
                            int remain = fileSize;
                            int busPos = 0;

                            while (remain > 0)
                            {
                                // 1024byteずつ読み込む
                                readSize = fs.Read(buf, busPos, Math.Min(1024, remain));

                                busPos += readSize;
                                remain -= readSize;
                            }
                        }
                        finally
                        {
                            if (sr != null)
                            {
                                sr.Dispose();
                            }
                        }
                    }
                    finally
                    {
                        if (fs != null)
                        {
                            fs.Dispose();
                        }
                    }
                }
            }
        }
    }
}
