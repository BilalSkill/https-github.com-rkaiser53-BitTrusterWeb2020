using BitTrusterWebApi.Helper;
using BitTrusterWebApi.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace BitTrusterWebApi.Services
{
    public interface IPolicyService
    {
        dynamic GetPolicies();
        dynamic GetPoliciesOld();
        dynamic DeletePolicy(int PolicyID);
        dynamic SetPolicy(dynamic policy);


        dynamic GetPoliciesFilters(int PolicyID);
        dynamic SetPolicyFilter(dynamic param);
        dynamic GetPolicyFilterValues();
    }



    public class btPolicies : DbContext,IPolicyService
    {
        public dynamic GetPolicies()
        {
            dynamic result = new ExpandoObject();

            #region SQL
            var sqlGetPolicies = $@"

SELECT 
                Policies.[PolicyID],
                Policies.[Name],
                Policies.[Description],
                Policies.[AlgorithmID],
                Policies.[ProtectorTypeID],
                Policies.[VolumeChoiceID],
                Policies.[TPMManagementOn],
                Policies.[IndividualPIN],
                Policies.[PredefinedPIN],
                Policies.[EncryptionDecryption],
                Policies.[WakeOnLan],

                ISNULL(CONVERT(varchar(5), Policies.ActionStart, 108), '') as 'ActionStart',
                ISNULL(CONVERT(varchar(5), Policies.ActionEnd, 108), '') as 'ActionEnd',
                Policies.[AutomaticReboot],
                Policies.[PauseEncryptionOnActionEnd],
                ISNULL(CONVERT(varchar(5), Policies.UpdateIntervalEncrypted, 108), '') as 'UpdateIntervalEncrypted',
                ISNULL(CONVERT(varchar(5), Policies.UpdateIntervalDecrypted, 108), '') as 'UpdateIntervalDecrypted',
                ISNULL(CONVERT(varchar(5), Policies.UpdateIntervalEncryptingDecrypting, 108), '') as 'UpdateIntervalEncryptingDecrypting',
                ISNULL(CONVERT(varchar(5), Policies.UpdateStart, 108), '') as 'UpdateStart',
                ISNULL(CONVERT(varchar(5), Policies.UpdateEnd, 108), '') as 'UpdateEnd',
                ISNULL(CONVERT(varchar(5), Policies.UpdateIntervalFailed, 108), '') as 'UpdateIntervalFailed',
                ISNULL(CONVERT(varchar(5), Policies.UpdateIntervalSuccessful, 108), '') as 'UpdateIntervalSuccessful',

                Policies.[EncryptAfterHardwareTest],
                Policies.[Monitoring],
                ISNULL(CONVERT(varchar(5), Policies.WolStartTime, 108), '') as 'WolStartTime',
                ISNULL(CONVERT(varchar(5), Policies.WolEndTime, 108), '') as 'WolEndTime',
                Policies.[NpLifeTime],
                Policies.[PinLifeTime],
                Policies.[FallBackPolicy],
                Policies.[ConverttoNTFS],
                Policies.[RebootAfterBitLockerPartitionCreation],
                Policies.[CreateBitLockerPartition],
                Policies.[CreateBitLockerPartitionCommand],
                Policies.[InstallationGrantTimeMinutes],
                Policies.[PassphraseFallback],
                Policies.[SwitchtoTPMifPossible],
                Policies.[UsePinOnSlates],
                Policies.[PpLifeTime],
                Policies.[PriorityOfPolicy],
                Policies.[ServiceAccountGroup],


                PolicyFilters.TableColumnName,
                PolicyFilters.TableColumeValue
            FROM Policies
                INNER JOIN PolicyFilters ON Policies.PolicyID = PolicyFilters.PolicyID

";
            #endregion

            IEnumerable<Policy> res;
            using (new BtSqlMonitor(sqlGetPolicies, new StackTrace()))
            {
                using (var con = GetConnection())
                {
                    con.Open();
                    res = con.Query<Policy>(sqlGetPolicies);
                }
            }

            var policies = from item in res
                           group item by item.PolicyID;

            result.policies = new List<Policy>();

            foreach (var policy in policies)
            {
                var p = policy.First();

                var filter = from item in policy
                             group item by item.TableColumnName;

                foreach (var f in filter)
                {
                    p.PoliciesFilters = new Dictionary<string, List<string>>();
                    List<string> values = new List<string>();

                    foreach (var item in f)
                    {
                        values.Add(item.TableColumeValue);
                    }
                    p.PoliciesFilters.Add(f.Key, values);
                }
                result.policies.Add(p);
            }



            return result;
        }

        public dynamic GetPoliciesOld()
        {
            dynamic result = new ExpandoObject();

            #region SQL
            var sqlGetPolicies = $@"SELECT
                   [PolicyID]
                  ,[Name]
                  ,[Description]
                  ,[AlgorithmID]
                  ,[ProtectorTypeID]
                  ,[VolumeChoiceID]
                  ,[TPMManagementOn]
                  ,[IndividualPIN]
                  ,[PredefinedPIN]
                  ,[EncryptionDecryption]
                  ,[WakeOnLan]
                  ,ISNULL(CONVERT(varchar(5), ActionStart, 108), '') as 'ActionStart'
                  ,ISNULL(CONVERT(varchar(5), ActionEnd, 108), '') as 'ActionEnd'
                  ,[AutomaticReboot]
                  ,[PauseEncryptionOnActionEnd]
                  ,ISNULL(CONVERT(varchar(5), UpdateIntervalEncrypted, 108), '') as 'UpdateIntervalEncrypted'
                  ,ISNULL(CONVERT(varchar(5), UpdateIntervalDecrypted, 108), '') as 'UpdateIntervalDecrypted'
                  ,ISNULL(CONVERT(varchar(5), UpdateIntervalEncryptingDecrypting, 108), '') as 'UpdateIntervalEncryptingDecrypting'
                  ,ISNULL(CONVERT(varchar(5), UpdateStart, 108), '') as 'UpdateStart'
                  ,ISNULL(CONVERT(varchar(5), UpdateEnd, 108), '') as 'UpdateEnd'
                  ,ISNULL(CONVERT(varchar(5), UpdateIntervalFailed, 108), '') as 'UpdateIntervalFailed'
                  ,ISNULL(CONVERT(varchar(5), UpdateIntervalSuccessful, 108), '') as 'UpdateIntervalSuccessful'
                  ,[EncryptAfterHardwareTest]
                  ,[Monitoring]
                  ,ISNULL(CONVERT(varchar(5), WolStartTime, 108), '') as 'WolStartTime'
                  ,ISNULL(CONVERT(varchar(5), WolEndTime, 108), '') as 'WolEndTime'
                  ,[NpLifeTime]
                  ,[PinLifeTime]
                  ,[FallBackPolicy]
                  ,[ConverttoNTFS]
                  ,[RebootAfterBitLockerPartitionCreation]
                  ,[CreateBitLockerPartition]
                  ,[CreateBitLockerPartitionCommand]
                  ,[InstallationGrantTimeMinutes]
                  ,[PassphraseFallback]
                  ,[SwitchtoTPMifPossible]
                  ,[UsePinOnSlates]
                  ,[PpLifeTime]
                  ,[PriorityOfPolicy]
                  ,[ServiceAccountGroup]
              FROM [Policies]";
            #endregion

            using (new BtSqlMonitor(sqlGetPolicies, new StackTrace()))
            {
                using (var con = GetConnection())
                {
                    con.Open();
                    result.policies = con.Query<dynamic>(sqlGetPolicies);
                }
            }
            foreach (dynamic policy in result.policies)
            {
                policy.PoliciesFilters = GetPoliciesFilters((int)policy.PolicyID);
            }
            result.PolicyFilterValues = GetPolicyFilterValues();
            return result;
        }

        public dynamic DeletePolicy(int PolicyID)
        {
            dynamic result = new ExpandoObject();
            DeletePolicyFilters(PolicyID);
            var sqlDeletePolicies = $@"DELETE from Policies WHERE PolicyID = @PolicyID";

            using (new BtSqlMonitor(sqlDeletePolicies, new StackTrace()))
            {
                using (var con = GetConnection())
                {
                    con.Open();
                    result = con.Query(sqlDeletePolicies, new { PolicyID = PolicyID });
                }
            }
            return result;
        }

        public dynamic DeletePolicyFilters(int PolicyID)
        {
            dynamic result = new ExpandoObject();

            var sqlDeletePolicyFilter = $@"DELETE from PolicyFilters WHERE PolicyID = @PolicyID";

            using (new BtSqlMonitor(sqlDeletePolicyFilter, new StackTrace()))
            {
                using (var con = GetConnection())
                {
                    con.Open();
                    result = con.Query(sqlDeletePolicyFilter, new { PolicyID = PolicyID });
                }
            }
            return result;
        }

        public dynamic SetPolicy(dynamic policy)
        {
            dynamic result = new ExpandoObject();

            #region SQL
            string sql = $@"UPDATE Policies SET
                        [Name]= @Name,
                        [Description]= @Description,
                        [AlgorithmID]= @AlgorithmID,
                        [ProtectorTypeID]= @ProtectorTypeID,
                        [VolumeChoiceID]= @VolumeChoiceID,
                        [TPMManagementOn]= @TPMManagementOn,
                        [IndividualPIN]= @IndividualPIN,
                        [PredefinedPIN]= @PredefinedPIN,
                        [EncryptionDecryption]= @EncryptionDecryption,
                        [WakeOnLan]= @WakeOnLan,
                        [ActionStart]= @ActionStart,
                        [ActionEnd]= @ActionEnd,
                        [AutomaticReboot]= @AutomaticReboot,
                        [PauseEncryptionOnActionEnd]= @PauseEncryptionOnActionEnd,
                        [UpdateIntervalEncrypted]= @UpdateIntervalEncrypted,
                        [UpdateIntervalDecrypted]= @UpdateIntervalDecrypted,
                        [UpdateIntervalEncryptingDecrypting]= @UpdateIntervalEncryptingDecrypting,
                        [UpdateStart]= @UpdateStart,
                        [UpdateEnd]= @UpdateEnd,
                        [UpdateIntervalFailed]= @UpdateIntervalFailed,
                        [UpdateIntervalSuccessful]= @UpdateIntervalSuccessful,
                        [EncryptAfterHardwareTest]= @EncryptAfterHardwareTest,
                        [Monitoring]= @Monitoring,
                        [WolStartTime]= @WolStartTime,
                        [WolEndTime]= @WolEndTime,
                        [NpLifeTime]= @NpLifeTime,
                        [PinLifeTime]= @PinLifeTime,
                        [FallBackPolicy]= @FallBackPolicy,
                        [CreateBitLockerPartitionCommand]= @CreateBitLockerPartitionCommand,
                        [CreateBitLockerPartition]= @CreateBitLockerPartition,
                        [PpLifeTime]= @PpLifeTime,
                        [UsePinOnSlates]= @UsePinOnSlates,
                        [SwitchtoTPMifPossible]= @SwitchtoTPMifPossible,
                        [PassphraseFallback]= @PassphraseFallback,
                        [PriorityOfPolicy]= @PriorityOfPolicy,
                        [ServiceAccountGroup]= @ServiceAccountGroup
                WHERE PolicyID = @PolicyID
           IF @@ROWCOUNT=0
                INSERT INTO Policies (
                                        [Name],
                                        [Description],
                                        [AlgorithmID],
                                        [ProtectorTypeID],
                                        [VolumeChoiceID],
                                        [TPMManagementOn],
                                        [IndividualPIN],
                                        [PredefinedPIN],
                                        [EncryptionDecryption],
                                        [WakeOnLan],
                                        [ActionStart],
                                        [ActionEnd],
                                        [AutomaticReboot],
                                        [PauseEncryptionOnActionEnd],
                                        [UpdateIntervalEncrypted],
                                        [UpdateIntervalDecrypted],
                                        [UpdateIntervalEncryptingDecrypting],
                                        [UpdateStart],
                                        [UpdateEnd],
                                        [UpdateIntervalFailed],
                                        [UpdateIntervalSuccessful],
                                        [EncryptAfterHardwareTest],
                                        [Monitoring],
                                        [WolStartTime],
                                        [WolEndTime],
                                        [NpLifeTime],
                                        [PinLifeTime],
                                        [FallBackPolicy],
                                        [CreateBitLockerPartitionCommand],
                                        [CreateBitLockerPartition],
                                        [PpLifeTime],
                                        [UsePinOnSlates],
                                        [SwitchtoTPMifPossible],
                                        [PassphraseFallback],
                                        [PriorityOfPolicy],
                                        [ServiceAccountGroup]
                                        )
                 VALUES (
                            @Name,
                            @Description,
                            @AlgorithmID,
                            @ProtectorTypeID,
                            @VolumeChoiceID,
                            @TPMManagementOn,
                            @IndividualPIN,
                            @PredefinedPIN,
                            @EncryptionDecryption,
                            @WakeOnLan,
                            @ActionStart,
                            @ActionEnd,
                            @AutomaticReboot,
                            @PauseEncryptionOnActionEnd,
                            @UpdateIntervalEncrypted,
                            @UpdateIntervalDecrypted,
                            @UpdateIntervalEncryptingDecrypting,
                            @UpdateStart,
                            @UpdateEnd,
                            @UpdateIntervalFailed,
                            @UpdateIntervalSuccessful,
                            @EncryptAfterHardwareTest,
                            @Monitoring,
                            @WolStartTime,
                            @WolEndTime,
                            @NpLifeTime,
                            @PinLifeTime,
                            @FallBackPolicy,
                            @CreateBitLockerPartitionCommand,
                            @CreateBitLockerPartition,
                            @PpLifeTime,
                            @UsePinOnSlates,
                            @SwitchtoTPMifPossible,
                            @PassphraseFallback,
                            @PriorityOfPolicy,
                            @ServiceAccountGroup
                        )";
            #endregion

            using (new BtSqlMonitor(sql, new StackTrace()))
            {
                using (var con = GetConnection())
                {
                    var policyDict = (IDictionary<string, object>)policy;
                    policyDict.Remove("PoliciesFilters");

                    var dynParam = new DynamicParameters(policy);
                    con.Execute(sql, dynParam);

                    // DeletePendingActions after Save Policy
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_DeletePendingActions", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

            }
            return result;
        }

        public dynamic GetPoliciesFilters(int PolicyID)
        {
            dynamic result = new ExpandoObject();
            var resultDict = (IDictionary<string, object>)result;

            string[] filterTypes = {
                "OSTypeName",
                "ComputerModel",
                "ComputerModelManufacturer",
                "ComputerModelBios",
                "OUName",
                "ComputerIsMobile"
            };

            using (var con = GetConnection())
            {
                con.Open();

                for (int i = 0; i < filterTypes.Length; i++)
                {
                    var TableColumnName = filterTypes[i];
                    var sql = $@"SELECT TableColumeValue from PolicyFilters 
                                    WHERE PolicyID = @PolicyID AND TableColumnName = @TableColumnName";

                    using (new BtSqlMonitor(sql, new StackTrace()))
                    {
                        resultDict.Add(TableColumnName, con.Query<string>(sql, new
                        {
                            PolicyID = PolicyID,
                            TableColumnName = TableColumnName
                        }));
                    }
                }
            }
            return result;
        }

        public dynamic SetPolicyFilter(dynamic param)
        {
            int PolicyID = (int)param.PolicyID;

            dynamic result = new ExpandoObject();
            var PolicyFilterOldDict = (IDictionary<string, object>)GetPoliciesFilters(PolicyID);
            var PolicyFilterNewDict = (IDictionary<string, object>)param.PolicyFilter;

            string[] filterTypes = {
                "OSTypeName",
                "ComputerModel",
                "ComputerModelManufacturer",
                "ComputerModelBios",
                "OUName",
                "ComputerIsMobile"
            };

            using (var con = GetConnection())
            {
                con.Open();
                for (int i = 0; i < filterTypes.Length; i++)
                {
                    var TableColumnName = filterTypes[i];

                    List<string> filterOld = (List<string>)(PolicyFilterOldDict[TableColumnName]);
                    List<object> filterNew = (List<object>)(PolicyFilterNewDict[TableColumnName]);
                    List<string> listNew = filterNew
                        .Select(x => x.ToString())
                        .ToList();

                    IEnumerable<string> deleteList = filterOld.Except(listNew);
                    IEnumerable<string> insertList = listNew.Except(filterOld);

                    foreach (var element in deleteList)
                    {
                        var sql = $@"DELETE from PolicyFilters 
                                    WHERE PolicyID = @PolicyID 
                                    AND TableColumnName = @TableColumnName
                                    AND TableColumeValue = @TableColumeValue";

                        using (new BtSqlMonitor(sql, new StackTrace()))
                        {
                            con.Query<string>(sql, new
                            {
                                PolicyID = PolicyID,
                                TableColumnName = TableColumnName,
                                TableColumeValue = element
                            });
                        }
                    }


                    foreach (var element in insertList)
                    {
                        var sql = $@"INSERT INTO PolicyFilters ([TableColumnName],[TableColumeValue],[PolicyID])
                                    VALUES (@TableColumnName, @TableColumeValue, @PolicyID)";

                        using (new BtSqlMonitor(sql, new StackTrace()))
                        {
                            con.Query<string>(sql, new
                            {
                                PolicyID = PolicyID,
                                TableColumnName = TableColumnName,
                                TableColumeValue = element
                            });
                        }
                    }
                }
            }
            return result;
        }

        public dynamic GetPolicyFilterValues()
        {
            dynamic result = new ExpandoObject();
            var sqlOSTypesList = "SELECT [Name] FROM OSTypes group by name";
            var sqlComputerModelManufacturerList = "SELECT Manufacturer as name from ComputerModels group by Manufacturer";
            var sqlOUList = "SELECT name from OUs group by name";
            var sqlComputerModelBiosList = "SELECT Manufacturer as name from ComputerBios group by Manufacturer";
            var sqlComputerModelList = "SELECT model as name from ComputerModels group by model";

            using (var con = GetConnection())
            {
                con.Open();
                result.OSTypesList = con.Query<string>(sqlOSTypesList);
                result.ComputerModelManufacturerList = con.Query<string>(sqlComputerModelManufacturerList);
                result.OUList = con.Query<string>(sqlOUList);
                result.ComputerModelBiosList = con.Query<string>(sqlComputerModelBiosList);
                result.ComputerModelList = con.Query<string>(sqlComputerModelList);

                result.isMobile = new List<string>();
                result.isMobile.Add("Notebook");
                result.isMobile.Add("Stationary");
            }
            return result;
        }

        /// <summary>
        /// Gibt die Policy zurück die vom Dienst auf diesen Computer angewendet wird.
        /// </summary>
        /// <param name="computerID">ID des Computers</param>
        /// <returns>Gibt ein Datensatz der Tabelle Policies zurück.</returns>
        public dynamic GetAppliedPolicy(long computerID)
        {
            using (var con = GetConnection())
            {
                var sql = $"SELECT * FROM Computers WHERE ComputerID = {computerID}";
                var resultStep1 = con.Query<dynamic>(sql);

                var resultStep2 = GetComputerInfo(computerID);
                foreach (var polItem in GetPoliciesFilter().GroupBy(c => c.PolicyID))
                {
                    if (resultStep2.Any())
                    {
                        if (MatchComputerToPolicy(GetComputerInfo(computerID).First(), polItem.ToList()))
                            return GetPolicyById(polItem.Key, "filter");
                    }
                }

                sql = @"SELECT * FROM Policies WHERE FallBackPolicy = 1";
                var resultStep3 = con.Query<dynamic>(sql);
                if (resultStep3.Any())
                    return GetPolicyById((int)resultStep3.First().PolicyID, "fallback");
            }
            return null;
        }

        /// <summary>
        /// Sucht einen bestimmten Datensatz der Tabelle Policies bei dem die PolicyID gleicht der Übergabe ist.
        /// </summary>
        /// <param name="polId">PolicyID nach der gefiltert wird.</param>
        /// <returns>Gibt ein Datensatz der Tabelle Policies zurück.</returns>
        public dynamic GetPolicyById(int PolicyID, string extension = "")
        {
            using (var con = GetConnection())
            {
                var sql = "SELECT * FROM Policies WHERE PolicyID = @PolicyID";
                if (extension.Length > 0)
                {
                    var result = con.Query<dynamic>(sql, new { PolicyID = PolicyID });
                    result.ToList()[0].Name = result.ToList()[0].Name.ToString() + $" ({extension})";
                    return result;
                }
                else
                    return con.Query<dynamic>(sql, new { PolicyID = PolicyID });
            }
        }

        /// <summary>
        /// Gibt die Policies angreichert mit den PolicyFilters zurück.
        /// </summary>
        /// <returns>Eine Liste von Policies mit den dazugehörigen PolicyFilters</returns>
        public IEnumerable<Policy> GetPoliciesFilter()
        {
            using (var con = GetConnection())
            {
                var sql = @"SELECT 
                                Policies.PolicyID,
                                Policies.Name,
                                PolicyFilters.TableColumnName,
                                PolicyFilters.TableColumeValue
                            FROM Policies
                                INNER JOIN PolicyFilters ON Policies.PolicyID = PolicyFilters.PolicyID
                            WHERE 
                                FallBackPolicy <> 1";

                return con.Query<Policy>(sql);
            }
        }

        /// <summary>
        /// Gibt einen Computer zurück an den Daten wie Model, Bios, OUs und OSType angreichert wurde.
        /// </summary>
        /// <param name="cmpId">ComputerID nach der in der Computers Tabelle gesucht wird.</param>
        /// <returns>Liste der der gefundenen Treffe. Es sollte maximal einen Datensatz enthalten.</returns>
        public IEnumerable<ComputerInfoModel> GetComputerInfo(long ComputerID)
        {
            using (var con = GetConnection())
            {
                var sql = $@"SELECT
                            Computers.ComputerID,
                            Computers.ComputerName,
                            Computers.OSTypeID,
                            OSTypes.Name AS OSTypeName,
                            ComputerModels.Model AS ComputerModel,
                            Computers.IsMobile AS ComputerIsMobile,
                            ComputerModels.Manufacturer AS ComputerModelManufacturer,
                            ComputerBios.Manufacturer AS ComputerBiosManufacturer,
                            OUs.Name AS OUName
                        FROM Computers
                        LEFT JOIN OSTypes ON Computers.OSTypeID = OSTypes.OSTypeID
                        LEFT JOIN ComputerModels ON Computers.ComputerModelID = ComputerModels.ComputerModelID
                        LEFT JOIN ComputerBios ON Computers.ComputerBiosID = ComputerBios.ComputerBiosID
                        LEFT JOIN OUs ON Computers.OUID = OUs.OUID
                        WHERE
                            ComputerID = @ComputerID";

                return con.Query<ComputerInfoModel>(sql, new { ComputerID = ComputerID });
            }
        }

        /// <summary>
        /// Prüft ob die übergebene Policy Liste filter enthällt die beim übergebenen Computerdatensatz 
        /// Treffer auslösen.
        /// </summary>
        /// <param name="comp">Computer bei dem im Model, Bios, OUs und OSType nach Übereinstimmungen gesucht wird.</param>
        /// <param name="pol">Liste der Filter einer Policy.</param>
        /// <returns></returns>
        bool MatchComputerToPolicy(ComputerInfoModel comp, List<Policy> pol)
        {
            var result = true;
            foreach (PropertyInfo property in comp.GetType().GetProperties())
            {
                var matches = pol.Where(c => c.TableColumnName.ToLower().Equals(property.Name.ToLower()));
                if (matches.Any())
                {
                    if (matches.Count() == 1)
                    {
                        if (property.GetValue(comp, null) == null || !property.GetValue(comp, null).ToString().Contains(matches.First().TableColumeValue))
                            return false;
                    }
                    else
                    {
                        var re = false;
                        foreach (var match in matches)
                        {
                            if (property.GetValue(comp, null) != null && property.GetValue(comp, null).ToString().Contains(match.TableColumeValue))
                            {
                                re = true;
                                break;
                            }
                        }
                        if (!re)
                            return false;
                    }
                }
            }
            return result;
        }

    }
}
