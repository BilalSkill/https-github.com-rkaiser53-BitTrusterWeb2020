using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BitTrusterWebApi.Helper
{
    public class BtConst
    {
        public const string ClaimType = "btClaimType";
        public enum BtPerm
        {

            None = 0,
            ManagementCenterTabAccess = 1,
            Recovery = 2,
            ComputerManagement = 3,
            PINfunctions = 10,
            RecoveryPasswordfunctions = 11,
            StolenComputerFunctions = 12,
            UserToComputerAssignmentFunctions = 13,
            AllowDashboardTab = 100,
            AllowRecoveryTab = 101,
            AllowAuditLogTab = 102,
            AllowPermissionsTab = 103,
            AllowADImportTab = 104,
            AllowPoliciesTab = 105,
            AllowReportingTab = 106,
            AllowGeneralSettingsTab = 107,
            AllowTestpageTab = 110,
            ShowPIN = 200,
            SendPINviaEmail = 201,
            ShowPassphrase = 202,
            SendPassphraseViaEmail = 203,
            ShowRecoveryPassword = 210,
            SendRecoveryPasswordViaEmail = 211,
            ShowTPMOwnerPassword = 220,
            ExportKeyPackage = 230,
            RecoveryOfStolenComputers = 240,
            ChangeManualPolicyAssignment = 300,
            AddUserToComputer = 310,
            RemoveUserFromComputer = 311,
            DeleteComputer = 320,
            SetStolenFlag = 330,
            RemoveStolenFlag = 331,
            AllowToViewThePIN = 1000,
            AllowToChangeThePINforCurrentComputer = 1001,
            AllowToViewThePassphrase = 1002,
            AllowToChangeThePassphrase = 1003,
            AllowToViewRecoveryPassword = 1100,
            AllowToMarkaComputerAsStolen = 1200,
            AllowRecoverOfStolenComputers = 1201,
            ShowStolenComputer = 1202,
            AllowToRemoveOtherUsers = 1300,
            AllowTheUserToRemoveHimselfFromComputer = 1301
        }

        // verwendete ActionIDs
        public enum BtActionID
        {
            unset = 0,

            show_pin = 35,
            send_recovery_pin = 28,

            show_Passphrase = 53,
            send_recovery_Passphrase = 52,

            show_np = 36,
            send_recovery_password = 27,

            show_tpm_owner_pwd = 39,

            add_user = 40,
            remove_user = 41,

            stolenFlag = 43,
            stolenFlagRemoved = 44,

            keyPackage = 1018,

            LogonFailed = 46,
            LogonAdmin = 47,
            ComputerDelete = 48,
            logon_successful = 1000,
            logon_failed_user_in_bt_but_inavtive = 1004,
            view_PIN = 1010,
            user_view_recovery_password = 1012,
        }


        public enum BtActionID_All
        {
            reserved = 0,
            full_update = 1,
            drive_info = 2,
            conversionstatus = 3,
            protectionstatus = 4,
            hw_test_status = 5,
            TPM_info = 6,
            logged_on_user = 7,
            protectors = 8,
            specific_protector = 9,
            gen_PIN = 10,
            send_mail = 11,
            ativate_TPM = 12,
            add_np_get_np = 13,
            add_TPM = 14,
            add_TPM_PIN = 15,
            add_TPM_PIN_StartupKey = 16,
            add_StartupKey = 17,
            add_certificate_file = 18,
            add_certificate_thumbprint = 19,
            rm_all_protectors = 20,
            rm_specific_protector = 21,
            enc = 22,
            dec = 23,
            pause_enc = 24,
            resume_enc = 25,
            wol = 26,
            send_recovery_password = 27,
            send_recovery_pin = 28,
            encrypt_other_volumes = 29,
            change_np = 30,
            change_pin = 31,
            switch_auth = 32,
            report = 33,
            add_certificate = 34,
            show_pin = 35,
            show_np = 36,
            convertToNtfs = 37,
            createBlPartition = 38,
            add_user = 40,
            remove_user = 41,
            OSreinstall = 42,
            stolenFlag = 43,
            stolenFlagRemoved = 44,
            keyPackage = 45,
            LogonFailed = 46,
            LogonAdmin = 47,
            ComputerDelete = 48,
            show_pin_no_change = 49,
            add_Passphrase = 50,
            change_Passphrase = 51,
            logon_successful = 1000,
            logon_failed_adpwd_wrong = 1001,
            logon_failed_adusr_notfound = 1002,
            logon_failed_user_not_in_bt = 1003,
            logon_failed_user_in_bt_but_inavtive = 1004,
            logon_failed_database_connection_error = 1005,
            logon_failed_unknown_error = 1006,
            logon_failed_ad_not_available = 1007,
            view_PIN = 1010,
            user_change_PIN = 1011,
            user_view_recovery_password = 1012,
            user_mark_as_stolen = 1013,
            remove_other_users = 1014,
            remove_himself = 1015,
            user_change_Passphrase = 1016,
            view_Passphrase = 1017,
        }

    }
}
