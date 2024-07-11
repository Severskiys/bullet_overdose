using System;
using System.Threading;
using UnityEngine;

namespace CodeBase.Logic
{
    //https://habr.com/ru/companies/skbkontur/articles/661097/comments/#comment_24276093
    
    static class UniqueId
    {
        class ThreadState
        {
            private readonly byte[] buffer;
            private ulong next;

            public ThreadState(Guid seed)
            {
                buffer = seed.ToByteArray();
                next = BitConverter.ToUInt64(buffer, 8);
            }

            public Guid NewGuid()
            {
                var span = buffer.AsSpan();
                bool r = BitConverter.TryWriteBytes(span[8..], unchecked(next++));
                return new Guid(span);
            }
        }

        static readonly ThreadLocal<ThreadState> state = new(() => new(Guid.NewGuid()));

        public static Guid NewGuid() => state.Value.NewGuid();
        public static string NewId() => state.Value.NewGuid().ToString();
    }
}