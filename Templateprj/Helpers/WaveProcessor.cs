using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Templateprj.Helpers
{
    using System;
    using System.Text;
    using System.IO;
    using System.Runtime.InteropServices;
    using NAudio.Wave;


    /// <summary>
    /// Summary description for WaveProcessor
    /// </summary>
    public class WaveProcessor
    {
        // Constants for default or base format [16bit 8kHz Mono]
        private const short CHNL = 1;
        private const int SMPL_RATE = 8000;
        private const int BIT_PER_SMPL = 16;
        private const short FILTER_FREQ_LOW = -10795;
        private const short FILTER_FREQ_HIGH = 10795;

        // Public Fields can be used for various operations
        public int Length;
        public short Channels;
        public int SampleRate;
        public int DataLength;
        public short BitsPerSample;
        public ushort MaxAudioLevel;

        private bool WaveHeaderIN(string strPath)
        {
            if (strPath == null) strPath = "";
            if (strPath == "") return false;

            FileStream fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);

            BinaryReader br = new BinaryReader(fs);
            try
            {
                Length = (int)fs.Length - 8;
                fs.Position = 22;
                Channels = br.ReadInt16(); //1
                fs.Position = 24;
                SampleRate = br.ReadInt32(); //8000
                fs.Position = 34;
                BitsPerSample = br.ReadInt16(); //16
                DataLength = (int)fs.Length - 44;
                byte[] arrfile = new byte[fs.Length - 44];
                fs.Position = 44;
                fs.Read(arrfile, 0, arrfile.Length);
            }
            catch (Exception)
            {

                return false;
            }
            finally
            {
                br.Close();
                fs.Close();
            }
            return true;
        }
        public int wavformatcheck(byte[] byteArray)
        {

            MemoryStream stream = new MemoryStream(byteArray);
            WaveFileReader wf = new WaveFileReader(stream);
            int channel = wf.WaveFormat.Channels;
            int Samplebit = wf.WaveFormat.BitsPerSample;
            int samplerate = wf.WaveFormat.SampleRate;
            if (channel == 1 && Samplebit == 8 && samplerate == 8000)
                return 1;
            else
                return 0;

        }

        private bool WaveHeaderOUT(string strPath)
        {
            if (strPath == null) strPath = "";
            if (strPath == "") return false;

            FileStream fs = new FileStream(strPath, FileMode.Create, FileAccess.Write);

            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                fs.Position = 0;
                bw.Write(new char[4] { 'R', 'I', 'F', 'F' });

                bw.Write(Length);

                bw.Write(new char[8] { 'W', 'A', 'V', 'E', 'f', 'm', 't', ' ' });

                bw.Write((int)16);

                bw.Write((short)1);
                bw.Write(Channels);

                bw.Write(SampleRate);

                bw.Write((int)(SampleRate * ((BitsPerSample * Channels) / 8)));

                bw.Write((short)((BitsPerSample * Channels) / 8));

                bw.Write(BitsPerSample);

                bw.Write(new char[4] { 'd', 'a', 't', 'a' });
                bw.Write(DataLength);
            }
            catch (Exception )
            {

                return false;
            }
            finally
            {
                bw.Close();
                fs.Close();
            }
            return true;
        }

        /// <summary>
        /// Ensure any given wave file path that the file matches with default or base format [16bit 8kHz Mono]
        /// </summary>
        /// <param name="strPath">Wave file path</param>
        /// <returns>True/False</returns>
        public bool Validate(string strPath)
        {
            if (strPath == null) strPath = "";
            if (strPath == "") return false;

            WaveProcessor wa_val = new WaveProcessor();
            wa_val.WaveHeaderIN(strPath);
            //  if (wa_val.BitsPerSample != BIT_PER_SMPL) return false;
            if (wa_val.Channels != CHNL) return false;
            if (wa_val.SampleRate != SMPL_RATE) return false;
            return true;
        }

        /// <summary>
        /// Compare two wave files to ensure both are in same format
        /// </summary>
        /// <param name="Wave1">ref. to processor object</param>
        /// <param name="Wave2">ref. to processor object</param>
        /// <returns>True/False</returns>
        private bool Compare(ref WaveProcessor Wave1, ref WaveProcessor Wave2)
        {
            if (Wave1.Channels != Wave2.Channels) return false;
            if (Wave1.BitsPerSample != Wave2.BitsPerSample) return false;
            if (Wave1.SampleRate != Wave2.SampleRate) return false;
            return true;
        }

        /// <summary>
        /// Increase or decrease volume of a wave file by percentage
        /// </summary>
        /// <param name="strPath">Source wave</param>
        /// <param name="booIncrease">True - Increase, False - Decrease</param>
        /// <param name="shtPcnt">1-100 in %-age</param>
        /// <returns>True/False</returns>
        public bool ChangeVolume(string strPath, bool booIncrease, short shtPcnt)
        {
            if (strPath == null) strPath = "";
            if (strPath == "") return false;
            if (shtPcnt > 100) return false;

            WaveProcessor wain = new WaveProcessor();
            WaveProcessor waout = new WaveProcessor();

            waout.DataLength = waout.Length = 0;

            if (!wain.WaveHeaderIN(@strPath)) return false;

            waout.DataLength = wain.DataLength;
            waout.Length = wain.Length;

            waout.BitsPerSample = wain.BitsPerSample;
            waout.Channels = wain.Channels;
            waout.SampleRate = wain.SampleRate;

            byte[] arrfile = GetWAVEData(strPath);


            //change volume
            for (int j = 0; j < arrfile.Length; j += 2)
            {
                short snd = ComplementToSigned(ref arrfile, j);
                try
                {
                    short p = Convert.ToInt16((snd * shtPcnt) / 100);
                    if (booIncrease)
                        snd += p;
                    else
                        snd -= p;
                }
                catch
                {
                    snd = ComplementToSigned(ref arrfile, j);
                }
                byte[] newval = SignedToComplement(snd);

                if (newval[0]!= null && newval[1]!= null)
                {
                    arrfile[j] = newval[0];
                    arrfile[j + 1] = newval[1];
                }

            }

            //write back to the file
            waout.DataLength = arrfile.Length;
            waout.WaveHeaderOUT(@strPath);
            WriteWAVEData(strPath, ref arrfile);

            return true;
        }

        /// <summary>
        /// Mix two wave files. The mixed data will be written back to the main wave file.
        /// </summary>
        /// <param name="strPath">Path for source or main wave file.</param>
        /// <param name="strMixPath">Path for wave file to be mixed with source.</param>
        /// <returns>True/False</returns>
        public bool WaveMix(string strPath, string strMixPath)
        {
            if (strPath == null) strPath = "";
            if (strPath == "") return false;

            if (strMixPath == null) strMixPath = "";
            if (strMixPath == "") return false;

            WaveProcessor wain = new WaveProcessor();
            WaveProcessor wamix = new WaveProcessor();
            WaveProcessor waout = new WaveProcessor();

            wain.DataLength = wamix.Length = 0;

            if (!wain.WaveHeaderIN(strPath)) return false;
            if (!wamix.WaveHeaderIN(strMixPath)) return false;

            waout.DataLength = wain.DataLength;
            waout.Length = wain.Length;

            waout.BitsPerSample = wain.BitsPerSample;
            waout.Channels = wain.Channels;
            waout.SampleRate = wain.SampleRate;

            byte[] arrfile = GetWAVEData(strPath);
            byte[] arrmix = GetWAVEData(strMixPath);

            for (int j = 0, k = 0; j < arrfile.Length; j += 2, k += 2)
            {
                if (k >= arrmix.Length) k = 0;
                short snd1 = ComplementToSigned(ref arrfile, j);
                short snd2 = ComplementToSigned(ref arrmix, k);
                short o = 0;
                // ensure the value is within range of signed short
                if ((snd1 + snd2) >= -32768 && (snd1 + snd2) <= 32767)
                    o = Convert.ToInt16(snd1 + snd2);
                byte[] b = SignedToComplement(o);
                arrfile[j] = b[0];
                arrfile[j + 1] = b[1];
            }

            //write mixed file
            waout.WaveHeaderOUT(@strPath);
            WriteWAVEData(strPath, ref arrfile);

            return true;
        }

        public bool StripSilence(string strPath)
        {
            if (strPath == null) strPath = "";
            if (strPath == "") return false;

            WaveProcessor wain = new WaveProcessor();
            WaveProcessor waout = new WaveProcessor();

            waout.DataLength = waout.Length = 0;

            if (!wain.WaveHeaderIN(@strPath)) return false;

            waout.DataLength = wain.DataLength;
            waout.Length = wain.Length;

            waout.BitsPerSample = wain.BitsPerSample;
            waout.Channels = wain.Channels;
            waout.SampleRate = wain.SampleRate;

            byte[] arrfile = GetWAVEData(strPath);

            //check for silence
            int startpos = 0;
            int endpos = arrfile.Length - 1;
            //At start
            try
            {
                for (int j = 0; j < arrfile.Length; j += 2)
                {
                    short snd = ComplementToSigned(ref arrfile, j);
                    if (snd > FILTER_FREQ_LOW && snd < FILTER_FREQ_HIGH) startpos = j;
                    //else
                    //    break;
                }
            }
            catch (Exception )
            {


            }
            //At end
            for (int k = arrfile.Length - 1; k >= 0; k -= 2)
            {
                short snd = ComplementToSigned(ref arrfile, k - 1);
                if (snd > FILTER_FREQ_LOW && snd < FILTER_FREQ_HIGH) endpos = k;
                //else
                //    break;
            }

            if (startpos == endpos) return false;
            if ((endpos - startpos) < 1) return false;

            byte[] newarr = new byte[(endpos - startpos) + 1];

            for (int ni = 0, m = startpos; m <= endpos; m++, ni++)
                newarr[ni] = arrfile[m];

            //write file
            waout.DataLength = newarr.Length;
            waout.WaveHeaderOUT(@strPath);
            WriteWAVEData(strPath, ref newarr);

            return true;
        }


        /// </summary>
        /// <param name="StartFile">Wave file to stay in the begining</param>
        /// <param name="EndFile">Wave file to stay at the end</param>
        /// <param name="OutFile">Merged output to wave file</param>
        /// <returns>True/False</returns>
        public bool Merge(string strStartFile, string strEndFile, string strOutFile)
        {
            if ((strStartFile == strEndFile) && (strStartFile == null)) return false;
            if ((strStartFile == strEndFile) && (strStartFile == "")) return false;
            if ((strStartFile == strOutFile) || (strEndFile == strOutFile)) return false;

            WaveProcessor wa_IN_Start = new WaveProcessor();
            WaveProcessor wa_IN_End = new WaveProcessor();
            WaveProcessor wa_out = new WaveProcessor();

            wa_out.DataLength = 0;
            wa_out.Length = 0;

            try
            {
                //Gather header data
                if (!wa_IN_Start.WaveHeaderIN(@strStartFile)) return false;
                if (!wa_IN_End.WaveHeaderIN(@strEndFile)) return false;
                if (!Compare(ref wa_IN_Start, ref wa_IN_End)) return false;

                wa_out.DataLength = wa_IN_Start.DataLength + wa_IN_End.DataLength;
                wa_out.Length = wa_IN_Start.Length + wa_IN_End.Length;

                //Recontruct new header
                wa_out.BitsPerSample = wa_IN_Start.BitsPerSample;
                wa_out.Channels = wa_IN_Start.Channels;
                wa_out.SampleRate = wa_IN_Start.SampleRate;
                wa_out.WaveHeaderOUT(@strOutFile);

                //Write data - modified code
                byte[] arrfileStart = GetWAVEData(strStartFile);
                byte[] arrfileEnd = GetWAVEData(strEndFile);
                int intLngthofStart = arrfileStart.Length;
                Array.Resize(ref arrfileStart, (arrfileStart.Length + arrfileEnd.Length));
                Array.Copy(arrfileEnd, 0, arrfileStart, intLngthofStart, arrfileEnd.Length);
                WriteWAVEData(strOutFile, ref arrfileStart);
            }
            catch (Exception ex)
            {

                throw ex;

            }
            return true;
        }

        /// <summary>
        /// In stereo wave format, samples are stored in 2's complement. For Mono, it's necessary to 
        /// convert those samples to their equivalent signed value. This method is used 
        /// by other public methods to equilibrate wave formats of different files.
        /// </summary>
        /// <param name="bytArr">Sample data in array</param>
        /// <param name="intPos">Array offset</param>
        /// <returns>Mono value as signed short</returns>
        private short ComplementToSigned(ref byte[] bytArr, int intPos) // 2's complement to normal signed value
        {
            short snd = BitConverter.ToInt16(bytArr, intPos);
            if (snd != 0)
                snd = Convert.ToInt16((~snd | 1));
            return snd;
        }
        /// <summary>
        /// Convert signed sample value back to 2's complement value equivalent to Stereo. This method is used 
        /// by other public methods to equilibrate wave formats of different files.
        /// </summary>
        /// <param name="shtVal">The mono signed value as short</param>
        /// <returns>Stereo 2's complement value as byte array</returns>
        private byte[] SignedToComplement(short shtVal) //Convert to 2's complement and return as byte array of 2 bytes
        {
            byte[] bt = new byte[2];
            shtVal = Convert.ToInt16((~shtVal | 1));
            bt = BitConverter.GetBytes(shtVal);
            return bt;
        }

        /// <summary>
        /// Read the WAVE file then position to DADA segment and return the chunk as byte array 
        /// </summary>
        /// <param name="strWAVEPath">Path of WAVE file</param>
        /// <returns>byte array</returns>
        public byte[] GetWAVEData(string strWAVEPath)
        {
            try
            {
                FileStream fs = new FileStream(@strWAVEPath, FileMode.Open, FileAccess.Read);
                byte[] arrfile = new byte[fs.Length - 44];
                fs.Position = 44;
                fs.Read(arrfile, 0, arrfile.Length);
                fs.Close();
                return arrfile;
            }
            catch (IOException ioex)
            {
                throw ioex;
            }
        }

        /// <summary>
        /// Write data chunk to the file. The header must be written before hand.
        /// </summary>
        /// <param name="strWAVEPath">Path of WAVE file</param>
        /// <param name="arrData">data in array</param>
        /// <returns>True</returns>
        private bool WriteWAVEData(string strWAVEPath, ref byte[] arrData)
        {
            try
            {
                FileStream fo = new FileStream(@strWAVEPath, FileMode.Append, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fo);
                bw.Write(arrData);
                bw.Close();
                fo.Close();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    } // End of WaveProcessor class

}