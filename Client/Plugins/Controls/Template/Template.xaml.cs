﻿using System;
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

using System.IO;

using TH_Configuration;
using TH_Device_Client;
using TH_PlugIns_Client_Control;

namespace Template
{
    /// <summary>
    /// Interaction logic for Template.xaml
    /// </summary>
    public partial class Template : UserControl, Control_PlugIn
    {
        public Template()
        {
            InitializeComponent();
        }

        #region "PlugIn"

        #region "Descriptive"

        /// <summary>
        /// Plugin Title
        /// </summary>
        public string Title { get { return "Plugin Title"; } }

        /// <summary>
        /// Short Description of the Plugin
        /// </summary>
        public string Description { get { return "Plugin Description"; } }

        /// <summary>
        /// Icon image used for identification
        /// </summary>
        public ImageSource Image { get { return null; } }

        /// <summary>
        /// Name of the Plugin's author
        /// </summary>
        public string Author { get { return null; } }

        /// <summary>
        /// Any additional author information (ex. Copyright Statement)
        /// </summary>
        public string AuthorText { get { return null; } }

        /// <summary>
        /// Icon image associated with the author (ex. Company Logo)
        /// </summary>
        public ImageSource AuthorImage { get { return null; } }

        /// <summary>
        /// Name of the Software License used for the Plugin
        /// </summary>
        public string LicenseName { get { return null; } }

        /// <summary>
        /// Full text of the software license used for the Plugin
        /// </summary>
        public string LicenseText { get { return null; } }

        #endregion

        #region "Plugin Properties/Options"

        /// <summary>
        /// Name of the "preferred" plugin that this plugin is supposed to be a child too (ex. Dashboard)
        /// </summary>
        public string DefaultParent { get { return null; } }

        /// <summary>
        /// Name of the Category inside the parent plugin (ex. Pages)
        /// </summary>
        public string DefaultParentCategory { get { return null; } }

        /// <summary>
        /// Used to turn on/off whether or not this plugin accepts child plugins
        /// </summary>
        public bool AcceptsPlugIns { get { return true; } }

        /// <summary>
        /// Used to determine whether this plugin's page is created immediately upon being enabled
        /// </summary>
        public bool OpenOnStartUp { get { return true; } }

        /// <summary>
        /// List of SubCatogories for this plugin to use for child plugins (ex. Pages for Dashboard)
        /// </summary>
        public List<PlugInConfigurationCategory> SubCategories { get; set; }

        /// <summary>
        /// List of child plugins
        /// </summary>
        public List<Control_PlugIn> PlugIns { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Method called whenever the plugin is first loaded or enabled
        /// </summary>
        public void Initialize() { Console.WriteLine(Title + " : Initialize()"); }

        /// <summary>
        /// Method called every 5 seconds (can be changed under General Options page)
        /// </summary>
        /// <param name="rd"></param>
        public void Update(ReturnData rd) { Console.WriteLine(Title + " : Update()"); }

        /// <summary>
        /// Method called while TrakHound Client is closing
        /// </summary>
        public void Closing() { Console.WriteLine(Title + " : Closing()"); }

        #endregion

        #region "Events"

        public void Update_DataEvent(DataEvent_Data de_d)
        {

        }

        public event DataEvent_Handler DataEvent;

        #endregion

        #region "Device Properties"

        /// <summary>
        /// List of Device_Client objects for each Device
        /// </summary>
        public List<Device_Client> Devices { get; set; }

        #endregion

        #region "Options"

        /// <summary>
        /// OptionsPage UserControl object for any options associated with this plugin
        /// </summary>
        public OptionsPage Options { get; set; }

        #endregion

        #endregion

    }
}