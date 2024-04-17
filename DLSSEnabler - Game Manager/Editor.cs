using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DLSSEnabler___Game_Manager
{
    public partial class Editor : Form
    {
        public Editor()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            // Basic View
            viewBox.SelectedItem = "Basic";

            populateComboBoxes();
        }

        private void viewBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedOption = viewBox.SelectedItem.ToString();

            if (selectedOption == "Basic")
            {

                frameGenBox.Visible = true;
                upscalersBox.Visible = true;
                sharpnessBox.Visible = true;
                casBox.Visible = true;

                qualityOverridesBox.Visible = false;
                colorBox.Visible = false;
                depthBox.Visible = false;
                motionVectorsBox.Visible = false;
                upscaleRatioBox.Visible = false;
                dx11w12Box.Visible = false;
                fsrBox.Visible = false;
                xessBox.Visible = false;

                logBox.Visible = false;
                hotfixBox.Visible = false;
            }
            else if (selectedOption == "Advanced")
            {

                frameGenBox.Visible = false;
                upscalersBox.Visible = false;
                sharpnessBox.Visible = false;
                casBox.Visible = false;

                qualityOverridesBox.Visible = true;
                colorBox.Visible = true;
                depthBox.Visible = true;
                motionVectorsBox.Visible = true;
                upscaleRatioBox.Visible = true;
                dx11w12Box.Visible = true;
                fsrBox.Visible = true;
                xessBox.Visible = true;

                logBox.Visible = false;
                hotfixBox.Visible = false;
            }
            else if (selectedOption == "Debug")
            {
                frameGenBox.Visible = false;
                upscalersBox.Visible = false;
                sharpnessBox.Visible = false;
                casBox.Visible = false;

                qualityOverridesBox.Visible = false;
                colorBox.Visible = false;
                depthBox.Visible = false;
                motionVectorsBox.Visible = false;
                upscaleRatioBox.Visible = false;
                dx11w12Box.Visible = false;
                fsrBox.Visible = false;
                xessBox.Visible = false;

                logBox.Visible = true;
                hotfixBox.Visible = true;
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            // Close the editor
            this.Close();
        }

        private void resetSettings()
        {
            generatorBox.SelectedItem = "auto";
            dx11Box.SelectedItem = "auto";
            dx12Box.SelectedItem = "auto";
            vkBox.SelectedItem = "auto";
            overrideBox.SelectedItem = "auto";
            sharpValueBox.SelectedItem = "auto";
            enabCasBox.SelectedItem = "auto";
            cscCasBox.SelectedItem = "auto";

            // Advanced view
            invertedBox.SelectedItem = "auto";
            ussqBox.SelectedItem = "auto";
            buildPipeBox.SelectedItem = "auto";
            vFovBox.SelectedItem = "auto";
            hFovBox.SelectedItem = "auto";
            jitterCancBox.SelectedItem = "auto";
            displayResBox.SelectedItem = "auto";
            autoExBox.SelectedItem = "auto";
            hdrBox.SelectedItem = "auto";
            qOverrideBox.SelectedItem = "auto";
            ratioOvBox.SelectedItem = "auto";
            ultraQBox.SelectedItem = "auto";
            qualityBox.SelectedItem = "auto";
            balancedBox.SelectedItem = "auto";
            perfBox.SelectedItem = "auto";
            uPerfBox.SelectedItem = "auto";
            ratioOvValBox.SelectedItem = "auto";

            // Debug view
            drmBox.SelectedItem = "auto";
            crbBox.SelectedItem = "auto";
            mvrbBox.SelectedItem = "auto";
            drbBox.SelectedItem = "auto";
            cmrbBox.SelectedItem = "auto";
            erbBox.SelectedItem = "auto";
            orbBox.SelectedItem = "auto";
            logeBox.SelectedItem = "auto";
            loglvlBox.SelectedItem = "auto";
            logtcBox.SelectedItem = "auto";
            logtfBox.SelectedItem = "auto";
            logtnvBox.SelectedItem = "auto";
            opconsBox.SelectedItem = "auto";
        }

        public void populateComboBoxes()
        {
            // Read nvngx.ini file and populate comboboxes with corresponding values

            // Set the correct path for nvngx.ini file
            string nvngxIniPath = this.Text.Replace("Customize DLSS - ", "") + "/nvngx.ini";

            string[] lines = File.ReadAllLines(nvngxIniPath);
            foreach (string line in lines)
            {
                // Check for each combobox
                if (line.StartsWith("Generator="))
                {
                    string value = line.Split('=')[1].Trim();
                    generatorBox.SelectedItem = value;
                }
                else if (line.StartsWith("Dx12Upscaler="))
                {
                    string value = line.Split('=')[1].Trim();
                    dx12Box.SelectedItem = value;
                }
                else if (line.StartsWith("Dx11Upscaler="))
                {
                    string value = line.Split('=')[1].Trim();
                    dx11Box.SelectedItem = value;
                }
                else if (line.StartsWith("VulkanUpscaler="))
                {
                    string value = line.Split('=')[1].Trim();
                    vkBox.SelectedItem = value;
                }
                else if (line.StartsWith("UseSafeSyncQueries="))
                {
                    string value = line.Split('=')[1].Trim();
                    ussqBox.SelectedItem = value;
                }
                else if (line.StartsWith("BuildPipelines="))
                {
                    string value = line.Split('=')[1].Trim();
                    buildPipeBox.SelectedItem = value;
                }
                else if (line.StartsWith("VerticalFov="))
                {
                    string value = line.Split('=')[1].Trim();
                    vFovBox.SelectedItem = value;
                }
                else if (line.StartsWith("HorizontalFov="))
                {
                    string value = line.Split('=')[1].Trim();
                    hFovBox.SelectedItem = value;
                }
                else if (line.StartsWith("OverrideSharpness="))
                {
                    string value = line.Split('=')[1].Trim();
                    overrideBox.SelectedItem = value;
                }
                else if (line.StartsWith("Sharpness="))
                {
                    string value = line.Split('=')[1].Trim();
                    sharpValueBox.SelectedItem = value;
                }
                else if (line.StartsWith("Enabled="))
                {
                    string value = line.Split('=')[1].Trim();
                    enabCasBox.SelectedItem = value;
                }
                else if (line.StartsWith("ColorSpaceConversion="))
                {
                    string value = line.Split('=')[1].Trim();
                    cscCasBox.SelectedItem = value;
                }
                else if (line.StartsWith("LoggingEnabled="))
                {
                    string value = line.Split('=')[1].Trim();
                    logeBox.SelectedItem = value;
                }
                else if (line.StartsWith("LogLevel="))
                {
                    string value = line.Split('=')[1].Trim();
                    loglvlBox.SelectedItem = value;
                }
                else if (line.StartsWith("LogToConsole="))
                {
                    string value = line.Split('=')[1].Trim();
                    logtcBox.SelectedItem = value;
                }
                else if (line.StartsWith("LogToFile="))
                {
                    string value = line.Split('=')[1].Trim();
                    logtfBox.SelectedItem = value;
                }
                else if (line.StartsWith("LogToNVSDK="))
                {
                    string value = line.Split('=')[1].Trim();
                    logtnvBox.SelectedItem = value;
                }
                else if (line.StartsWith("OpenConsole="))
                {
                    string value = line.Split('=')[1].Trim();
                    opconsBox.SelectedItem = value;
                }
                else if (line.StartsWith("DepthInverted="))
                {
                    string value = line.Split('=')[1].Trim();
                    invertedBox.SelectedItem = value;
                }
                else if (line.StartsWith("AutoExposure="))
                {
                    string value = line.Split('=')[1].Trim();
                    autoExBox.SelectedItem = value;
                }
                else if (line.StartsWith("HDR="))
                {
                    string value = line.Split('=')[1].Trim();
                    hdrBox.SelectedItem = value;
                }
                else if (line.StartsWith("JitterCancellation="))
                {
                    string value = line.Split('=')[1].Trim();
                    jitterCancBox.SelectedItem = value;
                }
                else if (line.StartsWith("DisplayResolution="))
                {
                    string value = line.Split('=')[1].Trim();
                    displayResBox.SelectedItem = value;
                }
                else if (line.StartsWith("UpscaleRatioOverrideEnabled="))
                {
                    string value = line.Split('=')[1].Trim();
                    ratioOvBox.SelectedItem = value;
                }
                else if (line.StartsWith("UpscaleRatioOverrideValue="))
                {
                    string value = line.Split('=')[1].Trim();
                    ratioOvValBox.SelectedItem = value;
                }
                else if (line.StartsWith("QualityRatioOverrideEnabled="))
                {
                    string value = line.Split('=')[1].Trim();
                    qOverrideBox.SelectedItem = value;
                }
                else if (line.StartsWith("QualityRatioUltraQuality="))
                {
                    string value = line.Split('=')[1].Trim();
                    ultraQBox.SelectedItem = value;
                }
                else if (line.StartsWith("QualityRatioQuality="))
                {
                    string value = line.Split('=')[1].Trim();
                    qualityBox.SelectedItem = value;
                }
                else if (line.StartsWith("QualityRatioBalanced="))
                {
                    string value = line.Split('=')[1].Trim();
                    balancedBox.SelectedItem = value;
                }
                else if (line.StartsWith("QualityRatioPerformance="))
                {
                    string value = line.Split('=')[1].Trim();
                    perfBox.SelectedItem = value;
                }
                else if (line.StartsWith("QualityRatioUltraPerformance="))
                {
                    string value = line.Split('=')[1].Trim();
                    uPerfBox.SelectedItem = value;
                }
                else if (line.StartsWith("DisableReactiveMask="))
                {
                    string value = line.Split('=')[1].Trim();
                    drmBox.SelectedItem = value;
                }
                else if (line.StartsWith("ColorResourceBarrier="))
                {
                    string value = line.Split('=')[1].Trim();
                    crbBox.SelectedItem = value;
                }
                else if (line.StartsWith("MotionVectorResourceBarrier="))
                {
                    string value = line.Split('=')[1].Trim();
                    mvrbBox.SelectedItem = value;
                }
                else if (line.StartsWith("DepthResourceBarrier="))
                {
                    string value = line.Split('=')[1].Trim();
                    drbBox.SelectedItem = value;
                }
                else if (line.StartsWith("ColorMaskResourceBarrier="))
                {
                    string value = line.Split('=')[1].Trim();
                    cmrbBox.SelectedItem = value;
                }
                else if (line.StartsWith("ExposureResourceBarrier="))
                {
                    string value = line.Split('=')[1].Trim();
                    erbBox.SelectedItem = value;
                }
                else if (line.StartsWith("OutputResourceBarrier="))
                {
                    string value = line.Split('=')[1].Trim();
                    orbBox.SelectedItem = value;
                }
            }
        }


        private void saveButton_Click(object sender, EventArgs e)
        {

            string nvngxIniPath = this.Text.Replace("Customize DLSS - ", "") + "/nvngx.ini";

            string selectedGenerator = generatorBox.SelectedItem.ToString();
            string selectedDx12Upscaler = dx12Box.SelectedItem.ToString();
            string selectedDx11Upscaler = dx11Box.SelectedItem.ToString();
            string selectedVulkanUpscaler = vkBox.SelectedItem.ToString();
            string selectedUseSafeSyncQueries = ussqBox.SelectedItem.ToString();
            string selectedBuildPipelines = buildPipeBox.SelectedItem.ToString();
            string selectedVerticalFov = vFovBox.SelectedItem.ToString();
            string selectedHorizontalFov = hFovBox.SelectedItem.ToString();
            string selectedOverrideSharpness = overrideBox.SelectedItem.ToString();
            string selectedSharpness = sharpValueBox.SelectedItem.ToString();
            string selectedEnabled = enabCasBox.SelectedItem.ToString();
            string selectedColorSpaceConversion = cscCasBox.SelectedItem.ToString();
            string selectedLoggingEnabled = logeBox.SelectedItem.ToString();
            string selectedLogLevel = loglvlBox.SelectedItem.ToString();
            string selectedLogToConsole = logtcBox.SelectedItem.ToString();
            string selectedLogToFile = logtfBox.SelectedItem.ToString();
            string selectedLogToNVSDK = logtnvBox.SelectedItem.ToString();
            string selectedOpenConsole = opconsBox.SelectedItem.ToString();
            string selectedDepthInverted = invertedBox.SelectedItem.ToString();
            string selectedAutoExposure = autoExBox.SelectedItem.ToString();
            string selectedHDR = hdrBox.SelectedItem.ToString();
            string selectedJitterCancellation = jitterCancBox.SelectedItem.ToString();
            string selectedDisplayResolution = displayResBox.SelectedItem.ToString();
            string selectedUpscaleRatioOverrideEnabled = ratioOvBox.SelectedItem.ToString();
            string selectedUpscaleRatioOverrideValue = ratioOvValBox.SelectedItem.ToString();
            string selectedQualityRatioOverrideEnabled = qOverrideBox.SelectedItem.ToString();
            string selectedQualityRatioUltraQuality = ultraQBox.SelectedItem.ToString();
            string selectedQualityRatioQuality = qualityBox.SelectedItem.ToString();
            string selectedQualityRatioBalanced = balancedBox.SelectedItem.ToString();
            string selectedQualityRatioPerformance = perfBox.SelectedItem.ToString();
            string selectedQualityRatioUltraPerformance = uPerfBox.SelectedItem.ToString();
            string selectedDisableReactiveMask = drmBox.SelectedItem.ToString();
            string selectedColorResourceBarrier = crbBox.SelectedItem.ToString();
            string selectedMotionVectorResourceBarrier = mvrbBox.SelectedItem.ToString();
            string selectedDepthResourceBarrier = drbBox.SelectedItem.ToString();
            string selectedColorMaskResourceBarrier = cmrbBox.SelectedItem.ToString();
            string selectedExposureResourceBarrier = erbBox.SelectedItem.ToString();
            string selectedOutputResourceBarrier = orbBox.SelectedItem.ToString();

            // Check for each combobox
            string[] lines = File.ReadAllLines(nvngxIniPath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("Generator="))
                {
                    lines[i] = "Generator=" + selectedGenerator;
                }
                else if (lines[i].StartsWith("Dx12Upscaler="))
                {
                    lines[i] = "Dx12Upscaler=" + selectedDx12Upscaler;
                }
                else if (lines[i].StartsWith("Dx11Upscaler="))
                {
                    lines[i] = "Dx11Upscaler=" + selectedDx11Upscaler;
                }
                else if (lines[i].StartsWith("VulkanUpscaler="))
                {
                    lines[i] = "VulkanUpscaler=" + selectedVulkanUpscaler;
                }
                else if (lines[i].StartsWith("UseSafeSyncQueries="))
                {
                    lines[i] = "UseSafeSyncQueries=" + selectedUseSafeSyncQueries;
                }
                else if (lines[i].StartsWith("BuildPipelines="))
                {
                    lines[i] = "BuildPipelines=" + selectedBuildPipelines;
                }
                else if (lines[i].StartsWith("VerticalFov="))
                {
                    lines[i] = "VerticalFov=" + selectedVerticalFov;
                }
                else if (lines[i].StartsWith("HorizontalFov="))
                {
                    lines[i] = "HorizontalFov=" + selectedHorizontalFov;
                }
                else if (lines[i].StartsWith("OverrideSharpness="))
                {
                    lines[i] = "OverrideSharpness=" + selectedOverrideSharpness;
                }
                else if (lines[i].StartsWith("Sharpness="))
                {
                    lines[i] = "Sharpness=" + selectedSharpness;
                }
                else if (lines[i].StartsWith("Enabled="))
                {
                    lines[i] = "Enabled=" + selectedEnabled;
                }
                else if (lines[i].StartsWith("ColorSpaceConversion="))
                {
                    lines[i] = "ColorSpaceConversion=" + selectedColorSpaceConversion;
                }
                else if (lines[i].StartsWith("LoggingEnabled="))
                {
                    lines[i] = "LoggingEnabled=" + selectedLoggingEnabled;
                }
                else if (lines[i].StartsWith("LogLevel="))
                {
                    lines[i] = "LogLevel=" + selectedLogLevel;
                }
                else if (lines[i].StartsWith("LogToConsole="))
                {
                    lines[i] = "LogToConsole=" + selectedLogToConsole;
                }
                else if (lines[i].StartsWith("LogToFile="))
                {
                    lines[i] = "LogToFile=" + selectedLogToFile;
                }
                else if (lines[i].StartsWith("LogToNVSDK="))
                {
                    lines[i] = "LogToNVSDK=" + selectedLogToNVSDK;
                }
                else if (lines[i].StartsWith("OpenConsole="))
                {
                    lines[i] = "OpenConsole=" + selectedOpenConsole;
                }
                else if (lines[i].StartsWith("DepthInverted="))
                {
                    lines[i] = "DepthInverted=" + selectedDepthInverted;
                }
                else if (lines[i].StartsWith("AutoExposure="))
                {
                    lines[i] = "AutoExposure=" + selectedAutoExposure;
                }
                else if (lines[i].StartsWith("HDR="))
                {
                    lines[i] = "HDR=" + selectedHDR;
                }
                else if (lines[i].StartsWith("JitterCancellation="))
                {
                    lines[i] = "JitterCancellation=" + selectedJitterCancellation;
                }
                else if (lines[i].StartsWith("DisplayResolution="))
                {
                    lines[i] = "DisplayResolution=" + selectedDisplayResolution;
                }
                else if (lines[i].StartsWith("UpscaleRatioOverrideEnabled="))
                {
                    lines[i] = "UpscaleRatioOverrideEnabled=" + selectedUpscaleRatioOverrideEnabled;
                }
                else if (lines[i].StartsWith("UpscaleRatioOverrideValue="))
                {
                    lines[i] = "UpscaleRatioOverrideValue=" + selectedUpscaleRatioOverrideValue;
                }
                else if (lines[i].StartsWith("QualityRatioOverrideEnabled="))
                {
                    lines[i] = "QualityRatioOverrideEnabled=" + selectedQualityRatioOverrideEnabled;
                }
                else if (lines[i].StartsWith("QualityRatioUltraQuality="))
                {
                    lines[i] = "QualityRatioUltraQuality=" + selectedQualityRatioUltraQuality;
                }
                else if (lines[i].StartsWith("QualityRatioQuality="))
                {
                    lines[i] = "QualityRatioQuality=" + selectedQualityRatioQuality;
                }
                else if (lines[i].StartsWith("QualityRatioBalanced="))
                {
                    lines[i] = "QualityRatioBalanced=" + selectedQualityRatioBalanced;
                }
                else if (lines[i].StartsWith("QualityRatioPerformance="))
                {
                    lines[i] = "QualityRatioPerformance=" + selectedQualityRatioPerformance;
                }
                else if (lines[i].StartsWith("QualityRatioUltraPerformance="))
                {
                    lines[i] = "QualityRatioUltraPerformance=" + selectedQualityRatioUltraPerformance;
                }
                else if (lines[i].StartsWith("DisableReactiveMask="))
                {
                    lines[i] = "DisableReactiveMask=" + selectedDisableReactiveMask;
                }
                else if (lines[i].StartsWith("ColorResourceBarrier="))
                {
                    lines[i] = "ColorResourceBarrier=" + selectedColorResourceBarrier;
                }
                else if (lines[i].StartsWith("MotionVectorResourceBarrier="))
                {
                    lines[i] = "MotionVectorResourceBarrier=" + selectedMotionVectorResourceBarrier;
                }
                else if (lines[i].StartsWith("DepthResourceBarrier="))
                {
                    lines[i] = "DepthResourceBarrier=" + selectedDepthResourceBarrier;
                }
                else if (lines[i].StartsWith("ColorMaskResourceBarrier="))
                {
                    lines[i] = "ColorMaskResourceBarrier=" + selectedColorMaskResourceBarrier;
                }
                else if (lines[i].StartsWith("ExposureResourceBarrier="))
                {
                    lines[i] = "ExposureResourceBarrier=" + selectedExposureResourceBarrier;
                }
                else if (lines[i].StartsWith("OutputResourceBarrier="))
                {
                    lines[i] = "OutputResourceBarrier=" + selectedOutputResourceBarrier;
                }
            }
            // Write changes to file
            File.WriteAllLines(nvngxIniPath, lines);
            MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void resetButton_Click(object sender, EventArgs e)
        {
            // Reset all the boxes to default values
            resetSettings();
        }

        private void settingsFile_Click(object sender, EventArgs e)
        {
            string nvngxIniPath = this.Text.Replace("Customize DLSS - ", "") + "/nvngx.ini";
            Process.Start(nvngxIniPath);
        }
    }
}
