// SqliteParser is a .NET class library to parse SQLite database .db files using only binary file read operations
// https://github.com/vurdalakov/sqliteparser
// Copyright (c) 2019 Vurdalakov. All rights reserved.
// SPDX-License-Identifier: MIT

namespace Vurdalakov
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class EventHandlerSaver : IDisposable
    {
        private Object _obj;
        private Dictionary<String, Object> _eventHandlers = new Dictionary<String, Object>();

        public EventHandlerSaver()
        {
        }

        public EventHandlerSaver(Object obj, params String[] fieldNames)
        {
            this.Save(obj, fieldNames);
        }

        public void Save(Object obj, params String[] fieldNames)
        {
            this._eventHandlers.Clear();

            if (0 == fieldNames.Length)
            {
                fieldNames = obj.GetEventHandlerNames();
            }

            foreach (var fieldName in fieldNames)
            {
                this._eventHandlers[fieldName] = obj.GetEventHandler(fieldName);
                obj.ClearEventHandle(fieldName);
            }

            this._obj = obj;
        }

        public void Restore()
        {
            foreach (var pair in this._eventHandlers)
            {
                this._obj.SetEventHandler(pair.Key, pair.Value);
            }
        }

        #region IDisposable Support

        private Boolean _isDisposed = false;

        protected virtual void Dispose(Boolean disposing)
        {
            if (!this._isDisposed)
            {
                if (disposing)
                {
                    this.Restore();
                }

                this._isDisposed = true;
            }
        }

        ~EventHandlerSaver()
        {
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
