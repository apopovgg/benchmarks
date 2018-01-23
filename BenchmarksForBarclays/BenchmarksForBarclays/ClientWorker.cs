using System;
using System.Collections.Generic;
using System.Linq;
using Apache.Ignite.Core.Cache;

namespace BenchmarksForBarclays
{
    public class ClientWorker<T>
    {
        protected readonly List<T> Objects;
        private readonly string _name;
        private const int BatchSize = 100;

        public ClientWorker(string name, List<T> objects)
        {
            _name = name;
            Objects = objects;
        }

        public void TestPut(int n, Action<int, T> action)
        {
            for (var i = 0; i < n; i++)
            {
                TestPut(action);
            }
        }

        public void TestGet(int n, Func<int, T> func)
        {
            for (var i = 0; i < n; i++)
            {
                TestGet(func);
            }
        }

        public void TestGetAll(int n, Func<IEnumerable<int>, ICollection<ICacheEntry<int, T>>> func)
        {
            for (var i = 0; i < n; i++)
            {
                TestGetAll(func);
            }
        }

        public void TestPutAll(int n, Action<List<KeyValuePair<int, T>>> action)
        {
            for (var i = 0; i < n; i++)
            {
                TestPutAll(action);
            }
        }


        protected void TestPut(Action<int, T> action)
        {
            GcCollect();

            var idx = 0;
            using (new LatencyChecker($"{_name}.Put", Objects.Count))
            {
                foreach (var obj in Objects)
                {
                    action(idx, obj);
                    idx++;
                }
            }
        }

        protected void TestGetAll(Func<IEnumerable<int>, ICollection<ICacheEntry<int, T>>> func)
        {
            var batches = new List<IEnumerable<int>>();
            for (var i = 0; i < Objects.Count; i += BatchSize)
            {
                batches.Add(Enumerable.Range(i, BatchSize));
            }
            GcCollect();

            using (new LatencyChecker($"{_name}.GetAll", batches.Count))
            {
                foreach (var batch in batches)
                {
                    func(batch);
                }
            }
        }

        protected void TestPutAll(Action<List<KeyValuePair<int, T>>> action)
        {
            var batches = new List<List<KeyValuePair<int, T>>>();
            for (var i = 0; i < Objects.Count; i += BatchSize)
            {
                batches.Add(Objects.Skip(i).Take(BatchSize).Select((v, k) => new KeyValuePair<int, T>(k, v)).ToList());
            }

            GcCollect();

            using (new LatencyChecker($"{_name}.PutAll", batches.Count))
            {
                foreach(var batch in batches)
                {
                    action(batch);
                }
            }
        }

        protected void TestGet(Func<int, T> func)
        {
            GcCollect();

            using (new LatencyChecker($"{_name}.Get", Objects.Count))
            {
                for (var i = 0; i < Objects.Count; i++)
                {
                    func(i);
                }
            }
        }

        private static void GcCollect()
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
        }
    }
}
