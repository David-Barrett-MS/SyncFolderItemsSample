using System;
using System.IO;
using Microsoft.Exchange.WebServices.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncFolderItemsSample
{
    public class ClassTraceListener : ITraceListener
    {
        string _traceFile = "";
        private StreamWriter _traceStream = null;
        object _writeLock = new object();

        public ClassTraceListener(string traceFile)
        {
            try
            {
                _traceStream = File.AppendText(traceFile);
                _traceFile = traceFile;
            }
            catch { }
        }

        ~ClassTraceListener()
        {
            try
            {
                _traceStream?.Flush();
                _traceStream?.Close();
            }
            catch { }
        }
        public void Trace(string traceType, string traceMessage)
        {
            if (_traceStream == null || String.IsNullOrEmpty(traceMessage))
                return;

            lock (_writeLock)
            {
                try
                {
                    _traceStream.WriteLine(traceMessage);
                    _traceStream.Flush();
                }
                catch { }
            }
        }

        /// <summary>
        /// Clear any existing trace data (wipes any file being written to)
        /// </summary>
        public void Clear()
        {
            if (String.IsNullOrEmpty(_traceFile))
                return;

            lock (_writeLock)
            {

                if (_traceStream != null)
                    _traceStream.Close();

                _traceStream = File.CreateText(_traceFile);
            }
        }
    }

}
