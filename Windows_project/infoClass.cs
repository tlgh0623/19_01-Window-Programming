using System.Diagnostics;
namespace WindowsFormsApp2
{
    class infoClass
    {
        Process[] processes = Process.GetProcesses();

        public string[] process_Load()
        {
            string[] process = new string[3];
            foreach (Process pro in processes)
            {
                process[0] = pro.ProcessName;
            }
            return process;
        }
    }
}
