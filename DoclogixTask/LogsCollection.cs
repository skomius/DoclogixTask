﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoclogixTask.Interface;
using DoclogixTask.Models;

namespace DoclogixTask
{
    public class LogsCollection: ILogsCollection
    {
        public LogsCollection()
        {
            LogsRecords = new List<Record>();
        }
        public List<Record> LogsRecords { get; set; }
    }
}
