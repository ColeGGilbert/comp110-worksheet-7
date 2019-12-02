using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comp110_worksheet_7
{

	// By Cole Gilbert

	public static class DirectoryUtils
	{
		// Return the size, in bytes, of the given file
		public static long GetFileSize(string filePath)
		{
			return new FileInfo(filePath).Length;
		}

		// Return true if the given path points to a directory, false if it points to a file
		public static bool IsDirectory(string path)
		{
			return File.GetAttributes(path).HasFlag(FileAttributes.Directory);
		}

		// Return the total size, in bytes, of all the files below the given directory
		public static long GetTotalSize(string directory)
		{
			long totalSize = 0;
			// Gets all files in the directory
			string[] filePaths = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
			// Iterates through each item in the array
			for (int i = 0; i < filePaths.Length; i++)
			{
				// Adds the size of each item to the total size
				totalSize += new FileInfo(filePaths[i]).Length;
			}
			// Returns the final total for totalSize
			return totalSize;
		}

		// Return the number of files (not counting directories) below the given directory
		public static int CountFiles(string directory)
		{
			// Get all files in the directory
			string[] filePaths = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
			// Returns the size of the array
			return filePaths.Length;
		}

		// Return the nesting depth of the given directory. A directory containing only files (no subdirectories) has a depth of 0.
		public static int GetDepth(string directory, int currentDepth = 0)
		{
			// Defines a new list that is unique to this run of the function
			List<int> depthValues = new List<int>();

			// Each directory is collected for use in iteration of this current run of the function
			string[] directoryDepth = Directory.GetDirectories(directory);

			// If the directory has more sub directories, then the current depth is incremented by 1
			if(directoryDepth.Length > 0)
			{
				currentDepth++;
			}
			// Loops through each of the subdirectories of this current run of the function
			for(int i = 0; i < directoryDepth.Length; i++)
			{
				// Adds the currentDepth values of each subdirectory to a list
				depthValues.Add(GetDepth(directoryDepth[i], currentDepth));
			}
			// For each of the items in the depthValues list...
			for(int i = 0; i < depthValues.Count; i++)
			{
				// If the value is more than the currentDepth for the original directory path
				if (depthValues[i] > currentDepth)
				{
					// It becomes the new currentDepth value
					currentDepth = depthValues[i];
				}
			}
			
			return currentDepth;
		}

		// Get the path and size (in bytes) of the smallest file below the given directory
		public static Tuple<string, long> GetSmallestFile(string directory)
		{
			// Get all filePaths in the directory
			string[] filePaths = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
			string smallestFile = "";

			// Iterates through every item in filePaths
			for(int i = 0; i < filePaths.Length; i++)
			{
				// The first item is assigned as the smallestFile
				if (smallestFile == "")
				{
					smallestFile = filePaths[i];
				}
				// Each next file is checked against the size of the current smallestFile
				else if (new FileInfo(filePaths[i]).Length < new FileInfo(smallestFile).Length)
				{
					// If the item was smaller, it becomes the new smallestFile
					smallestFile = filePaths[i];
				}
			}
			// Returns the path and size as a Tuple
			return new Tuple<string, long>(smallestFile, new FileInfo(smallestFile).Length);
		}

		// Get the path and size (in bytes) of the largest file below the given directory
		public static Tuple<string, long> GetLargestFile(string directory)
		{
			// This function works the same as GetSmallestFile, however naming is changed over and ">" is used over "<"
			string[] filePaths = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);
			string largestFile = "";
			for (int i = 0; i < filePaths.Length; i++)
			{
				if (largestFile == "")
				{
					largestFile = filePaths[i];
				}
				else if (new FileInfo(filePaths[i]).Length > new FileInfo(largestFile).Length)
				{
					largestFile = filePaths[i];
				}
			}
			return new Tuple<string, long>(largestFile, new FileInfo(largestFile).Length);
		}

		// Get all files whose size is equal to the given value (in bytes) below the given directory
		public static IEnumerable<string> GetFilesOfSize(string directory, long size)
		{
			string[] filePaths = Directory.GetFiles(directory, "*", SearchOption.AllDirectories);

			// For each file path in the directory
			foreach (string i in filePaths)
			{
				// Check if they are the requested size in bytes
				if(new FileInfo(i).Length == size)
				{
					// If so, return the filePath and continue the foreach loop
					yield return i;
				}
			}
		}
	}
}
