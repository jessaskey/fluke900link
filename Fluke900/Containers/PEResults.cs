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
            Results.Clear();
            //parse the array.
            byte[] headerBytes = bytes.SubArray((byte)CommandCharacters.Substitute);
            if (headerBytes.Length > 0)
            {
                string[] headerParts = Encoding.ASCII.GetString(headerBytes).Replace("\u0002","").Split(new char[1] { ' ' },StringSplitOptions.RemoveEmptyEntries);
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
                                            int currentIndex = headerBytes.Length+1;
                                            while (bytes[currentIndex] != (byte)CommandCharacters.Substitute)
                                            {
                                                PEResult result = new PEResult();
                                                byte[] faultMaskBytes = bytes.SubArray((byte)' ',currentIndex);
                                                result.FaultMask = int.Parse(Encoding.ASCII.GetString(faultMaskBytes));
                                                currentIndex += faultMaskBytes.Length+1;
                                                byte[] thresholdBytes = bytes.SubArray((byte)' ', currentIndex);
                                                result.Threshold = int.Parse(Encoding.ASCII.GetString(thresholdBytes));
                                                currentIndex += thresholdBytes.Length+1;
                                                byte[] unknownBytes1 = bytes.SubArray((byte)CommandCharacters.Substitute, currentIndex);
                                                //result.Threshold = int.Parse(Encoding.ASCII.GetString(thresholdBytes));
                                                currentIndex += unknownBytes1.Length+1;
                                                byte[] unknownBytes2 = bytes.SubArray((byte)CommandCharacters.Substitute, currentIndex);
                                                //result.Threshold = int.Parse(Encoding.ASCII.GetString(thresholdBytes));
                                                currentIndex += unknownBytes2.Length+1;
                                                result.PassFail = ((char)bytes[currentIndex]) == 'P';
                                                currentIndex += 1;
                                                //see what terminator is
                                                Results.Add(result);
                                                if (bytes[currentIndex] != (byte)'\r')
                                                {
                                                    break;
                                                }
                                                currentIndex += 1;
                                            }
                                            //now get suggessted pass setting
                                            currentIndex += 1;
                                            if (Encoding.ASCII.GetString(bytes.Skip(currentIndex).Take(7).ToArray()) == "RESULT:")
                                            {
                                                byte[] suggestionBytes = bytes.SubArray((byte)'\r', currentIndex + 7);
                                                string[] suggestions = Encoding.ASCII.GetString(suggestionBytes).Split(' ');
                                                if (suggestions.Length == 2)
                                                {
                                                    SuggestedFaultMask = int.Parse(suggestions[0]);
                                                    SuggestedThreshold = int.Parse(suggestions[1]);
                                                }
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
