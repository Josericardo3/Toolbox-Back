﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inti_repository
{
    public class MySQLConfiguration
    {
        public MySQLConfiguration(String connectionString)
        {
            ConnectionString = connectionString;
        }
        public String ConnectionString { get; set; }
    }
}
