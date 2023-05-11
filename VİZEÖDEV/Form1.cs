using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace finalodevasdasdasd
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(temp))
            {
                string jsondata = File.ReadAllText(temp);
                car = JsonSerializer.Deserialize<List<Cars>>(jsondata);
            }

            ShowList();
        }

        

        private List<Cars> car = new List<Cars>
        {
            new Cars()
            {
                Plaka = "29 TR 12",
                Marka = "TUMOSAN",
                Model = "8075",
                Yakit = "MAZOT",
                Renk = "Siyah",
                Vites = "MANUEL",
                KasaTipi = "KISA",
                Aciklama = "TEMİZ",
            },

            new Cars()
            {
                Plaka = "21 ATA 123",
                Marka = "MASSEY",
                Model = "TD400",
                Yakit = "Benzin",
                Renk = "Gri",
                Vites = "Manuel",
                KasaTipi = "KISA",
                Aciklama = "TEMİZ",
            }
        };


        public void ShowList()
        {
            listView1.Items.Clear();
            foreach (Cars cars in car)
            {
                AddCarsToListView(cars);

            }

        }

        public void AddCarsToListView(Cars cars) 
        {
            ListViewItem item = new ListViewItem(new String[]
                {
                                    cars.Plaka,
                                    cars.Marka,
                                    cars.Model,
                                    cars.Yakit,
                                    cars.Renk,
                                    cars.Vites,
                                    cars.KasaTipi,
                                    cars.Aciklama,
          
                });
            item.Tag = cars;
            listView1.Items.Add(item);
        }

        void EditCarsOnListView(ListViewItem cItem, Cars cars) 
        {
            cItem.SubItems[0].Text = cars.Plaka;
            cItem.SubItems[1].Text = cars.Marka;
            cItem.SubItems[2].Text = cars.Model;
            cItem.SubItems[3].Text = cars.Yakit;
            cItem.SubItems[4].Text = cars.Renk;
            cItem.SubItems[5].Text = cars.Vites;
            cItem.SubItems[6].Text = cars.KasaTipi;
            cItem.SubItems[7].Text = cars.Aciklama;

            cItem.Tag = cars;

        }



        private void AddCar(object sender, EventArgs e)
        {
            FrmCars frm = new FrmCars()
            {
                Text = "Araba Ekle",
                StartPosition = FormStartPosition.CenterParent,
                car = new Cars()
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {

                car.Add(frm.car);
                AddCarsToListView(frm.car);

            }
        }

        private void EditCars(object sender, EventArgs e)
        {

            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem cItem = listView1.SelectedItems[0];

            Cars secili = cItem.Tag as Cars;
            


            FrmCars frm = new FrmCars()

            {
                Text = "Araba Duzenle",
                StartPosition = FormStartPosition.CenterParent,
                car = Clone (secili),
            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.car;
                EditCarsOnListView(cItem, secili);
            }

        }

        private void SaveCars(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formati|*.json|Xml Formati|*.xml",

            };


            if (sf.ShowDialog() == DialogResult.OK) 
            {

                
                if (sf.FileName.EndsWith("json"))
                {
                
                        string data = JsonSerializer.Serialize(car);
                        File.WriteAllText(sf.FileName, data);
                    
                }


                else if (sf.FileName.EndsWith("xml")) 
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
                    serializer.Serialize(sw, car);
                    sw.Close();
                }
            
            }
        }
        private void LoadCars(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json ve Xml Formatlari|*.json;*.xml",

            };

            if (of.ShowDialog() == DialogResult.OK) 
            {
                if (of.FileName.ToLower().EndsWith("json"))
                {
                    string jsondata = File.ReadAllText(of.FileName);
                    car = JsonSerializer.Deserialize<List<Cars>>(jsondata);
                }


                else if (of.FileName.ToLower().EndsWith("xml"))
                {
                    StreamReader sr = new StreamReader(of.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Cars>));
                    car = (List<Cars>)serializer.Deserialize(sr);
                    sr.Close();
                }

                ShowList();

            }
        }

        string temp = Path.Combine(Application.CommonAppDataPath, "data");

        protected override void OnClosing(CancelEventArgs e)
        {
            string data = JsonSerializer.Serialize(car);
            File.WriteAllText(temp, data);

            base.OnClosing(e);
        }



        Cars Clone(Cars cars) 
        {
            return new Cars()
            {
                id = cars.ID,
                Plaka = cars.Plaka,
                Marka = cars.Marka,
                Model = cars.Model,
                Yakit = cars.Yakit,
                Renk = cars.Renk,
                Vites = cars.Vites,
                KasaTipi = cars.KasaTipi,
                Aciklama = cars.Aciklama,

            };
        
        }

        [Serializable]
        public class Cars
        {
            public string id;

            [Browsable(false)]

            public string ID
            {
                get
                {
                    if (id == null)
                        id = Guid.NewGuid().ToString();
                    return id;
                }

                set { id = value; }
            }
            [Category("Bilgiler"), DisplayName("Plaka")]
            public string Plaka { get; set; }
            [Category("Bilgiler"), DisplayName("Marka")]
            public string Marka { get; set; }
            [Category("Bilgiler"), DisplayName("Model")]
            public string Model { get; set; }
            [Category("Diger"), DisplayName("Yakit")]
            public string Yakit { get; set; }
            [Category("Diger"), DisplayName("Renk")]
            public string Renk { get; set; }
            [Category("Diger"), DisplayName("Vites")]
            public string Vites { get; set; }
            [Category("Diger"), DisplayName("Kasa Tipi")]
            public string KasaTipi { get; set; }
            [Category("Diger"), DisplayName("Aciklama")]
            public string Aciklama { get; set; }

        }

        private void DeleteCars(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem cItem = listView1.SelectedItems[0];

            Cars secili = cItem.Tag as Cars;

            var sonuc = MessageBox.Show($"Secili Araba Silinsin Mi?\n\n{secili.Plaka} {secili.Marka}", 
                "Silmeyi Onayla",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes) 
            {
                car.Remove(secili);
                listView1.Items.Remove(cItem);
            }
        }

        private void hakkindaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }
}
