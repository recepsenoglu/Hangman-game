using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adam_asmaca_oyunu           // Written by Recep Oğuzhan Şenoğlu
                                       // Recep Oğuzhan Şenoğlu tarafından yazılmıştır
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox11.Focus();
            textBoxes = new TextBox[8];
            textBoxes[0] = textBox1;
            textBoxes[1] = textBox2;
            textBoxes[2] = textBox3;
            textBoxes[3] = textBox4;
            textBoxes[4] = textBox5;
            textBoxes[5] = textBox6;
            textBoxes[6] = textBox7;
            textBoxes[7] = textBox8;

            Read_all_files();
            New_Word();
            label7.Text = "";
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "<Pending>")]
        string dir = Directory.GetCurrentDirectory(); // Skorların tutulduğu metin belgesinin yolunu belirlemek için bu kodu kullandım.

        // Gerekli Değişkenleri yarattığım kısım.
        TextBox[] textBoxes;
        string the_word;
        string letter;
        string return_value;
        string[] letters;
        string[] old_letters;
        int digits;
        int i = 0;
        int life;
        int word_count;
        int point = 0;
        bool boolean;
        string nick;
        int score;
        int l = 0;

        private void New_Word() // Yeni kelime yaratma metodu
        {
            Random random = new Random();
            int random_number = random.Next(1, word_count);
            the_word = Read_File(random_number); // Kelime veritabanından rastgele bir kelime seçtik.
            switch (the_word.Length)
            {
                case 5:
                    digits = 5;
                    i = 3;
                    break;
                case 6:
                    digits = 6;
                    i = 2;
                    break;
                case 7:
                    digits = 7;
                    i = 1;
                    break;
                case 8:
                    digits = 8;
                    i = 0;
                    break;
            }
            letters = new string[digits];
            for(int i=0; i < digits;i++) // Bulunacak kelimenin harflerini letters adlı diziye attık.
            {
                letters[i] = the_word[i].ToString();
            }
            life = digits + 2;
            old_letters = new string[life+10];
            label6.Text = life.ToString();
            pictureBox2.Image = Image.FromFile(dir + "\\icons\\" + i + ".png"); // Adam asmaca resmini ekledik.
            
            for(int j = digits; j<digits+i;j++)
            {
                textBoxes[j].Visible = false; // Fazlalık textboxları görünmez yaptık.(Eğer kelime 5 harfliyse 3 tane textbox fazla olacaktı.
            }         
        }
        private void Check(string letter1) // Harf kontrol metodu.
        {
            boolean = true;
          
            for(int i=0; i<digits; i++)
            {
                if(letters[i] == letter1.ToLower())
                {
                    //Doğru
                    textBoxes[i].Text = letter1;
                    boolean = false;
                    point++;
                    if(point == digits)
                    {
                        // Oyun kazanıldı
                        score = digits + point + 2;
                        Scores(nick, score);
                        panel2.Visible = true;
                        label16.Text = "Your score is: " + score;
                        label17.Text = "The word: " + the_word;
                    }
                }
            }
            if(boolean == true)
            {
                //yanlış
                life--;
                label6.Text = life.ToString();
                if (life >= 0)
                {
                    pictureBox2.Image = Image.FromFile(dir + "\\icons\\" + (i+1) + ".png"); // Adamı asmaya biraz daha yaklaştırıyoruz
                    i++;
                    if (label3.Text.Length <= 17) // Yanlış harfleri ekranda gösterdiğim kısım
                    {
                        label3.Text = label3.Text + letter1 + "    ";
                    }
                    else if (label4.Text.Length <= 22)
                    {
                        label4.Text = label4.Text + letter1 + "    ";
                    }
                    else
                    {
                        label5.Text = label5.Text + letter1 + "    ";
                    }
                }
                if(life <=0)
                {
                    // Eğer hak biterse oyun biter
                    panel2.Visible = true;
                    score = digits + point + 2;
                    label13.Text = "YOU LOSE THE GAME!";
                    label16.Text = "Your score is: " + score;
                    label17.Text = "The word: " + the_word;
                    Scores(nick, score);
                    button1.Enabled = false;
                    textBox9.Enabled = false;
                }
            }
        }       
        private string Read_File(int a) // Rastgele kelime seçmek için doyayı okuduğumuz metod.
        {
            string path = dir + @"\\word_database.txt"; // Dosyanın yolu
            StreamReader reader;
            reader = File.OpenText(path);

            int border = a;
            string word1;
            int i=0;
            
            while((word1 = reader.ReadLine()) != null)
            {
                if(i == border)
                {
                    return_value = word1.ToString(); // Seçilen kelimeyi geri döndüren kısım
                }
                i++;
            }

            reader.Close();
            return return_value;
        }        
        private void Read_all_files() // Tüm dosyayı okuduğumuz kısım
        {                          // Bu kısım kelime ekleme/çıkarma kısmı için. Ayrıca kaç tane kelime olduğunu da buluyoruz.
            word_count = 0;
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();
            string path = dir + @"\\word_database.txt";
            StreamReader reader;
            reader = File.OpenText(path);

            string word1;
            while ((word1 = reader.ReadLine()) != null)
            {
                word_count++;
                if (word1.ToString() != null) // Kelimelerin uzunluklarına göre gruplayıp farklı listboxlara ekliyoruz.
                {
                    if (word1.Length == 5)
                    {
                        listBox1.Items.Add(word1);
                    }
                    else if (word1.Length == 6)
                    {
                        listBox2.Items.Add(word1);
                    }
                    else if (word1.Length == 7)
                    {
                        listBox3.Items.Add(word1);
                    }
                    else if (word1.Length == 8)
                    {
                        listBox4.Items.Add(word1);
                    }
                }
            }
            reader.Close();
        }
        private void Write_to_file(string word) // Kelime veritabanına kelime ekleme kısmı
        {
            StreamWriter SW = File.AppendText(dir + "\\word_database.txt");
            SW.WriteLine(word);
            SW.Close();
        }
        private void Delete_from_file(string word1) // Kelime veritabanından kelime silme kısmı
        {
            StringBuilder newFile = new StringBuilder();
            string temp = "";
            string[] file = File.ReadAllLines(dir + "\\word_database.txt");
            foreach (string line in file)
            {
                if (line.Contains(word1))
                {
                    temp = line.Replace(
            word1,null);
                    newFile.Append(temp +
            "\r\n");
                    continue;
                }
                newFile.Append(line +
            "\r\n");
            }
            File.WriteAllText(dir + "\\word_database.txt", newFile.ToString());
        }
        private void Scores(string nick1,int score1) // Skorları kaydettiğimiz metod
        {
            string file_path = dir + "\\scores.txt";
            if (File.Exists(file_path)) // Eğer scores.txt dosyası zaten var ise dosyaya kullanıcı adını ve skoru ekliyor.
            {
                StreamWriter SW = File.AppendText(file_path);
                SW.WriteLine(nick1 + "'s score: " + score1);
                SW.Close();
            }
            else // Eğer yok ise yeni bir dosya yaratıp aynı işlemleri yapıyoruz.
            {
                FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(nick1 + "'s score: " + score1);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
        }       
        private void button1_Click(object sender, EventArgs e) // textbox a girdiğimiz harfi kontrol ettirme metoduna gönderiyoruz.           
        {                                                    // Ve diğer bazı önemli işlemleri yapıyoruz.
            letter = textBox9.Text;
            old_letters[l] = letter.ToUpper(); // Girdiğimiz harfleri birdaha giremeyelim diye harfi eski old_letters adlı diziye ekliyoruz.
            l++;
            Check(letter);
            textBox9.Clear();
        }
        private void textBox9_TextChanged_1(object sender, EventArgs e)
        {
            textBox9.CharacterCasing = CharacterCasing.Upper;
            if (textBox9.Text == "i" || textBox9.Text == "İ") textBox9.Text = "İ";
            else if (textBox9.Text == "ı") textBox9.Text = "I";
                       
            if (textBox9.Text == "")
            {
                button1.Enabled = false;
            }
            else
            {
                button1.Enabled = true;
            }
            for (int k = 0; k < digits+2; k++)
            {
                if (textBox9.Text == old_letters[k]) // Eğer girdiğimiz harfi tekrar girmeye çalışırsak, program engel oluyor.
                {
                    textBox9.Clear();
                }
            }           
        }
      
        // Kelime ekleme/silme kısmı
        int listbox1_length;
        int listbox2_length;
        int listbox3_length;
        int listbox4_length;
        private void button2_Click_1(object sender, EventArgs e)
        {
            panel1.Visible = true;
            listbox1_length = listBox1.Items.Count;
            listbox2_length = listBox2.Items.Count;
            listbox3_length = listBox3.Items.Count;
            listbox4_length = listBox4.Items.Count;
        }
        private void textBox10_TextChanged(object sender, EventArgs e)
        {
            int text_lenght = int.Parse(textBox10.TextLength.ToString());

            // sadece 4 ila 9 harf arasında kelime eklemek için yazdığımız sorgu
            if(text_lenght > 4 & text_lenght < 9)
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }
        // Kelime listesine kelime ekleme kısmı
        private void button3_Click(object sender, EventArgs e)
        {
            Write_to_file(textBox10.Text);
            textBox10.Clear();
            Read_all_files();
        }

        // Kelime listesinden kelime silme kısmı
        private void button5_Click(object sender, EventArgs e)
        {          
            if(listbox_click == "1")
            {
                Delete_from_file(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem);
                listBox1.ClearSelected();
            }
            else if(listbox_click == "2")
            {
                Delete_from_file(listBox2.SelectedItem.ToString());
                listBox2.Items.Remove(listBox2.SelectedItem);
                listBox2.ClearSelected();
            }
            else if (listbox_click == "3")
            {
                Delete_from_file(listBox3.SelectedItem.ToString());
                listBox3.Items.Remove(listBox3.SelectedItem);
                listBox3.ClearSelected();
            }
            else if (listbox_click == "4")
            {
                Delete_from_file(listBox4.SelectedItem.ToString());
                listBox4.Items.Remove(listBox4.SelectedItem);
                listBox4.ClearSelected();
            }
            button5.Enabled = false;
        }
        string listbox_click = "";
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex <= listbox1_length && listBox1.SelectedItem != null)
            {
                listbox_click = "1";
                button5.Enabled = true;
                listBox2.ClearSelected();
                listBox3.ClearSelected();
                listBox4.ClearSelected();
            }
        }
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex <= listbox2_length && listBox2.SelectedItem != null)
            {
                listbox_click = "2";
                button5.Enabled = true;
                listBox1.ClearSelected();
                listBox3.ClearSelected();
                listBox4.ClearSelected();
            }
        }
        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox3.SelectedIndex <= listbox3_length && listBox3.SelectedItem != null)
            {
                listbox_click = "3";
                button5.Enabled = true;
                listBox2.ClearSelected();
                listBox1.ClearSelected();
                listBox4.ClearSelected();
            }
        }
        private void listBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox4.SelectedIndex <= listbox4_length && listBox4.SelectedItem != null)
            {
                listbox_click = "4";
                button5.Enabled = true;
                listBox2.ClearSelected();
                listBox3.ClearSelected();
                listBox1.ClearSelected();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            if (point == digits || life == 0)
            {
                panel2.Visible = true;
                //panel3.Visible = true;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            panel2.Visible = false;
            panel1.Visible = true;
            listbox1_length = listBox1.Items.Count;
            listbox2_length = listBox2.Items.Count;
            listbox3_length = listBox3.Items.Count;
            listbox4_length = listBox4.Items.Count;
        }

        // Oyun sonundaki Yeniden oyna, Çık butonlarının kodları.
        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        // Giriş kısmı
        private void button9_Click(object sender, EventArgs e)
        {
            nick = textBox11.Text;
            panel3.Visible = false;
            if (nick == "rec.the.engineer") label7.Text = the_word; // Ufak bi admin hilesi koydum
            textBox9.Enabled = true;
            textBox9.Focus();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (textBox11.Text != "") button9.Enabled = true;
            else button9.Enabled = false;
        }

        private void textBox11_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button9_Click(button9, new EventArgs());
            }
        }
    }
}
// Written by Recep Oğuzhan Şenoğlu
// Recep Oğuzhan Şenoğlu tarafından yazılmıştır