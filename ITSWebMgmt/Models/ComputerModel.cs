﻿using System;
using System.Collections.Generic;
using System.Management;
using ITSWebMgmt.Caches;
using ITSWebMgmt.Helpers;

namespace ITSWebMgmt.Models
{
    public class ComputerModel : WebMgmtModel<ComputerADcache>
    {
        //SCCMcache
        public ManagementObjectCollection RAM { get => SCCMcache.RAM; private set { } }
        public ManagementObjectCollection LogicalDisk { get => SCCMcache.LogicalDisk; private set { } }
        public ManagementObjectCollection BIOS { get => SCCMcache.BIOS; private set { } }
        public ManagementObjectCollection VideoController { get => SCCMcache.VideoController; private set { } }
        public ManagementObjectCollection Processor { get => SCCMcache.Processor; private set { } }
        public ManagementObjectCollection Disk { get => SCCMcache.Disk; private set { } }
        public ManagementObjectCollection Software { get => SCCMcache.Software; private set { } }
        public ManagementObjectCollection Computer { get => SCCMcache.Computer; private set { } }
        public ManagementObjectCollection Antivirus { get => SCCMcache.Antivirus; private set { } }
        public ManagementObjectCollection System { get => SCCMcache.System; private set { } }
        public ManagementObjectCollection Collection { get => SCCMcache.Collection; private set { } }

        //ADcache
        public string ComputerNameAD { get => ADcache.ComputerName; }
        public string Domain { get => ADcache.Domain; }
        public bool ComputerFound { get => ADcache.ComputerFound; }
        public string AdminPasswordExpirationTime { get => ADcache.getProperty("ms-Mcs-AdmPwdExpirationTime"); }
        public string ManagedByAD { get => ADcache.getProperty("managedBy"); set => ADcache.saveProperty("managedBy", value); }
        public string DistinguishedName { get => ADcache.getProperty("distinguishedName"); }
        public DateTime WhenCreated { get => ADcache.getProperty("whenCreated"); }

        //Display
        public string ConfigPC = "Unknown";
        public string ConfigExtra = "False";
        public string ComputerName = "ITS\\AAU115359";
        public string ManagedBy;
        public string Raw;
        public string Result;
        public string PasswordExpireDate { get
            {
                if (AdminPasswordExpirationTime != null)
                {
                    return AdminPasswordExpirationTime;
                }
                else
                {
                    return "LAPS not Enabled";
                }
            }
        }
        public string SSCMInventoryTable;
        public string SCCMCollecionsSoftware;
        public string SCCMInventory;
        public string SCCMComputers;
        public string SCCMCollectionsTable;
        public string SCCMCollections;
        public string SCCMAV;
        public string SCCMLD;
        public string SCCMRAM;
        public string SCCMBIOS;
        public string SCCMVC;
        public string SCCMProcessor;
        public string SCCMDisk;
        public string ErrorCountMessage;
        public string ErrorMessages;
        public string ResultError;
        public bool ShowResultDiv = false;
        public bool ShowResultGetPassword = false;
        public bool ShowMoveComputerOUdiv = false;
        public bool ShowErrorDiv = false;
        public bool UsesOnedrive = false;

        public ComputerModel(string computerName)
        {
            if (computerName != null)
            {
                ADcache = new ComputerADcache(computerName);
                if (ADcache.ComputerFound)
                {
                    SCCMcache = new SCCMcache();
                    SCCMcache.ResourceID = getSCCMResourceIDFromComputerName(ComputerNameAD);
                    ComputerName = ADcache.ComputerName;
                    LoadDataInbackground();
                }
                else
                {
                    ShowResultDiv = false;
                    ShowErrorDiv = true;
                    ResultError = "Not found";
                }
            }
        }

        public string getSCCMResourceIDFromComputerName(string computername)
        {
            string resourceID = "";
            //XXX use ad path to get right object in sccm, also dont get obsolite
            foreach (ManagementObject o in SCCMcache.getResourceIDFromComputerName(computername))
            {
                resourceID = o.Properties["ResourceID"].Value.ToString();
                break;
            }

            return resourceID;
        }

        public List<string> setConfig()
        {
            ManagementObjectCollection Collection = this.Collection;
            ComputerModel ComputerModel = this;
            
            if (SCCM.HasValues(Collection))
            {
                List<string> namesInCollection = new List<string>();
                foreach (ManagementObject o in Collection)
                {
                    //o.Properties["ResourceID"].Value.ToString();
                    var collectionID = o.Properties["CollectionID"].Value.ToString();

                    if (collectionID.Equals("AA100015"))
                    {
                        ComputerModel.ConfigPC = "AAU7 PC";
                    }
                    else if (collectionID.Equals("AA100087"))
                    {
                        ComputerModel.ConfigPC = "AAU8 PC";
                    }
                    else if (collectionID.Equals("AA1000BC"))
                    {
                        ComputerModel.ConfigPC = "AAU10 PC";
                        ComputerModel.ConfigExtra = "True"; // Hardcode AAU10 is bitlocker enabled
                    }
                    else if (collectionID.Equals("AA100027"))
                    {
                        ComputerModel.ConfigPC = "Administrativ7 PC";
                    }
                    else if (collectionID.Equals("AA1001BD"))
                    {
                        ComputerModel.ConfigPC = "Administrativ10 PC";
                        ComputerModel.ConfigExtra = "True"; // Hardcode AAU10 is bitlocker enabled
                    }
                    else if (collectionID.Equals("AA10009C"))
                    {
                        ComputerModel.ConfigPC = "Imported";
                    }

                    if (collectionID.Equals("AA1000B8"))
                    {
                        ComputerModel.ConfigExtra = "True";
                    }

                    var pathString = "\\\\srv-cm12-p01.srv.aau.dk\\ROOT\\SMS\\site_AA1" + ":SMS_Collection.CollectionID=\"" + collectionID + "\"";
                    ManagementPath path = new ManagementPath(pathString);
                    ManagementObject obj = new ManagementObject();

                    obj.Scope = SCCM.ms;
                    obj.Path = path;
                    obj.Get();

                    namesInCollection.Add(obj["Name"].ToString());
                }
                return namesInCollection;
            }
            return null;
        }

        #region loading data
        public void InitBasicInfo()
        {
            //Managed By
            ManagedBy = "none";

            if (ManagedByAD != null)
            {
                string managerVal = ManagedByAD;

                if (!string.IsNullOrWhiteSpace(managerVal))
                {
                    string email = ADHelper.DistinguishedNameToUPN(managerVal);
                    ManagedBy = email;
                }
            }

            UsesOnedrive = OneDriveHelper.ComputerUsesOneDrive(ADcache);

            if (AdminPasswordExpirationTime != null)
            {
                ShowResultGetPassword = true;
            }
        }

        public void InitSCCMHW()
        {
            SCCMLD = TableGenerator.CreateVerticalTableFromDatabase(LogicalDisk,
                new List<string>() { "DeviceID", "FileSystem", "Size", "FreeSpace" },
                new List<string>() { "DeviceID", "File system", "Size (GB)", "FreeSpace (GB)" },
                "Disk information not found");

            if (SCCM.HasValues(RAM))
            {
                int total = 0;
                int count = 0;

                foreach (ManagementObject o in RAM) //Has one!
                {
                    total += int.Parse(o.Properties["Capacity"].Value.ToString()) / 1024;
                    count++;
                }

                SCCMRAM = $"{total} GB RAM in {count} slot(s)";
            }
            else
            {
                SCCMRAM = "RAM information not found";
            }

            SCCMBIOS = TableGenerator.CreateVerticalTableFromDatabase(BIOS,
                new List<string>() { "BIOSVersion", "Description", "Manufacturer", "Name", "SMBIOSBIOSVersion" },
                "BIOS information not found");

            SCCMVC = TableGenerator.CreateVerticalTableFromDatabase(VideoController,
                new List<string>() { "AdapterRAM", "CurrentHorizontalResolution", "CurrentVerticalResolution", "DriverDate", "DriverVersion", "Name" },
                "Video controller information not found");

            SCCMProcessor = TableGenerator.CreateVerticalTableFromDatabase(Processor,
                new List<string>() { "Is64Bit", "IsMobile", "IsVitualizationCapable", "Manufacturer", "MaxClockSpeed", "Name", "NumberOfCores", "NumberOfLogicalProcessors" },
                "Processor information not found");

            SCCMDisk = TableGenerator.CreateVerticalTableFromDatabase(Disk,
                new List<string>() { "Caption", "Model", "Partitions", "Size", "Name" },
                "Video controller information not found");
        }

        public void InitSCCMInfo()
        {
            /*
             *     strQuery = "SELECT * FROM SMS_FullCollectionMembership WHERE ResourceID="& computerID
                    for each fc in foundCollections
                       Set collection = SWbemServices.Get ("SMS_Collection.CollectionID=""" & fc.CollectionID &"""")
                       stringResult = stringResult & "<li> "  & collection.Name & "<br />"
                Next

             * SMS_Collection.CollectionID =
             *
             */

            //XXX: remeber to filter out computers that are obsolite in sccm (not active)
            string error = "";
            HTMLTableHelper groupTableHelper = new HTMLTableHelper(new string[] { "Collection Name" });
            var names = setConfig();

            if (names != null)
            {
                foreach (var name in names)
                {
                    groupTableHelper.AddRow(new string[] { name });
                }
            }
            else
            {
                error = "Computer not found i SCCM";
            }

            //Basal Info
            var tableAndList = TableGenerator.CreateTableAndRawFromDatabase(System, new List<string>() { "LastLogonUserName", "IPAddresses", "MACAddresses", "Build", "Config" }, "Computer not found i SCCM");

            SCCMComputers = error + groupTableHelper.GetTable();
            SCCMCollectionsTable = tableAndList.Item1; //Table
            SCCMCollections = tableAndList.Item2; //List
        }

        public void InitSCCMInventory()
        {
            var tableAndList = TableGenerator.CreateTableAndRawFromDatabase(Computer, new List<string>() { "Manufacturer", "Model", "SystemType", "Roles" }, "No inventory data");
            SSCMInventoryTable = tableAndList.Item1; //Table
            SCCMCollecionsSoftware = TableGenerator.CreateTableFromDatabase(Software,
                new List<string>() { "SoftwareCode", "ProductName", "ProductVersion", "TimeStamp" },
                new List<string>() { "Product ID", "Name", "Version", "Install date" },
                "Software information not found");
            SCCMInventory += tableAndList.Item2; //List
        }
        #endregion loding data
    }
}