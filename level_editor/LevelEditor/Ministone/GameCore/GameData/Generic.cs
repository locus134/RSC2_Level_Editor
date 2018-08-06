﻿using System.Collections.Generic;
using System.Diagnostics;

namespace Ministone.GameCore.GameData.Generic
{
    public static class StringExtend
    {
        public static int ToInt32(this string str)
        {
            return int.Parse(str);
        }

        public static float ToFloat(this string str)
        {
            return float.Parse(str);
        }
    }

    public class RangeData<T>
    {
        public T min;
        public T max;

        public RangeData(T _min, T _max)
        {
            min = _min;
            max = _max;
        }
    }

    public class Price
    {
        public int coin;
        public int gem;
    }

    public class KeyIndexMap<T>
    {
        Dictionary<string, T> m_keyIndex = new Dictionary<string, T>();
        Dictionary<int, T> m_idIndex = new Dictionary<int, T>();

        public bool GetValue(string key, out T val)
        {
            return m_keyIndex.TryGetValue(key, out val);
        }

        public bool GetValue(int id, out T val)
        {
            return m_idIndex.TryGetValue(id, out val);
        }

        public void AddValue(string key, int id, T val)
        {
            m_keyIndex.Add(key, val);
            m_idIndex.Add(id, val);
        }

        public bool IsExist(string key)
        {
            return m_keyIndex.ContainsKey(key);
        }

        public bool IsExist(int id)
        {
            return m_idIndex.ContainsKey(id);
        }

        public void RemoveValue(string key, int id)
        {
            m_keyIndex.Remove(key);
            m_idIndex.Remove(id);
        }

        public void Clear()
        {
            m_keyIndex.Clear();
            m_idIndex.Clear();
        }
    }

    public static class MyDebug
    {
        //public static TraceListenerCollection Listeners
        //{
        //    get;
        //}

        //public static bool AutoFlush
        //{
        //    get;
        //    set;
        //}

        //public static int IndentLevel
        //{
        //    get;
        //    set;
        //}

        //public static int IndentSize
        //{
        //    get;
        //    set;
        //}

        [Conditional("DEBUG")]
        public static void Flush()
        {
            Debug.Flush();
        }

        [Conditional("DEBUG")]
        //[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static void Close()
        {
            Debug.Close();
        }

        [Conditional("DEBUG")]
        public static void Assert(bool condition) { Debug.Assert(condition); }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message) { Debug.Assert(condition, message); }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message, string detailMessage) { Debug.Assert(condition, message, detailMessage); }

        [Conditional("DEBUG")]
        public static void Assert(bool condition, string message, string detailMessageFormat, params object[] args) { Debug.Assert(condition, message, detailMessageFormat, args); }

        [Conditional("DEBUG")]
        public static void Fail(string message) { Debug.Fail(message); }

        [Conditional("DEBUG")]
        public static void Fail(string message, string detailMessage) { Debug.Fail(message, detailMessage); }

        [Conditional("DEBUG")]
        public static void Print(string message) { Debug.Print(message); }

        [Conditional("DEBUG")]
        public static void Print(string format, params object[] args) { Debug.Print(format, args); }

        [Conditional("DEBUG")]
        public static void Write(string message) { Debug.Write(message); }

        [Conditional("DEBUG")]
        public static void Write(object value) { Debug.Write(value); }

        [Conditional("DEBUG")]
        public static void Write(string message, string category) { Debug.Write(message, category); }

        [Conditional("DEBUG")]
        public static void Write(object value, string category) { Debug.Write(value, category); }

        [Conditional("DEBUG")]
        public static void WriteLine(object value) { Debug.WriteLine(value); }

        [Conditional("DEBUG")]
        public static void WriteLine(object value, string category) { Debug.WriteLine(value, category); }

        [Conditional("DEBUG")]
        public static void WriteLine(string message) { Debug.WriteLine(message); }

        [Conditional("DEBUG")]
        public static void WriteLine(string format, params object[] args) { Debug.WriteLine(format, args); }

        [Conditional("DEBUG")]
        public static void WriteLine(string message, string category) { Debug.WriteLine(message, category); }

        [Conditional("DEBUG")]
        public static void WriteIf(bool condition, string message) { Debug.WriteIf(condition, message); }

        [Conditional("DEBUG")]
        public static void WriteIf(bool condition, object value) { Debug.WriteIf(condition, value); }

        [Conditional("DEBUG")]
        public static void WriteIf(bool condition, string message, string category) { Debug.WriteIf(condition, message, category); }

        [Conditional("DEBUG")]
        public static void WriteIf(bool condition, object value, string category) { Debug.WriteIf(condition, value, category); }

        [Conditional("DEBUG")]
        public static void WriteLineIf(bool condition, string message) { Debug.WriteLineIf(condition, message); }

        [Conditional("DEBUG")]
        public static void WriteLineIf(bool condition, object value) { Debug.WriteLineIf(condition, value); }

        [Conditional("DEBUG")]
        public static void WriteLineIf(bool condition, string message, string category) { Debug.WriteLineIf(condition, message, category); }

        [Conditional("DEBUG")]
        public static void WriteLineIf(bool condition, object value, string category) { Debug.WriteLineIf(condition, value, category); }

        [Conditional("DEBUG")]
        public static void Indent() { Debug.Indent(); }

        [Conditional("DEBUG")]
        public static void Unindent() { Debug.Unindent(); }
    }
}
