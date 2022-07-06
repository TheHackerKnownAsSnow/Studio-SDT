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
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows;
using System.Management;


namespace StudioSDT
{




    public partial class Form1 : Form
    {
        public double thisZoom = 0.0;
        public ChromiumWebBrowser chromeBrowser;

        public System.Diagnostics.Process procs = new System.Diagnostics.Process();
        public DateTime time = DateTime.Now;
        public DateTime time2;

        public static uint ColorToUInt(Color color)
        {
            return (uint)((color.A << 24) | (color.R << 16) | (color.G << 8) | (color.B << 0));
        }

       
        public Form1()
        {
            string dir = Application.ExecutablePath.ToString();

            dir = dir.Substring(0, dir.Length - 13);

            string http = dir + "server\\http-server\\node_modules\\.bin\\http-server.cmd ";
            string server = dir + "server";
            procs.StartInfo.FileName = http;
            procs.StartInfo.Arguments = server;
            procs.Start();
            
            
            

            InitializeComponent();
            CefSettings settings = new CefSettings();
            //settings.CefCommandLineArgs.Add("disable-gpu");
            settings.CefCommandLineArgs.Add("debug-plugin-loading");
            settings.CefCommandLineArgs.Add("always-authorize-plugins");
            settings.CefCommandLineArgs.Add("allow-outdated-plugins");
            settings.CefCommandLineArgs.Add("no-sandbox-and-elevated");
            //settings.CefCommandLineArgs.Add("ppapi-startup-dialog");

            settings.CefCommandLineArgs.Add("allow-universal-access-from-files");
            settings.CefCommandLineArgs.Add("dom-automation");
            settings.CefCommandLineArgs.Add("ppapi-flash-args");

            settings.CefCommandLineArgs.Add("allow-file-access-from-files");
            //settings.CefCommandLineArgs.Add("ppapi-in-process");

            settings.CefCommandLineArgs.Add("enable-pepper-testing");

            settings.CefCommandLineArgs.Add("enable-gpu-plugin");
            settings.CefCommandLineArgs.Add("enable-accelerated-plugins");

            settings.CefCommandLineArgs.Add("user-data-dir");

            //settings.CefCommandLineArgs.Add("enable-npapi", "1");
            try
            {
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\blob_storage", true);
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\Cache", true);
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\Code Cache", true);
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\GPUCache", true);
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\Local Storage", true);
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\Session Storage", true);
               Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest" + "\\Pepper Data", true);
            }
            catch { }

            settings.CachePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\WebTest";

            settings.CefCommandLineArgs.Add("allow-file-acccess-from-files");

            if (settings.CefCommandLineArgs.ContainsKey("enable-system-flash"))
                settings.CefCommandLineArgs.Remove("enable-system-flash");

            settings.CefCommandLineArgs.Add("enable-system-flash", "1");
            settings.CefCommandLineArgs.Add("disable-gpu-vsync", "0");
            //settings.CefCommandLineArgs.Add("ppapi-flash-path", @"C:/libpepflashplayer.so");
            //settings.CefCommandLineArgs.Add("ppapi-flash-version", "31.0.0.153.dll");

            settings.CefCommandLineArgs["plugin-policy"] = "allow";

            Cef.Initialize(settings, performDependencyCheck: false, browserProcessHandler: null);

            InitializeChromium();
        }

        public void Form1_Load(object sender, EventArgs e)
        {

           time2 = DateTime.Now; 

        }
        public bool canclose = false;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!canclose)
            {
                e.Cancel = true;
                canclose = true;
            }
            else 
            {
                return;
            }

           
            var processes = Process.GetProcessesByName("node");

            if (processes.Length == 0)
            {
                MessageBox.Show("Cannot find process Test");
            }
            else
            {
                foreach (var process in processes)
                {
                    if(process.StartTime >= time && process.StartTime <= time2)
                        process.Kill();
                }
            }
            
            canclose = true;
            this.Close();
            


        }




        private void InitializeChromium()
        {


            var browserSettings = new BrowserSettings();

            browserSettings.WebSecurity = CefState.Disabled;
            browserSettings.FileAccessFromFileUrls = CefState.Enabled;
            browserSettings.UniversalAccessFromFileUrls = CefState.Enabled;
            browserSettings.BackgroundColor = ColorToUInt(System.Drawing.Color.FromArgb(255, 40, 42, 48));
            browserSettings.Javascript = CefState.Enabled;

            chromeBrowser = new ChromiumWebBrowser("http://127.0.0.1:8080/");

            SplitContainer splitContainer = new SplitContainer();
            this.Controls.Add(chromeBrowser);
            //splitContainer.Dock = DockStyle.Fill;
            //splitContainer.Panel2.Controls.Add(chromeBrowser);
            //splitContainer.Orientation = Orientation.Horizontal;
            //splitContainer.Size = new Size(1280, 720);
            chromeBrowser.Dock = DockStyle.Fill;



            chromeBrowser.BackColor = Color.FromArgb(255, 40, 42, 48);

            chromeBrowser.BringToFront();

            chromeBrowser.BrowserSettings = browserSettings;





            chromeBrowser.IsBrowserInitializedChanged += (sender, args) =>
            {
                chromeBrowser.ShowDevTools();
                chromeBrowser.SetZoomLevel(2.0);

            };

        }
        
        

        public void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        
      
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

          
        }
    }
}
