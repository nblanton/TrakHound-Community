﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Reflection;
using System.Xml;

using TH_Configuration;

namespace TH_Parts
{
    public enum CalculationType
    {
        Incremental,
        Reset_At_Zero,
    }

    public class PartsConfiguration
    {

        public string PartsEventName { get; set; }

        public string PartsEventValue { get; set; }

        public string PartsCaptureItemLink { get; set; }

        public CalculationType CalculationType { get; set; }

        public static PartsConfiguration Read(XmlDocument xml)
        {
            var result = new PartsConfiguration();

            XmlNodeList nodes = xml.SelectNodes("/Settings/Parts");

            if (nodes != null)
            {
                if (nodes.Count > 0)
                {
                    XmlNode node = nodes[0];

                    foreach (XmlNode child in node.ChildNodes)
                    {
                        if (child.NodeType == XmlNodeType.Element)
                        {
                            if (child.Name.ToLower() == "calculationtype")
                            {
                                switch (child.InnerText.ToLower())
                                {
                                    case "incremental": result.CalculationType = CalculationType.Incremental; break;
                                    case "reset_at_zero": result.CalculationType = CalculationType.Reset_At_Zero; break;
                                }
                            }
                            else
                            {
                                Type Setting = typeof(PartsConfiguration);
                                PropertyInfo info = Setting.GetProperty(child.Name);

                                if (info != null)
                                {
                                    Type t = info.PropertyType;
                                    info.SetValue(result, Convert.ChangeType(child.InnerText, t), null);
                                }
                            } 
                        }
                    }
                }
            }

            return result;
        }

        public static PartsConfiguration Get(Configuration configuration)
        {
            PartsConfiguration result = null;

            var customClass = configuration.CustomClasses.Find(x => x.GetType() == typeof(PartsConfiguration));
            if (customClass != null) result = (PartsConfiguration)customClass;

            return result;
        }

    }
}
