using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VoxCharger
{
    public class EventCollection : ICollection<Event>
    {
        private List<Event> Events = new List<Event>();

        public int Count => Events.Count;

        public bool IsReadOnly => false;

        public EventCollection()
        {
        }

        public Event[] this[int measure]
        {
            get => Events.FindAll((ev) => ev.Time.Measure == measure).ToArray();
        }

        public Event[] this[Time time]
        {
            get => Events.FindAll((ev) => ev.Time == time).ToArray();
            set => Events.AddRange(value.Where(ev => ev != null).Select(ev => { ev.Time = time; return ev; }));
        }

        public Event.TimeSignature GetTimeSignature(int measure)
        {
            return GetTimeSignature(new Time(measure, 1, 0));
        }

        public Event.TimeSignature GetTimeSignature(Time time)
        {
            var timeSig = Events.LastOrDefault(ev =>
                ev is Event.TimeSignature && (ev.Time == time || ev.Time.Measure < time.Measure)
            ) as Event.TimeSignature;

            return timeSig != null ? timeSig : new Event.TimeSignature(time, 4, 4);
        }

        public Event.BPM GetBPM(int measure)
        {
            return GetBPM(new Time(measure, 1, 0));
        }

        public Event.BPM GetBPM(Time time)
        {
            return Events.LastOrDefault(ev =>
                ev is Event.BPM && (ev.Time == time || ev.Time.Measure < time.Measure)
            ) as Event.BPM;
        }

        public void Add(Event ev)
        {
            if (ev != null)
                Events.Add(ev);
        }

        public void Add(params Event[] ev)
        {
            Events.AddRange(new List<Event>(ev).FindAll(e => e != null));
        }

        public bool Remove(Event ev)
        {
            return Events.Remove(ev);
        }

        public bool Contains(Event ev)
        {
            return Events.Contains(ev);
        }

        public void CopyTo(Event[] events, int index)
        {
            Events.CopyTo(events, index);
        }

        public void Clear()
        {
            Events.Clear();
        }

        public IEnumerator<Event> GetEnumerator()
        {
            return Events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Events.GetEnumerator();
        }
    }
}
