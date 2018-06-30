using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashcode
{
    class SimulationClass
    {
        public int row { get; set; }
        public int col { get; set; }
        public int number_car { get; set; }
        public int number_rides { get; set; }
        public int number_bonus { get; set; }
        public int step { get; set; }
        public int current_step { get; set; }


        public List<Rides> todo { get; set; }
        public List<Car> car_list { get; set; }

        private void getHeader(string line)
        {
            string[] splitted = line.Split(' ');

            this.current_step = 0;
            this.todo = new List<Rides>();
            this.car_list = new List<Car>();
            this.row = Int32.Parse(splitted[0]);
            this.col = Int32.Parse(splitted[1]);
            this.number_car = Int32.Parse(splitted[2]);
            this.number_rides = Int32.Parse(splitted[3]);
            this.number_bonus = Int32.Parse(splitted[4]);
            this.step = Int32.Parse(splitted[5]);

        }


        private void TeslaFactory()
        {
            for (int i = 0; i < number_car; i++)
            {
                Car tmp = new Car(0, 0);
                this.car_list.Add(tmp);
            }
        }

        private void ConstructAutopilotV2Ride(string line, int index)
        {
            string[] splitted = line.Split(' ');

            Rides tmp = new Rides(splitted[0], splitted[1], splitted[2], splitted[3], splitted[4], splitted[5], index);
            this.todo.Add(tmp);
        }

        public void initSimulation(string path)
        {

            string line;
            int index = 0;

            System.IO.StreamReader file = new System.IO.StreamReader(path);

            line = file.ReadLine();

            this.getHeader(line);
            this.TeslaFactory();

            while ((line = file.ReadLine()) != null)
            {
                this.ConstructAutopilotV2Ride(line, index);
                index++;
            }
            this.todo.Sort((a, b) => (a.time_to_start.CompareTo(b.time_to_start)));
            file.Close();

        }

        public void run()
        {
            while (current_step < step)
            {
                foreach (var car in car_list)
                {
                    if (car.ride_todo == null && todo.Count() > 0)
                    {
                        car.ride_todo = GetBestRide(car);//todo.First();
                        todo.Remove(car.ride_todo);
                        car.state = StatesEnum.GO_TO_DEPARTURE;
                    }
                    if (car.ride_todo != null)
                    {
                        switch (car.state)
                        {
                            case StatesEnum.GO_TO_DEPARTURE:
                                this.moveCar(car, car.ride_todo.x_start, car.ride_todo.y_start);
                                if (car.ride_todo.x_start == car.x &&
                                    car.ride_todo.y_start == car.y && car.ride_todo.time_to_start > current_step)
                                    car.state = StatesEnum.WAIT;
                                else if (car.ride_todo.x_start == car.x &&
                                    car.ride_todo.y_start == car.y && car.ride_todo.time_to_start <= current_step)
                                    car.state = StatesEnum.GO_FINAL;
                                break;
                            case StatesEnum.WAIT:
                                if (car.ride_todo.time_to_start <= current_step)
                                    car.state = StatesEnum.GO_FINAL;
                                break;
                            case StatesEnum.GO_FINAL:
                                this.moveCar(car, car.ride_todo.x_stop, car.ride_todo.y_stop);
                                if (car.ride_todo.x_stop == car.x && car.ride_todo.y_stop == car.y)
                                {
                                    car.rides_dones.Add(car.ride_todo);
                                    car.ride_todo = null;
                                    car.state = StatesEnum.ARRIVED;
                                }
                                break;
                            case StatesEnum.ARRIVED:
                                break;
                            default:
                                break;
                        }
                    }
                }
                current_step++;
            }
        }

        public void moveCar(Car car, int x, int y)
        {
            if (car.x > x)
                car.x -= 1;
            else if (car.x < x)
                car.x += 1;
            else if (car.y > y)
                car.y -= 1;
            else if (car.y < y)
                car.y += 1;
        }

        public void exportdata(string path)
        {
            string line = "";

            using (StreamWriter writer = new StreamWriter(path))
            {
                foreach (var car in car_list)
                {
                    line = "" + car.rides_dones.Count();
                    foreach (var drive in car.rides_dones)
                    {
                        line += " " + drive.index;
                    }
                    writer.WriteLine(line);
                }
            }
        }

        // Get the most optimize ride for the given car
        private Rides GetBestRide(Car car)
        {
          int remainingSteps = step - current_step;
          Rides bestRide = null;
          int rideSteps;
          int bestRideSteps = int.MaxValue;

          foreach (Rides ride in todo)
          {
            rideSteps = GetRequiredSteps(car, ride);
            if (rideSteps >= remainingSteps || (current_step + rideSteps) >= ride.time_max_to_finish)
              continue;
            if (rideSteps < bestRideSteps)
            {
              bestRideSteps = rideSteps;
              bestRide = ride;
            }
          }
          return (bestRide);
        }

        // Return the requied steps for a given car to archive the given ride
        private int GetRequiredSteps(Car car, Rides ride)
        {
          int requiredTravelSteps = Math.Abs(ride.x_start - car.x) + Math.Abs(ride.y_start - car.y);
          int requiredRideSteps = Math.Abs(ride.x_stop - ride.x_start) + Math.Abs(ride.y_stop - ride.y_stop);

          if (ride.time_to_start > current_step)
          {
            int waitingSteps = (ride.time_to_start - current_step) - requiredTravelSteps;
            if (waitingSteps > 0)
              return (requiredTravelSteps + requiredRideSteps + waitingSteps);
          }
          return (requiredTravelSteps + requiredRideSteps);
        }
    }
}
