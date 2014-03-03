using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace rxfind {
    class Program {
        static void Main(string[] args) {

            bool recursive = false;
            bool help = false;
            bool includeFileName = false;
            bool includeLineNum = false;
            bool quiet = false;
            RegexOptions regexOption = RegexOptions.None;
            int bakOperation = 0;
            bool outputOnly = false;
            string pattern = "";
            string replacement = null;
            string[] fileList = { };

            #region - Parse args -

            // Parse args
            if (args.Length <= 1) { help = true; } else {
                foreach (string arg in args) {
                    switch (arg.ToLower()) {
                        case "/?": help = true; return;
                        case "/h": help = true; return;
                        case "/s": recursive = true; break;
                        case "/f": includeFileName = true; break;
                        case "/ln": includeLineNum = true; break;
                        case "/q": quiet = true; break;
                        case "/i": regexOption = regexOption | RegexOptions.IgnoreCase; break;
                        case "/sl": regexOption = regexOption | RegexOptions.Singleline; break;
                        case "/b:1": bakOperation = 1; break;
                        case "/b:2": bakOperation = 2; break;
                        case "/o": outputOnly = true; break;
                        default:
                            if (arg.ToLower().StartsWith("/p:") && arg.Length > 3) { pattern = arg.Substring(3); }
                            else if (arg.ToLower().StartsWith("/r:") && arg.Length > 3) { replacement = arg.Substring(3); }
                            else if (arg.ToLower().StartsWith("/r:") && arg.Length == 3) { replacement = ""; }
                            break;
                    }
                }
            }

            #endregion

            if (help) {
                DisplayHelp();
                return;
            } else if (pattern != "") {

                if (File.Exists(args[0])) {

                    // Search just 1 file
                    fileList = new string[] { args[0] };

                } else if (args[0].LastIndexOf(@"\") <= 0 && args[0].IndexOf("*") > -1) {

                    // Wildcard current directory
                    fileList = Program.GetFiles(".", args[0], recursive);

                } else if (args[0].LastIndexOf(@"\") > 0) {

                    // Parse directory name and search Wildcard file
                    string _dir = args[0].Substring(0, args[0].LastIndexOf(@"\"));
                    string _file = args[0].Substring(args[0].LastIndexOf(@"\") + 1);
                    if (Directory.Exists(_dir) && _file.IndexOf("*") != -1) {
                        fileList = Program.GetFiles(_dir, _file, recursive);
                    }

                }

            }

            if (fileList.Length > 0) {

                foreach (string fileName in fileList) {

                    string oldFileData = File.ReadAllText(fileName);
                    StringBuilder newFileData = new StringBuilder("");
                    
                    int index = 0;

                    // Setup Regex
                    Match match = null;
                    try {
                        match = Regex.Match(oldFileData, pattern, regexOption);
                    } catch {
                        Console.WriteLine("Regular Expression '{0}' is invalid.", pattern);
                        return;
                    }

                    if (match.Success) {
                        StreamWriter outStream = null;
                        int lastStartLf = -1; // used to prevent multiples on the same line being output.

                        // Create output stream?
                        if (!outputOnly && replacement != null) { outStream = CreateOutputStream(fileName, bakOperation); }

                        while (match.Success) {

                            // output to console?
                            if (!quiet) {

                                // find the start and end of the line
                                int startLf = oldFileData.LastIndexOf('\n', match.Index); startLf = (startLf == -1) ? 0 : startLf + 1;
                                int endLf = oldFileData.IndexOf('\n', match.Index + match.Length); if (endLf == -1) endLf = oldFileData.Length;

                                // do we display it?
                                if (startLf < endLf && startLf != lastStartLf) {
                                    string line = oldFileData.Substring(startLf, endLf - startLf);
                                    if (replacement != null) line = Regex.Replace(line, pattern, replacement, regexOption);
                                    lastStartLf = startLf;

                                    // include file name?
                                    string prefix = String.Empty;
                                    if(includeFileName)
                                        prefix = String.Format("{0}:", fileName);
                                    if(includeLineNum)
                                    {
                                        int lineNum = GetLineNum(ref oldFileData, match.Index);
                                        prefix = String.Format("{0}{1}:", prefix, lineNum);
                                    }
                                    Console.WriteLine("{0} {1}", prefix, line);
                                }
                            }

                            // write to output stream?
                            if (outStream != null) {
                                outStream.Write(oldFileData.Substring(index, match.Index - index));
                                outStream.Write(Regex.Replace(match.Value, pattern, replacement, regexOption));
                                index = match.Index + match.Length;
                            }

                            match = match.NextMatch();
                        }

                        // close temp file and perform renames?
                        if (outStream != null) {
                            outStream.Write(oldFileData.Substring(index));
                            outStream.Close();

                            if (bakOperation == 1 && File.Exists(fileName + ".bak")) File.Delete(fileName + ".bak");
                            if (bakOperation == 2) { File.Delete(fileName); }
                            else { File.Move(fileName, fileName + ".bak"); }

                            File.Move(fileName + ".tmp", fileName);
                        }
                    }

                }

            } else if (pattern == "") {
                Console.WriteLine("Search Pattern Not Found.");
            } else {
                Console.WriteLine("File Not Found.");
            }

        }

        private static int GetLineNum(ref string text, int index)
        {
            int lineNum = 1;
            if(!String.IsNullOrEmpty(text))
            {
                foreach(char ch in text)
                {
                    if(index > 0)
                        index--;
                    else
                        return lineNum;

                    if(ch == '\n')
                        lineNum++;
                }
            }
            return lineNum;
        }

        #region - Private Methods (CreateOutputStream, GetFiles, DisplayHelp) -

        /// <summary>Creates temporary file to write to.</summary>
        /// <returns>Either a StreamWriter object or null depending upon conditions.</returns>
        private static StreamWriter CreateOutputStream(string fileName, int bakOperation) {
            StreamWriter value = null;
            
            if (File.Exists(fileName + ".tmp")) {
                Console.WriteLine("Error: Temp file {0}.tmp could not be created.", fileName);
            } else if (bakOperation == 0 && File.Exists(fileName + ".bak")) {
                Console.WriteLine("Error: Backup file {0}.bak could not be created.", fileName);
            } else {
                value = File.CreateText(fileName + ".tmp");
            }

            return value;
        }

        /// <summary>Recursive method retreives a list of files to parse.</summary>
        private static string[] GetFiles(string directory, string file, bool recursive) {
            ArrayList value = new ArrayList();

            string[] files = Directory.GetFiles(directory, file);
            foreach (string fileName in files) { value.Add(fileName); }
            if (recursive) {
                string[] dirs = Directory.GetDirectories(directory);
                foreach (string dir in dirs) {
                    string[] recursiveFiles = GetFiles(dir, file, recursive);
                    foreach (string fileName in recursiveFiles) {
                        value.Add(fileName.StartsWith(@".\") ? fileName.Substring(2) : fileName);
                    }
                }
            }

            return (string[])value.ToArray(typeof(string));
        }

        /// <summary>Displays the help menu</summary>
        private static void DisplayHelp() {
            Console.WriteLine(
                "Performs a search and/or replace in a directory.\r\n\r\n" +
                "RXFIND [drive:][path]filename [/S] [/F] [/I] [/B:0|1|2] [/SL] [/O] [/Q] /P:[searchpattern] [/R:replacestring]\r\n\r\n" +
                "  [drive:][path]filename\r\n" +
                "              Specifies drive, directory, and files to list.\r\n\r\n" +
                "  /?          Displays this help file.\r\n" +
                "  /S          Search this directory and all subdirectories \r\n" +
                "  /P          Regex search pattern\r\n" +
                "  /R          Replacement string\r\n" +
                "  /I          Ignore case\r\n" +
                "  /SL         Single line pattern matching\r\n" +
                "  /F          Includes the filename in console output\r\n" +
                "  /LN         Includes the line number in console output\r\n" +
                "  /B:0        if .bak file exists no replace will be performed. (default)\r\n" +
                "  /B:1        if .bak file exists, it is overwritten\r\n" +
                "  /B:2        No .bak file is created\r\n" +
                "  /O          Console output only.  File is not modified.\r\n" +
                "  /Q          Quiet operation - No results output to console\r\b");
        }

        #endregion
    }
}


  