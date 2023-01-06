using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxCharger
{
    public class EventCollection : ICollection<Event>
    {
        private List<Event> _events = new List<Event>();

        public int Count => _events.Count;

        public bool IsReadOnly => false;

        public EventCollection()
        {
        }

        public Event[] this[int measure]
        {
            get => _events.FindAll((ev) => ev.Time.Measure == measure).ToArray();
        }

        public Event[] this[Time time]
        {
            get => _events.FindAll((ev) => ev.Time == time).ToArray();
            set => _events.AddRange(value.Where(ev => ev != null).Select(ev => { ev.Time = time; return ev; }));
        }

        public Event.TimeSignature GetTimeSignature(int measure)
        {
            return GetTimeSignature(new Time(measure, 1, 0));
        }

        public Event.TimeSignature GetTimeSignature(Time time)
        {
            var timeSig = _events.LastOrDefault(ev =>
                ev is Event.TimeSignature && (ev.Time == time || ev.Time.Measure < time.Measure)
            ) as Event.TimeSignature;

            return timeSig != null ? timeSig : new Event.TimeSignature(time, 4, 4);
        }

        public Event.Bpm GetBpm(int measure)
        {
            return GetBpm(new Time(measure, 1, 0));
        }

        public Event.Bpm GetBpm(Time time)
        {
            return _events.LastOrDefault(ev =>
                ev is Event.Bpm && (ev.Time == time || ev.Time.Measure < time.Measure)
            ) as Event.Bpm;
        }

        public void Add(Event ev)
        {
            if (ev != null)
                _events.Add(ev);
        }

        public void Add(params Event[] ev)
        {
            _events.AddRange(new List<Event>(ev).FindAll(e => e != null));
        }

        public bool Remove(Event ev)
        {
            return _events.Remove(ev);
        }

        public bool Contains(Event ev)
        {
            return _events.Contains(ev);
        }

        public void CopyTo(Event[] events, int index)
        {
            _events.CopyTo(events, index);
        }

        public void Clear()
        {
            _events.Clear();
        }

        public IEnumerator<Event> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _events.GetEnumerator();
        }
    }
}
