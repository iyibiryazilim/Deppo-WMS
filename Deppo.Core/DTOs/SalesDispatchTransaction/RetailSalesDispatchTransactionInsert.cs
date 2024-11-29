﻿using Deppo.Core.DTOs.BaseTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deppo.Core.DTOs.SalesDispatchTransaction
{
    public class RetailSalesDispatchTransactionInsert : BaseDispatchTransactionDto
    {
        public RetailSalesDispatchTransactionInsert()
        {
            Lines = new List<RetailSalesDispatchTransactionLineInsert>();
        }

        public List<RetailSalesDispatchTransactionLineInsert> Lines { get; set; }
    }
}