﻿using ITSWebMgmt.Models;
using System.Collections.Generic;

namespace ITSWebMgmt.WebMgmtErrors
{
    public class WebMgmtErrorList
    {
        private List<WebMgmtError> errors;
        private int[] ErrorCount = { 0, 0, 0 };
        public string ErrorMessages;

        public WebMgmtErrorList(List<WebMgmtError> errors)
        {
            this.errors = errors;
            processErrors();
        }

        private void processErrors()
        {
            foreach (WebMgmtError error in errors)
            {
                if (error.HaveError())
                {
                    ErrorCount[(int)error.Severeness]++;
                    ErrorMessages += generateMessage(error);
                }
            }
            if (ErrorMessages == null)
            {
                ErrorMessages = "No warnings found";
            }
        }

        private string generateMessage(WebMgmtError error)
        {
            string messageType = "";

            switch (error.Severeness)
            {
                case Severity.Error:
                    messageType = "negative";
                    break;
                case Severity.Warning:
                    messageType = "warning";
                    break;
                case Severity.Info:
                    messageType = "info";
                    break;
            }

            if (error is MacWebMgmtError)
            {
                //TODO handle these errors special
                return $"<div class=\"ui {messageType} message\" runat= \"server\">" +
                    $"<div class=\"header\">{error.Heading}</div>" +
                    $"<p>{error.Description}</p>" +
                    $"</div>";
            }

            return $"<div class=\"ui {messageType} message\" runat= \"server\">" +
                    $"<div class=\"header\">{error.Heading}</div>" +
                    $"<p>{error.Description}</p>" +
                    $"</div>";
        }

        public string getErrorCountMessage()
        {
            string messageType = "";
            string heading = "";

            if (ErrorCount[(int)Severity.Error] > 0)
            {
                messageType = "negative";
                heading = "Errors";
            }
            else if (ErrorCount[(int)Severity.Warning] > 0)
            {
                messageType = "warning";
                heading = "Warnings";
            }
            else if (ErrorCount[(int)Severity.Info] > 0)
            {
                messageType = "info";
                heading = "Infos";
            }

            return messageType == "" ? "" : $"<div class=\"ui {messageType} message\" runat= \"server\">" +
                    $"<div class=\"header\">{heading} found</div>" +
                    $"<p>Found {ErrorCount[(int)Severity.Error]} errors, {ErrorCount[(int)Severity.Warning]} warnings, and {ErrorCount[(int)Severity.Info]} infos.</p>" +
                    $"</div>";
        }
    }
}