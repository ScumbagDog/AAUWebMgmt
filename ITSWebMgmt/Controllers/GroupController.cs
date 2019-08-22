﻿using ITSWebMgmt.Caches;
using ITSWebMgmt.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ITSWebMgmt.Models;
using ITSWebMgmt.Models.Log;

namespace ITSWebMgmt.Controllers
{
    public class GroupController : WebMgmtController
    {
        //https://localhost:44322/group/index?grouppath=LDAP:%2f%2fCN%3dcm12_config_AAU10%2cOU%3dConfigMgr%2cOU%3dGroups%2cDC%3dsrv%2cDC%3daau%2cDC%3ddk
        public IActionResult Index(string grouppath, bool forceviewgroup = false)
        {
            GroupModel = new GroupModel(grouppath);
            if (forceviewgroup == false && isFileShare(GroupModel.DistinguishedName))
            {
                string[] tables = GetFileshareTables();
                GroupModel.GroupSegment = tables[0];
                GroupModel.GroupsAllSegment = tables[1];
                GroupModel.GroupOfSegment = tables[2];
                GroupModel.GroupsOfAllSegment = tables[3];
                GroupModel.IsFileShare = true;

                return View("FileShare", GroupModel);
            }
            return View(GroupModel);
        }

        public GroupModel GroupModel;

        public GroupController(LogEntryContext context) : base(context) {}

        public bool isGroup()
        {
            ///XXX we expect a group check its a group
            return GroupModel.ADcache.DE.SchemaEntry.Name.Equals("group", StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool isFileShare(string value)
        {
            var split = value.Split(',');
            var oupath = split.Where(s => s.StartsWith("OU=")).ToArray();
            int count = oupath.Count();

            return ((count == 3 && oupath[count - 1].Equals("OU=Groups") && oupath[count - 2].Equals("OU=Resource Access")));
        }

        public string[] GetFileshareTables()
        {
            //TODO Things to show in basic info: Type fileshare/department and Domain plan/its/adm

            HTMLTableHelper[] groupTableHelper = new HTMLTableHelper[4];
            for (int i = 0; i < 4; i++)
            {
                groupTableHelper[i] = new HTMLTableHelper(new string[] { "Name", "Type", "Access" });
            }

            List<string> accessNames = new List<string> { "Full", "Modify", "Read", "Edit", "Contribute" };
            foreach (string accessName in accessNames)
            {
                string temp = Regex.Replace(GroupModel.adpath, @"_[a-zA-Z]*,OU", $"_{accessName},OU");
                ADcache group = null;
                try
                {
                    group = new GroupADcache(temp);
                }
                catch (Exception)
                {
                }

                if (group != null)
                {
                    List<string>[] adpaths = new List<string>[] { group.getGroups("member"), group.getGroupsTransitive("member"), group.getGroups("memberOf"), group.getGroupsTransitive("memberOf") };
                    for (int i = 0; i < 4; i++)
                    {
                        TableGenerator.createGroupTableRows(adpaths[i], groupTableHelper[i], accessName);
                    }
                }
            }

            return new string[] {
            groupTableHelper[0].GetTable(),
            groupTableHelper[1].GetTable(),
            groupTableHelper[2].GetTable(),
            groupTableHelper[3].GetTable()};
        }
    }
}