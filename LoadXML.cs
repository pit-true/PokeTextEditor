using System;
using System.Linq;
using System.Xml.Linq;

namespace WpfApplication1
{
    class LoadXML
    {
        public static string mojiToID(String moji)
        {
            if (MainWindow.specialFlag)
            {   /*
                specialFlagが立っている場合にspecial.xmlを読み込み
                []で囲まれた文字をF9 XXで始まるコードに変換する
                */
                var xelm = XElement.Load(@"setting\special.xml");
                var emp = (
            from p in xelm.Elements("node")
            where p.Element("moji").Value == moji
            select p).SingleOrDefault();

                return emp?.Element("ID")?.Value ?? "";

            }

            else if (MainWindow.leaveFlag)
            {   // そのまま返す <01>を01とする
                return moji;
            }

            else
            {   // 通常の文字変換処理
                var xelm = XElement.Load(@"setting\moji.xml");
                var emp = (
            from p in xelm.Elements("node")
            where p.Element("moji").Value == moji
            select p).FirstOrDefault();

                return emp?.Element("ID")?.Value ?? "";

            }
        }

        public static bool existsMoji(String ID)
        {

            var xelm = XElement.Load(@"setting\moji.xml");
            var emp = (
        from p in xelm.Elements("node")
        where p.Element("ID").Value == ID
        select p).SingleOrDefault();

            try {
                return !emp.IsEmpty;
            }
            catch {
                return false;
            }

        }
        public static string IDtoMoji(string ID)
        {   // F9 XXで始まる特殊記号はspecial.xmlを読み込む

            if (MainWindow.specialFlag)
            {
                var xelmSP = XElement.Load(@"setting\special.xml");
                var emp = (
                    from p in xelmSP.Elements("node")
                    where p.Element("ID").Value == ID
                    select p).SingleOrDefault();

                return emp?.Element("moji")?.Value ?? "";

            }
            else
            {
                var xelm = XElement.Load(@"setting\moji.xml");

                var emp = (
            from p in xelm.Elements("node")
            where p.Element("ID").Value == ID
            select p).SingleOrDefault();

                return emp?.Element("moji")?.Value ?? "";

            }
        }
        public static String mojiToBrailleID(String moji)
        {
            var xelm = XElement.Load(@"setting\braille.xml");
            var emp = (
        from p in xelm.Elements("node")
        where p.Element("moji").Value == moji
        select p).SingleOrDefault();

            return emp?.Element("ID")?.Value ?? "";
        }

        public static String BrailleIDtoMoji(String ID)
        {
            var xelm = XElement.Load(@"setting\braille.xml");

            // 表記ゆれのあるコードを一つに絞る(全角スペース半角スペースなど)
            if (ID == "00")
            {
                var m = "　";
                return m;
            }
            else if (ID == "0e")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "0";
                    return m;
                }
                else
                {
                    var m = "ろ";
                    return m;
                }
            }
            else if (ID == "01")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "1";
                    return m;
                }
                else
                {
                    var m = "あ";
                    return m;
                }
            }
            else if (ID == "05")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "2";
                    return m;
                }
                else
                {
                    var m = "い";
                    return m;
                }
            }
            else if (ID == "03")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "3";
                    return m;
                }
                else
                {
                    var m = "う";
                    return m;
                }
            }
            else if (ID == "0b")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "4";
                    return m;
                }
                else
                {
                    var m = "る";
                    return m;
                }
            }
            else if (ID == "09")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "5";
                    return m;
                }
                else
                {
                    var m = "ら";
                    return m;
                }
            }
            else if (ID == "07")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "6";
                    return m;
                }
                else
                {
                    var m = "え";
                    return m;
                }
            }
            else if (ID == "0f")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "7";
                    return m;
                }
                else
                {
                    var m = "れ";
                    return m;
                }
            }
            else if (ID == "0d")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "8";
                    return m;
                }
                else
                {
                    var m = "り";
                    return m;
                }
            }
            else if (ID == "06")
            {
                if (MainWindow.numberFlag)
                {
                    var m = "9";
                    return m;
                }
                else
                {
                    var m = "お";
                    return m;
                }
            }

            else
            {
                var emp = (
            from p in xelm.Elements("node")
            where p.Element("ID").Value == ID
            select p).SingleOrDefault();

                return emp?.Element("moji")?.Value ?? "";

            }
        }
    public static String shortcutMoji(int i)
        {
            var xelmSP = XElement.Load(@"setting\special.xml");
            var emp = (
                from p in xelmSP.Elements("shortcut")
                where p.Element("number").Value == i.ToString()
                select p).SingleOrDefault();

            return emp.Element("moji").Value;
        }
        public static String shortcutID(int i)
        {
            var xelmSP = XElement.Load(@"setting\special.xml");
            var emp = (
                from p in xelmSP.Elements("shortcut")
                where p.Element("number").Value == i.ToString()
                select p).SingleOrDefault();

            return emp.Element("ID").Value;
        }
    }
}