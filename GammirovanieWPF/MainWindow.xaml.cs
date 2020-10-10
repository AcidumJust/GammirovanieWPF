using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace GammirovanieWPF
{
    /// <summary>
    /// Вывод гаммы пробел это 0
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        static List<string> G = new List<string>();
        static string alfabetRU = " абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ0123456789";
        static string RU = "абвгдеёжзийклмнопрстуфхцчшщъыьэюяАБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ";
        static string alfabetEN = " abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        static string EN = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static string alf = alfabetRU;
        private void rbK_Click(object sender, RoutedEventArgs e)
        {
            if (sender == rbA)
            {
                txtKeyW.IsReadOnly = txtKeyG.IsReadOnly = !false;
                txtKeyW.Text = txtKeyG.Text = "";
            }
            if (sender == rbK)
            {
                txtKeyW.IsReadOnly = !true;
                txtKeyG.IsReadOnly = !false;
                txtKeyW.Text = txtKeyG.Text = "";
            }
            if (sender == rbG)
            {
                txtKeyW.IsReadOnly = !false;
                txtKeyG.IsReadOnly = !true;
                txtKeyW.Text = txtKeyG.Text = "";
            }
        }
        //Генерация листа криптостойких гамм
        static List<string> GammaListGenerate(string alf)
        {
            List<string> res = new List<string>();
            string s;
            for (int i = 1; i < alf.Length; ++i)
            {
                s = ConverterIntTo01(i);
                if (s.Count(c1 => c1 == '0') == s.Count(c1 => c1 == '1'))
                    res.Add(s);
            }
            return res;
        }
        //Авто генерация гаммы
        List<string> GenerateStrongGamma(string str, string alf)
        {
            List<string> msg = new List<string>();
            List<string> strongG = GammaListGenerate(alf);
            Random rand = new Random();
            foreach (var c in str)
            {
                if (alf.Contains(c))
                    msg.Add(strongG[rand.Next(strongG.Count)]);
            }
            return msg;
        }
        static List<string> GenerateWordGamma(string str, string alf)
        {
            List<string> msg = new List<string>();
            foreach (var c in str)
            {
                if (alf.Contains(c))
                    msg.Add(ConverterIntTo01(alf.IndexOf(c)));
            }

            return msg;
        }
        //static List<string> GenerateSubGamma(string str, List<string> g, string alfabet)
        //{
        //    List<string> msg = new List<string>();
        //    int k = 0;
        //    foreach (var c in str)
        //    {
        //        if (alfabet.Contains(c))
        //            msg.Add(g[k++]);
        //        else
        //            msg.Add("");
        //        if (k >= g.Count)
        //            k = 0;
        //    }
        //    return msg;
        //}
        
        static string SumMod2(string str1, string str2)
        {
            string str = "";
            for (int i = 0; i < str1.Length; ++i)
            {
                str += (int.Parse(str1[i].ToString()) + int.Parse(str2[i].ToString())) % 2;
            }

            return str;
        }
        //Из гаммы в инт
        static int Converter01ToInt(string s)
        {
            int res = 0;
            for (int i = s.Length - 1, j = 0; i >= 0; --i, ++j)
                if (s[i] == '1')
                    res += (int)Math.Pow(2, j);

            return res;
        }
        //Из инта в гамму
        static string ConverterIntTo01(int s)
        {
            Stack<int> st = new Stack<int>();
            string res = "";
            while (s > 0)
            {
                st.Push(s % 2);
                s = (s - s % 2) / 2;
            }
            while (st.Count < 8)
                st.Push(0);
            while (st.Count > 0)
                res += st.Pop();
            return res;
        }

        private bool GenerateG(object sender)
        {
            try
            {
                if ((bool)rbK.IsChecked)
                {
                    if (txtKeyW.Text.Length > 0)
                    {
                        string[] s = txtKeyW.Text.Split();
                        txtKeyG.Text = txtKeyW.Text = "";
                        
                        foreach (var v in s)
                            txtKeyW.Text += v;
                        
                        LanguageKeyWCheck(txtKeyW.Text);
                        
                        foreach (var v in txtKeyW.Text)
                            if (!alf.Contains(v))
                                throw new Exception("KeyWord");
                        G = GenerateWordGamma(txtKeyW.Text, alf);
                        
                        for (int i = 0; i < G.Count - 1; ++i)
                            if (G[i].Length > 1)
                                txtKeyG.Text += G[i] + " ";
                        
                        if (G[G.Count - 1].Length > 1)
                            txtKeyG.Text += G[G.Count - 1];
                        
                        return true;
                    }
                }
                else if ((bool)rbG.IsChecked)
                {
                    string[] s = txtKeyG.Text.Split();
                    foreach (var c in s)
                        if (c.Length == 8)
                        {
                            foreach (var a in c)
                            {
                                if (a != '1' && a != '0')
                                {
                                    throw new Exception("KeyG");
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("KeyG");
                        }
                    G.Clear();
                    G.AddRange(s);
                    txtKeyG.Text = "";
                    for (int i = 0; i < G.Count - 1; ++i)
                    {
                        if (G[i].Length > 1)
                        {
                            //txtKeyW.Text += alf[Converter01ToInt(G[i])];
                            txtKeyG.Text += G[i] + " ";
                        }
                    }
                    if (G[G.Count - 1].Length > 1)
                    {
                        txtKeyG.Text += G[G.Count - 1];
                        //txtKeyW.Text += alf[Converter01ToInt(G[G.Count - 1])];
                    }
                    return true;
                }
                else if ((bool)rbA.IsChecked)
                {
                    if (txtOrig.Text.Length > 0 && sender != btnDeshifr)
                    {
                        G = GenerateStrongGamma(txtOrig.Text, alf);
                        txtKeyG.Text = txtKeyW.Text = "";
                        for (int i = 0; i < G.Count - 1; ++i)
                        {
                            if (G[i].Length > 1)
                            {
                                txtKeyW.Text += alf[Converter01ToInt(G[i])];
                                txtKeyG.Text += G[i] + " ";
                            }
                        }
                        if (G[G.Count - 1].Length > 1)
                        {
                            txtKeyG.Text += G[G.Count - 1];
                            txtKeyW.Text += alf[Converter01ToInt(G[G.Count - 1])];
                        }
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "KeyWord":
                        MessageBox.Show("Ключ-слово не соответствует требованиям");
                        break;
                    case "KeyG":
                        MessageBox.Show("Ключ-гамма не соответствует требованиям");
                        break;
                    case "EN_txt_in_RU_txt":
                        MessageBox.Show("В тексте сообщения обнаружены АНГЛИЙСКИЕ символы");
                        break;
                    case "RU_txt_in_EN_txt":
                        MessageBox.Show("В тексте сообщения обнаружены РУССКИЕ символы");
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
            return false;
        }

        private void LanguageKeyWCheck(string str)
        {
            foreach (var a in str)
            {
                if (alfBox.SelectionBoxItem.ToString() == "RU")
                {
                    if (EN.IndexOf(a) >= 0)
                    {
                        throw new Exception("KeyWord");
                    }
                }
                else if (alfBox.SelectionBoxItem.ToString() == "EN")
                {
                    if (RU.IndexOf(a) >= 0)
                    {
                        throw new Exception("KeyWord");
                    }
                }
            }
        }
        private void LanguageCheck(string str)
        {
            foreach (var a in str)
            {
                if (alfBox.SelectionBoxItem.ToString() == "RU")
                {
                    if (EN.IndexOf(a) >= 0)
                    {
                        throw new Exception("EN_txt_in_RU_txt");
                    }
                }
                else if (alfBox.SelectionBoxItem.ToString() == "EN")
                {
                    if (RU.IndexOf(a) >= 0)
                    {
                        throw new Exception("RU_txt_in_EN_txt");
                    }
                }
            }
        }

        private void alfBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((alfBox.SelectedItem as ComboBoxItem).Content.ToString() == "RU")
            {
                alf = alfabetRU;
            }
            else if ((alfBox.SelectedItem as ComboBoxItem).Content.ToString() == "EN")
            {
                alf = alfabetEN;
            }
        }

        private void btnShifr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LanguageCheck(txtOrig.Text);
                txtGOrig.Text = "";
                if (txtOrig.Text.Length > 0)
                {
                    for (int i = 0; i < txtOrig.Text.Length - 1; ++i)
                        if (alf.Contains(txtOrig.Text[i]))
                            txtGOrig.Text += ConverterIntTo01(alf.IndexOf(txtOrig.Text[i])) + " ";
                        else
                            txtGOrig.Text += txtOrig.Text[i] + " ";
                    if (alf.Contains(txtOrig.Text[txtOrig.Text.Length - 1]))
                        txtGOrig.Text += ConverterIntTo01(alf.IndexOf(txtOrig.Text[txtOrig.Text.Length - 1]));
                    else
                        txtGOrig.Text += txtOrig.Text[txtOrig.Text.Length - 1];
                }

                if (GenerateG(sender))
                {
                    txtShifr.Text = "";
                    int k = 0;
                    for (int i = 0; i < txtOrig.Text.Length; ++i)
                    {
                        if (alf.Contains(txtOrig.Text[i]))
                        {
                            txtShifr.Text += SumMod2(G[k++], ConverterIntTo01(alf.IndexOf(txtOrig.Text[i]))) + " ";
                        }
                        else
                            txtShifr.Text += txtOrig.Text[i] + " ";
                        if (k >= G.Count)
                            k = 0;
                    }
                    //for (int i = 0; i < txtOrig.Text.Length; ++i)
                    //{
                    //    if (alf.Contains(txtOrig.Text[i]))
                    //    {
                    //        txtShifr.Text += alf[Converter01ToInt(SumMod2(G[k++], ConverterIntTo01(alf.IndexOf(txtOrig.Text[i])))) % alf.Length];
                    //    }
                    //    else
                    //        txtShifr.Text += txtOrig.Text[i];
                    //    if (k >= G.Count)
                    //        k = 0;
                    //}
                }
            }
            catch(Exception ex)
            {
                switch (ex.Message)
                {
                    case "KeyWord":
                        MessageBox.Show("Ключ-слово не соответствует требованиям");
                        break;
                    case "KeyG":
                        MessageBox.Show("Ключ-гамма не соответствует требованиям");
                        break;
                    case "EN_txt_in_RU_txt":
                        MessageBox.Show("В тексте сообщения обнаружены АНГЛИЙСКИЕ символы");
                        break;
                    case "RU_txt_in_EN_txt":
                        MessageBox.Show("В тексте сообщения обнаружены РУССКИЕ символы");
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        private void btnDeshifr_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string[] s = txtShifr.Text.Split();
                if (GenerateG(sender))
                {
                    txtDeshifr.Text = "";
                    int k = 0;
                    for (int i = 0; i < s.Length; ++i)
                    {
                        if (s[i].Length > 1)
                            txtDeshifr.Text += alf[Converter01ToInt(SumMod2(G[k++], s[i]))];
                        else
                            txtDeshifr.Text += s[i];
                        if (k >= G.Count)
                            k = 0;
                    }
                    //for (int i = 0; i < txtShifr.Text.Length; ++i)
                    //{
                    //    if (alf.Contains(txtShifr.Text[i]))
                    //        txtDeshifr.Text += alf[Converter01ToInt(SumMod2(G[k++], ConverterIntTo01(alf.IndexOf(txtShifr.Text[i])))) % alf.Length];
                    //    else
                    //        txtDeshifr.Text += txtShifr.Text[i];
                    //    if (k >= G.Count)
                    //        k = 0;
                    //}
                }
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "KeyWord":
                        MessageBox.Show("Ключ-слово не соответствует требованиям");
                        break;
                    case "KeyG":
                        MessageBox.Show("Ключ-гамма не соответствует требованиям");
                        break;
                    case "EN_txt_in_RU_txt":
                        MessageBox.Show("В тексте сообщения обнаружены АНГЛИЙСКИЕ символы");
                        break;
                    case "RU_txt_in_EN_txt":
                        MessageBox.Show("В тексте сообщения обнаружены РУССКИЕ символы");
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }
    }
}