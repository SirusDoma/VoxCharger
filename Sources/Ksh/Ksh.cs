using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace VoxCharger
{
    public partial class Ksh
    {
        private static readonly Encoding DefaultEncoding = Encoding.GetEncoding("Shift_JIS");
        private const string VolPositions = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmno";

        public string Title          { get; set; }
        public string Artist         { get; set; }
        public string Effector       { get; set; }
        public string Illustrator    { get; set; }
        public string JacketFileName { get; set; }
        public string MusicFileName  { get; set; }
        public int Volume            { get; set; }
        public Difficulty Difficulty { get; set; }
        public int Level             { get; set; }
        public int MusicOffset       { get; set; }
        public int PreviewOffset     { get; set; }
        public float BpmMin          { get; set; }
        public float BpmMax          { get; set; }
        public string Background     { get; set; }

        public EventCollection Events { get; private set; } = new EventCollection();

        public class ParseOption
        {
            public bool RealignOffset     { get; set; } = false;
            public bool EnableChipFx      { get; set; } = true;
            public bool EnableLongFx      { get; set; } = true;
            public bool EnableCamera      { get; set; } = true;
            public bool EnableSlamImpact  { get; set; } = true;
            public bool EnableLaserTrack  { get; set; } = true;
            public bool EnableButtonTrack { get; set; } = true;
        }

        public Ksh()
        {
        }

        public void Parse(string fileName, ParseOption opt = null)
        {
            // I pulled all nighters for few days, all for this piece of trash codes :)
            // Reminder: there's lot of "offset" that can be ambigous between many contexts everywhere
            var lines = File.ReadAllLines(fileName, DefaultEncoding);

            if (opt == null)
                opt = new ParseOption();

            Time time      = new Time(1, 1, 0);
            var signature  = new Event.TimeSignature(time, 4, 4);
            var filter     = Event.LaserFilter.Peak;
            int rangeLeft  = 0;
            int rangeRight = 0;
            var cameras    = new Dictionary<Camera.WorkType, Camera>();
            var lastCamera = new Dictionary<Camera.WorkType, Camera>();
            var hitFx      = new Dictionary<Event.ButtonTrack, KshSoundEffect>();
            var holdFx     = new Dictionary<Event.ButtonTrack, Effect>();
            var longNotes  = new Dictionary<Event.ButtonTrack, Event.Button>();
            var lasers     = new Dictionary<Event.LaserTrack, List<Event.Laser>>();

            int offset    = 0;
            int measure   = 0;

            int measureCount = lines.Count(l => l.Trim() == "--");
            int fxCount   = 0;
            int noteCount = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("//"))
                    continue;


                // First measure is 1,1,0 but initial counter start from 0 until actual measure started
                if (measure > 0)
                {
                    /** Time 
                     * Behold! time conversion between ksh format to vox
                     * 
                     * Vox Time Format:
                     * Timestamp value has beat to be matched with current signature
                     * However, the offset is base of 48, this might be lead into confusion in non standard signature
                     * 
                     * For example:
                     * In 8 / 4 signature the maximum timestamp that means:
                     * - Maximum timestamp is 999 / 8 / 48 for each measure, beat and cells respectively
                     * - Value of beat adapt to the signature beat
                     * - May only contain 4 notes per beat in normal behavior since signature note is 4
                     * 
                     * And since offset need to be base of 48 (not 192) this value need to be converted
                     * for example the 3rd note in the beat should be 36 and not 24
                     * 
                     * Ksh Time Format:
                     * This too, depends on beat but the number of actual event definitions can be much greater than the beat itself
                     * for example, measure that has 8/4 signature may contains 441 events, though much of them are just padding
                     * this padding required since the event didn't define timestamp in each line (easier to debug with larger file size tradeoff)
                     */

                    float position = (measure * 192f) + ((offset / (float)noteCount) * 192f);
                    if (opt.RealignOffset)
                        position -= MusicOffset; // Attempt to align

                    time = Time.FromOffset(position, signature);
                    //int p = time.AsAbsoluteOffset(signature);
                    //if ((int)Math.Round(position) != p)
                    //    Debug.WriteLine("");


                    // Magic happens here!
                    if (time.Measure < 0)
                        continue; // Might happen when try to realign music offset

                    if (time.Beat > signature.Beat)
                        Debug.WriteLine($"[KSH] {i + 1:D4}: Measure has more than {signature.Beat} beats");
                }

                if (line.Contains("="))
                {
                    // Might just pull it to first bar
                    if (time.Measure == 0)
                        time = new Time(1, 1, 0);

                    var data = line.Split('=');
                    if (data.Length < 2)
                    {
                        Debug.WriteLine($"[KSH] {i+1:D4}: Invalid attribute");
                        continue;
                    }

                    string prop  = data[0].Trim().ToLower();
                    string value = data[1].Trim();
                    var param    = value.Split(';');
                    if (string.IsNullOrEmpty(value))
                        Debug.WriteLine($"[KSH] {i + 1:D4}: {prop} value is empty (reset value)");

                    /** WARNING !!!
                     * Cancer code ahead, proceed with caution!
                     * Well.. the rest of codes aren't any better either lol
                     */
                    int maxDuration = (measureCount - measure) * 192;
                    switch (prop) // At first i just want try to use switch-case pattern, turns out became cancerous (indent and) code lol
                    {
                        #region --- Metadata ---
                        case "title":       Title          = value; break;
                        case "artist":      Artist         = value; break;
                        case "effect":      Effector       = value; break;
                        case "illustrator": Illustrator    = value; break;
                        case "jacket":      JacketFileName = value; break;
                        case "m":           MusicFileName  = value; break;               
                        case "po"    when int.TryParse(value, out int po):  PreviewOffset = po;                  break;
                        case "mvol"  when int.TryParse(value, out int vol): Volume        = vol;                 break;
                        case "level" when int.TryParse(value, out int lv):  Level         = lv;                  break;
                        case "difficulty" when value == "light":            Difficulty    = Difficulty.Novice;   break;
                        case "difficulty" when value == "challenge":        Difficulty    = Difficulty.Advanced; break;
                        case "difficulty" when value == "extended":         Difficulty    = Difficulty.Exhaust;  break;
                        case "difficulty" when value == "infinite":         Difficulty    = Difficulty.Infinite; break;
                        case "bg"    when string.IsNullOrEmpty(Background):
                        case "layer" when string.IsNullOrEmpty(Background):
                            Background = value;
                            break;
                        #endregion
                        #region --- Event Info ---
                        case "o" when int.TryParse(value, out int o): 
                            MusicOffset = o;

                            // Should be fine if there's no bpm change
                            if (MusicOffset % 48 != 0 && opt.RealignOffset)
                                Debug.WriteLine($"[KSH] Music Offset is not base of 48 signature (Offset: {MusicOffset})");

                            break;
                        case "t":
                            foreach (string ts in value.Split('-'))
                            {
                                if (!float.TryParse(ts, out float t))
                                    break;

                                if (BpmMin == 0f || t < BpmMin)
                                    BpmMin = t;

                                if (BpmMax == 0f || t > BpmMax)
                                    BpmMax = t;

                                if (!value.Contains("-"))
                                {
                                    var bpm = new Event.BPM(new Time(time.Measure, time.Beat, 0), t); // beat still acceptable
                                    Events.Add(bpm);
                                }
                            }

                            // BPM change with unaligned music offset, proceed with caution!
                            if (BpmMin != BpmMax && MusicOffset % 48 != 0 && opt.RealignOffset)
                                Debug.WriteLine($"[KSH] BPM change with unaligned music offset");

                            break;
                        case "stop" when float.TryParse(value, out float duration):
                            Event.BPM start = null;
                            time = new Time(time.Measure, time.Beat, 0);
                            foreach (var ev in Events[time])
                            {
                                if (ev is Event.BPM x)
                                {
                                    start = x;
                                    start.IsStop = true;
                                    break;
                                }
                            }

                            if (start == null)
                            {
                                var last = Events.GetBPM(time);
                                if (last == null)
                                    break;

                                start = new Event.BPM(time, last.Value);
                                start.IsStop = true;
                                Events.Add(start);
                            }

                            Event.BPM end = null;
                            var target    = Time.FromOffset((int)duration, signature);
                            foreach (var ev in Events[target])
                            {
                                if (ev is Event.BPM x)
                                {
                                    end = x;
                                    end.IsStop = false;
                                    break;
                                }
                            }

                            if (end == null)
                            {
                                var last = Events.GetBPM(target);
                                if (last == null)
                                    break;

                                end = new Event.BPM(target, last.Value);
                                end.IsStop = false;
                                Events.Add(end);
                            }

                            // Stop with unaligned music offset can break other stuffs too
                            if (MusicOffset % 48 != 0 && opt.RealignOffset)
                                Debug.WriteLine($"[KSH] Stop event unaligned music offset");

                            break;
                        case "beat": 
                            var sig = value.Split('/');
                            if (sig.Length != 2 || !int.TryParse(sig[0], out int b) || !int.TryParse(sig[1], out int n))
                                break;

                            signature = new Event.TimeSignature(new Time(time.Measure, 1, 0), b, n); // beat unacceptable, only at start measure
                            Events.Add(signature);

                            break;
                        case "fx-l":
                        case "fx-r":
                            if (!opt.EnableLongFx)
                                break;

                            var htrack  = prop == "fx-l" ? Event.ButtonTrack.FxL : Event.ButtonTrack.FxR;
                            holdFx[htrack] = new Effect();
                            
                            var fx = Effect.FromKsh(value);
                            if (fx != null)
                            {
                                fx.Id = ++fxCount;
                                holdFx[htrack] = fx;
                            }

                            break;
                        case "fx-l_se":
                        case "fx-r_se":
                            if (!opt.EnableChipFx)
                                break;

                            var fxTrack = prop == "fx-l_se" ? Event.ButtonTrack.FxL : Event.ButtonTrack.FxR;
                            hitFx[fxTrack] = new KshSoundEffect();
                            
                            if (Enum.TryParse(param[0], true, out Event.ChipFx hit))
                                hitFx[fxTrack].Effect = hit;

                            if (param.Length >= 2 && int.TryParse(param[1], out int hitCount))
                                hitFx[fxTrack].HitCount = hitCount + 1;
                            else
                                hitFx[fxTrack].HitCount = 1;

                            break;
                        case "filtertype":
                            switch (value.Trim())
                            {
                                case "peak": filter = Event.LaserFilter.Peak; break;
                                case "hpf1": filter = Event.LaserFilter.HighPass; break;
                                case "lpf1": filter = Event.LaserFilter.LowPass; break;
                                case var f when f.Contains("bitc"):
                                    filter = Event.LaserFilter.BitCrusher;
                                    break;
                                default:
                                    filter = (Event.LaserFilter)6;
                                    break;
                            }

                            break;
                        case "laserrange_l" when int.TryParse(value.Replace("x", ""), out int r): rangeLeft  = r; break;
                        case "laserrange_r" when int.TryParse(value.Replace("x", ""), out int r): rangeRight = r; break;
                        #endregion
                        #region --- Camera ---
                        case "tilt" when !float.TryParse(value.Trim(), out float _):
                            if (!opt.EnableCamera)
                                break;

                            switch (value.Trim()) // everything untested
                            {
                                case "normal":
                                    Events.Add(new Event.TiltMode(time, Event.TiltType.Normal));
                                    break;
                                case "bigger":
                                    Events.Add(new Event.TiltMode(time, Event.TiltType.Large));
                                    break;
                                case "keep_bigger":
                                    Events.Add(new Event.TiltMode(time, Event.TiltType.Incremental));
                                    break;
                            }

                            break;
                        case "tilt":        // Tilt
                        case "zoom_top":    // CAM_RotX
                        case "zoom_bottom": // CAM_Radi
                        case "lane_toggle": // LaneY
                            if (!opt.EnableCamera)
                                break;

                            if (!float.TryParse(value, out float cameraOffset))
                                break;

                            Camera.WorkType work = Camera.WorkType.None; 
                            switch (prop)
                            {
                                case "zoom_top":    
                                    work = Camera.WorkType.Rotation;
                                    cameraOffset /= 150.0f;
                                    break;
                                case "zoom_bottom": 
                                    work = Camera.WorkType.Radian;
                                    cameraOffset /= -150.0f;
                                    break;
                                case "tilt":        
                                    work = Camera.WorkType.Tilt;
                                    cameraOffset /= 1.0f; // untested
                                    break;
                                case "lane_toggle":
                                    work = Camera.WorkType.LaneClear; // untested too
                                    break;
                            }

                            Camera camera = null;
                            switch (work)
                            {
                                case Camera.WorkType.Rotation:  
                                case Camera.WorkType.Radian:
                                case Camera.WorkType.Tilt:
                                    camera = Camera.Create(work, time, maxDuration, cameraOffset, cameraOffset); 
                                    break;
                                case Camera.WorkType.LaneClear: 
                                    camera = new Camera.LaneClear(time, (int)cameraOffset, 0.00f, 1024.00f);     
                                    break;
                            }

                            if (!cameras.ContainsKey(work))
                            {
                                cameras[work]    = null;
                                lastCamera[work] = camera;
                                Events.Add(camera);

                                continue;
                            }

                            var pairCamera = cameras[work];
                            if (pairCamera == null)
                            {
                                switch (work)
                                {
                                    case Camera.WorkType.Rotation:
                                    case Camera.WorkType.Radian:
                                    case Camera.WorkType.Tilt:
                                        cameras[work] = Camera.Create(work, time, -1, cameraOffset, cameraOffset);     
                                        break;
                                    case Camera.WorkType.LaneClear: 
                                        var ev = new Camera.LaneClear(time, (int)cameraOffset, 1024.00f, 0.00f);
                                        cameras[work] = ev;
                                        Events.Add(ev);

                                        break;
                                }
                            }
                            else 
                            {
                                // Can't really use Time.Difference since time signature can be different across different measures
                                float curOffset  = time.GetAbsoluteOffset(signature);
                                float pairOffset = pairCamera.Time.GetAbsoluteOffset(Events.GetTimeSignature(pairCamera.Time));

                                if (work == Camera.WorkType.LaneClear)
                                {
                                    camera.Start = pairCamera.End;
                                    camera.End   = pairCamera.Start;

                                    if ((int)Math.Abs(curOffset - pairOffset) != pairCamera.Duration)
                                    {
                                        var fill       = new Camera.LaneClear(null, 0, pairCamera.End, camera.Start);
                                        var fillOffset = pairOffset + pairCamera.Duration;
                                        var timeSig    = Events.GetTimeSignature((int)(fillOffset / 192f));

                                        fill.Time     = Time.FromOffset(fillOffset, timeSig);
                                        fill.Duration = (int)Math.Abs(curOffset - fill.Time.GetAbsoluteOffset(timeSig));
                                        Events.Add(fill);
                                    }

                                    cameras[work] = camera;
                                    Events.Add(camera);

                                    break;
                                }

                                // Assign camera properties
                                camera.Time     = pairCamera.Time;
                                camera.Duration = (int)Math.Abs(curOffset - pairOffset);
                                camera.Start    = pairCamera.Start;

                                // Find gap between last camera event
                                var prevCamera = lastCamera[work];

                                float lastOffset = prevCamera.Time.GetAbsoluteOffset(Events.GetTimeSignature(prevCamera.Time));
                                int diff = (int)Math.Abs(pairOffset - lastOffset); 

                                // There's seems to be gap
                                if (prevCamera != null && prevCamera.Duration != diff)
                                {
                                    // Check gap, is indeed something that we can fill
                                    if (prevCamera.Duration < diff)
                                    {
                                        // Adjust time and duration to fill the gap, make sure to use correct time signature!
                                        var fill       = Camera.Create(work, time, 0, prevCamera.End, pairCamera.Start);
                                        var fillOffset = lastOffset + prevCamera.Duration;
                                        var timeSig    = Events.GetTimeSignature((int)(fillOffset / 192f));

                                        fill.Time     = Time.FromOffset(fillOffset, timeSig);
                                        fill.Duration = (int)Math.Abs(pairOffset - fill.Time.GetAbsoluteOffset(timeSig));
                                        Events.Add(fill);
                                    }
                                    // The previous duration is overlap with current time, readjust previous event duration
                                    else
                                    { 
                                        prevCamera.Duration = diff;
                                        prevCamera.End = camera.Start;
                                    }
                                }

                                cameras[work] = null;
                                lastCamera[work] = camera;
                                Events.Add(camera);
                            }

                            break;
                        #endregion
                    }
                }
                else if (line == "--")
                {
                    // Reset time counter
                    measure  += 1;
                    offset    = 0;
                    noteCount = 0;

                    // Reset laser range, unless there's active laser
                    if (!lasers.ContainsKey(Event.LaserTrack.Left))
                        rangeLeft  = 1;

                    if (!lasers.ContainsKey(Event.LaserTrack.Right))
                        rangeRight = 1;

                    float position = measure * 192f;
                    if (measure > 1 && opt.RealignOffset)
                        position -= MusicOffset;

                    time = Time.FromOffset(position, signature);
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        string ln = lines[j];
                        if (char.IsDigit(ln[0]))
                            noteCount++;
                        else if (ln == "--")
                            break;
                    }
                }
                else if (char.IsDigit(line[0]))
                {
                    // Increment offset when current line is event
                    offset++;

                    #region --- BT & FX ---
                    for (int channel = 0; channel < 7; channel++)
                    {
                        if (!opt.EnableButtonTrack)
                            break;

                        Event.ButtonTrack track;
                        switch(channel)
                        {
                            case 0: track = Event.ButtonTrack.A; break;
                            case 1: track = Event.ButtonTrack.B; break;
                            case 2: track = Event.ButtonTrack.C; break;
                            case 3: track = Event.ButtonTrack.D; break;
                            case 5: track = Event.ButtonTrack.FxL; break;
                            case 6: track = Event.ButtonTrack.FxR; break;
                            default: continue;
                        }

                        // FxL and FxR accept any char for long note
                        if (!int.TryParse(line[channel].ToString(), out int flag) && (track != Event.ButtonTrack.FxL && track != Event.ButtonTrack.FxR))
                            continue;

                        // Who's right in the mind to split fx and bt event logic just because the flag is opposite? lol
                        bool isFx   = track == Event.ButtonTrack.FxL || track == Event.ButtonTrack.FxR;
                        bool isChip = ((isFx && flag == 2) || (!isFx && flag == 1)) && flag != 0;
                        bool isHold = ((isFx && flag != 2) || (!isFx && flag == 2)) && flag != 0;

                        if (isChip)
                        {
                            var hit = Event.ChipFx.None;
                            if (isFx)
                            {
                                if (!hitFx.ContainsKey(track))
                                    hitFx[track] = new KshSoundEffect();

                                var fx = hitFx[track];
                                hit    = fx.HitCount > fx.Used ? fx.Effect : Event.ChipFx.None;

                                fx.Used++;
                            }

                            // Overlapping long notes
                            if (longNotes.ContainsKey(track))
                            {
                                var ev        = longNotes[track];
                                var timeSig   = Events.GetTimeSignature(ev.Time.Measure);
                                ev.HoldLength = time.Difference(ev.Time, timeSig);

                                Events.Add(ev);
                            }

                            longNotes.Remove(track);
                            Events.Add(new Event.Button(time, track, 0, null, hit));
                        }
                        else if (isHold)
                        {
                            if (!longNotes.ContainsKey(track))
                            {
                                Effect fx = null;
                                if (isFx)
                                {
                                    if (!holdFx.ContainsKey(track))
                                        holdFx[track] = null;
                                    
                                    if (holdFx[track] != null && holdFx[track].Type != FxType.None)
                                        fx = holdFx[track];
                                }

                                longNotes[track] = new Event.Button(time, track, 0, fx, Event.ChipFx.None);
                            }
                            else
                            {
                                var ev = longNotes[track];
                                ev.HoldLength = Math.Abs(time.Difference(ev.Time, signature));
                            }
                        }
                        else
                        {
                            // Empty event (Event flag: 0)
                            if (longNotes.ContainsKey(track))
                            {
                                var ev        = longNotes[track];
                                var timeSig   = Events.GetTimeSignature(ev.Time.Measure);
                                ev.HoldLength = time.Difference(ev.Time, timeSig);

                                Events.Add(longNotes[track]);
                            }

                            longNotes.Remove(track);
                        }
                    }
                    #endregion

                    #region  --- Laser ---
                    foreach (var track in new[] { Event.LaserTrack.Left, Event.LaserTrack.Right })
                    {
                        if (!opt.EnableLaserTrack)
                            break;

                        int channel = track == Event.LaserTrack.Left ? 8 : 9;
                        int range   = track == Event.LaserTrack.Left ? rangeLeft : rangeRight;
                        char flag   = line[channel];
                        var impact  = Event.SlamImpact.None;

                        // Ignore tick
                        if (flag == ':')
                            continue;

                        if (line.Length >= 13 && opt.EnableSlamImpact)
                        {
                            // Ignore direction, use slam offset instead
                            char f = line[10];
                            if (f == '@')
                            {
                                // Map the impact types
                                char spin = line[11];
                                if (spin == '(' || spin == ')')
                                    impact = Event.SlamImpact.Measure;
                                else if (spin == '<' || spin == '>')
                                    impact = Event.SlamImpact.HalfMeasure;

                                // Try to realign with impact length, seriously, why would impact has length in kshoot?
                                if (int.TryParse(line.Substring(12), out int length))
                                {
                                    // This mapping may incorrect.. and stupid
                                    if ((impact == Event.SlamImpact.Measure || impact == Event.SlamImpact.HalfMeasure) && length < 96)
                                        impact = Event.SlamImpact.Swing;
                                    else if (impact == Event.SlamImpact.Measure && length <= 144)
                                        impact = Event.SlamImpact.ThreeBeat;
                                }
                            }
                            else if (f == 'S')
                                impact = Event.SlamImpact.Swing;
                        }

                        var laser = new Event.Laser(
                            time,
                            track,
                            0,
                            Event.LaserFlag.Start,
                            impact,
                            filter,
                            range
                        );

                        laser.Slam = noteCount >= 32;
                        if (flag != '-')
                        {
                            // Determine laser offset
                            for (int x = 0; x < VolPositions.Length; x++)
                            {
                                if (VolPositions[x] == flag)
                                    laser.Offset = (int)((x / 51f) * 127f);
                            }

                            if (lasers.ContainsKey(track))
                                laser.Flag = Event.LaserFlag.Tick;
                            else
                                lasers[track] = new List<Event.Laser>();

                            lasers[track].Add(laser);
                        }
                        else if (lasers.ContainsKey(track))
                        {
                            var nodes = lasers[track];
                            lasers.Remove(track);

                            // There suppose to be 2 point of laser, no?
                            if (nodes.Count < 2)
                                continue;

                            /** Slam Laser
                             * Every lasers inside 1/32 that 6 cells apart or less are slam in kshoot
                             * However, Vox may produce bug when slam defined more than 2 laser events within certain amount of distance
                             * It seems the threshold determined based on current active bpm, the exact formula is still unclear
                             * Therefore, eliminate the duplicate / unnecessary ticks
                             * 
                             * For example, this code will transform following events:
                             * 
                             *    001,01,00    25    1    0    0    2    0
                             *    001,01,00    95    0    0    0    2    0 // -> Unnecessary event, since the next event is the same offset
                             *    001,01,06    95    0    0    0    2    0 // -> This cause bug because same offset and located within distance threshold
                             *    002,01,00    95    0    0    0    2    0
                             *    002,01,06    50    2    0    0    2    0
                             *    
                             * To this:
                             * 
                             *    001,01,00    25    1    0    0    2    0
                             *    001,01,06    95    0    0    0    2    0 // -> Keep 001,01,06 and throw one of the duplicate
                             *    002,01,00    95    0    0    0    2    0
                             *    002,01,00    50    2    0    0    2    0
                             * 
                             */

                            int counter = 0;
                            Event.Laser last = null;
                            foreach (var node in nodes.ToArray())
                            {
                                if (last == null)
                                {
                                    last = node;
                                    continue;
                                }

                                if (node.Offset == last.Offset)
                                    counter++;
                                else
                                    counter = 0;

                                if (counter >= 2)
                                    nodes.Remove(last);

                                last = node;
                            }

                            // TODO: As inefficient as it gets lol, merge into previous loop if possible
                            for (int n = 0; n < nodes.Count; n++)
                            {
                                if (n + 1 >= nodes.Count)
                                    break;

                                var start   = nodes[n];
                                var end     = nodes[n + 1];
                                var timeSig = Events.GetTimeSignature(start.Time);

                                // Due to terrible ksh format for slam that appear in 1/32 or shorter 
                                // the output of conversion may have 6 cells gap apart each other
                                // TODO: Use offset instead of Time.Difference, cuz it might start and finish in different measure
                                if (start.Slam && end.Slam && end.Time.Difference(start.Time, timeSig) <= 6)
                                {
                                    // Pull one of the offset
                                    // In some rare cases, not pulling the offset will ended up normal laser instead of slam, especially in slow bpm
                                    int min  = Math.Min(start.Time.Offset, end.Time.Offset);
                                    start.Time.Offset = end.Time.Offset = min;

                                    n += 1;
                                }
                            }

                            // Don't forget to set the flag
                            nodes[0].Flag = Event.LaserFlag.Start;
                            last.Flag     = Event.LaserFlag.End;

                            // TODO: Reset laser range when it's no longer active in current measure(?)
                            Events.Add(nodes.ToArray());
                        }
                    }
                    #endregion
                }
            }
        }
    }
}
