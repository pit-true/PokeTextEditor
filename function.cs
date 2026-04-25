using System;
using System.IO;
using System.Windows.Forms;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Linq;
using System.Linq;


namespace WpfApplication1
{

    public static class function
    {
        
        public static void roadROM()
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
            ofd.CheckFileExists = true;
            //存在しないパスが指定されたとき警告を表示する
            //デフォルトでTrueなので指定する必要はない
            ofd.CheckPathExists = true;

            //オープンファイルダイアログを表示する
            DialogResult result = ofd.ShowDialog();


            if (ofd.FileName == "")
            {
                // 何もしない
            }
            else {
                if (result == DialogResult.OK)
                {

                }
                FileStream fs = new FileStream(
                    ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                MainWindow.fileName = ofd.FileName;

                MainWindow.fileSize = (int)fs.Length;
                MainWindow.buf = new byte[MainWindow.fileSize];

                int readSize;
                int remain = MainWindow.fileSize;
                int busPos = 0;

                while (remain > 0)
                {
                    // 1024byteずつ読み込む
                    readSize = fs.Read(MainWindow.buf, busPos, Math.Min(1024, remain));

                    busPos += readSize;
                    remain -= readSize;
                }
                fs.Dispose();

                // 0xa8からあるROM名取得 10進数にしてnumに代入
                int num = Int32.Parse("a8", System.Globalization.NumberStyles.HexNumber);

                String romName = "";
                byte[] bytesData;
                bytesData = new byte[8];

                for (int i = 0; i < 8; i++)
                {
                    bytesData[i] = MainWindow.buf[num + i];

                }

                romName = System.Text.Encoding.UTF8.GetString(bytesData);

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

            }
        }

        public static int judgeOffset(String o)
        {
            String offset = o;
            bool oFlag = false;

            if (offset.Length == 8)
            {

                if ((o[0] == '0' && o[1] == 'x') || (o[0] == '0' && o[1] == '0') || (o[0] == '0' && o[1] == '8'))
                {   // 先頭が0xか00か08ならOK
                    offset = "";
                    // 最初の0xなどを削ってoffsetへ代入
                    for (int i = 0; i < o.Length - 2; i++)
                    {
                        offset += o[i + 2];
                    }
                }
                else if (o[0] == '0' && o[1] == '9')
                {   // 先頭が09なら01@@@@@@にする必要がある
                    offset = "";
                    offset += o[0];
                    offset += '1';
                    // offsetへ代入
                    for (int i = 2; i < o.Length; i++)
                    {
                        offset += o[i];
                    }
                }
                else if (o[0] == '0' && o[1] == '1')
                {
                    offset = "";
                    // 拡張領域01@@@@@@なのでそのままoffsetへ代入
                    for (int i = 0; i < o.Length; i++)
                    {
                        offset += o[i];
                    }
                }
            }
            else if (offset.Length == 10)
            {
                if (o[0] == '0' && o[1] == 'x' && ((o[2] == '0' && o[3] == '0') || (o[2] == '0' && o[3] == '8')))
                {   // 0x00@@@@@@, 0x08@@@@@@ならOK
                    offset = "";
                    // 最初の0x08などを削ってoffsetへ代入
                    for (int i = 0; i < o.Length - 4; i++)
                    {
                        offset += o[i + 4];
                    }
                }
                else if (o[0] == '0' && o[1] == 'x' && o[2] == '0' && o[3] == '1')
                {   // こっちは拡張領域、先頭が0xで次が01ならOK
                    offset = "";
                    // 最初の0xを削ってoffsetへ代入
                    for (int i = 0; i < o.Length - 2; i++)
                    {
                        offset += o[i + 2];
                    }
                }
            }
            else
            {
                // ******より短くても問題ないので続行
            }
            // A~Fまでをa~fに置換する
            offset = offset.Replace("A", "a");
            offset = offset.Replace("B", "b");
            offset = offset.Replace("C", "c");
            offset = offset.Replace("D", "d");
            offset = offset.Replace("E", "e");
            offset = offset.Replace("F", "f");
            offset = offset.Replace("0x", "");

            foreach (char c in offset)
            {
                if (c == '0' || c == '1' || c == '2' || c == '3')
                {
                }
                else if (c == '4' || c == '5' || c == '6' || c == '7')
                {
                }
                else if (c == '8' || c == '9' || c == 'a' || c == 'b')
                {
                }
                else if (c == 'c' || c == 'd' || c == 'e' || c == 'f')
                {
                }
                else
                {
                    oFlag = true;
                }
            }

            if (oFlag && offset.Length <= 8)
            {
                System.Media.SystemSounds.Hand.Play();
                System.Windows.MessageBox.Show("アドレスに使えない値が含まれています", "エラー");
            }
            

            // offsetを10進数に直す
            try
            {
                if (offset.Length > 8)
                {
                    System.Media.SystemSounds.Hand.Play();
                    System.Windows.MessageBox.Show("オフセットが長すぎます\n    ****** 0x******\n00****** 08******\n拡張領域なら\n01****** 0x01******\n09****** 0x09******の\nいずれかの形で入力してください", "エラー");

                    return -1;
                }
                else {
                    return Int32.Parse(offset, System.Globalization.NumberStyles.HexNumber);
                }
            }
            // 例外処理 オフセット以外の平仮名や漢字が書かれていた場合は-1を返す
            catch (Exception)
            {
                return -1;
            }
        }
        public static String loadMessage(String o)
        {
            // numに10進数化されたオフセットを代入
            int offsetTo10d = judgeOffset(o);
            String binaryData = "";

            if (offsetTo10d == -1)
            {
                return ""; // 何もしない
            }
            else {
                // 終点のffまで読み込む
                for (int i = 0; MainWindow.buf[offsetTo10d + i] != 0xff; i++)
                {   // 10進数で表示されてしまうのでStringFormat表記
                    if (String.Format("{0:X}", MainWindow.buf[offsetTo10d + i]).Length == 1)
                    {   // 0aなどがaとして認識されてしまうので0を付け加える
                        binaryData += 0.ToString() + String.Format("{0:X}", MainWindow.buf[offsetTo10d + i]) + " ";
                    }
                    else {
                        binaryData += String.Format("{0:X}", MainWindow.buf[offsetTo10d + i]) + " ";
                    }
                }

                // 統一感を出したいので
                binaryData = binaryData.Replace('A', 'a');
                binaryData = binaryData.Replace('B', 'b');
                binaryData = binaryData.Replace('C', 'c');
                binaryData = binaryData.Replace('D', 'd');
                binaryData = binaryData.Replace('E', 'e');
                binaryData = binaryData.Replace('F', 'f');

                // ffで処理が終わるので付け加えて終了
                return binaryData += "ff";
            }
        }

        public static String searchFirst(String t)
        {
            int num = judgeOffset(t);
            int location;
            int i = 0;
            String offset;

            if (num == -1)
            {
                return null;
            }
            else {
                while (MainWindow.buf[num - i] != 0xff)
                {
                    i++;
                }

                location = num - i;
                offset = Convert.ToString(location + 1, 16);

                return offset;
            }
        }

        public static void CastHex(String rightText)
        {
            int j = 0;
            char[] rightTextToCharArray;

            // A~Fまでをa~fに置換する
            rightText = rightText.Replace("  ", " ");
            rightText = rightText.Replace("A", "a");
            rightText = rightText.Replace("B", "b");
            rightText = rightText.Replace("C", "c");
            rightText = rightText.Replace("D", "d");
            rightText = rightText.Replace("E", "e");
            rightText = rightText.Replace("F", "f");
            rightTextToCharArray = rightText.ToCharArray();

            // 文字に空白や16進数以外が含まれていたらzに置換する
            for (int i = 0; i < rightText.Length; i++)
            {
                if (' ' == rightTextToCharArray[i]) { }
                else if ('0' == rightTextToCharArray[i]) { }
                else if ('1' == rightTextToCharArray[i]) { }
                else if ('2' == rightTextToCharArray[i]) { }
                else if ('3' == rightTextToCharArray[i]) { }
                else if ('4' == rightTextToCharArray[i]) { }
                else if ('5' == rightTextToCharArray[i]) { }
                else if ('6' == rightTextToCharArray[i]) { }
                else if ('7' == rightTextToCharArray[i]) { }
                else if ('8' == rightTextToCharArray[i]) { }
                else if ('9' == rightTextToCharArray[i]) { }
                else if ('a' == rightTextToCharArray[i]) { }
                else if ('b' == rightTextToCharArray[i]) { }
                else if ('c' == rightTextToCharArray[i]) { }
                else if ('d' == rightTextToCharArray[i]) { }
                else if ('e' == rightTextToCharArray[i]) { }
                else if ('f' == rightTextToCharArray[i]) { }
                else
                {
                    rightTextToCharArray[i] = 'z';
                }
            }

            // textBox2から16進数以外を削除する
            rightText = new String(rightTextToCharArray);
            rightText = rightText.Replace("z", "");

            String trimRightText = rightText.Replace(" ", "");
            int be = rightText.Replace(" ", "").Length / 2;
            MainWindow.bin = new byte[be];

            for (int i = 0; i < be * 2; i += 2)
            {   // byte型に変換する
                String w = trimRightText.Substring(i, 2);
                MainWindow.bin[j] = Convert.ToByte(w, 16);
                j++;
            }

            j = 0;

        }

        public static int SearchBytes(this byte[] text, byte[] pattern, int foundIndex)
        {
            int patternLen = pattern.Length, textLen = text.Length;

            // 移動量テーブルの作成
            int[] qs_table = new int[byte.MaxValue + 1];

            // デフォルト（パターン中に存在しないキャラクタが比較範囲の直後にあった）の場合、
            // 次の比較範囲はそのキャラクタの次。（＝比較範囲ずらし幅はパターン長＋１）
            for (int i = qs_table.Length; i-- > 0;) qs_table[i] = patternLen + 1;

            // パターンに存在するキャラクタが比較範囲の直後にあった場合、
            // 次の比較範囲は、そのキャラクタとパターン中のキャラクタを一致させる位置に。
            for (int n = 0; n < patternLen; ++n) qs_table[pattern[n]] = patternLen - n;

            int pos;

            // 移動量テーブルを用いて、文章の末尾に達しない範囲で比較を繰り返す
            for (pos = foundIndex; pos < textLen - patternLen; pos += qs_table[text[pos + patternLen]])
            {
                // 一致するか比較。一致したら、そのときの比較位置を返す。
                if (CompareBytes(text, pos, pattern, patternLen)) return pos;
            }

            // 文章の末尾がまだ未比較なら、そこも比較しておく
            if (pos == textLen - patternLen)
            {
                // 一致するか比較。一致したら、そのときの比較位置を返す。
                if (CompareBytes(text, pos, pattern, patternLen)) return pos;
            }

            // 一致する位置はなかった。
            return -1;
        }

        static bool CompareBytes(byte[] text, int pos, byte[] pattern, int patternLen)
        {
            for (int comparer = 0; comparer < patternLen; ++comparer)
            {
                if (text[comparer + pos] != pattern[comparer]) return false;
            }
            return true;
        }
    }
}

