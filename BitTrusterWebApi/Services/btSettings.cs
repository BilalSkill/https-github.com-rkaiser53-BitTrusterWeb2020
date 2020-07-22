using BitTrusterWebApi.Helper;
using BitTrusterWebApi.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrusterWebApi.Services
{
    public interface ISettingService
    {
        public dynamic GetAllSettings();
        public void SetAllSettings(dynamic param);
    }
    public class btSettings:DbContext, ISettingService
    {
        string ConnectionString = null;
        public dynamic GetAllSettings()
        {
            dynamic result = new ExpandoObject();

            result.Settings = GetSettings();
            result.MessageTemplates = GetMessageTemplates();
            result.MailServer = GetMailServer();

            return result;
        }
        public void SetAllSettings(dynamic param)
        {
            SetSettings(param.Settings);
            SetMessageTemplates(param.MessageTemplates);
            SetMailServer(param.MailServer);
        }

        public dynamic GetSettings()
        {
            dynamic result = new ExpandoObject();
            var resultDict = (IDictionary<string, object>)result;

            using (var con = GetConnection())
            {
                con.Open();
                var sql = $@"SELECT [Key],[Value]
                    FROM GeneralSettings";

                dynamic dbResult = null;
                using (new BtSqlMonitor(sql, new StackTrace()))
                {
                    dbResult = con.Query<dynamic>(sql);
                }

                foreach (var item in dbResult)
                {
                    resultDict.Add(item.Key, item.Value);
                }
            }
            return result;
        }

        public void SetSettings(dynamic newSettings)
        {
            var newSettingsDict = newSettings.ToObject<Dictionary<string, object>>();
            foreach (var item in newSettingsDict)
            {
                using (var con = GetConnection())
                {
                    con.Open();
                    var sql = $@"UPDATE GeneralSettings 
                        SET [Value] = @Value WHERE [Key]=@Key
                        IF @@ROWCOUNT=0
                            INSERT INTO GeneralSettings ([Key], [Value]) 
                                        VALUES (@Key, @Value)";

                    using (new BtSqlMonitor(sql, new StackTrace()))
                    {
                        con.Execute(sql, new
                        {
                            Key = item.Key.ToString(),
                            Value = item.Value.ToString()
                        });
                    }
                }
            }
        }

        public dynamic GetMessageTemplates()
        {
            dynamic result = new ExpandoObject();

            using (var con = GetConnection())
            {
                con.Open();
                var sql = $@"SELECT * FROM MessageTemplates";

                using (new BtSqlMonitor(sql, new StackTrace()))
                {
                    result = con.Query<dynamic>(sql);
                }

            }
            return result;
        }

        public void SetMessageTemplates(dynamic newMessageTemlates)
        {
            using (var con = GetConnection())
            {
                con.Open();
                foreach (dynamic item in newMessageTemlates)
                {
                    #region SQL
                    var sql = $@"
                        UPDATE MessageTemplates 
                        SET 
                            [MessageTemplateKey] = @MessageTemplateKey, 
                            [Content] = @Content, 
                            [Subject] = @Subject 
                        WHERE [ID]=@ID
                        IF @@ROWCOUNT=0
                            INSERT INTO MessageTemplates ([ID], 
                                                          [MessageTemplateKey],
                                                          [Content],
                                                          [Subject]
                                                         ) 
                                        VALUES (@ID, @MessageTemplateKey, @Content, @Subject)";
                    #endregion

                    using (new BtSqlMonitor(sql, new StackTrace()))
                    {
                        con.Execute(sql, new
                        {
                            ID = (int)item.ID,
                            MessageTemplateKey = (string)item.MessageTemplateKey,
                            Content = (string)item.Content,
                            Subject = (string)item.Subject
                        });
                    }
                }
            }
        }

        public dynamic GetMailServer()
        {
            dynamic result = new ExpandoObject();

            using (var con = GetConnection())
            {
                con.Open();
                var sql = $@"SELECT * FROM MailServer";

                using (new BtSqlMonitor(sql, new StackTrace()))
                {
                    result = con.Query<dynamic>(sql);
                }
            }
            return result;
        }

        public void SetMailServer(dynamic newMailServer)
        {
            using (var con = GetConnection())
            {
                con.Open();
                var sql = $@"DELETE MailServer";

                using (new BtSqlMonitor(sql, new StackTrace()))
                {
                    con.Execute(sql);
                }

                foreach (var item in newMailServer)
                {
                    sql = $@"INSERT INTO MailServer 
                                ([Server], [Domain], [UserName], [Password], [TLS]) 
                             VALUES (@Server, @Domain, @UserName, @Password, @TLS)";

                    using (new BtSqlMonitor(sql, new StackTrace()))
                    {
                        con.Execute(sql, new
                        {
                            Server = (string)item.Server,
                            Domain = (string)item.Domain,
                            UserName = (string)item.UserName,
                            Password = (string)item.Password,
                            TLS = (string)item.TLS
                        });
                    }
                }
            }
        }

    }
}
