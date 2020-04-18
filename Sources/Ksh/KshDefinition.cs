using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoxCharger
{
    public class KshDefinition
    {
        private Dictionary<string, string> definitions;
        public bool IsNormalized { get; set; } = true;
        public string Value { get; private set; }

        public KshDefinition(string data)
        {
            definitions = new Dictionary<string, string>();
            foreach (string p in data.Split(';'))
            {
                var prop = p.Split('=');
                if (prop.Length != 2)
                {
                    if (Value != null)
                        Value = prop[1].Trim();

                    continue;
                }

                definitions.Add(
                    prop[0].Trim(),
                    prop[1].Trim()
                );
            }
        }

        public bool GetString(string pname, out string result)
        {
            result = string.Empty;
            if (definitions.ContainsKey(pname))
            {
                result = definitions[pname];
                return true;
            }

            return false;
        }

        public bool GetValue(string pname, out int result)
        {
            result = 0;
            if (definitions.ContainsKey(pname))
            {
                string str = GetDominantValue(definitions[pname]);
                if (str.Contains('/'))
                    return GetFraction(pname, out result);
                else if (str.Contains('%'))
                    return GetPercentage(pname, out result);
                else if (!char.IsDigit(str.Last()))
                    str = new string(str.Select(c => char.IsDigit(c) ? c : '\0').ToArray());

                return int.TryParse(str.Trim(), out result);
            }

            return false;
        }

        public bool GetValue(string pname, out float result)
        {
            result = 0;
            if (definitions.ContainsKey(pname))
            {
                string str = GetDominantValue(definitions[pname]);
                if (str.Contains('/'))
                    return GetFraction(pname, out result);
                else if (str.Contains('%'))
                    return GetPercentage(pname, out result);
                else if (!char.IsDigit(str.Last()))
                    str = new string(str.Select(c => char.IsDigit(c) ? c : '\0').ToArray());

                return float.TryParse(str.Trim(), out result);
            }

            return false;
        }

        public bool GetFraction(string pname, out int result)
        {
            result = 0;
            if (GetFraction(pname, out int numerator, out int denominator))
            {
                result = IsNormalized ? numerator * denominator : numerator / denominator;
                return true;
            }

            return false;
        }

        public bool GetFraction(string pname, out float result)
        {
            result = 0;
            if (GetFraction(pname, out float numerator, out float denominator))
            {
                result = IsNormalized ? numerator * denominator : numerator / denominator;
                return true;
            }

            return false;
        }

        public bool GetFraction(string pname, out int numerator, out int denominator)
        {
            denominator = numerator = 0;
            if (definitions.ContainsKey(pname))
            {
                var data = definitions[pname].Split('/');
                return data.Length == 2 && int.TryParse(data[0], out numerator) && int.TryParse(data[1], out denominator);
            }

            return false;
        }

        public bool GetFraction(string pname, out float numerator, out float denominator)
        {
            denominator = numerator = 0f;
            if (definitions.ContainsKey(pname))
            {
                var data = definitions[pname].Split('/');
                return data.Length == 2 && float.TryParse(data[0], out numerator) && float.TryParse(data[1], out denominator);
            }

            return false;
        }

        public bool GetPercentage(string pname, out int result)
        {
            result = 0;
            if (definitions.ContainsKey(pname))
            {
                string str = definitions[pname];
                if (float.TryParse(str.Replace("%", string.Empty), out float percentage)) 
                {
                    result = IsNormalized ? (int)(percentage / 100f) : (int)percentage;
                    return true;
                }
            }

            return false;
        }

        public bool GetPercentage(string pname, out float result)
        {
            result = 0f;
            if (definitions.ContainsKey(pname))
            {
                string str = definitions[pname];
                if (float.TryParse(str.Replace("%", string.Empty), out result))
                {
                    result = IsNormalized ? result / 100f : result;
                    return true;
                }
            }

            return false;
        }

        private string GetDominantValue(string data)
        {
            foreach (var separator in new char[] { '-', '>' })
            {
                if (data.Contains(separator))
                {
                    var values = data.Split('-');
                    if (values.Length < 2)
                        continue;

                    return values[0];
                }
            }

            return data;
        }
    }
}
