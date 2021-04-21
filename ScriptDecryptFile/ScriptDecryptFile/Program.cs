
using System;
using System.IO;

string path = @"D:\LearningPaths\DotNet\ScriptDecryptFile\ScriptDecryptFile\testFile.txt";
File.Encrypt(path);

FileInfo test = new FileInfo(path);
Console.WriteLine(File.ReadAllText(test.FullName));

Console.WriteLine("Hello World! " + test.FullName);
