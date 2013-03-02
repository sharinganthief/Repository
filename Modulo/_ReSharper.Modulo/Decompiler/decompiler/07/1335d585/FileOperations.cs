// Type: Helpers.FileOperations
// Assembly: Helpers, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Users\sharbison\Documents\GitHub\Repository\Helpers\obj\Debug\Helpers.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Helpers
{
  public static class FileOperations
  {
    public static string ReadAllFromFile(string path)
    {
      if (string.IsNullOrWhiteSpace(path))
        return string.Empty;
      else
        return File.ReadAllText(path);
    }

    public static void AppendToFile(string path, string stringToWrite, bool addNewLine = false)
    {
      if (string.IsNullOrWhiteSpace(path))
        return;
      File.AppendAllText(path, string.Format("{0}{1}", (object) stringToWrite, addNewLine ? (object) Environment.NewLine : (object) string.Empty));
    }

    public static void WriteListToFile(string path, List<string> stringListToWrite)
    {
      try
      {
        File.WriteAllLines(path, (IEnumerable<string>) stringListToWrite);
      }
      catch (Exception ex)
      {
        ExceptionHelpers.ThrowFormattedException("Error occured while trying to write to file: [ Path = {0}, Error = {1} ]", (object) path, (object) ex.Message);
      }
    }

    public static void MoveFile(string sourcePath, string targetPath, string fileName)
    {
      string sourceFileName = Path.Combine(sourcePath, fileName);
      string destFileName = Path.Combine(targetPath, fileName);
      if (!Directory.Exists(targetPath))
        Directory.CreateDirectory(targetPath);
      try
      {
        File.Move(sourceFileName, destFileName);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format("Exception occured while attempting to move file [ Source = '{0}', Target = '{1}', File = '{2}', Exception = {3} ]", (object) sourcePath, (object) targetPath, (object) fileName, (object) ex.Message));
      }
    }

    public static void CopyFile(string sourcePath, string targetPath, string newFileName)
    {
      string str1 = Enumerable.LastOrDefault<string>((IEnumerable<string>) sourcePath.Split(new char[1]
      {
        '\\'
      }));
      if (string.IsNullOrEmpty(str1))
        return;
      string str2 = Path.Combine(targetPath, string.IsNullOrEmpty(newFileName) ? str1 : newFileName);
      if (!Directory.Exists(targetPath))
        Directory.CreateDirectory(targetPath);
      try
      {
        if (File.Exists(str2))
          File.Delete(str2);
        File.Copy(sourcePath, str2);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format("Exception occured while attempting to copy file [ Source = '{0}', Target = '{1}', File = '{2}', Exception = {3} ]", (object) sourcePath, (object) targetPath, (object) str1, (object) ex.Message));
      }
    }

    public static void CopyAllFiles(string sourcePath, string targetPath)
    {
      if (!Directory.Exists(sourcePath))
        throw new ApplicationException(string.Format("Source path [ {0} ] does not exist!", (object) sourcePath));
      foreach (string str in Directory.GetFiles(sourcePath))
      {
        string fileName = Path.GetFileName(str);
        string destFileName = Path.Combine(targetPath, fileName);
        File.Copy(str, destFileName, true);
      }
    }

    public static void DeleteFile(string fullPathToDelete)
    {
      if (!File.Exists(fullPathToDelete))
        return;
      try
      {
        File.Delete(fullPathToDelete);
      }
      catch (IOException ex)
      {
        throw new ApplicationException(string.Format("Error while trying to delete file [ File = '{0}', Exception = '{1}' ]", (object) fullPathToDelete, (object) ex.Message));
      }
    }

    public static void RenameFile(string sourcePath, string fileName, string newFileName)
    {
      string sourceFileName = Path.Combine(sourcePath, fileName);
      string destFileName = Path.Combine(sourcePath, newFileName);
      try
      {
        File.Move(sourceFileName, destFileName);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format("Exception occured while attempting to rename file [ Source = '{0}', Target = '{1}', Exception = {2} ]", (object) sourcePath, (object) destFileName, (object) ex.Message));
      }
    }

    public static List<string> GetAllFilesFromDirectory(string directoryPath)
    {
      return Enumerable.ToList<string>((IEnumerable<string>) Directory.GetFiles(directoryPath, "*.mp3", SearchOption.AllDirectories));
    }

    public static int GetFileCount(string directoryPath, string pattern)
    {
      return Enumerable.Count<string>((IEnumerable<string>) Directory.GetFiles(directoryPath, pattern));
    }
  }
}
