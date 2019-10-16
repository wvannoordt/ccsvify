using System;
using System.IO;
using System.Collections.Generic;

namespace ccsvify
{
	public class Section
	{
		private string header;
		private CsvableString[] contents;
		public Section(string _header, CsvableString[] _contents)
		{
			header = _header;
			contents = _contents;
		}
		public void Summarize()
		{
			Console.WriteLine(header);
			if (contents.Length < 5)
			{
				foreach (CsvableString i in contents)
				{
					Console.WriteLine(i);
				}
			}
			else
			{
				Console.WriteLine(contents[0]);	
				Console.WriteLine(contents[1]);	
				Console.WriteLine("[...]");	
				Console.WriteLine(contents[contents.Length - 2]);	
				Console.WriteLine(contents[contents.Length - 1]);			
			}
		}
		public string[] GetContents()
		{
			string[] output = new string[contents.Length];
			for (int i = 0; i < output.Length; i++)
			{
				output[i] = contents[i].ProcessedContents;
			}
			return output;
		}
		public static Section[] ProcessFile(string input_file)
		{
			string[] filecontents = File.ReadAllLines(input_file);
			if (filecontents.Length == 0) return new Section[]{};
			List<Section> section_list = new List<Section>();
			
			string currentheader = "[beginning of file]";
			List<CsvableString> current_stringlist = new List<CsvableString>();
			bool on_csvable_section = (new CsvableString(filecontents[0]).IsValid);
			for (int i = 0; i < filecontents.Length; i++)
			{
				CsvableString currentline = new CsvableString(filecontents[i]);
				bool currentvalid = currentline.IsValid;
				if (on_csvable_section && !currentvalid)
				{
					section_list.Add(new Section((String)currentheader.Clone(), current_stringlist.ToArray()));
					current_stringlist = new List<CsvableString>();
					on_csvable_section = false;
				}
				if (!currentvalid) currentheader = currentline.ToString();
				else current_stringlist.Add(currentline);
				if (!on_csvable_section && currentvalid)
				{
					on_csvable_section = true;
				}
			}
			if (current_stringlist.Count > 0) section_list.Add(new Section(currentheader, current_stringlist.ToArray()));
			return section_list.ToArray();
		}
	}
}
