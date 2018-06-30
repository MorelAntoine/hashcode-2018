using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hashcode
{
    class Program
    {
        static void Main(string[] args)
        {
            SimulationClass simu = new SimulationClass();

            simu.initSimulation(args[0]);
            simu.run();
            simu.exportdata("c:/test/output1.out");
            Console.WriteLine("Done");

            simu = new SimulationClass();

            simu.initSimulation(args[1]);
            simu.run();
            simu.exportdata("c:/test/output2.out");
            Console.WriteLine("Done");

            simu = new SimulationClass();

            simu.initSimulation(args[2]);
            simu.run();
            simu.exportdata("c:/test/output3.out");
            Console.WriteLine("Done");

            simu = new SimulationClass();

            simu.initSimulation(args[3]);
            simu.run();
            simu.exportdata("C:/test/output4.out");
            Console.WriteLine("Done");

            simu = new SimulationClass();

            simu.initSimulation(args[4]);
            simu.run();
            simu.exportdata("C:/test/output5.out");
            Console.WriteLine("all Done");
            Console.ReadLine();
            
        }
    }
}
