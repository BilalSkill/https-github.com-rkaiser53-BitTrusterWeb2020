using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrusterWebApi.Helper
{
    public class BtSqlMonitor : IDisposable
    {
        ILog log = null;
        Stopwatch watch = new Stopwatch();
        StackFrame sf = null;
        string sql = string.Empty;

        public BtSqlMonitor(string sql, StackTrace st)
        {
            watch.Start();
            this.sf = st.GetFrame(0);
            log = log4net.LogManager.GetLogger(sf.GetMethod().DeclaringType);
            this.sql = sql;
        }
        public void Dispose()
        {
            watch.Stop();
            log.Debug($"Methode: {sf.GetMethod().Name}");
            log.Debug($"Sql elapsed Time: {watch.Elapsed.TotalSeconds:0.000} sec.");
            log.Debug($"Sql Statement: {sql}");
        }
    }
}
