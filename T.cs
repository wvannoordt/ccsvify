using System;

namespace ccsvify
{
	public static class T
	{
		public static void tests()
		{
			CsvableString l = new CsvableString("3.2   er  4.3	1.2     3.6,    	4");
			l.Summarize();
		}
	}
}
