using System;
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
        public static GpuInfo GetPrimaryGpuInfo()
        {
            GpuInfo gpuInfo = new GpuInfo();

            try
            {
                // Query WMI to retrieve information about video controllers
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_VideoController");
                foreach (ManagementObject obj in searcher.Get().OfType<ManagementObject>())
                {
                    // Get the name of the GPU
                    gpuInfo.Name = obj["Name"]?.ToString();

                    // Convert AdapterRAM value to ulong
                    object adapterRAMValue = obj["AdapterRAM"];
                    gpuInfo.AdapterRAM = (adapterRAMValue != null) ? Convert.ToUInt64(adapterRAMValue) : 0;

                    // Check if the GPU is primary
                    object adapterCompatibilityValue = obj["AdapterCompatibility"];
                    if (adapterCompatibilityValue != null && adapterCompatibilityValue.ToString().Equals("Primary"))
                    {
                        // Found primary GPU, break the loop
                        break;
                    }
                }
            }
            catch (Exception)
            {
                // Log the error or handle it silently
            }

            return gpuInfo;
        }
    }

    internal static class GpuInfoManager
    {
        public static void FindGpuArchitecture(Label GPUlabel)
        {
            // Retrieve information about the primary GPU
            GpuInfo gpuInfo = GpuHelper.GetPrimaryGpuInfo();

            // Set label text and color based on GPU architecture
            if (!string.IsNullOrEmpty(gpuInfo.Name) && gpuInfo.AdapterRAM > 0)
            {
                if (gpuInfo.Name.Contains("AMD") || gpuInfo.Name.Contains("Radeon"))
                {
                    GPUlabel.Text = gpuInfo.Name;
                    GPUlabel.ForeColor = Color.Red; // Set color to red for AMD GPUs
                }
                else if (gpuInfo.Name.Contains("NVIDIA"))
                {
                    GPUlabel.Text = gpuInfo.Name;
                    GPUlabel.ForeColor = Color.Green; // Set color to green for NVIDIA GPUs
                }
                else if (gpuInfo.Name.Contains("Intel"))
                {
                    GPUlabel.Text = gpuInfo.Name;
                    GPUlabel.ForeColor = Color.Blue; // Set color to blue for Intel GPUs
                }
            }
            else
            {
                GPUlabel.Text = gpuInfo.Name;
                GPUlabel.ForeColor = Color.Black; // Set color to black if GPU information is not available
            }
        }
    }
}

