using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VoxCharger
{
    public partial class VoxChart
    {
        private static readonly Encoding DefaultEncoding = Encoding.GetEncoding("shift_jis");

        private const    string   VolCodes = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmno";
        private readonly string[] Filters  = new string[] { "peak", "lpf1", "lpf1", "hpf1", "hpf1", "lbic", "nof" };

        public int Version            { get; private set; } = 10;

        public string Lyric           { get; private set; }

        public EventCollection Events { get; private set; } = new EventCollection();

        public List<Effect> Effects   { get; private set; } = new List<Effect>();

        public Time EndPosition       { get; private set; }

        public VoxChart()
        {
        }

        public void Import(Ksh chart)
        {
            // Ksh events is already adjusted to be compatible with vox upon parsing
            Events = chart.Events;
        }

        public void Parse(string fileName)
        {
            var current = Section.NO_STATE;
            var lines   = File.ReadAllLines(fileName, DefaultEncoding);

            Event.Stop stop = null;
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim().Trim('\t');
                if (string.IsNullOrEmpty(line) || line == "//")
                    continue;

                // Identify current section
                if (line.StartsWith("#"))
                {
                    if (current == Section.NO_STATE)
                    {
                        var candidate = Section.NO_STATE;
                        string header = line.Substring(1).Trim().Replace(' ', '_');
                        foreach (Section section in Enum.GetValues(typeof(Section)))
                        {
                            // e.g there's BPM and BPM_INFO, make sure simple BPM isn't registered as BPM_INFO
                            string name = Enum.GetName(typeof(Section), section);
                            if (header == name)
                            {
                                current = section;
                                break;
                            }
                            else if (header.StartsWith(name))
                                candidate = section; 
                        }

                        if (current == Section.NO_STATE)
                            current = candidate;
                    }
                    else
                    {
                        if (line == "#END")
                            current = Section.NO_STATE;
                    }

                    continue;
                }

                #region --- TIME ---
                /**
                 * Legends:
                 * mmm = measure
                 * bbb = beat
                 * ccc = cell
                 */ 
                var data = line.Trim().Split('\t').Select(p => p.Trim()).ToArray();
                var time = Time.FromString(data[0]);
                #endregion

                #region --- FORMAT VERSION ---
                if (current == Section.FORMAT_VERSION)
                {
                    /*
                     * Format Version
                     * Parse version of Vox
                     */

                    if (int.TryParse(line, out int ver))
                        Version = ver;
                    else
                        Debug.WriteLine($"[FORMAT] {i+1:D4}: Invalid version format");
                }
                #endregion
                #region --- BEAT INFO ---
                else if (current == Section.BEAT_INFO)
                {
                    /*
                     * Beat Info (a.k.a Signature)
                     * Parse signature changes throughout the music, first signature change may referred as main signature
                     * 
                     * Format:
                     * mmm,bbb,ccc    b    n
                     * 
                     * * Arguments:
                     * b = beat
                     * n = note
                     * 
                     * Note:
                     * signature = beat/note (e.g: 1/4)
                     */

                    if (data.Length != 3 || time == null)
                    {
                        Debug.WriteLine($"[BEAT_INFO] {i+1:D4}: Invalid event format");
                        continue;
                    }

                    if (!int.TryParse(data[1], out int measure) || !int.TryParse(data[2], out int beat))
                    {
                        Debug.WriteLine($"[BEAT_INFO] {i+1:D4}: Invalid signature format");
                        continue;
                    }

                    if (time.Beat != 1 || time.Offset != 0)
                    {
                        Debug.WriteLine($"[BEAT_INFO] {i+1:D4}: Invalid Signature change position");
                        continue;
                    }

                    var signature = new Event.TimeSignature(time, measure, beat);
                    Events.Add(signature);
                }
                #endregion
                #region --- BPM (Simple) ---
                else if (current == Section.BPM)
                {
                    /*
                     * BPM (Simple)
                     * Parse intial BPM of music
                     * 
                     * Format:
                     * ttt.tt
                     * 
                     * Arguments:
                     * t = bpm
                     * 
                     * Note:
                     * - BPM time offset is always positioned at 1st measure and 1st beat (001,001,000)
                     * - BPM Format is {000.####}
                     */

                    // TODO: Process certain instructions
                    if (line.Contains("BAROFF"))
                    {
                        Debug.WriteLine($"[BPM_INFO] {i+1:D4}: \"BAROFF\" instruction skipped");
                        continue;
                    }

                    if (!float.TryParse(line, out float value))
                        Debug.WriteLine($"[BPM] {i:D4}: Invalid BPM value");
                    else
                        Events.Add(new Event.BPM(new Time(1, 1, 0), value)); 
                }
                #endregion
                #region --- BPM (Extended) ---
                else if (current == Section.BPM_INFO)
                {
                    /*
                     * BPM (Extended)
                     * Parse intial BPM of music
                     * 
                     * Format:
                     * mmm,bbb,ccc    ttt.tt    s(-)
                     * 
                     * Arguments:
                     * t = bpm
                     * s = signature
                     * 
                     * Note:
                     * - signature can be ended with minus sign (-) which indicate bpm stop 
                     * - stop event duration can be obtained by calculating difference of time offset of the next bpm event occurence 
                     * - signature most of the time is always 4
                     * - BPM Format is {000.####}
                     */

                    if (data.Length != 3 || time == null)
                    {
                        Debug.WriteLine($"[BPM_INFO] {i+1:D4}: Invalid event format");
                        continue;
                    }
                    
                    if (data[1] == "BAROFF")
                    {
                        Debug.WriteLine($"[BPM_INFO] {i+1:D4}: \"BAR\" instruction skipped"); // TODO: Process certain instructions
                        continue;
                    }

                    if (!float.TryParse(data[1], out float value))
                    {
                        Debug.WriteLine($"[BPM_INFO] {i+1:D4}: Invalid BPM value");
                        continue;
                    }

                    if (!int.TryParse(data[2].Replace("-", string.Empty), out int beat))
                        Debug.WriteLine($"[BPM_INFO] {i+1:D4}: Invalid beat division");

                    if (beat != 4)
                        Debug.WriteLine($"[BPM_INFO] {i+1:D4}: Non-4 beat division ({beat})");

                    // Handle stop event
                    if (data[2].EndsWith("-"))
                    {
                        if (stop != null)
                            Debug.WriteLine($"[BPM_INFO] {i+1:D4}: Duplicate stop event");

                        stop = new Event.Stop(time);
                    }
                    else
                    {
                        // Handle stop event if previously assigned
                        if (stop != null)
                        {
                            // Retrieve last signature for current measure
                            var signature = Events.GetTimeSignature(stop.Time);
                            if (signature == null)
                            {
                                Debug.WriteLine($"[BPM_INFO] {i+1:D4}: No valid signature for stop event");

                                stop = null;
                                continue;
                            }

                            stop.Duration = time.Difference(stop.Time, signature);
                            Events.Add(stop);

                            stop = null;
                        }
                    }

                    Events.Add(new Event.BPM(time, value));
                }
                #endregion
                #region --- TILT MODE INFO ---
                else if (current == Section.TILT)
                {
                    /*
                     * Tilt Mode
                     * Parse camera tilt event
                     * 
                     * Format:
                     * mmm,bbb,ccc    m
                     * 
                     * Arguments:
                     * m = mode
                     * 
                     * Note:
                     * Refer to TiltMode for value mapping
                     */

                    if (data.Length != 2 && time == null)
                    {
                        Debug.WriteLine($"[TILT_MODE_INFO] {i+1:D4}: Invalid event format");
                        continue;
                    }

                    if (!int.TryParse(data[1], out int tilt))
                    {
                        Debug.WriteLine($"[TILT_MODE_INFO] {i+1:D4}: Invalid tilt code format");
                        continue;
                    }

                    if (!Enum.IsDefined(typeof(Event.TiltType), tilt))
                    {
                        Debug.WriteLine($"[TILT_MODE_INFO] {i+1:D4}: Invalid tilt mode value");
                        continue;
                    }

                    Events.Add(new Event.TiltMode(time, (Event.TiltType)tilt));
                }
                #endregion
                #region --- LYRIC INFO ---
                else if (current == Section.LYRIC)
                {
                    /*
                    * Lyric Info
                    * Parse Lyric information
                    * 
                    * Format:
                    * ???
                    * 
                    * Note:
                    * Assumed has no format and lyric under plain text
                    */
                    if (!string.IsNullOrEmpty(line))
                        Lyric += line + '\n';
                }
                #endregion
                #region --- END POSITION ---
                else if (current == Section.END_POSITION)
                {
                    /*
                    * End Position
                    * Parse end position; the last measure of music
                    * 
                    * Format:
                    * mmm,bbb,ccc
                    */

                    if (time == null)
                        Debug.WriteLine($"[END_POSITION] {i+1:D4}: Invalid event format");
                    else
                        EndPosition = time;
                }
                #endregion
                #region --- TAB EFFECT INFO ---
                else if (current == Section.TAB_EFFECT)
                {
                    /*
                    * Tab Effect Info
                    * Parse Tab Effect info
                    * 
                    * Format:
                    * ?,    ???.??, ???.??, ??????.??,  ?.??
                    */

                    Debug.WriteLine($"[TAB_EFFECT] {i+1:D4}: {line}");
                }
                #endregion
                #region --- FXBUTTON EFFECT INFO ---
                else if (current == Section.FXBUTTON_EFFECT)
                {
                    /*
                    * FxButton Effect Info
                    * Parse FxButton Effect Mapping
                    * 
                    * Format:
                    * Depends on Fx, check implementation class for details
                    */

                    if (line.StartsWith("0"))
                        continue; // Skip padding

                    var fx = Effect.FromVox(line);
                    if (fx == null || fx.Type == FxType.None)
                        Debug.WriteLine($"[FXBUTTON_EFFECT] {i+1:D4}: Invalid FX data");
                    else
                    {
                        fx.Id = Effects.Count;
                        Effects.Add(fx);
                    }
                }
                #endregion
                #region --- TAB PARAM ASSIGN INFO ---
                else if (current == Section.TAB_PARAM)
                {
                    /*
                    * Tab Param Info
                    * Parse Tab Param Assign Info
                    * 
                    * Format:
                    * ?,    ?,  ?.??,   ?.??
                    */

                    Debug.WriteLine($"[TAB_PARAM] {i+1:D4}: {line}");
                }
                #endregion
                #region --- REVERB EFFECT PARAM ---
                else if (current == Section.REVERB)
                {
                    /*
                    * Reverb Effect
                    * Parse Reverb Effect
                    */

                    Debug.WriteLine($"[REVERB_EFFECT] {i+1:D4}: {line}");
                }
                #endregion
                #region --- SPCONTROLLER INFO ---
                else if (current == Section.SPCONTROLER)
                {
                    /*
                    * SP Controller Info
                    * Parse camera works
                    * 
                    * Format:
                    * Depends on type, refer to Camera
                    */

                    Debug.WriteLine($"[SPCONTROLLER_INFO] {i+1:D4}: {line}");

                }
                #endregion
                #region --- TRACK ---
                else if (IsTrackSection(current))
                {
                    int trackId = GetTrackNumber(current);
                    if (trackId <= 0)
                    {
                        Debug.WriteLine($"[TRACK] {i+1:D4}: Invalid track id");
                        continue;
                    }

                    // Laser Channel
                    if (trackId == (int)Event.LaserTrack.Left || trackId == (int)Event.LaserTrack.Right)
                    {
                        /*
                        * Track - Laser
                        * Parse Laser Info
                        * 
                        * Format:
                        * mmm,bbb,ccc   o   f   t   x   r   ?
                        * 
                        * Arguments:
                        * o = Offset
                        * f = Flag
                        * t = Tick Tempo
                        * x = Filter
                        * r = Range
                        * 
                        * Note:
                        * Slam defined as 2 laser events with same time and side, the offset of start laser must be less than offset of end laser
                        */

                        if (data.Length < 5 || time == null)
                        {
                            Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid event format");
                            continue;
                        }

                        if (!int.TryParse(data[1], out int offset))
                        {
                            Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid laser offset");
                            continue;
                        }

                        if (!Enum.TryParse(data[2], out Event.LaserFlag flag))
                        {
                            Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid laser flag");
                            continue;
                        }

                        var impact = Event.SlamImpact.None;
                        if (data.Length > 5)
                        {
                            if (!Enum.TryParse(data[3], out impact))
                                Debug.WriteLine($"[TRACK_{trackId}] {i + 1:D4}: Invalid laser tick tempo");
                        }

                        var laser = new Event.Laser(time, (Event.LaserTrack)trackId, offset, flag, impact);
                        string filterStr = data.Length > 5 ? data[4] : data[3];
                        string rangeStr  = data.Length > 5 ? data[5] : data[4];

                        if (int.TryParse(filterStr, out int filterId))
                        {
                            switch (filterId)
                            {
                                case 1:
                                case 2:  laser.Filter = Event.LaserFilter.LowPass;    break;
                                case 3:
                                case 4:  laser.Filter = Event.LaserFilter.HighPass;   break;
                                case 5:  laser.Filter = Event.LaserFilter.BitCrusher; break;
                                default: laser.Filter = Event.LaserFilter.Peak;       break;
                            }
                        }
                        else
                            Debug.WriteLine($"[TRACK_{trackId}] {i + 1:D4}: Invalid laser filter");

                        if (!int.TryParse(rangeStr, out int range))
                            Debug.WriteLine($"[TRACK_{trackId}] {i + 1:D4}: Invalid laser range");
                        else
                            laser.Range = range;

                        // Locate slam
                        var slam = Events[time].FirstOrDefault(ev => 
                            (ev is Event.Laser l && l.Track == laser.Track) ||
                            (ev is Event.Slam  s && s.Track == laser.Track)
                        );

                        if (slam != null)
                        {
                            // Duplicate slam, handle gracefully
                            if (slam is Event.Slam s)
                                slam = s.End;

                            var start = slam as Event.Laser;
                            var end   = laser;

                            if (start.Offset == end.Offset)
                            {
                                Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid slam offset");
                                continue;
                            }

                            if (start.Track != end.Track)
                            {
                                Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid slam track");
                                continue;
                            }

                            Events.Add(new Event.Slam(time, start, end));
                        }
                        else
                            Events.Add(laser);
                    }
                    else
                    {
                        var track = (Event.ButtonTrack)trackId;
                        if (!int.TryParse(data[1], out int holdLength))
                            Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid chip hold length");

                        var chip = new Event.Button(time, track, holdLength, null);
                        if (chip.IsFx)
                        {
                            // Hold Fx
                            if (holdLength > 0)
                            {
                                if (int.TryParse(data[2], out int fxIndex) && fxIndex - 2 < Effects.Count)
                                    chip.HoldFx = Effects[fxIndex - 2];
                                else
                                    Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid chip hold effect index");
                            }
                            else
                            {
                                // Clap, snare, etc
                                if (Enum.TryParse(data[2], out Event.ChipFx hitFx))
                                    chip.HitFx = hitFx;
                                else
                                    Debug.WriteLine($"[TRACK_{trackId}] {i+1:D4}: Invalid chip hit effect value");
                            }
                        }
                        
                        Events.Add(chip);
                    }
                }
                #endregion
            }
        }

        public void Serialize(string filename)
        {
            string result = string.Empty;
            result += "//====================================\n";
            result += "// SOUND VOLTEX OUTPUT TEXT FILE\n";
            result += "//====================================\n\n";

            result += "#FORMAT VERSION\n";
            result += $"{Version}\n";
            result += "#END\n\n";

            result += "#BEAT INFO\n";
            foreach (var ev in Events)
            {
                if (ev is Event.TimeSignature signature)
                    result += signature.ToString() + "\n";
            }
            result += "#END\n\n";

            result += "#BPM INFO\n";
            Event.BPM lastBpm = null;
            foreach (var ev in Events)
            {
                if (ev is Event.BPM bpm)
                {
                    lastBpm = bpm;
                    result += bpm.ToString() + "\n";
                }
            }
            result += "#END\n\n";

            result += "#TILT MODE INFO\n";
            foreach (var ev in Events)
            {
                if (ev is Event.TiltMode tilt)
                    result += tilt.ToString() + "\n";
            }
            result += "#END\n\n";

            result += "#LYRIC INFO\n";
            result += Lyric;
            result += "#END\n\n";

            result += "#END POSITION\n";
            if (EndPosition == null)
                EndPosition = new Time(Events.Max(ev => ev.Time.Measure) + 1, 1, 0);
            result += EndPosition + "\n";
            result += "#END\n\n";

            result += "#TAB EFFECT INFO\n";
            result += "1,\t90.00,\t400.00,\t18000.00,\t0.70\n";
            result += "1,\t90.00,\t600.00,\t15000.00,\t5.00\n";
            result += "2,\t50.00,\t40.00,\t5000.00,\t0.70\n";
            result += "2,\t90.00,\t40.00,\t2000.00,\t3.00\n";
            result += "3,\t100.00,\t30\n";
            result += "#END\n\n";


            var fxInfo = new List<Effect>();
            var padding = new Effect();
            foreach (var ev in Events)
            {
                if (ev is Event.Button bt && bt.HoldFx != null && fxInfo.Find(f => f.Id == bt.HoldFx.Id) == null)
                    fxInfo.Add(bt.HoldFx);
            }

            result += "#FXBUTTON EFFECT INFO\n";
            fxInfo.Sort((a, b) => a.Id.CompareTo(b.Id));
            foreach (var fx in fxInfo)
            {
                result += fx.ToString() + "\n";
                result += padding.ToString() + "\n\n";
            }
            result += "#END\n\n";

            result += "#TAB PARAM ASSIGN INFO\n";
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 2; j++)
                    result += $"{i},\t0,\t0.00,\t0.00\n";
            }
            result += "#END\n\n";

            result += "#REVERB EFFECT PARAM\n";
            result += "#END\n\n";

            result += "//====================================\n";
            result += "// TRACK INFO\n";
            result += "//====================================\n\n";

            for (int trackId = 1; trackId <= 8; trackId++)
            {
                result += $"#TRACK{trackId}\n";
                if (trackId == (int)Event.LaserTrack.Left || trackId == (int)Event.LaserTrack.Right)
                {
                    var track = (Event.LaserTrack)trackId;
                    foreach (var ev in Events)
                    {
                        if (ev is Event.Laser laser && laser.Track == track)
                            result += laser.ToString() + "\n";
                    }
                }
                else
                {
                    var track = (Event.ButtonTrack)trackId;
                    foreach (var ev in Events)
                    {
                        if (ev is Event.Button button && button.Track == track)
                            result += button.ToString() + "\n";
                    }
                }


                result += "#END\n\n";
                result += "//====================================\n\n";
            }

            result += "#TRACK AUTO TAB\n";
            result += "#END\n\n";
            result += "//====================================\n\n";

            result += "#SPCONTROLER\n";
            result += "001,01,00\tRealize\t3\t0\t36.12\t60.12\t110.12\t0.00\n";
            result += "001,01,00\tRealize\t4\t0\t0.62\t0.72\t1.03\t0.00\n";
            result += "001,01,00\tAIRL_ScaX\t1\t0\t0.00\t1.00\t0.00\t0.00\n";
            result += "001,01,00\tAIRR_ScaX\t1\t0\t0.00\t2.00\t0.00\t0.00\n";
            foreach (Camera.WorkType work in Enum.GetValues(typeof(Camera.WorkType)))
            {
                foreach (var ev in Events)
                {
                    if (ev is Camera camera && camera.Work == work)
                        result += camera.ToString() + "\n";
                }
            }
            
            result += "#END\n\n";
            result += "//====================================\n\n";

            File.WriteAllText(filename, result, DefaultEncoding);
        }
    }
}
