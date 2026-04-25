using System;
using System.IO;
using System.Security.Permissions;

namespace WpfApplication1
{
    class Change
    {
        public static String mojiToBinary(String t)
        {
            // textBox1のテキストを変換してtextBox2にバイナリテキストで渡す

            String binaryText = ""; // 返り値
            String moji="";
            String t1 = t;
            String m = "";
            bool unPlusFlag = false;

            t1 = t1.Replace("\r\n", "");
            t1 = t1.Replace("¥", @"\");

            // 点字用変換にチェックが入っている場合Braille.xmlを読み込む
            if (MainWindow.brilleFlag)
            {
                // 一応カタカナ対応
                t1 = t1.Replace('ア', 'あ');
                t1 = t1.Replace('イ', 'い');
                t1 = t1.Replace('ウ', 'う');
                t1 = t1.Replace('エ', 'え');
                t1 = t1.Replace('オ', 'お');
                t1 = t1.Replace('カ', 'か');
                t1 = t1.Replace('キ', 'き');
                t1 = t1.Replace('ク', 'く');
                t1 = t1.Replace('ケ', 'け');
                t1 = t1.Replace('コ', 'こ');
                t1 = t1.Replace('サ', 'さ');
                t1 = t1.Replace('ス', 'し');
                t1 = t1.Replace('ス', 'す');
                t1 = t1.Replace('セ', 'せ');
                t1 = t1.Replace('ソ', 'そ');
                t1 = t1.Replace('タ', 'た');
                t1 = t1.Replace('チ', 'ち');
                t1 = t1.Replace('ツ', 'つ');
                t1 = t1.Replace('テ', 'て');
                t1 = t1.Replace('ト', 'と');
                t1 = t1.Replace('ナ', 'な');
                t1 = t1.Replace('ニ', 'に');
                t1 = t1.Replace('ヌ', 'ぬ');
                t1 = t1.Replace('ネ', 'ね');
                t1 = t1.Replace('ノ', 'の');
                t1 = t1.Replace('ハ', 'は');
                t1 = t1.Replace('ヒ', 'ひ');
                t1 = t1.Replace('フ', 'ふ');
                t1 = t1.Replace('ヘ', 'へ');
                t1 = t1.Replace('ホ', 'ほ');
                t1 = t1.Replace('マ', 'ま');
                t1 = t1.Replace('ミ', 'み');
                t1 = t1.Replace('ム', 'む');
                t1 = t1.Replace('メ', 'め');
                t1 = t1.Replace('モ', 'も');
                t1 = t1.Replace('ヤ', 'や');
                t1 = t1.Replace('ユ', 'ゆ');
                t1 = t1.Replace('ヨ', 'よ');
                t1 = t1.Replace('ラ', 'ら');
                t1 = t1.Replace('リ', 'り');
                t1 = t1.Replace('ル', 'る');
                t1 = t1.Replace('レ', 'れ');
                t1 = t1.Replace('ロ', 'ろ');
                t1 = t1.Replace('ワ', 'わ');
                t1 = t1.Replace('ヲ', 'を');
                t1 = t1.Replace('ン', 'ん');

                // 表記ミスを正す
                t1 = t1.Replace("ガ", "゛か");
                t1 = t1.Replace("ギ", "゛き");
                t1 = t1.Replace("グ", "゛く");
                t1 = t1.Replace("ゲ", "゛け");
                t1 = t1.Replace("ゴ", "゛こ");
                t1 = t1.Replace("ザ", "゛さ");
                t1 = t1.Replace("ジ", "゛し");
                t1 = t1.Replace("ズ", "゛す");
                t1 = t1.Replace("ゼ", "゛せ");
                t1 = t1.Replace("ゾ", "゛そ");
                t1 = t1.Replace("ダ", "゛た");
                t1 = t1.Replace("ヂ", "゛ち");
                t1 = t1.Replace("ヅ", "゛つ");
                t1 = t1.Replace("デ", "゛て");
                t1 = t1.Replace("ド", "゛と");
                t1 = t1.Replace("バ", "゛は");
                t1 = t1.Replace("ビ", "゛ひ");
                t1 = t1.Replace("ブ", "゛ふ");
                t1 = t1.Replace("ベ", "゛へ");
                t1 = t1.Replace("ボ", "゛ほ");
                t1 = t1.Replace("パ", "゜は");
                t1 = t1.Replace("ピ", "゜ひ");
                t1 = t1.Replace("プ", "゜ふ");
                t1 = t1.Replace("ペ", "゜へ");
                t1 = t1.Replace("ポ", "゜ほ");

                // 表記ミスを正す
                t1 = t1.Replace("が", "゛か");
                t1 = t1.Replace("ぎ", "゛き");
                t1 = t1.Replace("ぐ", "゛く");
                t1 = t1.Replace("げ", "゛け");
                t1 = t1.Replace("ご", "゛こ");
                t1 = t1.Replace("ざ", "゛さ");
                t1 = t1.Replace("じ", "゛し");
                t1 = t1.Replace("ず", "゛す");
                t1 = t1.Replace("ぜ", "゛せ");
                t1 = t1.Replace("ぞ", "゛そ");
                t1 = t1.Replace("だ", "゛た");
                t1 = t1.Replace("ぢ", "゛ち");
                t1 = t1.Replace("づ", "゛つ");
                t1 = t1.Replace("で", "゛て");
                t1 = t1.Replace("ど", "゛と");
                t1 = t1.Replace("ば", "゛は");
                t1 = t1.Replace("び", "゛ひ");
                t1 = t1.Replace("ぶ", "゛ふ");
                t1 = t1.Replace("べ", "゛へ");
                t1 = t1.Replace("ボ", "゛ほ");
                t1 = t1.Replace("ぱ", "゜は");
                t1 = t1.Replace("ぴ", "゜ひ");
                t1 = t1.Replace("ぷ", "゜ふ");
                t1 = t1.Replace("ぺ", "゜へ");
                t1 = t1.Replace("ぽ", "゜ほ");

                //　次の文字がゃゅょの場合纏める
                t1 = t1.Replace("゛きゃ", "ぎゃ");
                t1 = t1.Replace("゛きゅ", "ぎゅ");
                t1 = t1.Replace("゛きょ", "ぎょ");
                t1 = t1.Replace("゛しゃ", "じゃ");
                t1 = t1.Replace("゛しゅ", "じゅ");
                t1 = t1.Replace("゛しょ", "じょ");
                t1 = t1.Replace("゛ちゃ", "ぢゃ");
                t1 = t1.Replace("゛ちゅ", "ぢゅ");
                t1 = t1.Replace("゛ちょ", "ぢょ");
                t1 = t1.Replace("゛ひゃ", "びゃ");
                t1 = t1.Replace("゛ひゅ", "びゅ");
                t1 = t1.Replace("゛ひょ", "びょ");
                t1 = t1.Replace("゜ひゃ", "ぴゃ");
                t1 = t1.Replace("゜ひゅ", "ぴゅ");
                t1 = t1.Replace("゜ひょ", "ぴょ");
                t1 = t1.Replace(">\n", ">");

                // 特殊記号もまとめる
                t1 = t1.Replace("゛き符", "ぎ符");

                for (int i = 0; i < t1.Length; i++)
                {
                    //  \n対応
                    if (t1[i] == '\\' && t1[i + 1] == 'n')
                    {
                        moji = t1[i].ToString() + t1[i + 1].ToString();
                        i++;
                    }
                    // {数符}, {つなぎ符}を纏めた後変換する
                    else if (t1[i] == '{')
                    {
                        m = "";
                        for (; t1[i] != '}'; i++)
                        {
                            m += t1[i].ToString();
                        }
                        m += '}';
                        moji = m;
                    }
                    // <>で囲まれた1byte文字(01など)をそのまま出力する(int型整数をtoStringするようなもの)
                    else if (t1[i] == '<')
                    {
                        MainWindow.leaveFlag = true;
                    }
                    else if (t1[i] == '>')
                    {
                        MainWindow.leaveFlag = false;
                    }
                    else
                    {
                        switch (t1[i])
                        {
                            case 'き':
                                unPlusFlag = true;
                                break;

                            case 'ぎ':
                                unPlusFlag = true;
                                break;

                            case 'し':
                                unPlusFlag = true;
                                break;

                            case 'じ':
                                unPlusFlag = true;
                                break;

                            case 'ち':
                                unPlusFlag = true;
                                break;

                            case 'ぢ':
                                unPlusFlag = true;
                                break;

                            case 'に':
                                unPlusFlag = true;
                                break;

                            case 'ひ':
                                unPlusFlag = true;
                                break;

                            case 'ぴ':
                                unPlusFlag = true;
                                break;

                            case 'び':
                                unPlusFlag = true;
                                break;

                            case 'み':
                                unPlusFlag = true;
                                break;

                            case 'り':
                                unPlusFlag = true;
                                break;

                            default:
                                break;
                        }

                        if (unPlusFlag)
                        {
                            if (t1[i + 1] == 'ゃ' || t1[i + 1] == 'ゅ' || t1[i + 1] == 'ょ')
                            {
                                moji = t1[i].ToString() + t1[i + 1].ToString();
                                i++;
                            }
                            else
                            {
                                moji = t1[i].ToString();
                                unPlusFlag = false;
                            }
                        }
                        else
                        {
                            moji = t1[i].ToString();
                            unPlusFlag = false;
                        }
                    }

                    // leaveFlagがtrueなら例えば0と1を01のように纏める
                    if (MainWindow.leaveFlag)
                    {
                        i++;
                        moji = t1[i].ToString() + t1[i + 1].ToString();
                        i++;
                        MainWindow.processingBinaryText = moji;
                        binaryText += MainWindow.processingBinaryText + " ";
                        MainWindow.leaveFlag = false;
                    }
                    else
                    {
                        MainWindow.processingBinaryText = LoadXML.mojiToBrailleID(moji);
                        binaryText += MainWindow.processingBinaryText + " ";
                    }
                }
                binaryText = binaryText.Replace("ERROR", "");
                binaryText = binaryText.Replace("  ", " ");
                t1 = t1.Replace("\\n", "\\n\n");
                t1 = t1.Replace("゛か", "が");
                t1 = t1.Replace("゛き", "ぎ");
                t1 = t1.Replace("゛く", "ぐ");
                t1 = t1.Replace("゛け", "げ");
                t1 = t1.Replace("゛こ", "ご");
                t1 = t1.Replace("゛さ", "ざ");
                t1 = t1.Replace("゛し", "じ");
                t1 = t1.Replace("゛す", "ず");
                t1 = t1.Replace("゛せ", "ぜ");
                t1 = t1.Replace("゛そ", "ぞ");
                t1 = t1.Replace("゛た", "だ");
                t1 = t1.Replace("゛ち", "ぢ");
                t1 = t1.Replace("゛つ", "づ");
                t1 = t1.Replace("゛て", "で");
                t1 = t1.Replace("゛と", "ど");
                t1 = t1.Replace("゛は", "ば");
                t1 = t1.Replace("゛ひ", "び");
                t1 = t1.Replace("゛ふ", "ぶ");
                t1 = t1.Replace("゛へ", "べ");
                t1 = t1.Replace("゛ほ", "ボ");
                t1 = t1.Replace("゜は", "ぱ");
                t1 = t1.Replace("゜ひ", "ぴ");
                t1 = t1.Replace("゜ふ", "ぷ");
                t1 = t1.Replace("゜へ", "ぺ");
                t1 = t1.Replace("゜ほ", "ぽ");
                t1 = t1.Replace("\n\n", "\n");

                if (MainWindow.nameData == "ルビー(J)" || MainWindow.nameData == "サファイア(J)" || MainWindow.nameData == "エメラルド(J)")
                {
                    t1 = t1.Replace(">", ">\n");
                    t1 = t1.Replace("\n<", "<");
                }

                MainWindow.displayingText = t1;
                binaryText = binaryText.TrimEnd();
                return binaryText;
            }

            else {
                // 普通の文字変換処理
                // 文字と対応するIDへの変換はloadXMLクラスで行う
                t1 = t1.Replace("０", "0");
                t1 = t1.Replace("１", "1");
                t1 = t1.Replace("２", "2");
                t1 = t1.Replace("３", "3");
                t1 = t1.Replace("４", "4");
                t1 = t1.Replace("５", "5");
                t1 = t1.Replace("６", "6");
                t1 = t1.Replace("７", "7");
                t1 = t1.Replace("８", "8");
                t1 = t1.Replace("９", "9");
                t1 = t1.Replace(" ", "　");

                for (int i = 0; i < t1.Length; i++)
                {

                    // []で囲まれた文字を特殊記号(F9 XX)に変換する
                    if (t1[i] == '[')
                    {
                        MainWindow.specialFlag = true;
                    }
                    else if (t1[i] == ']')
                    {
                        MainWindow.specialFlag = false;
                    }
                    // <>で囲まれた1byte文字(01など)をそのまま出力する(int型整数をtoStringするようなもの)
                    else if (t1[i] == '<')
                    {
                        MainWindow.leaveFlag = true;
                    }
                    else if (t1[i] == '>') {
                        MainWindow.leaveFlag = false;
                    }

                    // leaveFlagがtrueなら例えば0と1を01のように纏める
                    else if (MainWindow.leaveFlag)
                    {
                        moji = t1[i].ToString() + t1[i + 1].ToString();
                        i++;
                        MainWindow.processingBinaryText = LoadXML.mojiToID(moji);
                        binaryText += MainWindow.processingBinaryText + " ";
                    }

                    // \n, \p, \mを纏めたあと変換する
                    else if (t1[i] == '\\' && t1[i + 1] == 'n' || t1[i] == '\\' && t1[i + 1] == 'p' || t1[i] == '\\' && t1[i + 1] == 'm')
                    {
                        moji = t1[i].ToString() + t1[i + 1].ToString();
                        i++;
                        MainWindow.processingBinaryText = LoadXML.mojiToID(moji);
                        binaryText += MainWindow.processingBinaryText + " ";
                    }
                    // {Lv}, {PP}, {ID.}, {No}などを纏めた後変換する
                    else if (t1[i] == '{')
                    {
                        for (; t1[i] != '}'; i++)
                        {
                            m += t1[i].ToString();
                        }
                        m += '}';
                        moji = m;
                        MainWindow.processingBinaryText = LoadXML.mojiToID(moji);
                        binaryText += MainWindow.processingBinaryText + " ";
                    }
                    // 何もなければ普通に変換
                    else
                    {
                        moji = t1[i].ToString();
                        MainWindow.processingBinaryText = LoadXML.mojiToID(moji);
                        binaryText += MainWindow.processingBinaryText + " ";
                    }
                }
                binaryText = binaryText.Replace("ERROR", "");
                binaryText = binaryText.Replace("  ", " ");
                binaryText = binaryText.TrimEnd();

                t1 = t1.Replace("\\n", "\\n\n");
                t1 = t1.Replace("\\p", "\\p\n");
                t1 = t1.Replace("\\m", "\\m\n");
                t1 = t1.Replace("\n\n", "\n");
                MainWindow.displayingText = t1;

                return binaryText;
            }
        }

        public static String binaryToMoji(String t) {

            // textBox2のバイナリテキストを変換してtextBox1にテキストで渡す
            String ChangedText = ""; // 返り値
            String hex = "", secondHex, thirdHex = "", fourthHex ="", fifthHex = "";
            String binaryText = t;
            char[] ArrayBinaryText; // binaryTextをcharの配列にして一つずつ読み解くための変数
            bool onlyOnceFlag = false;
            bool unPlusFlag = false;
            bool roadTextScriptParamFlag = false;   //テキストスクリプトfc @@ xxのとき
            bool road2byteParamFlag = false;    // テキストスクリプト fc 0b xx xx, fc 10 xx xxのとき
            bool road3byteParamFlag = false;    // テキストスクリプト fc 04 xx yy zzのとき
            bool brailleParamFlagForRSE = false;   // RSEでは最初の6byteがメッセージウィンドウの位置、点字表示位置に割り当てられているため、<xx>で表示する

            // A~Fまでをa~fに置換する
            binaryText = binaryText.Replace("  ", " ");
            binaryText = binaryText.Replace("A", "a");
            binaryText = binaryText.Replace("B", "b");
            binaryText = binaryText.Replace("C", "c");
            binaryText = binaryText.Replace("D", "d");
            binaryText = binaryText.Replace("E", "e");
            binaryText = binaryText.Replace("F", "f");
            ArrayBinaryText = binaryText.ToCharArray();

            // 文字に空白や16進数以外が含まれていたらzに置換する
            for (int i = 0; i < binaryText.Length; i++)
            {

                if (' ' == binaryText[0]) { ArrayBinaryText[0] = 'z'; } // 0番目が半角スペースだとバグる
                if (' ' == binaryText[i]) { }
                else if ('0' == binaryText[i]) { }
                else if ('1' == binaryText[i]) { }
                else if ('2' == binaryText[i]) { }
                else if ('3' == binaryText[i]) { }
                else if ('4' == binaryText[i]) { }
                else if ('5' == binaryText[i]) { }
                else if ('6' == binaryText[i]) { }
                else if ('7' == binaryText[i]) { }
                else if ('8' == binaryText[i]) { }
                else if ('9' == binaryText[i]) { }
                else if ('a' == binaryText[i]) { }
                else if ('b' == binaryText[i]) { }
                else if ('c' == binaryText[i]) { }
                else if ('d' == binaryText[i]) { }
                else if ('e' == binaryText[i]) { }
                else if ('f' == binaryText[i]) { }
                else
                {
                    ArrayBinaryText[i] = 'z';
                }
            }

            // 16進数以外を削除する
            binaryText = new String(ArrayBinaryText); //ArrayBinaryTextの編集が終わったのでbinaryTextを最新のものに更新
            binaryText = binaryText.Replace("z", "");
            MainWindow.displayingBinaryText = binaryText;

            if (MainWindow.nameData == "ルビー(J)" || MainWindow.nameData == "サファイア(J)" || MainWindow.nameData == "エメラルド(J)")
            {
                brailleParamFlagForRSE = true;
            }

            // 点字用変換にチェックが入っている場合Braille.xmlを読み込む
            if (MainWindow.brilleFlag)
            {
                for (int i = 0; i < binaryText.Length; i++)
                {

                    if (binaryText[i] == ' ')
                    {
                        //空白は何もしない
                    }
                    else
                    {
                        if (i + 3 > binaryText.Length)
                        {
                            // 次の文字がないので何もしない
                        }
                        else
                        {
                            hex = binaryText[i].ToString() + binaryText[i + 1].ToString();
                        }
                        if (!brailleParamFlagForRSE)
                        {
                            {
                                if (hex == "3a")
                                {
                                    // 数符が来たら数字にする
                                    MainWindow.numberFlag = true;
                                }
                                else if (hex == "30" || hex == "00")
                                {
                                    // つなぎ符かスペースが来たら普通に読む
                                    MainWindow.numberFlag = false;
                                }
                                // 02か0aか22で始まる文字列なら2byte読み込み
                                else if (hex == "02")
                                {
                                    if (i + 3 > binaryText.Length)
                                    {
                                        // 次の文字がないので何もしない
                                    }
                                    else
                                    {
                                        secondHex = binaryText[i + 3].ToString() + binaryText[i + 4].ToString();

                                        switch (secondHex)
                                        {

                                            case "21":
                                                break;

                                            case "23":
                                                break;

                                            case "26":
                                                break;

                                            case "29":
                                                break;

                                            case "2b":
                                                break;

                                            case "2e":
                                                break;

                                            case "19":
                                                break;

                                            case "1b":
                                                break;

                                            case "1e":
                                                break;

                                            case "11":
                                                break;

                                            case "13":
                                                break;

                                            case "16":
                                                break;

                                            case "31":
                                                break;

                                            case "33":
                                                break;

                                            case "68":
                                                break;

                                            case "39":
                                                break;

                                            case "3b":
                                                break;

                                            case "3e":
                                                break;

                                            case "09":
                                                break;

                                            case "0b":
                                                break;

                                            case "0e":
                                                break;

                                            default:
                                                unPlusFlag = true;
                                                break;
                                        }
                                        if (!unPlusFlag)
                                        {
                                            hex = hex + " " + secondHex;
                                            i += 3;
                                        }
                                        else
                                        {
                                            /*
                                            何もしない
                                            次に使うので元に戻しておく
                                            */
                                            unPlusFlag = false;
                                        }
                                    }
                                }
                                else if (hex == "0a")
                                {
                                    if (i + 3 > binaryText.Length)
                                    {
                                        // 次の文字がないので何もしない
                                    }
                                    else
                                    {
                                        secondHex = binaryText[i + 3].ToString() + binaryText[i + 4].ToString();

                                        switch (secondHex)
                                        {
                                            case "21":
                                                break;

                                            case "23":
                                                break;

                                            case "26":
                                                break;

                                            case "29":
                                                break;

                                            case "2b":
                                                break;

                                            case "19":
                                                break;

                                            case "1b":
                                                break;

                                            case "1e":
                                                break;

                                            case "31":
                                                break;

                                            case "33":
                                                break;

                                            case "68":
                                                break;

                                            default:
                                                unPlusFlag = true;
                                                break;
                                        }
                                        if (!unPlusFlag)
                                        {
                                            hex = hex + " " + secondHex;
                                            i += 3;
                                        }
                                        else
                                        {
                                            /*
                                            何もしない
                                            次に使うので元に戻しておく
                                            */
                                            unPlusFlag = false;
                                        }
                                    }
                                }
                                else if (hex == "22")
                                {
                                    if (i + 3 > binaryText.Length)
                                    {
                                        // 次の文字がないので何もしない
                                    }
                                    else
                                    {
                                        secondHex = binaryText[i + 3].ToString() + binaryText[i + 4].ToString();

                                        switch (secondHex)
                                        {
                                            case "31":
                                                break;

                                            case "33":
                                                break;

                                            case "68":
                                                break;

                                            default:
                                                unPlusFlag = true;
                                                break;
                                        }

                                        if (!unPlusFlag)
                                        {
                                            hex = hex + " " + secondHex;
                                            i += 3;
                                        }
                                        else
                                        {
                                            /*
                                            何もしない
                                            次に使うので元に戻しておく
                                            */
                                            unPlusFlag = false;
                                        }
                                    }
                                }
                            }
                            MainWindow.processingBinaryText = LoadXML.BrailleIDtoMoji(hex);
                            ChangedText += MainWindow.processingBinaryText;
                        }
                        else
                        {
                            // RSE点字用 最初の6byteを<xx>で表示
                            for (int j = 0; j < 18; j++)
                            {
                                if (binaryText[j] != ' ')
                                {
                                    hex = binaryText[j].ToString() + binaryText[j + 1].ToString();
                                    MainWindow.processingBinaryText = '<' + hex + '>';
                                    ChangedText += MainWindow.processingBinaryText;
                                    j++;
                                }
                            }
                            brailleParamFlagForRSE = false;
                            i += 17;
                        }
                    }
                }
            

                hex = hex = binaryText[binaryText.Length - 2].ToString() + binaryText[binaryText.Length - 1].ToString(); 
                
                MainWindow.processingBinaryText = LoadXML.BrailleIDtoMoji(hex);
                ChangedText += MainWindow.processingBinaryText;

                ChangedText = ChangedText.Replace("ERROR", "");
                ChangedText = ChangedText.Replace("゛か", "が");
                ChangedText = ChangedText.Replace("゛き", "ぎ");
                ChangedText = ChangedText.Replace("゛く", "ぐ");
                ChangedText = ChangedText.Replace("゛け", "げ");
                ChangedText = ChangedText.Replace("゛こ", "ご");
                ChangedText = ChangedText.Replace("゛さ", "ざ");
                ChangedText = ChangedText.Replace("゛し", "じ");
                ChangedText = ChangedText.Replace("゛す", "ず");
                ChangedText = ChangedText.Replace("゛せ", "ぜ");
                ChangedText = ChangedText.Replace("゛そ", "ぞ");
                ChangedText = ChangedText.Replace("゛た", "だ");
                ChangedText = ChangedText.Replace("゛ち", "ぢ");
                ChangedText = ChangedText.Replace("゛つ", "づ");
                ChangedText = ChangedText.Replace("゛て", "で");
                ChangedText = ChangedText.Replace("゛と", "ど");
                ChangedText = ChangedText.Replace("゛は", "ば");
                ChangedText = ChangedText.Replace("゛ひ", "び");
                ChangedText = ChangedText.Replace("゛ふ", "ぶ");
                ChangedText = ChangedText.Replace("゛へ", "べ");
                ChangedText = ChangedText.Replace("゛ほ", "ぼ");
                ChangedText = ChangedText.Replace("゜は", "ぱ");
                ChangedText = ChangedText.Replace("゜ひ", "ぴ");
                ChangedText = ChangedText.Replace("゜ふ", "ぷ");
                ChangedText = ChangedText.Replace("゜へ", "ぺ");
                ChangedText = ChangedText.Replace("゜ほ", "ぽ");
                ChangedText = ChangedText.Replace("\\n", "\\n\n");

                if (MainWindow.nameData == "ルビー(J)" || MainWindow.nameData == "サファイア(J)" || MainWindow.nameData == "エメラルド(J)")
                {
                    ChangedText = ChangedText.Replace(">", ">\n");
                    ChangedText = ChangedText.Replace("\n<", "<");
                }

                MainWindow.displayingText = ChangedText;
                MainWindow.numberFlag = false;
                return ChangedText;
            }
            else {
                // 普通の文字変換処理
                for (int i = 0; i < binaryText.Length; i++)
                {

                    if (binaryText[i] == ' ')
                    {
                        //空白は何もしない
                    }

                    else
                    {
                        hex = binaryText[i].ToString() + binaryText[i + 1].ToString();
                        // fdで始まる文字列なら1byte読み込み
                        // f9かfcで始まる文字列なら2byte読み込み
                        // fcの次のbyteが01~06以外なら3byte読み込み

                        if (hex == "fc")
                        {
                            if (i + 3 > binaryText.Length)
                            {   // 次の文字がないので何もしない
                            }
                            else
                            {
                                secondHex = binaryText[i + 3].ToString() + binaryText[i + 4].ToString();
                                if (i + 6 > binaryText.Length)
                                {
                                    hex = hex + " " + secondHex;
                                    i += 3;
                                }
                                else
                                {
                                    thirdHex = binaryText[i + 6].ToString() + binaryText[i + 7].ToString();

                                    if (LoadXML.existsMoji(hex + " " + secondHex + " " + thirdHex))
                                    {
                                        hex = hex + " " + secondHex + " " + thirdHex;
                                        i += 6;
                                    }
                                    else
                                    {
                                        hex = hex + " " + secondHex;

                                        if (secondHex == "04")
                                        {
                                            thirdHex = binaryText[i + 6].ToString() + binaryText[i + 7].ToString();
                                            fourthHex = binaryText[i + 9].ToString() + binaryText[i + 10].ToString();
                                            fifthHex = binaryText[i + 12].ToString() + binaryText[i + 13].ToString();
                                            i += 12;
                                            road3byteParamFlag = true;


                                        }
                                        else if (secondHex == "0b" || secondHex == "10")
                                        {
                                            thirdHex = binaryText[i + 6].ToString() + binaryText[i + 7].ToString();
                                            fourthHex = binaryText[i + 9].ToString() + binaryText[i + 10].ToString();
                                            i += 3;
                                            road2byteParamFlag = true;

                                        }
                                        else
                                        {
                                            //  2022/10/28, fc 07, fc 09, fc 0a, fc 0f, fc 17, fc 18など後ろの引数がないものに対応
                                            if (secondHex == "07" || secondHex == "09" || secondHex == "0a" || secondHex == "0f" || secondHex == "17" || secondHex == "18")
                                            {
                                                i += 3;
                                            }
                                            else
                                            {
                                                i += 6;
                                                roadTextScriptParamFlag = true;
                                            }

                                        }
                                    }
                                }
                            }
                        }

                        else if (hex == "fd")
                        {
                            if (i + 3 > binaryText.Length)
                            {   // 次の文字がないので何もしない
                            }
                            else
                            {
                                secondHex = binaryText[i + 3].ToString() + binaryText[i + 4].ToString();
                                hex = hex + " " + secondHex;
                                i += 3;
                                //roadTextScriptParamFlag = true;
                            }
                        }
                        else if (hex == "f9")
                        {
                            if (i + 3 > binaryText.Length)
                            {   // 次の文字がないので何もしない
                            }
                            else
                            {

                                secondHex = binaryText[i + 3].ToString() + binaryText[i + 4].ToString();

                                // f9 00 ~ f9 16までは既存のコードなのでspecial.xmlを読みにいかない
                                switch (secondHex)
                                {
                                    case "00":
                                        break;

                                    case "01":
                                        break;

                                    case "02":
                                        break;

                                    case "03":
                                        break;

                                    case "04":
                                        break;

                                    case "05":
                                        break;

                                    case "06":
                                        break;

                                    case "07":
                                        break;

                                    case "08":
                                        break;

                                    case "09":
                                        break;

                                    case "0a":
                                        break;

                                    case "0b":
                                        break;

                                    case "0c":
                                        break;

                                    case "0d":
                                        break;

                                    case "0e":
                                        break;

                                    case "0f":
                                        break;

                                    case "10":
                                        break;

                                    case "11":
                                        break;

                                    case "12":
                                        break;

                                    case "13":
                                        break;

                                    case "14":
                                        break;

                                    case "15":
                                        break;

                                    case "16":
                                        break;

                                    default:
                                        // special.xmlを読みに行く
                                        MainWindow.specialFlag = true;
                                        hex = hex + " " + secondHex;
                                        i += 3;
                                        // f9で始まる特殊記号を示す[を一度だけ入れる
                                        if (!onlyOnceFlag)
                                        {
                                            ChangedText += '[';
                                            onlyOnceFlag = true;
                                        }
                                        break;
                                }
                                if (!MainWindow.specialFlag)
                                {
                                    hex = hex + " " + secondHex;
                                    i += 3;
                                }
                            }
                        }
                        if (!roadTextScriptParamFlag)
                        {
                            MainWindow.processingBinaryText = LoadXML.IDtoMoji(hex);
                        }
                        else
                        {
                            MainWindow.processingBinaryText = LoadXML.IDtoMoji(hex) + '<' + thirdHex + '>';

                            if (road2byteParamFlag)
                            {
                                MainWindow.processingBinaryText += '<' + fourthHex + '>';
                                road2byteParamFlag = false;
                            }
                            else if (road3byteParamFlag)
                            {
                                MainWindow.processingBinaryText += '<' + fourthHex + '>' + '<' + fifthHex + '>';
                                road3byteParamFlag = false;
                            }
                            roadTextScriptParamFlag = false;
                        }

                        ChangedText += MainWindow.processingBinaryText;

                        // 以降f9がなかったら]を入れて終了
                        if (MainWindow.specialFlag && (i > binaryText.LastIndexOf("f9")))
                        {
                            ChangedText += ']';
                            onlyOnceFlag = false;
                            MainWindow.specialFlag = false;
                        }
                    }

                    i += 2;

                }
                ChangedText = ChangedText.Replace("ERROR", "");
            }
            ChangedText = ChangedText.Replace("\\n", "\\n\n");
            ChangedText = ChangedText.Replace("\\m", "\\m\n");
            ChangedText = ChangedText.Replace("\\p", "\\p\n");

            return ChangedText;

        }
    public static void binary(String o, String t) {
            byte[] bin;

            // numに10進数化されたオフセットを代入
            int num = function.judgeOffset(o);

            String binaryText = t.Replace(" ", "");
            int be = t.Replace(" ", "").Length / 2;
            bin = new byte[be];
            int j = 0;

            for (int i = 0; i < be*2; i+=2)
            {   // byte型に変換する
                String w = binaryText.Substring(i, 2);
                bin[j] = Convert.ToByte(w, 16);
                j++;
            }

            // オフセットに代入していく
            for (int i = 0; i < be; i++)
            {
                MainWindow.buf[num + i] = bin[i];
            }
            // 保存
            System.IO.FileStream br = new System.IO.FileStream(
                MainWindow.fileName,
                System.IO.FileMode.Create,
                System.IO.FileAccess.ReadWrite);

            br.Write(MainWindow.buf,0, MainWindow.fileSize);
            br.Close();   

        }
    }
}

