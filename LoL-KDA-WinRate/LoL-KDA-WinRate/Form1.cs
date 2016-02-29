using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CefSharp;
using CefSharp.WinForms;
using System.IO;

namespace LoL_KDA_WinRate
{
    public partial class Form1 : Form
    {
        public ChromiumWebBrowser browser;
        private string jsCode = "";
        public Form1()
        {
            InitializeComponent();
            jsCode = File.ReadAllText(Application.StartupPath + @"\jsCode.js");
            InitBrowser();
        }

        public void InitBrowser()
        {
            Cef.Initialize(new CefSettings());
            browser = new ChromiumWebBrowser("www.google.com");

            Panel p = new Panel();
            p.Size = new Size(0, 0);
            p.Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(p);
            browser.LoadingStateChanged += OnLoadingStateChanged;
        }

        //A very basic example
        private void OnLoadingStateChanged(object sender, LoadingStateChangedEventArgs args)
        {
            //Wait for the Page to finish loading
            if (args.IsLoading == false)
            {
                browser.EvaluateScriptAsync(jsCode).ContinueWith(x =>
                {
                    var response = x.Result;

                    if (response.Success && response.Result != null)
                    {
                        string result = (string)response.Result;
                        Invoke(new MethodInvoker(() => { Result_Loaded(result); }));
                    }
                });
            }
        }

        void Result_Loaded(string result)
        {
            string[] images = result.Split(';');
            PictureBox[] pic = { pictureBox1, pictureBox2, pictureBox3 };
            for (int i = 0; i < images.Length; i++)
            {
                pic[i].LoadAsync(images[i]);
            }
        }
    }
}
