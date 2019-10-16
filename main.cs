using System;
using System.IO;
using System.Collections.Generic;

namespace ccsvify
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length > 0)
			{
				string target_directory, error;
				string[] allfiles;
				bool quiet;
				if (parse_input(args, out target_directory, out allfiles, out quiet, out error))
				{
					if (!quiet) {Console.WriteLine("Outputting to " + target_directory + "\nConverting...");}
					foreach (string file in allfiles)
					{
						do_convert(file, target_directory, quiet);
					}
				}
				else
				{
					Console.WriteLine(error);
				}
			}
			else
			{
				Console.WriteLine("No file specified.");
			}
		}
		private static void do_convert(string input_filename, string target_directory, bool quiet)
		{
			string output_base_filename = get_output_base_filename(input_filename);
			output_base_filename = Path.Combine(target_directory,output_base_filename);
			Section[] data_sections = Section.ProcessFile(input_filename);
		    int number = data_sections.Length;
		    int k = 0;
			foreach (Section i in data_sections)
			{
				if (!quiet) {i.Summarize();Console.WriteLine();}
				string filename = (number > 1) ? (output_base_filename + match(k, number)) : output_base_filename;
				k++;
				File.WriteAllLines(filename + ".csv", i.GetContents());
			}
		}
		private static string match(int i, int n)
		{
			string output = "";
			int ct = n.ToString().Length;
			int cti = i.ToString().Length;
			while (output.Length + cti < ct) output += '0';
			return output + i.ToString();
		}
		private static bool parse_input(string[] args, out string target_directory, out string[] all_convert_files, out bool quiet, out string error)
		{
			quiet = false;
			error = "NONE";
			all_convert_files = new string[]{};
			target_directory = "./";
			List<string> all_input_files = new List<string>();
			foreach (string i in args)
			{
				if (i.StartsWith("-"))
				{
					if (i == "-q") quiet = true;
					else if (i.StartsWith("-odir:"))
					{
						string candidate_target = last<string>(i.Split(':'));
						if (Directory.Exists(candidate_target)) target_directory = candidate_target;
						else {error = "Cannot find output directory " + candidate_target; return false;}
					}
					else {error = "Unkown option " + i; return false;}
				}
				else
				{
					string[] candidate_files = Directory.GetFiles(".", i);
					foreach (string candidate_file in candidate_files)
					{
						if (File.Exists(candidate_file)) all_input_files.Add(candidate_file);
						else {error = "Cannot find input file " + candidate_file; return false;}
					}
				}
			}
			all_convert_files = all_input_files.ToArray();
			return true;
		}
		private static string get_output_base_filename(string inputfilename)	
		{
			string raw_filename = Path.GetFileNameWithoutExtension(inputfilename);
			return raw_filename;
		}
		private static T last<T>(T[] arr)
		{
			return arr[arr.Length - 1];
		}
		public static void printall(string[] a)
		{
			foreach (string i in a)
			{
				Console.WriteLine(i);
			}
		}
	}
}
