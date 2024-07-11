// –‒–––‒––‒––––‒––––‒‒–––‒–‒‒––––‒–‒‒–‒––––‒‒‒–‒‒‒–––‒––‒‒––‒‒‒–––––‒‒–‒––
// Коммерческая лицензия подписчика
// (c) 2023 Leopotam <leopotam@yandex.ru>
// –‒–––‒––‒––––‒––––‒‒–––‒–‒‒––––‒–‒‒–‒––––‒‒‒–‒‒‒–––‒––‒‒––‒‒‒–––––‒‒–‒––

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace Leopotam.SpaceHash {
#if ENABLE_IL2CPP
    [Il2CppSetOption (Option.NullChecks, false)]
    [Il2CppSetOption (Option.ArrayBoundsChecks, false)]
#endif
    public class SpaceHash2<T> {
        readonly List<Item>[] _space;
        readonly float _invCellSize;
        readonly float _minX, _minY;
        readonly int _spaceX, _spaceY;

        const int DefaultCellCapacity = 64;
        readonly List<List<Item>> _pool;
        readonly List<List<Item>> _activeLists;

        public SpaceHash2 (float cellSize, float minX, float minY, float maxX, float maxY) {
#if DEBUG
            if (cellSize <= 0f) { throw new Exception ("некорректный размер клетки"); }
#endif
            _invCellSize = 1f / cellSize;
            _spaceX = (int) Math.Ceiling ((maxX - minX) * _invCellSize);
            _spaceY = (int) Math.Ceiling ((maxY - minY) * _invCellSize);
#if DEBUG
            if (_spaceX <= 0 || _spaceY <= 0) { throw new Exception ("некорректные параметры пространства"); }
#endif
            (_minX, _minY) = (minX, minY);
            _space = new List<Item>[_spaceX * _spaceY];
            _pool = new (_space.Length);
            _activeLists = new (_space.Length);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public void Add (T id, float x, float y) {
            var hash = Hash (
                WorldToSpace (x, _minX, _spaceX),
                WorldToSpace (y, _minY, _spaceY));
            var list = _space[hash];
            if (list == null) {
                list = GetList ();
                _space[hash] = list;
                _activeLists.Add (list);
            }
            list.Add (new () { Id = id, X = x, Y = y });
        }

        public void Clear () {
            Array.Clear (_space, 0, _space.Length);
            foreach (var list in _activeLists) {
                list.Clear ();
                RecycleList (list);
            }
            _activeLists.Clear ();
        }

        public bool Has (float xPos, float yPos, float radius, bool selfIgnore) {
            if (radius < 0f) {
                return false;
            }
            var minCellX = WorldToSpace (xPos - radius, _minX, _spaceX);
            var minCellY = WorldToSpace (yPos - radius, _minY, _spaceY);
            var maxCellX = WorldToSpace (xPos + radius, _minX, _spaceX);
            var maxCellY = WorldToSpace (yPos + radius, _minY, _spaceY);
            float xDiff, yDiff, distSqr;
            var rSqr = radius * radius;
            for (var y = minCellY; y <= maxCellY; y++) {
                for (var x = minCellX; x <= maxCellX; x++) {
                    var list = _space[Hash (x, y)];
                    if (list != null) {
                        foreach (var item in list) {
                            xDiff = xPos - item.X;
                            yDiff = yPos - item.Y;
                            distSqr = xDiff * xDiff + yDiff * yDiff;
                            if (distSqr <= rSqr) {
                                if (distSqr < 1e-4f && selfIgnore) {
                                    continue;
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public (SpaceHashHit<T> hit, bool ok) GetOne (float xPos, float yPos, float radius, bool selfIgnore) {
            if (radius < 0f) {
                return (default, false);
            }
            var minCellX = WorldToSpace (xPos - radius, _minX, _spaceX);
            var minCellY = WorldToSpace (yPos - radius, _minY, _spaceY);
            var maxCellX = WorldToSpace (xPos + radius, _minX, _spaceX);
            var maxCellY = WorldToSpace (yPos + radius, _minY, _spaceY);
            float xDiff, yDiff, distSqr;
            SpaceHashHit<T> hit;
            hit.Id = default;
            hit.DistSqr = radius * radius;
            var found = false;
            for (var y = minCellY; y <= maxCellY; y++) {
                for (var x = minCellX; x <= maxCellX; x++) {
                    var list = _space[Hash (x, y)];
                    if (list != null) {
                        foreach (var item in list) {
                            xDiff = xPos - item.X;
                            yDiff = yPos - item.Y;
                            distSqr = xDiff * xDiff + yDiff * yDiff;
                            if (distSqr <= hit.DistSqr) {
                                if (distSqr < 1e-4f && selfIgnore) {
                                    continue;
                                }
                                found = true;
                                hit.DistSqr = distSqr;
                                hit.Id = item.Id;
                            }
                        }
                    }
                }
            }
            return (hit, found);
        }

        public List<SpaceHashHit<T>> Get (float xPos, float yPos, float radius, bool selfIgnore, List<SpaceHashHit<T>> result = default) {
            if (result == null) {
                result = new (DefaultCellCapacity);
            } else {
                result.Clear ();
            }
            if (radius < 0f) {
                return result;
            }
            var minCellX = WorldToSpace (xPos - radius, _minX, _spaceX);
            var minCellY = WorldToSpace (yPos - radius, _minY, _spaceY);
            var maxCellX = WorldToSpace (xPos + radius, _minX, _spaceX);
            var maxCellY = WorldToSpace (yPos + radius, _minY, _spaceY);
            var rSqr = radius * radius;
            float xDiff, yDiff, distSqr;
            for (var y = minCellY; y <= maxCellY; y++) {
                for (var x = minCellX; x <= maxCellX; x++) {
                    var list = _space[Hash (x, y)];
                    if (list != null) {
                        foreach (var item in list) {
                            xDiff = xPos - item.X;
                            yDiff = yPos - item.Y;
                            distSqr = xDiff * xDiff + yDiff * yDiff;
                            if (distSqr <= rSqr) {
                                if (distSqr < 1e-4f && selfIgnore) {
                                    continue;
                                }
                                result.Add (new () { Id = item.Id, DistSqr = distSqr });
                            }
                        }
                    }
                }
            }
            if (result.Count > 1) {
                result.Sort (OnSort);
            }
            return result;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public List<Item>[] Space () => _space;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public int PointHash (float x, float y) {
            var cellX = WorldToSpace (x, _minX, _spaceX);
            var cellY = WorldToSpace (y, _minY, _spaceY);
            return Hash (cellX, cellY);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        public List<Item> PointSpace (float x, float y) => _space[PointHash (x, y)];

        static int OnSort (SpaceHashHit<T> x, SpaceHashHit<T> y) => x.DistSqr < y.DistSqr ? -1 : 1;

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        List<Item> GetList () {
            var count = _pool.Count;
            if (count > 0) {
                count--;
                var l = _pool[count];
                _pool.RemoveAt (count);
                return l;
            }
            return new (DefaultCellCapacity);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        void RecycleList (List<Item> list) {
            _pool.Add (list);
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        int Hash (int x, int y) {
            return y * _spaceX + x;
        }

        [MethodImpl (MethodImplOptions.AggressiveInlining)]
        int WorldToSpace (float v, float min, int spaceMax) {
            // допустимо ошибочное округление отрицательных чисел
            // без floor(), т.к дальше будет отсечение до нуля.
            var res = (int) ((v - min) * _invCellSize);
            if (res >= spaceMax) {
                res = spaceMax - 1;
            } else {
                if (res < 0) {
                    res = 0;
                }
            }
            return res;
        }

        public struct Item {
            public T Id;
            public float X, Y;
        }
    }

    public struct SpaceHashHit<T> {
        public T Id;
        public float DistSqr;
    }
}
