﻿// Copyright (c) 2016 Feenux LLC, All Rights Reserved.

// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

using TrakHound.API;
using TrakHound.Logging;
using TrakHound.Tools.Web;

namespace TrakHound.Servers.DataStorage
{
    public class LocalStorageServer
    {
        /// <summary>
        /// API Server Port
        /// </summary>
        public const int PORT = 8472; // ASCII Dec for 'T' and 'H'

        private HttpListener listener;

        private System.Timers.Timer backupTimer;

        private ManualResetEvent stop;

        private const int BACKUP_INTERVAL = 300000; // 5 Minutes
        private const int LISTENER_ERROR_RETRY_INTERVAL = 5000; // 5 Seconds


        public LocalStorageServer()
        {
            var apiMonitor = new ApiConfiguration.Monitor();
            apiMonitor.ApiConfigurationChanged += ApiMonitor_ApiConfigurationChanged;
        }

        private void ApiMonitor_ApiConfigurationChanged(ApiConfiguration config)
        {
            if (config != null && config.DataHost != ApiConfiguration.LOCAL_API_HOST) Stop();
            else Start();
        }

        public void Start()
        {
            Stop();

            stop = new ManualResetEvent(false);

            StartBackupTimer();

            ThreadPool.QueueUserWorkItem(new WaitCallback(Worker));
        }

        public void Worker(object o)
        {
            try
            {
                listener = new HttpListener();
                listener.Prefixes.Add("http://localhost:" + PORT + "/api/");
                listener.Start();

                ThreadPool.QueueUserWorkItem((x) =>
                {
                    Console.WriteLine("TrakHound Data Server Started...");

                    while (listener.IsListening && !stop.WaitOne(10, true))
                    {
                        try
                        {
                            ThreadPool.QueueUserWorkItem((c) =>
                            {
                                var ctx = c as HttpListenerContext;

                                try
                                {
                                    string rstr = ProcessRequest(ctx);
                                    if (!string.IsNullOrEmpty(rstr))
                                    {
                                        byte[] buf = Encoding.UTF8.GetBytes(rstr);
                                        ctx.Response.ContentLength64 = buf.Length;
                                        ctx.Response.OutputStream.Write(buf, 0, buf.Length);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //Logger.Log("Local Data Server :: Exception :: " + ex.Message, LogLineType.Warning);
                                }
                                finally
                                {
                                    // always close the stream
                                    ctx.Response.OutputStream.Close();
                                }
                            }, listener.GetContext());
                        }
                        catch (ObjectDisposedException ex)
                        {
                            Logger.Log("Local Data Server :: ObjectDisposedException :: " + ex.Message, LogLineType.Warning);
                        }
                        catch (HttpListenerException ex)
                        {
                            if (ex.ErrorCode != 995)
                            {
                                Logger.Log("Local Data Server :: HttpListenerException :: " + ex.ErrorCode + " :: " + ex.Message, LogLineType.Warning);
                            }
                        }
                        catch (InvalidOperationException ex)
                        {
                            Logger.Log("Local Data Server :: InvalidOperationException :: " + ex.Message, LogLineType.Warning);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Local Data Server :: Exception :: " + ex.Message, LogLineType.Warning);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Log("Local Data Server :: Error starting server :: Exception :: " + ex.Message, LogLineType.Warning);
                Logger.Log("Local Data Server :: Error starting server :: Restarting Data Server in 5 Seconds..");

                if (!stop.WaitOne(LISTENER_ERROR_RETRY_INTERVAL, true)) Worker(null);
            }
        }

        public void Stop()
        {
            if (stop != null) stop.Set();

            if (listener != null)
            {
                try
                {
                    listener.Stop();
                    listener.Close();
                }
                catch (Exception ex) { }
            }

            StopBackupTimer();

            Logger.Log("Local Data Server Stopped", LogLineType.Console);
        }

        public bool IsStopped
        {
            get
            {
                return !listener.IsListening && backupTimer == null;
            }
        }


        private string ProcessRequest(HttpListenerContext context)
        {
            string result = null;

            try
            {
                string path = context.Request.Url.AbsolutePath;
                if (!string.IsNullOrEmpty(path) && path.Length > 1)
                {
                    // Remove beginning forward slash
                    path = path.Substring(1);

                    path = path.Substring("api/".Length);

                    // Split path by forward slashes
                    var paths = path.Split('/');
                    if (paths.Length > 1 && paths[0] != "/" && !string.IsNullOrEmpty(paths[0]))
                    {
                        switch (paths[0].ToLower())
                        {
                            case "data":

                                // Remove 'data' from path
                                path = path.Substring(paths[0].Length);

                                // Remove first forward slash
                                path = path.TrimStart('/');

                                // Process the Data Request and return response
                                if (context.Request.HttpMethod == "GET")
                                {
                                    paths = path.Split('/');
                                    if (paths.Length > 0)
                                    {
                                        switch (paths[0].ToLower())
                                        {
                                            case "get": result = Data.Get(context.Request.Url.ToString()); break;
                                        }
                                    }
                                }
                                else if (context.Request.HttpMethod == "POST")
                                {
                                    paths = path.Split('/');
                                    if (paths.Length > 0)
                                    {
                                        switch (paths[0].ToLower())
                                        {
                                            case "update": result = Data.Update(context); break;
                                        }
                                    }
                                }
                                else result = "Incorrect REQUEST METHOD";

                                break;

                            case "devices":

                                // Remove 'data' from path
                                path = path.Substring(paths[0].Length);

                                // Remove first forward slash
                                path = path.TrimStart('/');

                                if (context.Request.HttpMethod == "GET")
                                {
                                    paths = path.Split('/');
                                    if (paths.Length > 0)
                                    {
                                        switch (paths[0].ToLower())
                                        {
                                            case "list": result = Data.List(context.Request.Url.ToString()); break;
                                        }
                                    }
                                }

                                break;

                            case "config":

                                var apiConfig = ApiConfiguration.Read();
                                if (apiConfig != null)
                                {
                                    string json = JSON.FromObject(apiConfig);
                                    if (json != null)
                                    {
                                        result = json;
                                    }
                                    else
                                    {
                                        result = "Error Reading API Configuration";
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            catch (Exception ex) { Logger.Log("Error Processing Local Server Request :: " + ex.Message, LogLineType.Error); }

            return result;
        }


        private void StartBackupTimer()
        {
            try
            {
                if (backupTimer != null) backupTimer.Stop();
                else
                {
                    backupTimer = new System.Timers.Timer();
                    backupTimer.Interval = 10000;
                    backupTimer.Elapsed += BackupTimer_Elapsed;
                }

                backupTimer.Start();
            }
            catch (Exception ex) { Logger.Log("Error Starting Local Server Backup Timer :: " + ex.Message, LogLineType.Error); }
        }

        private void BackupTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = (System.Timers.Timer)sender;
            timer.Interval = BACKUP_INTERVAL;

            Backup.Create(Data.DeviceInfos.ToList());
        }

        private void StopBackupTimer()
        {
            if (backupTimer != null)
            {
                backupTimer.Stop();
                backupTimer = null;
            }
        }


        private static class Data
        {
            private static bool first = true;

            private static object _lock = new object();

            public static List<API.Data.DeviceInfo> DeviceInfos = new List<API.Data.DeviceInfo>();

            public static string Get(string url)
            {
                string response = null;

                try
                {
                    lock(_lock)
                    {
                        var uri = new Uri(url);

                        // Get From Parameter
                        string sFrom = HttpUtility.ParseQueryString(uri.Query).Get("from");
                        string sTo = HttpUtility.ParseQueryString(uri.Query).Get("to");

                        DateTime from = DateTime.MinValue;
                        DateTime to = DateTime.MinValue;

                        DateTime.TryParse(sFrom, out from);
                        DateTime.TryParse(sTo, out to);

                        var _deviceInfos = new List<API.Data.DeviceInfo>();

                        if (from > DateTime.MinValue && to > DateTime.MinValue)
                        {
                            foreach (var info in DeviceInfos.ToList())
                            {
                                var deviceInfo = info.Copy();

                                from = from.ToUniversalTime();
                                to = to.ToUniversalTime();
                                
                                if (deviceInfo.Hours != null)
                                {
                                    var _hours = deviceInfo.Hours.FindAll(o => TestHourDate(o, from, to));
                                    deviceInfo.Hours = _hours;
                                    deviceInfo.Oee = API.Data.HourInfo.GetOeeInfo(_hours);
                                    deviceInfo.Timers = API.Data.HourInfo.GetTimersInfo(_hours);
                                }

                                _deviceInfos.Add(deviceInfo);
                            }
                        }
                        else _deviceInfos = DeviceInfos.ToList();


                        // Get Devices Parameter
                        string json = HttpUtility.ParseQueryString(uri.Query).Get("devices");
                        if (!string.IsNullOrEmpty(json))
                        {
                            var devices = JSON.ToType<List<API.Data.DeviceListItem>>(json);
                            if (devices != null)
                            {
                                var deviceInfos = new List<API.Data.DeviceInfo>();

                                foreach (var device in devices)
                                {
                                    var deviceInfo = _deviceInfos.Find(o => o.UniqueId == device.UniqueId);
                                    if (deviceInfo != null)
                                    {
                                        deviceInfos.Add(deviceInfo);
                                    }
                                }

                                if (deviceInfos.Count > 0) response = API.Data.DeviceInfo.ListToJson(deviceInfos);
                            }
                        }
                        else
                        {
                            response = API.Data.DeviceInfo.ListToJson(_deviceInfos);
                        }
                    }
                }
                catch (Exception ex) { Logger.Log("Error Getting Local Server Data :: " + ex.Message, LogLineType.Error); }

                return response;
            }

            public static string Update(HttpListenerContext context)
            {
                string response = null;

                try
                {
                    lock (_lock)
                    {
                        using (var reader = new StreamReader(context.Request.InputStream))
                        {
                            var body = reader.ReadToEnd();

                            string json = HTTP.GetPostValue(body, "devices");
                            if (!string.IsNullOrEmpty(json))
                            {
                                var devices = JSON.ToType<List<API.Data.DeviceInfo>>(json);
                                if (devices != null && devices.Count > 0)
                                {
                                    // Load backup data on first pass
                                    if (first)
                                    {
                                        var backupInfos = Backup.Load(devices.ToArray());
                                        if (backupInfos != null && backupInfos.Count > 0)
                                        {
                                            DeviceInfos.AddRange(backupInfos);
                                        }
                                    }

                                    foreach (var device in devices)
                                    {
                                        int i = DeviceInfos.FindIndex(o => o.UniqueId == device.UniqueId);
                                        if (i < 0)
                                        {
                                            DeviceInfos.Add(device);
                                            i = DeviceInfos.FindIndex(o => o.UniqueId == device.UniqueId);
                                        }

                                        var info = DeviceInfos[i];

                                        object obj = null;

                                        obj = device.GetClass("status");
                                        if (obj != null) { info.AddClass("status", obj); }

                                        obj = device.GetClass("controller");
                                        if (obj != null) info.AddClass("controller", obj);

                                        // Get HourInfos for current day
                                        List<API.Data.HourInfo> hours = null;
                                        obj = info.GetClass("hours");
                                        if (obj != null)
                                        {
                                            info.RemoveClass("hours");
                                            hours = (List<API.Data.HourInfo>)obj;
                                        }

                                        // Add new HourInfo objects and then combine them into the current list
                                        obj = device.GetClass("hours");
                                        if (obj != null)
                                        {
                                            if (hours == null) hours = new List<API.Data.HourInfo>();

                                            hours.AddRange((List<API.Data.HourInfo>)obj);
                                        }

                                        if (hours != null)
                                        {
                                            // Probably a more elegant way of getting the Time Zone Offset could be done here
                                            int timeZoneOffset = Convert.ToInt32((DateTime.UtcNow - DateTime.Now).TotalHours);

                                            DateTime d = DateTime.Now;
                                            DateTime from = new DateTime(d.Year, d.Month, d.Day, timeZoneOffset, 0, 0);
                                            DateTime to = from.AddDays(1);

                                            hours = hours.FindAll(o => TestHourDate(o, from, to));
                                            hours = API.Data.HourInfo.CombineHours(hours);

                                            // Add Hours
                                            info.AddClass("hours", hours);

                                            // Add OEE
                                            var oee = API.Data.HourInfo.GetOeeInfo(hours);
                                            if (oee != null) info.AddClass("oee", oee);

                                            // Add Timers
                                            var timers = new API.Data.TimersInfo();
                                            timers.Total = hours.Select(o => o.TotalTime).Sum();

                                            timers.Active = hours.Select(o => o.Active).Sum();
                                            timers.Idle = hours.Select(o => o.Idle).Sum();
                                            timers.Alert = hours.Select(o => o.Alert).Sum();

                                            timers.Production = hours.Select(o => o.Production).Sum();
                                            timers.Setup = hours.Select(o => o.Setup).Sum();
                                            timers.Teardown = hours.Select(o => o.Teardown).Sum();
                                            timers.Maintenance = hours.Select(o => o.Maintenance).Sum();
                                            timers.ProcessDevelopment = hours.Select(o => o.ProcessDevelopment).Sum();

                                            info.AddClass("timers", timers);

                                            response = "Devices Updated Successfully";
                                        }
                                    }

                                    first = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { Logger.Log("Error Updating Local Server Data :: " + ex.Message, LogLineType.Error); }

                return response;
            }

            public static string List(string url)
            {
                string response = null;

                try
                {
                    var uri = new Uri(url);

                    // Get Devices Parameter
                    string json = HttpUtility.ParseQueryString(uri.Query).Get("devices");
                    if (!string.IsNullOrEmpty(json))
                    {
                        var devices = JSON.ToType<List<API.Data.DeviceListItem>>(json);
                        if (devices != null)
                        {
                            var deviceInfos = new List<API.Data.DeviceInfo>();

                            foreach (var device in devices)
                            {
                                var deviceInfo = DeviceInfos.Find(o => o.UniqueId == device.UniqueId);
                                if (deviceInfo != null)
                                {
                                    var info = new API.Data.DeviceInfo();
                                    info.UniqueId = deviceInfo.UniqueId;
                                    info.Enabled = deviceInfo.Enabled;
                                    info.Index = deviceInfo.Index;
                                    info.Description = deviceInfo.Description;

                                    deviceInfos.Add(info);
                                }
                            }

                            if (deviceInfos.Count > 0) response = API.Data.DeviceInfo.ListToJson(deviceInfos);
                        }
                    }
                    else
                    {
                        var deviceInfos = new List<API.Data.DeviceInfo>();

                        foreach (var deviceInfo in DeviceInfos)
                        {
                            var info = new API.Data.DeviceInfo();
                            info.UniqueId = deviceInfo.UniqueId;
                            info.Enabled = deviceInfo.Enabled;
                            info.Index = deviceInfo.Index;
                            info.Description = deviceInfo.Description;

                            deviceInfos.Add(info);
                        }

                        if (deviceInfos.Count > 0) response = API.Data.DeviceInfo.ListToJson(deviceInfos);
                    }
                }
                catch (Exception ex) { Logger.Log("Error Getting Local Server Devices :: " + ex.Message, LogLineType.Error); }

                return response;
            }

            private static bool TestHourDate(API.Data.HourInfo hourInfo, DateTime from, DateTime to)
            {
                DateTime hourTime = hourInfo.GetDateTime();
                return hourTime >= from && hourTime <= to;
            }
        }

    }
}
