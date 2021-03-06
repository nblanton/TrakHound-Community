﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using MTConnect;
using MTConnect.Application.Components;
using System.Collections.Generic;
using System.Data;

namespace TrakHound.Configurations.AutoGenerate
{
    public static class Oee
    {
        public static void Add(DataTable table, List<DataItem> dataItems)
        {
            DeviceConfiguration.EditTable(table, "/OEE/OperatingEventName", "device_status", null);
            DeviceConfiguration.EditTable(table, "/OEE/OperatingEventValue", "Active", null);

            int i = 0;

            // Add Feedrate Override Values
            var items = dataItems.FindAll(x => (x.Category == DataItemCategory.EVENT && x.Type == "PATH_FEEDRATE_OVERRIDE" && x.SubType == "PROGRAMMED") ||
                (x.Category == DataItemCategory.SAMPLE && x.Type == "PATH_FEEDRATE" && x.Units == "PERCENT"));
            foreach (var item in items)
            {
                string format = "id||{0};";
                string attributes = string.Format(format, i.ToString("00"));

                DeviceConfiguration.EditTable(table, "/OEE/Overrides/Override||" + i.ToString("00"), "Feedrate Override " + (i + 1).ToString(), attributes);
                i++;
            }
        }
    }
}
