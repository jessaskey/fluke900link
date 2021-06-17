using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluke900.Containers
{
    public class PEResults
    {
        public List<PEResult> Results { get; set; } = new List<PEResult>();
        public int FaultMask { get; set; }
        public int FaultMaskStep { get; set; }
        public int FaultMaskStepCount { get; set; }
        public int Threshold { get; set; }
        public int ThresholdStep { get; set; }
        public int ThresholdStepCount { get; set; }

        public int SuggestedFaultMask { get; set; }
        public int SuggestedThreshold { get; set; }
      
        public bool HasError { get; set; }
        public string LastError { get; set; }

        public PEResults()
        {

        }

        public PEResults(byte[] bytes)
        {
            //parse the array.
            byte[] headerBytes = bytes.SubArray((byte)CommandCharacters.Substitute);
            if (headerBytes.Length > 0)
            {
                string[] headerParts = Encoding.ASCII.GetString(headerBytes).Split(' ');
                if (headerParts.Length == 6)
                {
                    int faultMask = 0;
                    int faultMaskStep = 0;
                    int faultMaskStepCount = 0;
                    int threshold = 0;
                    int thresholdStep = 0;
                    int thresholdStepCount = 0;
                    if (int.TryParse(headerParts[0],out faultMask))
                    {
                        if (int.TryParse(headerParts[1], out faultMaskStep))
                        {
                            if (int.TryParse(headerParts[2], out faultMaskStepCount))
                            {
                                if (int.TryParse(headerParts[3], out threshold))
                                {
                                    if (int.TryParse(headerParts[4], out thresholdStep))
                                    {
                                        if (int.TryParse(headerParts[5], out thresholdStepCount))
                                        {
                                            //whew, header is good... now iterate result sets.
                                            int currentIndex = headerBytes.Length + 1;
                                            while (bytes[currentIndex] != (byte)CommandCharacters.Substitute)
                                            {
                                                
                                            }
                                        }
                                        else
                                        {
                                            HasError = true;
                                            LastError = "Returned Threshold Step Count value was not numeric: " + headerParts[5];
                                        }
                                    }
                                    else
                                    {
                                        HasError = true;
                                        LastError = "Returned Threshold Step value was not numeric: " + headerParts[4];
                                    }
                                }
                                else
                                {
                                    HasError = true;
                                    LastError = "Returned Threshold value was not numeric: " + headerParts[3];
                                }
                            }
                            else
                            {
                                HasError = true;
                                LastError = "Returned Fault Mask Step Count value was not numeric: " + headerParts[2];
                            }
                        }
                        else
                        {
                            HasError = true;
                            LastError = "Returned Fault Mask Step value was not numeric: " + headerParts[1];
                        }
                    }
                    else
                    {
                        HasError = true;
                        LastError = "Returned Fault Mask value was not numeric: " + headerParts[0];
                    }
                }
                else
                {
                    HasError = true;
                    LastError = "Improperly formed response, header was not correct length. Expected 6, found " + headerParts.Length.ToString();
                }
            }
            else
            {
                HasError = true;
                LastError = "Improperly formed response, missing header substitute marker.";
            }
        }
    }
}
