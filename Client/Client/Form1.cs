using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Client
{
    public partial class Form1 : Form
    {
        CryptoService.CryptoServiceClient client;
        public Form1()
        {
            InitializeComponent();
            client = new CryptoService.CryptoServiceClient();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog()==System.Windows.Forms.DialogResult.OK)
            {
                textBox3.Text = ofd.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox4.Text = ofd.FileName;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(textBox3.Text.Length!=0 && textBox4.Text.Length!=0)
            {
                byte[] ucitaniBajtovi = client.loadBytesAsync(textBox3.Text).Result;
                byte[] bajtoviEnkripcija = client.A51_EncryptBytesAsync(ucitaniBajtovi).Result;
                client.saveBytesAsync(bajtoviEnkripcija, textBox4.Text);
                MessageBox.Show("Fajl enkriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite ulazni i izlazni fajl!");
            }
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length != 0 && textBox4.Text.Length != 0)
            {
                byte[] ucitaniBajtoviDekripcija = client.loadBytesAsync(textBox3.Text).Result; 
                string a = client.A51_DecryptBytesAsync(ucitaniBajtoviDekripcija).Result;
                client.saveTextAsync(a, textBox4.Text);
                MessageBox.Show("Fajl dekriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite ulazni i izlazni fajl!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox5.Text = ofd.FileName;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length != 0 && textBox6.Text.Length != 0 && textBox17.Text.Length != 0 && textBox18.Text.Length != 0)
            {
                int p = Convert.ToInt32(textBox17.Text);
                int q = Convert.ToInt32(textBox18.Text);
                int[] ucitaniInt = client.readFromFileIntAsync(textBox5.Text).Result;
                string izlaz = client.RSA_DecryptAsync(ucitaniInt, p, q).Result;
                client.saveTextAsync(izlaz, textBox6.Text);
                MessageBox.Show("Fajl dekriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite p, q, ulazni i izlazni fajl!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox5.Text.Length != 0 && textBox6.Text.Length != 0 && textBox17.Text.Length != 0 && textBox18.Text.Length != 0)
            {
                byte[] ucitaniBajtovi = client.loadBytesAsync(textBox5.Text).Result;
                int p = Convert.ToInt32(textBox17.Text);
                int q = Convert.ToInt32(textBox18.Text);
                int[] res = client.RSA_EncryptAsync(ucitaniBajtovi,p,q).Result;
                client.writeToFileIntAsync(textBox6.Text, res);
                MessageBox.Show("Fajl enkriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite p, q, ulazni i izlazni fajl!");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox6.Text = ofd.FileName;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox7.Text = ofd.FileName;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox8.Text = ofd.FileName;
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox7.Text.Length != 0 && textBox8.Text.Length != 0 && textBox9.Text.Length != 0)
            {
                string ucitaniText = client.readTextAsync(textBox7.Text).Result;
                string IV = textBox9.Text;
                byte[] bajtoviEnkripcija = client.CFB_EncryptAsync(ucitaniText, IV).Result;
                client.saveBytesAsync(bajtoviEnkripcija, textBox8.Text);
                MessageBox.Show("Fajl enkriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite ulazni i izlazni fajl i initialization vector!");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (textBox7.Text.Length != 0 && textBox8.Text.Length != 0 && textBox9.Text.Length != 0)
            {
                byte[] ucitaniBajtoviDekripcija = client.loadBytesAsync(textBox7.Text).Result;
                string IV = textBox9.Text;
                string dekriptovanText = client.CFB_DecryptAsync(ucitaniBajtoviDekripcija, IV).Result;
                client.saveTextAsync(dekriptovanText, textBox8.Text);
                MessageBox.Show("Fajl dekriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite ulazni i izlazni fajl i initialization vector!");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox10.Text = ofd.FileName;
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox11.Text = ofd.FileName;
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (textBox10.Text.Length != 0 && textBox11.Text.Length != 0 && textBox12.Text.Length!=0)
            {
                string ucitanText = client.readTextAsync(textBox10.Text).Result;
                string key = textBox12.Text;
                string enkriptovanText = client.PlayFair_EncryptAsync(ucitanText, key).Result;
                client.saveTextAsync(enkriptovanText, textBox11.Text);
                MessageBox.Show("Fajl enkriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite ulazni i izlazni fajl i kljuc!");
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (textBox10.Text.Length != 0 && textBox11.Text.Length != 0 && textBox12.Text.Length != 0)
            {
                string ucitanText = client.readTextAsync(textBox10.Text).Result;
                string key = textBox12.Text;
                string dekriptovanText = client.PlayFair_DecryptAsync(ucitanText, key).Result;
                client.saveTextAsync(dekriptovanText, textBox11.Text);
                MessageBox.Show("Fajl dekriptovan!");
            }
            else
            {
                MessageBox.Show("Odaberite ulazni i izlazni fajl i kljuc!");
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox13.Text = ofd.FileName;
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "Text file|*.txt";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox14.Text = ofd.FileName;
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if(textBox13.Text.Length!=0 && textBox14.Text.Length!=0)
            {
                string ucitanText1 = client.readTextAsync(textBox13.Text).Result;
                string ucitanText2 = client.readTextAsync(textBox14.Text).Result;
                string hash1 = client.SHA2HashAsync(ucitanText1).Result;
                string hash2 = client.SHA2HashAsync(ucitanText2).Result;
                if (String.Compare(hash1, hash2) == 0)
                {
                    MessageBox.Show("Dekripcija je dobra!");
                }
                else
                {
                    MessageBox.Show("Dekripcija nije dobra!");
                }
            }
            else
            {
                MessageBox.Show("Morate odabrati prvi i drugi fajl!");
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "BMP Image|*.bmp";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox15.Text = ofd.FileName;
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open file";
            ofd.Filter = "BMP Image|*.bmp";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox16.Text = ofd.FileName;
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if(textBox15.Text.Length != 0 && textBox16.Text.Length != 0)
            {
                byte[] bajtovi = client.loadBitmap(textBox15.Text);
                byte[] prvih54byte = new byte[54];
                for (int i = 0; i < 54; i++)
                {
                    prvih54byte[i] = bajtovi[i];
                }
                byte[] bajtoviZaEnkripciju = new byte[bajtovi.Length - 54];
                for (int i = 54; i < bajtovi.Length; i++)
                {
                    bajtoviZaEnkripciju[i - 54] = bajtovi[i];
                }
                byte[] enkriptovaniBajtovi = client.A51_EncryptBytesAsync(bajtoviZaEnkripciju).Result;
                byte[] bajtoviZaUpis = new byte[prvih54byte.Length + enkriptovaniBajtovi.Length];
                for (int i = 0; i < prvih54byte.Length; i++)
                    bajtoviZaUpis[i] = prvih54byte[i];
                for (int j = 0; j < enkriptovaniBajtovi.Length; j++)
                    bajtoviZaUpis[prvih54byte.Length + j] = enkriptovaniBajtovi[j];
                client.saveBitmap(bajtoviZaUpis, textBox16.Text);
                MessageBox.Show("Enkripcija uspesna!");
            }
            else
            {
                MessageBox.Show("Morate odabrati ulazni i izlazni fajl!");
            }
            
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (textBox10.Text.Length != 0 && textBox11.Text.Length != 0 && textBox12.Text.Length!=0 && textBox19.Text.Length != 0)
            {
                int thNum = Convert.ToInt32(textBox19.Text);
                client.PlayFair_EncryptParallel(textBox10.Text, textBox11.Text, thNum, textBox12.Text);
                MessageBox.Show("Fajl enkriptovan!");
            }
            else
            {
                MessageBox.Show("Popunite sva polja!");
            }
        }
    }
}
