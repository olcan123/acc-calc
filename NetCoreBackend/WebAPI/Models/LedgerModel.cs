using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Concrate;

namespace WebAPI.Models
{
    public class LedgerModel
    {
        public Ledger Ledger { get; set; }
        public List<LedgerEntry> LedgerEntries { get; set; }
    }
}