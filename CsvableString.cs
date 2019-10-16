using System;

namespace ccsvify
{
	public class CsvableString
	{
		private string stuff, condensed_stuff;
		private bool is_valid;
		private char[] DEFAULT_DELIMITERS = {' ', ',', '\t'};
		public bool IsValid {get{return is_valid;}}
		public string ProcessedContents {get{return condensed_stuff;}}
		public CsvableString(string _stuff)
		{
			stuff = _stuff.Trim();
			condensed_stuff = condense(stuff, DEFAULT_DELIMITERS, ',');
			is_valid = validate();
		}
		public override string ToString()
		{
			return stuff;
		}
		public void Summarize()
		{
			Console.WriteLine("Input: " + stuff);
			Console.WriteLine("Output: " + condensed_stuff);
			Console.WriteLine("Valid: " + is_valid);
		}
		private bool validate()
		{
			string[] spt = condensed_stuff.Split(',');
			double junk;
			foreach (string i in spt)
			{
				if (!double.TryParse(i, out junk)) return false;
			}
			return true;
		}
		private string condense(string s, char[] bufferchar, char replacechar)
		{
			//x       y     z -> x,y,z
			string output = "";
			bool onspace = false;
			for (int i = 0; i < s.Length; i++)
			{
				//Lead
				if (!onspace && (search(DEFAULT_DELIMITERS, s[i])))
				{
					output += replacechar;
					onspace = true;
				}
				//Fall
				if (onspace && (!search(DEFAULT_DELIMITERS, s[i]))) onspace = false;
				if (!onspace) output += s[i];
			}
			return output;
		}
		private bool search(char[] xs, char y)
		{	
			foreach(char x in xs)
			{
				if (y == x) return true;
			}
			return false;
		}
	}
}
