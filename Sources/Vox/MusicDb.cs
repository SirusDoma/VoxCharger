using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Xml;
using System.Globalization;

namespace VoxCharger
{
    public class MusicDb : IEnumerable<VoxHeader>
    {
        private const string DummyYomigana = "ダミー";
        private static readonly Encoding DefaultEncoding = Encoding.GetEncoding("Shift_JIS");

        private Dictionary<int, VoxHeader> _headers;
        private int _max = 0;

        public int Count  => _headers.Count;

        public int LastId => _headers.Values.Max(h => h.Id);

        public MusicDb()
        {
            _headers = new Dictionary<int, VoxHeader>();
        }

        public void Load(string path, bool append = false)
        {
            _headers = append ? _headers : new Dictionary<int, VoxHeader>();
            var doc = new XmlDocument();
            using (var reader = new StreamReader(path, DefaultEncoding))
            {
                // Load via reader to ensure correct encoding is used
                doc.Load(reader);
            }

            foreach (XmlNode music in doc.SelectSingleNode("mdb"))
            {
                int id = int.Parse(music.Attributes["id"].Value);
                if (append && _headers.ContainsKey(id))
                    continue;

                var info = music.SelectSingleNode("info");
                var header = new VoxHeader
                {
                    Id               = id,
                    Title            = info.SelectSingleNode("title_name").InnerText,
                    TitleYomigana    = info.SelectSingleNode("title_yomigana").InnerText,
                    Artist           = info.SelectSingleNode("artist_name").InnerText,
                    ArtistYomigana   = info.SelectSingleNode("artist_yomigana").InnerText,
                    Ascii            = info.SelectSingleNode("ascii").InnerText,
                    DistributionDate = DateTime.ParseExact(info.SelectSingleNode("distribution_date").InnerText, "yyyyMMdd", CultureInfo.InvariantCulture),
                    Volume           = short.Parse(info.SelectSingleNode("volume").InnerText),
                    BackgroundId     = short.Parse(info.SelectSingleNode("bg_no").InnerText),
                    GenreId          = int.Parse(info.SelectSingleNode("genre").InnerText),
                    Version          = (GameVersion)int.Parse(info.SelectSingleNode("version").InnerText),
                    InfVersion       = (InfiniteVersion)int.Parse(info.SelectSingleNode("inf_ver").InnerText),
                };

                // For some reason, konmai decide to use int32 with thousands value for the BPM, e.g 18000 to represent 180.00 BPM
                // Why? I don't know, probably the usual fuckery
                string bpmMax = info.SelectSingleNode("bpm_max").InnerText;
                header.BpmMax = double.Parse($"{bpmMax.Substring(0, bpmMax.Length - 2)}.{bpmMax.Substring(bpmMax.Length - 2)}");
                header.BpmMax = header.BpmMax >= 1000 ? header.BpmMax / 100D : header.BpmMax;

                string bpmMin = info.SelectSingleNode("bpm_min").InnerText;
                header.BpmMin = double.Parse($"{bpmMin.Substring(0, bpmMin.Length - 2)}.{bpmMin.Substring(bpmMin.Length - 2)}");
                header.BpmMin = header.BpmMin >= 1000 ? header.BpmMin / 100D : header.BpmMin;

                header.Levels = new Dictionary<Difficulty, VoxLevelHeader>();
                foreach (XmlNode diffInfo in music.SelectSingleNode("difficulty"))
                {
                    Difficulty current;
                    if (diffInfo.Name == "novice")
                        current = Difficulty.Novice;
                    else if (diffInfo.Name == "advanced")
                        current = Difficulty.Advanced;
                    else if (diffInfo.Name == "exhaust")
                        current = Difficulty.Exhaust;
                    else if (diffInfo.Name == "infinite" || diffInfo.Name == "maximum")
                        current = Difficulty.Infinite;
                    else
                        continue; // Unknown difficulty, 

                    var lvHeader = new VoxLevelHeader
                    {
                        Difficulty  = current,
                        Level       = int.Parse(diffInfo.SelectSingleNode("difnum").InnerText),
                        Effector    = diffInfo.SelectSingleNode("effected_by").InnerText,
                        Illustrator = diffInfo.SelectSingleNode("illustrator").InnerText,
                        Price       = int.Parse(diffInfo.SelectSingleNode("price").InnerText),
                        Limited     = int.Parse(diffInfo.SelectSingleNode("limited").InnerText),
                        JacketPrint = int.Parse(diffInfo.SelectSingleNode("jacket_print").InnerText),
                        JacketMask  = int.Parse(diffInfo.SelectSingleNode("jacket_mask").InnerText),
                    };

                    if (diffInfo.SelectSingleNode("radar") != null)
                    {
                        var radar = diffInfo.SelectSingleNode("radar");
                        lvHeader.Radar = new VoxLevelRadar()
                        {
                            Notes    = byte.Parse(radar.SelectSingleNode("notes").InnerText),
                            Peak     = byte.Parse(radar.SelectSingleNode("peak").InnerText),
                            Lasers   = byte.Parse(radar.SelectSingleNode("tsumami").InnerText),
                            Tricky   = byte.Parse(radar.SelectSingleNode("tricky").InnerText),
                            HandTrip = byte.Parse(radar.SelectSingleNode("hand-trip").InnerText),
                            OneHand  = byte.Parse(radar.SelectSingleNode("one-hand").InnerText),
                        };


                    }

                    if (lvHeader.Level <= 0)
                        continue; // Invalid level?
                    
                    header.Levels[current] = lvHeader;
                }

                Add(header);
            }
        }

        public void Save(string filename)
        {
            var doc    = new XmlDocument();
            var master = doc.CreateElement("mdb");

            var createMetaElement = new Func<XmlDocument, string, string, string, XmlElement>((d, n, v, t) =>
            {
                var prop  = doc.CreateElement(n);
                if (v != null)
                    prop.AppendChild(doc.CreateTextNode(v));
                
                if (!string.IsNullOrEmpty(t))
                    prop.SetAttribute("__type", t);

                return prop;
            });

            foreach (var header in _headers.Values)
            {
                if (header == null)
                    continue;

                var music = doc.CreateElement("music");
                music.SetAttribute("id", header.Id.ToString());

                var info = doc.CreateElement("info");
                info.AppendChild(createMetaElement(doc, "label",             header.Id.ToString(), string.Empty));
                info.AppendChild(createMetaElement(doc, "title_name",        header.Title, string.Empty));
                info.AppendChild(createMetaElement(doc, "title_yomigana",    header.TitleYomigana, string.Empty));
                info.AppendChild(createMetaElement(doc, "artist_name",       header.Artist, string.Empty));
                info.AppendChild(createMetaElement(doc, "artist_yomigana",   header.ArtistYomigana, string.Empty));
                info.AppendChild(createMetaElement(doc, "ascii",             header.Ascii, string.Empty));
                info.AppendChild(createMetaElement(doc, "bpm_max",           header.BpmMax.ToString("000.00").Replace(".", string.Empty).Replace(",", string.Empty), "u32"));
                info.AppendChild(createMetaElement(doc, "bpm_min",           header.BpmMin.ToString("000.00").Replace(".", string.Empty).Replace(",", string.Empty), "u32"));
                info.AppendChild(createMetaElement(doc, "distribution_date", header.DistributionDate.ToString("yyyyMMdd"), "u32"));
                info.AppendChild(createMetaElement(doc, "volume",            header.Volume.ToString(), "u16"));
                info.AppendChild(createMetaElement(doc, "bg_no",             header.BackgroundId.ToString(), "u16"));
                info.AppendChild(createMetaElement(doc, "genre",             header.GenreId.ToString(), "u8"));
                info.AppendChild(createMetaElement(doc, "is_fixed",          "1", "u8"));
                info.AppendChild(createMetaElement(doc, "version",           ((int)header.Version).ToString(), "u8"));
                info.AppendChild(createMetaElement(doc, "demo_pri",          "0", "s8"));
                info.AppendChild(createMetaElement(doc, "inf_ver",           ((int)header.InfVersion).ToString(), "u8"));

                var diffInfo = doc.CreateElement("difficulty");
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    string diffName = Enum.GetName(typeof(Difficulty), difficulty).ToLower();
                    if (difficulty == Difficulty.Infinite && header.InfVersion == InfiniteVersion.Mxm)
                        diffName = "maximum";

                    var detail = doc.CreateElement(diffName);
                    var lvHeader  = new VoxLevelHeader() { Level = 0 };
                    if (header.Levels.ContainsKey(difficulty))
                        lvHeader = header.Levels[difficulty];

                    detail.AppendChild(createMetaElement(doc, "difnum",       lvHeader.Level.ToString(), "u8"));
                    detail.AppendChild(createMetaElement(doc, "illustrator",  lvHeader.Illustrator, string.Empty));
                    detail.AppendChild(createMetaElement(doc, "effected_by",  lvHeader.Effector, string.Empty));
                    detail.AppendChild(createMetaElement(doc, "price",        lvHeader.Price.ToString(), "s32"));
                    detail.AppendChild(createMetaElement(doc, "limited",      lvHeader.Limited.ToString(), "u8"));
                    detail.AppendChild(createMetaElement(doc, "jacket_print", lvHeader.JacketPrint.ToString(), "s32"));
                    detail.AppendChild(createMetaElement(doc, "jacket_mask",  lvHeader.JacketMask.ToString(), "s32"));

                    if (lvHeader.Radar != null)
                    {
                        var radar = detail.AppendChild(createMetaElement(doc, "radar", null, null));
                        radar.AppendChild(createMetaElement(doc, "notes", lvHeader.Radar.Notes.ToString(), "u8"));
                        radar.AppendChild(createMetaElement(doc, "peak", lvHeader.Radar.Peak.ToString(), "u8"));
                        radar.AppendChild(createMetaElement(doc, "tsumami", lvHeader.Radar.Lasers.ToString(), "u8"));
                        radar.AppendChild(createMetaElement(doc, "tricky", lvHeader.Radar.Tricky.ToString(), "u8"));
                        radar.AppendChild(createMetaElement(doc, "hand-trip", lvHeader.Radar.HandTrip.ToString(), "u8"));
                        radar.AppendChild(createMetaElement(doc, "one-hand", lvHeader.Radar.OneHand.ToString(), "u8"));
                    }

                    diffInfo.AppendChild(detail);
                }

                music.AppendChild(info);
                music.AppendChild(diffInfo);

                master.AppendChild(music);
            }

            var settings = new XmlWriterSettings { Indent = true, Encoding = DefaultEncoding };
            using (var output = new StreamWriter(filename, false, DefaultEncoding))
            using (var writer = XmlWriter.Create(output, settings))
            {
                doc.AppendChild(master);
                doc.Save(writer);
            }
        }

        public void Add(VoxHeader header)
        {
            if (_max < header.Id)
                _max = header.Id;

            _headers[header.Id] = header;
        }

        public void Remove(int id)
        {
            if (_max == id)
                _max = 0;

            _headers.Remove(id);
        }

        public void Remove(VoxHeader header)
        {
            Remove(header.Id);
        }

        public bool Contains(int id)
        {
            return _headers.ContainsKey(id);
        }

        public bool Contains(VoxHeader header)
        {
            return Contains(header.Id);
        }

        public VoxHeader GetHeader(int id)
        {
            return _headers[id];
        }

        public void Clear()
        {
            // Clear the list but not the last id
            _headers.Clear();
        }

        public IEnumerator<VoxHeader> GetEnumerator()
        {
            return _headers.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _headers.Values.GetEnumerator();
        }
    }
}
