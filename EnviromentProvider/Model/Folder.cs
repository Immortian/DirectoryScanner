using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnviromentProvider.Model
{
    public class Folder : Item
    {
        public List<Item> Items { get; set; }
    }
}
