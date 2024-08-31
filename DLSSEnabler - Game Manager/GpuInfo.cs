using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    internal class GpuInfo
    {
        public string Name { get; set; }
        public ulong AdapterRAM { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, AdapterRAM: {AdapterRAM}";
        }
    }

    internal static class GpuHelper
    {
        private static readonly string[] IntegratedNameKeywords =
        {
            "Intel(R) HD Graphics", "AMD Radeon HD"
        };

        public static GpuInfo GetPrimaryGpuInfo()
        {
            GpuInfo gpuInfo = null;
            List<GpuInfo> integratedGpus = new List<GpuInfo>();

            try
            {
                using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController"))
                {
                    foreach (ManagementObject obj in searcher.Get().OfType<ManagementObject>())
                    {
                        string gpuName = obj["Name"]?.ToString();
                        ulong gpuAdapterRAM = GetAdapterRAM(obj);

                        bool isIntegrated = IsIntegratedGpu(gpuName);

                        if (!isIntegrated && gpuAdapterRAM > 0)
                        {
                            gpuInfo = new GpuInfo { Name = gpuName, AdapterRAM = gpuAdapterRAM };
                            if (obj["AdapterCompatibility"]?.ToString() == "Primary")
                            {
                                break;
                            }
                        }
                        else if (isIntegrated)
                        {
                            integratedGpus.Add(new GpuInfo { Name = gpuName, AdapterRAM = gpuAdapterRAM });
                        }
                    }

                    gpuInfo = gpuInfo ?? integratedGpus.OrderByDescending(g => g.AdapterRAM).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                // Log the error (consider using a logger library)
                Console.WriteLine("Error during GPU information retrieval: " + ex.Message);
            }

            return gpuInfo;
        }

        private static ulong GetAdapterRAM(ManagementObject obj)
        {
            object adapterRAMValue = obj["AdapterRAM"];
            return adapterRAMValue != null && !string.IsNullOrEmpty(adapterRAMValue.ToString()) ?
                Convert.ToUInt64(adapterRAMValue) : 0;
        }

        private static bool IsIntegratedGpu(string gpuName)
        {
            return gpuName == null ||
                   IntegratedNameKeywords.Any(keyword => gpuName.Contains(keyword)) ||
                   gpuName.Contains("Graphics");
        }

    }

    internal static class GpuInfoManager
    {
        public static void FindGpuArchitecture(Label GPUlabel)
        {
            GpuInfo gpuInfo = GpuHelper.GetPrimaryGpuInfo();

            if (gpuInfo != null && !string.IsNullOrEmpty(gpuInfo.Name) && gpuInfo.AdapterRAM > 0)
            {
                GPUlabel.Text = gpuInfo.Name;
                GPUlabel.ForeColor = gpuInfo.Name.Contains("AMD") || gpuInfo.Name.Contains("Radeon") ? Color.Red :
                                      gpuInfo.Name.Contains("NVIDIA") ? Color.Green :
                                      Color.Blue; // Set blue for Intel or unknown
            }
            else
            {
                GPUlabel.Text = gpuInfo?.Name ?? "GPU Information Unavailable";
                GPUlabel.ForeColor = Color.Black;
            }
        }
    }
}
