﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.Composition;
using System.Windows.Media;

using TH_Device_Client;

namespace TH_PlugIns_Client_Control
{

    /// <summary>
    /// This is the interface for writing Control PlugIns for TrakHound-Client. 
    /// All PlugIns MUST contain the following properties and methods.
    /// </summary>
    [InheritedExport(typeof(Control_PlugIn))]
    public interface Control_PlugIn
    {

        #region "Descriptive"

        /// <summary>
        /// Sets the Title of the Plugin
        /// (ex. TH_Dashboard's Title = "Dashboard")
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Used to describe the functions of the Plugin
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Image associated with the Plugin
        /// </summary>
        ImageSource Image { get; }

        /// <summary>
        /// Author's name
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Any other text associated with the author
        /// (ex. copyright statement)
        /// </summary>
        string AuthorText { get; }

        /// <summary>
        /// Image associated with the author
        /// </summary>
        ImageSource AuthorImage { get; }

        /// <summary>
        /// Name of the software license for the plugin
        /// (ex. GPLv2)
        /// </summary>
        string LicenseName { get; }

        /// <summary>
        /// Actual text of the software license for the plugin
        /// </summary>
        string LicenseText { get; }

        #endregion

        #region "Update Information"

        /// <summary>
        /// Url pointing to the 'appinfo' file to use for Automatic Updates
        /// </summary>
        string UpdateFileURL { get; }

        #endregion

        #region "Plugin Properties/Options"

        /// <summary>
        /// Used to set the default parent plugin's name 
        /// (ex. if plugin is desiged for Dashboard, DefaultParent should = "Dashboard")
        /// </summary>
        string DefaultParent { get; }

        /// <summary>
        /// Used to set the default parent plugin's category
        /// (ex. if plugin is designed for Dashboard's Pages category, DefaultParentCategory should = "Pages")
        /// </summary>
        string DefaultParentCategory { get; }

        /// <summary>
        /// Used to turn on/off ability for the plugin to accept "child" plugins
        /// </summary>
        bool AcceptsPlugIns { get; }

        /// <summary>
        /// Used to force the plugin to open up when intially loaded/enabled
        /// </summary>
        bool OpenOnStartUp { get; }

        /// <summary>
        /// Used to toggle whether to show in App Launcher menu
        /// </summary>
        bool ShowInAppMenu { get; }

        /// <summary>
        /// Contains the Subcategories for this plugin's "child" plugins
        /// (ex. Dashboard has "Pages" as a subcategory)
        /// </summary>
        List<PlugInConfigurationCategory> SubCategories { get; set; }

        /// <summary>
        /// Contains the Plugin's "child" plugins
        /// </summary>
        List<Control_PlugIn> PlugIns { get; set; }

        #endregion

        #region "Methods"

        /// <summary>
        /// Used to initialize the Plugin after the other properties have been set
        /// (ex. called after DeviceData has been set)
        /// </summary>
        void Initialize();

        /// <summary>
        /// Updated information sent so that the UI can be updated
        /// </summary>
        /// <param name="rd">ReturnData object with updated information</param>
        void Update( ReturnData rd);

        /// <summary>
        /// Called when parent Window or plugin is closing so that the plugin can 
        /// close any connections or dispose of anything it needs to
        /// </summary>
        void Closing();

        #endregion

        #region "Data Events"

        /// <summary>
        /// Get Data from another plugin
        /// </summary>
        /// <param name="de_data"></param>
        void Update_DataEvent(DataEvent_Data de_d);

        /// <summary>
        /// Send data to other plugins
        /// </summary>
        event DataEvent_Handler DataEvent;

        /// <summary>
        /// Send Request to Add/Show this Page in the Client
        /// </summary>
        event PlugInTools.ShowRequested_Handler ShowRequested;

        #endregion

        #region "Device Properties"

        /// <summary>
        /// List of Device_Client objects
        /// </summary>
        List<Device_Client> Devices { get; set; }

        #endregion

        #region "Options"

        /// <summary>
        /// Sets the OptionsPage object to be displayed in the Clients Options menu as a seperate page
        /// (use if Plugin has parameters or options for how it operates or looks)
        /// </summary>
        OptionsPage Options { get; set; }

        #endregion

    }

    public delegate void DataEvent_Handler(DataEvent_Data de_d);

    public class DataEvent_Data
    {

        public string id { get; set; }

        public object data { get; set; }

    }

    public class PluginCategory
    {

        public string Name { get; set; }

        public List<Control_PlugIn> PlugIns { get; set; }

    }

    public class PluginShowInfo
    {
        public string PageTitle { get; set; }
        public ImageSource PageImage { get; set; }
        public object Page { get; set; }
    }

    public static class PlugInTools
    {

        public delegate void SelectedDeviceChanged_Handler(int Index);
        public delegate void ShowRequested_Handler(PluginShowInfo info);

    }


}
