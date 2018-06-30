using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashcode
{
    class Rides
    {
        public int x_start { get; set; }
        public int y_start { get; set; }

        public int x_stop { get; set; }

        public int y_stop { get; set; }
        public int time_to_start { get; set; }
        public int time_max_to_finish { get; set; }
        public int index { get; set; }

        public Rides(string x, string y, string a, string b, string start, string stop, int index)
        {
            this.x_start = Int32.Parse(x);
            this.y_start = Int32.Parse(y);
            this.x_stop = Int32.Parse(a);
            this.y_stop = Int32.Parse(b);
            this.time_to_start = Int32.Parse(start);
            this.time_max_to_finish = Int32.Parse(stop);
            this.index = index;
        }
    }
}
