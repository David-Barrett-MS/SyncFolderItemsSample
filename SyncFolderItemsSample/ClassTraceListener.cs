﻿/*
 * By David Barrett, Microsoft Ltd. 2018-2022. Use at your own risk.  No warranties are given.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * */

using System;
using System.IO;
using Microsoft.Exchange.WebServices.Data;

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
