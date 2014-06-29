using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Core
{
    /// <summary>
    /// Interface to emulate log4net
    /// </summary>
    public interface IObLogger
    {
        void Debug(string msg);
        void DebugFormat(string fmt, params object[] args);
        void Info(string msg);
        void InfoFormat(string fmt, params object[] args);
        void Warn(string msg);
        void WarnFormat(string fmt, params object[] args);
        void Error(string msg);
        void Error(string msg, Exception ex);
        void ErrorFormat(string fmt, params object[] args);
    }

    public class ObFakeLogger : IObLogger
    {

        public void Debug(string msg)
        {
        }

        public void DebugFormat(string fmt, params object[] args)
        {
        }

        public void Info(string msg)
        {
        }

        public void InfoFormat(string fmt, params object[] args)
        {
        }

        public void Warn(string msg)
        {
        }

        public void WarnFormat(string fmt, params object[] args)
        {
        }

        public void Error(string msg)
        {
        }

        public void Error(string msg, Exception ex)
        {
        }

        public void ErrorFormat(string fmt, params object[] args)
        {
        }
    }
}
