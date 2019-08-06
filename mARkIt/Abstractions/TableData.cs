using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mARkIt.Abstractions
{
    public abstract class TableData
    {
        public bool deleted { get; set; }
        public DateTime updatedAt { get; set; }
        public DateTime createdAt { get; set; }
        public string version { get; set; }
        public string id { get; set; }
    }
}
