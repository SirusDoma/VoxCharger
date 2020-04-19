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

        private Dictionary<int, VoxHeader> headers;
        private int max = 0;

        public int Count  => headers.Count;

        public int LastID => headers.Values.Max(h => h.ID);

        public MusicDb()
        {
            headers = new Dictionary<int, VoxHeader>();
        }

        public void Load(string path, bool append = false)
        {
            headers = append ? headers : new Dictionary<int, VoxHeader>();
            var doc = new XmlDocument();
            using (var reader = new StreamReader(path, DefaultEncoding))
            {
                // Load via reader to ensure correct encoding is used
                doc.Load(reader);
            }

            foreach (XmlNode music in doc.SelectSingleNode("mdb"))
            {
                int id = int.Parse(music.Attributes["id"].Value);
                if (append && headers.ContainsKey(id))
                    continue;

                var info = music.SelectSingleNode("info");
                var header = new VoxHeader
                {
                    ID               = id,
                    Title            = info.SelectSingleNode("title_name").InnerText,
                    Artist           = info.SelectSingleNode("artist_name").InnerText,
                    Ascii            = info.SelectSingleNode("ascii").InnerText,
                    DistributionDate = DateTime.ParseExact(info.SelectSingleNode("distribution_date").InnerText, "yyyyMMdd", CultureInfo.InvariantCulture),
                    Volume           = short.Parse(info.SelectSingleNode("volume").InnerText),
                    BackgroundId     = short.Parse(info.SelectSingleNode("bg_no").InnerText),
                    GenreId          = int.Parse(info.SelectSingleNode("genre").InnerText),
                    Version          = (GameVersion)int.Parse(info.SelectSingleNode("version").InnerText),
                    InfVersion       = (InfiniteVersion)int.Parse(info.SelectSingleNode("inf_ver").InnerText),
                };

                string bpmMax = info.SelectSingleNode("bpm_max").InnerText;
                header.BpmMax = double.Parse($"{bpmMax.Substring(0, bpmMax.Length - 2)}.{bpmMax.Substring(bpmMax.Length - 2)}");

                string bpmMin = info.SelectSingleNode("bpm_min").InnerText;
                header.BpmMin = double.Parse($"{bpmMin.Substring(0, bpmMin.Length - 2)}.{bpmMin.Substring(bpmMin.Length - 2)}");

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
                        Limited     = int.Parse(diffInfo.SelectSingleNode("limited").InnerText),
                        Price       = int.Parse(diffInfo.SelectSingleNode("price").InnerText),
                    };

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

            var CreateMetaElement = new Func<XmlDocument, string, string, string, XmlElement>((d, n, v, t) =>
            {
                var prop  = doc.CreateElement(n);
                var value = doc.CreateTextNode(v);

                prop.AppendChild(value);
                if (!string.IsNullOrEmpty(t))
                    prop.SetAttribute("__type", t);

                return prop;
            });

            foreach (var header in headers.Values)
            {
                if (header == null)
                    continue;

                var music = doc.CreateElement("music");
                music.SetAttribute("id", header.ID.ToString());

                var info = doc.CreateElement("info");
                info.AppendChild(CreateMetaElement(doc, "label",             header.ID.ToString(), string.Empty));
                info.AppendChild(CreateMetaElement(doc, "title_name",        header.Title, string.Empty));
                info.AppendChild(CreateMetaElement(doc, "title_yomigana",    DummyYomigana, string.Empty));
                info.AppendChild(CreateMetaElement(doc, "artist_name",       header.Artist, string.Empty));
                info.AppendChild(CreateMetaElement(doc, "artist_yomigana",   DummyYomigana, string.Empty));
                info.AppendChild(CreateMetaElement(doc, "ascii",             header.Ascii, string.Empty));
                info.AppendChild(CreateMetaElement(doc, "bpm_max",           header.BpmMax.ToString("000.00").Replace(".", string.Empty), "u32"));
                info.AppendChild(CreateMetaElement(doc, "bpm_min",           header.BpmMin.ToString("000.00").Replace(".", string.Empty), "u32"));
                info.AppendChild(CreateMetaElement(doc, "distribution_date", header.DistributionDate.ToString("yyyyMMdd"), "u32"));
                info.AppendChild(CreateMetaElement(doc, "volume",            header.Volume.ToString(), "u16"));
                info.AppendChild(CreateMetaElement(doc, "bg_no",             header.BackgroundId.ToString(), "u16"));
                info.AppendChild(CreateMetaElement(doc, "genre",             header.GenreId.ToString(), "u8"));
                info.AppendChild(CreateMetaElement(doc, "is_fixed",          "1", "u8"));
                info.AppendChild(CreateMetaElement(doc, "version",           ((int)header.Version).ToString(), "u8"));
                info.AppendChild(CreateMetaElement(doc, "demo_pri",          "0", "s8"));
                info.AppendChild(CreateMetaElement(doc, "inf_ver",           ((int)header.InfVersion).ToString(), "u8"));

                var diffInfo = doc.CreateElement("difficulty");
                foreach (Difficulty difficulty in Enum.GetValues(typeof(Difficulty)))
                {
                    string diffName = Enum.GetName(typeof(Difficulty), difficulty).ToLower();
                    if (difficulty == Difficulty.Infinite && header.InfVersion == InfiniteVersion.MXM)
                        diffName = "maximum";

                    var detail = doc.CreateElement(diffName);
                    var lvHeader  = new VoxLevelHeader() { Level = 0 };
                    if (header.Levels.ContainsKey(difficulty))
                        lvHeader = header.Levels[difficulty];

                    detail.AppendChild(CreateMetaElement(doc, "difnum",      lvHeader.Level.ToString(), "u8"));
                    detail.AppendChild(CreateMetaElement(doc, "illustrator", lvHeader.Illustrator, string.Empty));
                    detail.AppendChild(CreateMetaElement(doc, "effected_by", lvHeader.Effector, string.Empty));
                    detail.AppendChild(CreateMetaElement(doc, "price",       lvHeader.Price.ToString(), "s32"));
                    detail.AppendChild(CreateMetaElement(doc, "limited",     lvHeader.Limited.ToString(), "u8"));

                    diffInfo.AppendChild(detail);
                }

                music.AppendChild(info);
                music.AppendChild(diffInfo);

                master.AppendChild(music);
            }

            var settings = new XmlWriterSettings { Indent = true };
            using (var output = new StreamWriter(filename, false, DefaultEncoding))
            using (var writer = XmlWriter.Create(output, settings))
            {
                doc.AppendChild(master);
                doc.Save(writer);
            }
        }

        public void Add(VoxHeader header)
        {
            if (max < header.ID)
                max = header.ID;

            headers[header.ID] = header;
        }

        public void Remove(int id)
        {
            if (max == id)
                max = 0;

            headers.Remove(id);
        }

        public void Remove(VoxHeader header)
        {
            Remove(header.ID);
        }

        public bool Contains(int id)
        {
            return headers.ContainsKey(id);
        }

        public bool Contains(VoxHeader header)
        {
            return Contains(header.ID);
        }

        public VoxHeader GetHeader(int id)
        {
            return headers[id];
        }

        public void Clear()
        {
            // Clear the list but not the last id
            headers.Clear();
        }

        public IEnumerator<VoxHeader> GetEnumerator()
        {
            return headers.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return headers.Values.GetEnumerator();
        }
    }
}
