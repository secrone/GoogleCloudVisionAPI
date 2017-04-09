using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Secron2
{
    public partial class Form1 : Form
    {

       
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread th1 = new Thread(new ThreadStart(getResults));
            th1.Start();

        }

        private void getResults()
        {
            
                var ext = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".png", ".jpg", ".gif" };
                var dir = @"C:\Users\Toshiba\Desktop\Melike_files";
                var files =
                    Directory
                        .GetFiles(dir, "*.*", SearchOption.AllDirectories)
                        .Where(f => ext.Contains(Path.GetExtension(f)))
                        .Select(f => new FileInfo(f))
                        .ToArray();

                //create service
                var credentails = Program.CreateCredentials("C:\\Users\\Toshiba\\Desktop\\user_credentials.json");
                var service = Program.CreateService("Secrone2", credentails);

                //file da ki dosyaları okut
                foreach (var file in files)
                {
                    string f = file.FullName;
                    Console.WriteLine("Reading " + f + ":");

                    var task = service.AnnotateAsync(file, "LABEL_DETECTION");
                    var result = task.Result;

                    var keywords = result.LabelAnnotations.Select(s => s.Description).ToArray();

                    foreach(string s in keywords){
                        if (s == "hair")
                            MessageBox.Show("Çıktı");
                            
                    }
                    var words = String.Join(", ", keywords);
                    Console.WriteLine(words);

                    label1.Text = "1.resim için: " + words;


                   

                /*    f += ".keywords.txt";
                    File.WriteAllText(f, words);*/
                }
            
        }

    }
}
