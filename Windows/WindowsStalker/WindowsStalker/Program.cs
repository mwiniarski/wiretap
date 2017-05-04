using System;
using System.Collections.Generic;
using System.Management;

namespace WindowsStalker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Scheduler scheduler = new Scheduler();
        }

        public static string GetComputerID()
        {
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorID"].ToString();
            }

            return id;
        }
    }
}