using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
namespace Helpers
{
    public static class FileOperations
    {
		public static string ReadAllFromFile( string path )
		{
			if (string.IsNullOrWhiteSpace(path)) return string.Empty;
			 string text = File.ReadAllText(path);
			return text;
		}

		public static void AppendToFile( string path, string stringToWrite, bool addNewLine = false )
		{
			if (string.IsNullOrWhiteSpace(path)) return;
			File.AppendAllText( path , 
				string.Format("{0}{1}", stringToWrite, addNewLine ? Environment.NewLine : string.Empty));
		}

		public static void WriteListToFile(string path, List<string> stringListToWrite)
	    {
			try
			{
			   File.WriteAllLines( path, stringListToWrite);
			}
			catch (Exception exception)
			{
				"Error occured while trying to write to file: [ Path = {0}, Error = {1} ]".ThrowFormattedException(new object[] { path, exception.Message});
			}
	    }

		public static void MoveFile( string sourcePath, string targetPath, string fileName )
		{
			// Use Path class to manipulate file and directory paths. 
			string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
			string destFile = System.IO.Path.Combine(targetPath, fileName);

			// To copy a folder's contents to a new location: Create a new target folder, if necessary. 
			if (!System.IO.Directory.Exists(targetPath))
				System.IO.Directory.CreateDirectory(targetPath);

			try
			{
				System.IO.File.Move(sourceFile, destFile);
			}
			catch (Exception ex)
			{
				throw new ApplicationException( 
					string.Format( "Exception occured while attempting to move file [ Source = '{0}', Target = '{1}', File = '{2}', Exception = {3} ]",
					sourcePath, targetPath, fileName, ex.Message));
			}
		}

		public static void CopyFile( string sourcePath, string targetPath, string newFileName )
		{
			// Use Path class to manipulate file and directory paths. 
			//string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
			string filename = sourcePath.Split(new[] {'\\'}).LastOrDefault();
			if ( string.IsNullOrEmpty( filename ) ) return;


			string destFile = System.IO.Path.Combine(targetPath, string.IsNullOrEmpty(newFileName) ? filename : newFileName);

			// To copy a folder's contents to a new location: Create a new target folder, if necessary. 
			if (!System.IO.Directory.Exists( targetPath ) )
				System.IO.Directory.CreateDirectory(targetPath);

			try
			{
				if (System.IO.File.Exists(destFile)) System.IO.File.Delete(destFile);
				System.IO.File.Copy(sourcePath, destFile);
			}
			catch (Exception ex)
			{
				throw new ApplicationException( 
					string.Format( "Exception occured while attempting to copy file [ Source = '{0}', Target = '{1}', File = '{2}', Exception = {3} ]",
					sourcePath, targetPath, filename, ex.Message));
			}
		}

		//public static void MoveAndRenameFile( string fullSourcePath, string targetPath, string newFileName )
		//{
		//	// Use Path class to manipulate file and directory paths. 
		//	string destFile = System.IO.Path.Combine(targetPath, newFileName);

		//	// To copy a folder's contents to a new location: Create a new target folder, if necessary. 
		//	if (!System.IO.Directory.Exists(targetPath))
		//		System.IO.Directory.CreateDirectory(targetPath);

		//	try
		//	{
		//		System.IO.File.Move(fullSourcePath, destFile);
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new ApplicationException( 
		//			string.Format( "Exception occured while attempting to move file [ Source = '{0}', Target = '{1}', New File Name = '{2}', Exception = {3} ]",
		//			fullSourcePath, targetPath, newFileName, ex.Message));
		//	}
		//}

		public static void CopyAllFiles( string sourcePath, string targetPath  )
		{
			// To copy all the files in one directory to another directory. 
        // Get the files in the source folder. (To recursively iterate through 
        // all subfolders under the current directory, see 
        // "How to: Iterate Through a Directory Tree.")
        // Note: Check for target path was performed previously 
        //       in this code example. 
		if (!System.IO.Directory.Exists(sourcePath))
			throw new ApplicationException(string.Format("Source path [ {0} ] does not exist!", sourcePath));

		string[] files = System.IO.Directory.GetFiles(sourcePath);

		// Copy the files and overwrite destination files if they already exist. 
		foreach (string s in files)
		{
			// Use static Path methods to extract only the file name from the path.
			string fileName = System.IO.Path.GetFileName(s);
			string destFile = System.IO.Path.Combine(targetPath, fileName);
			System.IO.File.Copy(s, destFile, true);
		}
			
		}

		public static void DeleteFile( string fullPathToDelete)
		{
			// Delete a file by using File class static method... 
			if (!System.IO.File.Exists(fullPathToDelete)) return;
			
			try
			{
				System.IO.File.Delete(fullPathToDelete);
			}
			catch (System.IO.IOException e)
			{
				throw new ApplicationException(string.Format("Error while trying to delete file [ File = '{0}', Exception = '{1}' ]",
				                                             fullPathToDelete, e.Message));
			}

			// ...or by using FileInfo instance method.
		//System.IO.FileInfo fi = new System.IO.FileInfo(@"C:\Users\Public\DeleteTest\test2.txt");
		//try
		//{
		//	fi.Delete();
		//}
		//catch (System.IO.IOException e)
		//{
		//	Console.WriteLine(e.Message);
		//}

		//// Delete a directory. Must be writable or empty. 
		//try
		//{
		//	System.IO.Directory.Delete(@"C:\Users\Public\DeleteTest");
		//}
		//catch (System.IO.IOException e)
		//{
		//	Console.WriteLine(e.Message);
		//}
		//// Delete a directory and all subdirectories with Directory static method... 
		//if(System.IO.Directory.Exists(@"C:\Users\Public\DeleteTest"))
		//{
		//	try
		//	{
		//		System.IO.Directory.Delete(@"C:\Users\Public\DeleteTest", true);
		//	}

		//	catch (System.IO.IOException e)
		//	{
		//		Console.WriteLine(e.Message);
		//	}
		//}

		//// ...or with DirectoryInfo instance method.
		//System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(@"C:\Users\Public\public");
		//// Delete this dir and all subdirs. 
		//try
		//{
		//	di.Delete(true);
		//}
		//catch (System.IO.IOException e)
		//{
		//	Console.WriteLine(e.Message);
		//}
		}

		public static void RenameFile ( string sourcePath, string fileName, string newFileName)
		{
			string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
			string destFile = System.IO.Path.Combine(sourcePath, newFileName);
			try
			{
				System.IO.File.Move(sourceFile, destFile);
			}
			catch (Exception ex)
			{
				throw new ApplicationException( 
					string.Format( "Exception occured while attempting to rename file [ Source = '{0}', Target = '{1}', Exception = {2} ]",
					sourcePath, destFile, ex.Message));
			}
		}

		public static List<string> GetAllFilesFromDirectory( string directoryPath )
		{
			//string[] dirs = Directory.GetFiles(@"c:\", "c*"); // Starts with

			return Directory.GetFiles(directoryPath,  "*.mp3", SearchOption.AllDirectories).ToList(); // all files

			//string[] filePaths = Directory.GetFiles(@"c:\MyDir\", "*.bmp"); .. all files with extension
			
			// All files with extension include subdirectories
			//string[] filePaths = Directory.GetFiles(@"c:\MyDir\", "*.bmp", SearchOption.AllDirectories); 

			
		}

	    public static int GetFileCount(string directoryPath, string pattern)
	    {

		    return Directory.GetFiles(directoryPath, pattern).Count();

	    }


    }
}
