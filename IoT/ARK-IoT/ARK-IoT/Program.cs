using System;
using System.Media;

namespace ARK_IoT
{
	class Program
	{
		static void Main(string[] args)
		{
			Reader reader = new Reader();
			reader.LoadSettings("Configuration/readerConfig.json");
			reader.Start();

			Console.ReadLine();
		}
	}
}
