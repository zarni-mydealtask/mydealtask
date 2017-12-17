using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PassengerNameListWebApi.BusinessLogic
{
    public class RecordParser : IRecordParser
    {
        public bool ParseRecord(string record, out string passengerName, out string recordLocator)
        {
            passengerName = string.Empty;
            recordLocator = string.Empty;
            if (string.IsNullOrEmpty(record) || IsIgnoreLine(record))
                return false;

            record = record.Trim();

            if (IsPassengerLine(record) && ValidRecordLocator(record,out int foundIndex))
            {
                passengerName = record.Substring(1, foundIndex-1);
                if (RequireToTrimPassengerName(passengerName))
                {
                    passengerName = passengerName.Substring(0, passengerName.Length - 3);
                }

                recordLocator = record.Substring(foundIndex + 4);
                return true;
            }
            return false;
        }

        public static bool IsIgnoreLine(string record)
        {
            return record.StartsWith(".R");
        }

        public static bool IsPassengerLine(string record)
        {
            return record.StartsWith("1");
        }
        public static bool RequireToTrimPassengerName(string name)
        {
            return name.Substring(name.Length-3,1) == "-";
        }
        public static bool ValidRecordLocator(string record,out int foundIndex)
        {
            foundIndex = -1;
            if(record.Length > 10)
            {
                foundIndex = record.IndexOf(" .L/");
                return foundIndex > 0;
            }
                
            return false;
        }
    }
}