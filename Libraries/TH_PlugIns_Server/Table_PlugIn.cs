﻿// Copyright (c) 2015 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System.ComponentModel.Composition;

using TH_Configuration;
using TH_MTC_Data;

namespace TH_PlugIns_Server
{

    [InheritedExport(typeof(Table_PlugIn))]
    public interface Table_PlugIn
    {

        /// <summary>
        /// Name of the Plugin
        /// (ex. TH_InstanceTable's name is "TH_InstanceTable")
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Sets the priority level so that other plugins can always run before or after this one
        /// (ex. TH_GeneratedData relies on TH_IntstanceTable's data so 
        /// TH_GeneratedData's priority would be a bigger number than TH_InstanceTable's)
        /// </summary>
        //int Priority { get; }

        /// <summary>
        /// Called to initialize the plugin using the Configuration file passed as a parameter
        /// </summary>
        /// <param name="Config"></param>
        void Initialize(Configuration Config);

        /// <summary>
        /// Called when the device is closing
        /// </summary>
        void Closing();

        /// <summary>
        /// Called when a Probe is received
        /// </summary>
        /// <param name="returnData"></param>
        void Update_Probe(TH_MTC_Data.Components.ReturnData returnData);

        /// <summary>
        /// Called when a Current is received
        /// </summary>
        /// <param name="returnData"></param>
        void Update_Current(TH_MTC_Data.Streams.ReturnData returnData);

        /// <summary>
        /// Called when a Sample is received
        /// </summary>
        /// <param name="returnData"></param>
        void Update_Sample(TH_MTC_Data.Streams.ReturnData returnData);

        /// <summary>
        /// Get Data from another plugin
        /// </summary>
        /// <param name="de_data"></param>
        void Update_DataEvent(DataEvent_Data de_data);

        /// <summary>
        /// Send data to other plugins
        /// </summary>
        event DataEvent_Handler DataEvent;

    }

    public delegate void DataEvent_Handler(DataEvent_Data de_data);

    public class DataEvent_Data
    {

        public string id { get; set; }

        public object data { get; set; }

    }


}