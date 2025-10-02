// converted from python by chatgpt
// required editing

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace SimpleHexViewer {

    public static class Toggle1k {

        private static void Mirror256(byte[] bytesArray) {
            for (int i = 0; i < bytesArray.Length; i++) {
                byte b = bytesArray[i];
                //b = (byte)(255 - b);
                b = (byte)(~b);
                bytesArray[i] = b;
            }
        }

        private static bool ContainsWildcard(string s) {
            return (s.Contains("*") || s.Contains("?"));
        }

        public static void Do1k(string filename) {
            string filePath = Path.GetFullPath(filename);
            if (File.Exists(filePath)) { //not required 
                using (FileStream originalFile = new FileStream(filePath, 
                                                     FileMode.Open,
                                                     FileAccess.ReadWrite))
                {
                    byte[] first1024Bytes = new byte[1024];
                    var bytesRead = originalFile.Read(first1024Bytes, 0, 1024);
                    // toggles the byte values (255 - byte)
                    Mirror256(first1024Bytes);
                    originalFile.Seek(0, SeekOrigin.Begin);
                    originalFile.Write(first1024Bytes, 0, bytesRead);
                }
            }

        }

    }
}