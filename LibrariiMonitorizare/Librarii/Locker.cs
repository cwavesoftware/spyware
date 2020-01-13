using System;
using System.IO;
using System.Security.AccessControl;

namespace Monitorizare
{
    public static class Locker
    {
        public static void AddDirSecurity(string dirname, string account, FileSystemRights rights, AccessControlType controlType)
        {
            if (Directory.Exists(dirname))
            {
                // Get a FileSecurity object that represents the 
                // current security settings.
                DirectorySecurity dirsec = Directory.GetAccessControl(dirname);
                // Add the FileSystemAccessRule to the security settings.
                dirsec.AddAccessRule(new FileSystemAccessRule(account, rights, controlType));
                // Set the new access settings.
                Directory.SetAccessControl(dirname, dirsec);
            }
        }
        public static void RemoveDirSecurity(string dirname, string account, FileSystemRights rights, AccessControlType controlType)
        {
            if (Directory.Exists(dirname))
            {
                // Get a FileSecurity object that represents the 
                // current security settings.
                DirectorySecurity dirsec = Directory.GetAccessControl(dirname);
                // Remove the FileSystemAccessRule from the security settings.
                dirsec.RemoveAccessRule(new FileSystemAccessRule(account, rights, controlType));
                // Set the new access settings.
                Directory.SetAccessControl(dirname, dirsec);
            }
        }
        // Locks application root directory
        public static void Lock(DirectoryInfo directory, string username)
        {
            AddDirSecurity(directory.FullName, username, FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Deny);
            AddDirSecurity(directory.FullName, username, FileSystemRights.Read, AccessControlType.Deny);
            AddDirSecurity(directory.FullName, username, FileSystemRights.ChangePermissions, AccessControlType.Deny);
        }
        // Unlocks application root directory
        public static void Unlock(DirectoryInfo directory, string username)
        {
            RemoveDirSecurity(directory.FullName, username, FileSystemRights.DeleteSubdirectoriesAndFiles, AccessControlType.Deny);
            RemoveDirSecurity(directory.FullName, username, FileSystemRights.Read, AccessControlType.Deny);
            RemoveDirSecurity(directory.FullName, username, FileSystemRights.ChangePermissions, AccessControlType.Deny);
        }
    }
}
