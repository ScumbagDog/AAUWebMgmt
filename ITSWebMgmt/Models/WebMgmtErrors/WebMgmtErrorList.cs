using ITSWebMgmt.Models;
using System;
using System.Collections.Generic;

namespace ITSWebMgmt.WebMgmtErrors
{
    public class WebMgmtErrorList
    {
        public List<WebMgmtError> PossibleErrors { get; private set; }
        //Blev træt af at skulle tjekke for HaveError i mine løkker, så nu er CurrentErrors en ting
        public List<WebMgmtError> CurrentErrors {
            get
            {
                return PossibleErrors.FindAll(x => x.HaveError());
            }
        }
        public bool CurrentlyHasErrors { 
            get
            {
                return CurrentErrors.Count != 0;
            }
        }
        public int[] ErrorCount { get; private set; } = { 0, 0, 0 };
        public string ErrorMessages;
        public Severity MostImportantCurrentSeverity
        {
            get
            {
                foreach(Severity s in Enum.GetValues(typeof(Severity)))
                { //Iterer over hver værdi i enummen
                    if (getSeverityCount(s) > 0)
                    { //Hvis den givne enumværdi har en fejl, så returner den. Husk at løkken kører fra error til warning til info. 
                        return s;
                    }
                }
                return Severity.Info; //Dette burde ikke ske, men i tilfælde af at det gør, så vælg den mindst kritiske.
            }
        }

        public WebMgmtErrorList(List<WebMgmtError> errors)
        {
            this.PossibleErrors = errors;
            processErrors();
        }
        public int getSeverityCount(Severity severityToCount)
        {
            int count = 0;
            foreach (WebMgmtError error in CurrentErrors)
            {
                if (error.Severeness.Equals(severityToCount))
                {
                    count++;
                }
            }
            return count;
        }
        private void processErrors()
        {
            foreach (WebMgmtError error in PossibleErrors)
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

            if (error is MissingGroup)
            {
                var macError = error as MissingGroup;
                //TODO handle these errors special
                return $"<div class=\"ui {messageType} message\" runat= \"server\">" +
                    $"<div class=\"header\">{macError.Heading}</div>" +
                    $"<p>{macError.Description}</p><br/>" +
                    $"<a href=\"{macError.CaseLink}\">Case<a/>" +
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