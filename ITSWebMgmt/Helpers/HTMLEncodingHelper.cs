﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITSWebMgmt.Helpers
{
    public class HTMLEncodingHelper
    {

        public static string convertUTF8encodingToWindows1252(string s)
        {

            string toReturn = s;
            string[][] table = new string[][] {
                //Table from  http://www.w3schools.com/tags/ref_urlencode.asp 
                new string[] {"%82","%E2%80%9A"},
                new string[] {"%83","%C6%92"},
                new string[] {"%84","%E2%80%9E"},
                new string[] {"%85","%E2%80%A6"},
                new string[] {"%86","%E2%80%A0"},
                new string[] {"%87","%E2%80%A1"},
                new string[] {"%88","%CB%86"},
                new string[] {"%89","%E2%80%B0"},
                new string[] {"%8A","%C5%A0"},
                new string[] {"%8B","%E2%80%B9"},
                new string[] {"%8C","%C5%92"},
                new string[] {"%8D","%C5%8D"},
                new string[] {"%8E","%C5%BD"},
                new string[] {"%90","%C2%90"},
                new string[] {"%91","%E2%80%98"},
                new string[] {"%92","%E2%80%99"},
                new string[] {"%93","%E2%80%9C"},
                new string[] {"%94","%E2%80%9D"},
                new string[] {"%95","%E2%80%A2"},
                new string[] {"%96","%E2%80%93"},
                new string[] {"%97","%E2%80%94"},
                new string[] {"%98","%CB%9C"},
                new string[] {"%99","%E2%84"},
                new string[] {"%9A","%C5%A1"},
                new string[] {"%9B","%E2%80"},
                new string[] {"%9C","%C5%93"},
                new string[] {"%9E","%C5%BE"},
                new string[] {"%9F","%C5%B8"},
                new string[] {"%A0","%C2%A0"},
                new string[] {"%A1","%C2%A1"},
                new string[] {"%A2","%C2%A2"},
                new string[] {"%A3","%C2%A3"},
                new string[] {"%A4","%C2%A4"},
                new string[] {"%A5","%C2%A5"},
                new string[] {"%A6","%C2%A6"},
                new string[] {"%A7","%C2%A7"},
                new string[] {"%A8","%C2%A8"},
                new string[] {"%A9","%C2%A9"},
                new string[] {"%AA","%C2%AA"},
                new string[] {"%AB","%C2%AB"},
                new string[] {"%AC","%C2%AC"},
                new string[] {"%AD","%C2%AD"},
                new string[] {"%AE","%C2%AE"},
                new string[] {"%AF","%C2%AF"},
                new string[] {"%B0","%C2%B0"},
                new string[] {"%B1","%C2%B1"},
                new string[] {"%B2","%C2%B2"},
                new string[] {"%B3","%C2%B3"},
                new string[] {"%B4","%C2%B4"},
                new string[] {"%B5","%C2%B5"},
                new string[] {"%B6","%C2%B6"},
                new string[] {"%B7","%C2%B7"},
                new string[] {"%B8","%C2%B8"},
                new string[] {"%B9","%C2%B9"},
                new string[] {"%BA","%C2%BA"},
                new string[] {"%BB","%C2%BB"},
                new string[] {"%BC","%C2%BC"},
                new string[] {"%BD","%C2%BD"},
                new string[] {"%BE","%C2%BE"},
                new string[] {"%BF","%C2%BF"},
                new string[] {"%C0","%C3%80"},
                new string[] {"%C1","%C3%81"},
                new string[] {"%C2","%C3%82"},
                new string[] {"%C3","%C3%83"},
                new string[] {"%C4","%C3%84"},
                new string[] {"%C5","%C3%85"},
                new string[] {"%C6","%C3%86"},
                new string[] {"%C7","%C3%87"},
                new string[] {"%C8","%C3%88"},
                new string[] {"%C9","%C3%89"},
                new string[] {"%CA","%C3%8A"},
                new string[] {"%CB","%C3%8B"},
                new string[] {"%CC","%C3%8C"},
                new string[] {"%CD","%C3%8D"},
                new string[] {"%CE","%C3%8E"},
                new string[] {"%CF","%C3%8F"},
                new string[] {"%D0","%C3%90"},
                new string[] {"%D1","%C3%91"},
                new string[] {"%D2","%C3%92"},
                new string[] {"%D3","%C3%93"},
                new string[] {"%D4","%C3%94"},
                new string[] {"%D5","%C3%95"},
                new string[] {"%D6","%C3%96"},
                new string[] {"%D7","%C3%97"},
                new string[] {"%D8","%C3%98"},
                new string[] {"%D9","%C3%99"},
                new string[] {"%DA","%C3%9A"},
                new string[] {"%DB","%C3%9B"},
                new string[] {"%DC","%C3%9C"},
                new string[] {"%DD","%C3%9D"},
                new string[] {"%DE","%C3%9E"},
                new string[] {"%DF","%C3%9F"},
                new string[] {"%E0","%C3%A0"},
                new string[] {"%E1","%C3%A1"},
                new string[] {"%E2","%C3%A2"},
                new string[] {"%E3","%C3%A3"},
                new string[] {"%E4","%C3%A4"},
                new string[] {"%E5","%C3%A5"},
                new string[] {"%E6","%C3%A6"},
                new string[] {"%E7","%C3%A7"},
                new string[] {"%E8","%C3%A8"},
                new string[] {"%E9","%C3%A9"},
                new string[] {"%EA","%C3%AA"},
                new string[] {"%EB","%C3%AB"},
                new string[] {"%EC","%C3%AC"},
                new string[] {"%ED","%C3%AD"},
                new string[] {"%EE","%C3%AE"},
                new string[] {"%EF","%C3%AF"},
                new string[] {"%F0","%C3%B0"},
                new string[] {"%F1","%C3%B1"},
                new string[] {"%F2","%C3%B2"},
                new string[] {"%F3","%C3%B3"},
                new string[] {"%F4","%C3%B4"},
                new string[] {"%F5","%C3%B5"},
                new string[] {"%F6","%C3%B6"},
                new string[] {"%F7","%C3%B7"},
                new string[] {"%F8","%C3%B8"},
                new string[] {"%F9","%C3%B9"},
                new string[] {"%FA","%C3%BA"},
                new string[] {"%FB","%C3%BB"},
                new string[] {"%FC","%C3%BC"},
                new string[] {"%FD","%C3%BD"},
                new string[] {"%FE","%C3%BE"},
                new string[] {"%FF","%C3%BF"},

            };


        foreach(var replace in table){
            toReturn = toReturn.Replace(replace[1], replace[0]);
            toReturn = toReturn.Replace(replace[1].ToLower(), replace[0].ToLower());
        }

        return toReturn;

        }



    }
}