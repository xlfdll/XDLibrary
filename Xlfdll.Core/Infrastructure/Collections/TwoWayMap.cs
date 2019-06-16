using System;
using System.Collections.Generic;

namespace Xlfdll.Collections
{
    public class TwoWayMap<T1, T2>
    {
        public TwoWayMap()
        {
            this.ForwardDictionary = new Dictionary<T1, T2>();
            this.BackwardDictionary = new Dictionary<T2, T1>();

            this.Forward = new Indexer<T1, T2>(this.ForwardDictionary);
            this.Backward = new Indexer<T2, T1>(this.BackwardDictionary);
        }

        private Dictionary<T1, T2> ForwardDictionary { get; }
        private Dictionary<T2, T1> BackwardDictionary { get; }

        public void Add(T1 t1, T2 t2)
        {
            // Non-thread-safe for now
            this.ForwardDictionary.Add(t1, t2);
            this.BackwardDictionary.Add(t2, t1);
        }

        public Indexer<T1, T2> Forward { get; }
        public Indexer<T2, T1> Backward { get; }

        public class Indexer<S1, S2>
        {
            public Indexer(Dictionary<S1, S2> dictionary)
            {
                this.Dictionary = dictionary;
            }

            private Dictionary<S1, S2> Dictionary { get; }

            public S2 this[S1 key]
            {
                get => this.Dictionary[key];
                set => this.Dictionary[key] = value;
            }

            public Boolean Contains(S1 key)
            {
                return this.Dictionary.ContainsKey(key);
            }
        }
    }
}