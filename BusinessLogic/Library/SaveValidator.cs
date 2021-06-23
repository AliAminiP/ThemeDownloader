using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BusinessLogic.Library
{
    public static class SaveValidator
    {
        public static bool IsFileNameValid(string fileName, out bool isEmpty)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                isEmpty = true;
                return false;
            }
            isEmpty = false;

            return fileName.All(f => !Path.GetInvalidFileNameChars().Contains(f));
        }
        public static bool IsPathNameValid(string pathName, out bool isEmpty)
        {
            if (string.IsNullOrEmpty(pathName))
            {
                isEmpty = true;
                return false;
            }
            isEmpty = false;

            return pathName.All(f => !Path.GetInvalidPathChars().Contains(f));
        }
        public static string MakePathNameValid(string pathName)
        {
            if (!IsPathNameValid(pathName, out bool isEmpty))
            {
                if (isEmpty)
                {
                    pathName = "New Folder 1";
                    return pathName;
                }
                foreach (char c in Path.GetInvalidPathChars())
                {
                    pathName = pathName.Replace(c, '_');
                }
            }
            return pathName;
        }
        public static string MakeFileNameValid(string fileName)
        {
            if (!IsFileNameValid(fileName, out bool isEmpty))
            {
                if (isEmpty)
                {
                    fileName = "New File 1";
                    return fileName;
                }
                foreach (char c in Path.GetInvalidFileNameChars())
                {
                    fileName = fileName.Replace(c, '_');
                }
            }
            return fileName;
        }
    }
}
