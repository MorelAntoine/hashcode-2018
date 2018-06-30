using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashcode
{
    class Car
    {
        public int x { get; set; }
        public int y { get; set; }
        public Rides ride_todo { get; set; }
        public List<Rides> rides_dones { get; set; }
        public StatesEnum state { get; set; }

        public Car(int x, int y)
        {
            this.x = x;
            this.y = y;
            state = StatesEnum.BEGIN;
            rides_dones = new List<Rides>();
            ride_todo = null;
        }

    }
}
