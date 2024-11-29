﻿using Deppo.Core.DTOs.BaseTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deppo.Core.DTOs.TransferTransaction
{
    public class TransferTransactionLineDto : BaseTransactionLineDto
    {
        public int? DestinationWarehouseNumber { get; set; }
    }
}