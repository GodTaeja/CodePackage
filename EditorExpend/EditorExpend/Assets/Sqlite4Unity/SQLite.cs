//
// Copyright (c) 2009-2012 Krueger Systems, Inc.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using Sqlite3DatabaseHandle = System.IntPtr;

namespace SQLite4Unity3d
{
    [Flags]
    public enum SQLiteOpenFlags
    {
        ReadOnly = 1, ReadWrite = 2, Create = 4,
        NoMutex = 0x8000, FullMutex = 0x10000,
        SharedCache = 0x20000, PrivateCache = 0x40000,
        ProtectionComplete = 0x00100000,
        ProtectionCompleteUnlessOpen = 0x00200000,
        ProtectionCompleteUntilFirstUserAuthentication = 0x00300000,
        ProtectionNone = 0x00400000
    }

    [Flags]
    public enum CreateFlags
    {
        None = 0,
        ImplicitPK = 1, // create a primary key for field called 'Id' (Orm.ImplicitPkName)
        ImplicitIndex = 2, // create an index for fields ending in 'Id' (Orm.ImplicitIndexSuffix)
        AllImplicit = 3, // do both above

        AutoIncPK = 4 // force PK field to be auto inc
    }

    /// <summary>
    /// Represents an open connection to a SQLite database.
    /// </summary>
    public class SQLiteConnection
    {
        private bool _open;
        private TimeSpan _busyTimeout;

        private IntPtr _stmt;

        public Sqlite3DatabaseHandle Handle { get; private set; }

        internal static readonly Sqlite3DatabaseHandle NullHandle = default(Sqlite3DatabaseHandle);

        public string DatabasePath { get; private set; }

        // Dictionary of synchronization objects.
        //
        // To prevent database disruption, a database file must be accessed *synchronously*.
        // For the purpose we create synchronous objects for each database file and store in the
        // static dictionary to share it among all connections.
        // The key of the dictionary is database file path and its value is an object to be used
        // by lock() statement.
        //
        // Use case:
        // - database file lock is done implicitly and automatically.
        // - To prepend deadlock, application may lock a database file explicity by either way:
        //   - RunInTransaction(Action) locks the database during the transaction (for insert/update)
        //   - RunInDatabaseLock(Action) similarly locks the database but no transaction (for query)
        private static readonly Dictionary<string, object> syncObjects = new Dictionary<string, object>();


        public bool StoreDateTimeAsTicks { get; private set; }


        /// <summary>
        /// Constructs a new SQLiteConnection and opens a SQLite database specified by databasePath.
        /// </summary>
        /// <param name="databasePath">
        /// Specifies the path to the database file.
        /// </param>
        /// <param name="storeDateTimeAsTicks">
        /// Specifies whether to store DateTime properties as ticks (true) or strings (false). You
        /// absolutely do want to store them as Ticks in all new projects. The default of false is
        /// only here for backwards compatibility. There is a *significant* speed advantage, with no
        /// down sides, when setting storeDateTimeAsTicks = true.
        /// </param>
        public SQLiteConnection(string key, string databasePath, SQLiteOpenFlags openFlags, bool storeDateTimeAsTicks = false)
        {
            DatabasePath = databasePath;
            mayCreateSyncObject(databasePath);

            Sqlite3DatabaseHandle handle;

            UnityEngine.Debug.Log("start SQLite3.Open");
            // open using the byte[]
            // in the case where the path may include Unicode
            // force open to using UTF-8 using sqlite3_open_v2
            var databasePathAsBytes = GetNullTerminatedUtf8(DatabasePath);
            var r = SQLite3.Open(databasePathAsBytes, out handle, (int) openFlags, IntPtr.Zero);

            UnityEngine.Debug.LogFormat("open result = {0}", r);
            if (r != SQLite3.Result.OK)
            {
                throw new Exception(String.Format("Could not open database file: {0} ({1})", DatabasePath, r));
            }

            _open = true;
            Handle = handle;
            StoreDateTimeAsTicks = storeDateTimeAsTicks;
            BusyTimeout = TimeSpan.FromSeconds(0.1);

            if (!string.IsNullOrEmpty(key))
            {
                UnityEngine.Debug.Log("start SQLite3.Key");
                var rr = SQLite3.Key(Handle, key, key.Length);
                UnityEngine.Debug.LogFormat("key result = {0}", rr);
                if (rr != SQLite3.Result.OK)
                {
                    string msg = SQLite3.GetErrmsg(Handle);
                    throw new Exception(msg);
                }
            }
        }


        void mayCreateSyncObject(string databasePath)
        {
            if (!syncObjects.ContainsKey(databasePath))
            {
                syncObjects[databasePath] = new object();
            }
        }

        /// <summary>
        /// Gets the synchronous object, to be lock the database file for updating.
        /// </summary>
        /// <value>The sync object.</value>
        public object SyncObject
        {
            get { return syncObjects[DatabasePath]; }
        }

        static byte[] GetNullTerminatedUtf8(string s)
        {
            var utf8Length = System.Text.Encoding.UTF8.GetByteCount(s);
            var bytes = new byte[utf8Length + 1];
            System.Text.Encoding.UTF8.GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }

        /// <summary>
        /// Used to list some code that we want the MonoTouch linker
        /// to see, but that we never want to actually execute.
        /// </summary>
        /// <summary>
        /// Sets a busy handler to sleep the specified amount of time when a table is locked.
        /// The handler will sleep multiple times until a total time of <see cref="BusyTimeout"/> has accumulated.
        /// </summary>
        public TimeSpan BusyTimeout
        {
            get { return _busyTimeout; }
            set
            {
                _busyTimeout = value;
                if (Handle != NullHandle)
                {
                    SQLite3.BusyTimeout(Handle, (int) _busyTimeout.TotalMilliseconds);
                }
            }
        }


        public void Close()
        {
            if (_open && Handle != NullHandle)
            {
                try
                {
                    var r = SQLite3.Close(Handle);
                    if (r != SQLite3.Result.OK)
                    {
                        string msg = SQLite3.GetErrmsg(Handle);
                        throw new Exception(msg);
                    }
                }
                finally
                {
                    Handle = NullHandle;
                    _open = false;
                }
            }
        }

        public string CommandText { get; set; }

        public void Prepare()
        {
            var stmt = SQLite3.Prepare2(Handle, CommandText);
            _stmt = stmt;
        }

        public int ReadInt(int index)
        {
            return SQLite3.ColumnInt(_stmt, index);
        }

        public long ReadLong(int index)
        {
            return SQLite3.ColumnInt64(_stmt, index);
        }

        public float ReadFloat(int index)
        {
            return (float) SQLite3.ColumnDouble(_stmt, index);
        }

        public double ReadDouble(int index)
        {
            return SQLite3.ColumnDouble(_stmt, index);
        }

        public string ReadString(int index)
        {
            return SQLite3.ColumnString(_stmt, index);
        }

        public object ReadCol(int index, Type clrType)
        {
            var colType = ColumnType(index);
            return ReadCol(index, colType, clrType);
        }

        public object ReadCol(int index, SQLite3.ColType type, Type clrType)
        {
            var stmt = _stmt;
            if (type == SQLite3.ColType.Null)
            {
                return null;
            }

            if (clrType == typeof(String))
            {
                return SQLite3.ColumnString(stmt, index);
            }

            if (clrType == typeof(Int32))
            {
                return SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(Boolean))
            {
                return SQLite3.ColumnInt(stmt, index) == 1;
            }

            if (clrType == typeof(double))
            {
                return SQLite3.ColumnDouble(stmt, index);
            }

            if (clrType == typeof(float))
            {
                return (float) SQLite3.ColumnDouble(stmt, index);
            }

            /*
            else if (clrType == typeof(TimeSpan))
            {
                return new TimeSpan(SQLite3.ColumnInt64(stmt, index));
            }
            else if (clrType == typeof(DateTime))
            {
                if (_conn.StoreDateTimeAsTicks)
                {
                    return new DateTime(SQLite3.ColumnInt64(stmt, index));
                }
                else
                {
                    var text = SQLite3.ColumnString(stmt, index);
                    return DateTime.Parse(text);
                }
            }
            else if (clrType == typeof(DateTimeOffset))
            {
                return new DateTimeOffset(SQLite3.ColumnInt64(stmt, index), TimeSpan.Zero);
            }
            */
            //for ILRuntime
            if (clrType.IsEnum || clrType.IsClass)
            {
                return SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(Int64))
            {
                return SQLite3.ColumnInt64(stmt, index);
            }

            if (clrType == typeof(UInt32))
            {
                return (uint) SQLite3.ColumnInt64(stmt, index);
            }

            if (clrType == typeof(decimal))
            {
                return (decimal) SQLite3.ColumnDouble(stmt, index);
            }

            if (clrType == typeof(Byte))
            {
                return (byte) SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(UInt16))
            {
                return (ushort) SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(Int16))
            {
                return (short) SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(sbyte))
            {
                return (sbyte) SQLite3.ColumnInt(stmt, index);
            }

            if (clrType == typeof(byte[]))
            {
                return SQLite3.ColumnByteArray(stmt, index);
            }

            /*
            else if (clrType == typeof(Guid))
            {
                var text = SQLite3.ColumnString(stmt, index);
                return new Guid(text);
            }
            */
            throw new NotSupportedException("Don't know how to read " + clrType);
        }

        public int GetColumnCont()
        {
            return SQLite3.ColumnCount(_stmt);
        }

        public void ReleaseStmt()
        {
            SQLite3.Finalize(_stmt);
        }

        public string ColumnName16(int i)
        {
            return SQLite3.ColumnName16(_stmt, i);
        }

        public void FinalizeStmt()
        {
            SQLite3.Finalize(_stmt);
        }

        public SQLite3.Result Step()
        {
            return SQLite3.Step(_stmt);
        }

        public SQLite3.ColType ColumnType(int i)
        {
            return SQLite3.ColumnType(_stmt, i);
        }
    }
}