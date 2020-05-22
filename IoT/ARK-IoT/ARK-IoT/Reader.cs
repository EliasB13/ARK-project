using ARK_IoT.Configuration;
using ARK_IoT.Menu;
using ARK_IoT.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Media;
using ARK_IoT.Sensors;

namespace ARK_IoT
{
	public class Reader
	{
		private ReaderSettings ReaderSettings { get; set; }

		public void LoadSettings(string fileName)
		{
			if (!File.Exists(fileName))
			{
				throw new ArgumentException($"File '{fileName}' wasn't found");
			}

			ReaderSettings = JsonConvert.DeserializeObject<ReaderSettings>(File.ReadAllText(fileName));
		}

		public void Start()
		{
			ConsoleMenu.DrawInit();
			LoadMainMenu();
		}

		private void LoadMainMenu()
		{
			var mainMenuItems = new List<string> { "Observe", "View reader info" };

			string input = "";
			int selector = 0;
			bool isNumber = false;

			while (true)
			{
				Console.Clear();
				ConsoleMenu.DrawMenu(mainMenuItems);

				input = Console.ReadLine();
				if (input == "off")
					Environment.Exit(1);

				isNumber = int.TryParse(input, out selector);
				if (isNumber && selector > 0 && selector <= mainMenuItems.Count)
				{
					switch (selector)
					{
						case 1:
							LoadObservationMenu();
							Console.Clear();
							ConsoleMenu.DrawMenu(mainMenuItems);
							break;
						case 2:
							LoadReaderInfoMenu();
							Console.Clear();
							ConsoleMenu.DrawMenu(mainMenuItems);
							break;
					}
				}
				else
				{
					ConsoleMenu.DrawError("Wrong input");
					ConsoleMenu.DrawMenu(mainMenuItems);
				}
			}
		}

		private void LoadObservationMenu()
		{
			string input = "";
			while (true)
			{
				Console.Clear();
				Console.WriteLine("Come to the reader (enter person card rfid number):");
				Console.WriteLine("\n\n\n\n0. Return to previous menu");
				Console.WriteLine("\nType 'off' to turn off reader");
				Console.SetCursorPosition(0, 1);

				input = RfidSensor.GetRfidCardNumberInRadius();

				if (input == "0")
					return;
				else if (input == "off")
					Environment.Exit(1);
				else if (input.Length != 20)
				{
					ConsoleMenu.DrawError("RFID length is 20 chars");
				}
				else if (input.Length == 20)
				{
					LoadObservationMenu(input);
					return;
				}
				else
					ConsoleMenu.DrawError("Wrong input");
			}
		}

		private void LoadObservationMenu(string personCardRfid)
		{
			Console.Clear();
			Console.WriteLine("Sending request");
			ReaderService service = new ReaderService(ReaderSettings);
			var res = service.Observe(personCardRfid).Result;
			
			if (res.Message == null)
			{
				Console.WriteLine("Observation registered");
				if (!res.IsRestricted) Console.WriteLine("User has access");
				else
				{
					Console.WriteLine("Observation restricted! Alarm!");
					SystemSounds.Beep.Play();
				}
				Thread.Sleep(2000);
				Console.Clear();
				LoadMainMenu();
			}
			else
			{
				ConsoleMenu.DrawError(res.Message);
			}
			Console.ReadLine();
		}

		private void LoadReaderInfoMenu()
		{
			Console.Clear();
			Console.WriteLine("Loading");

			ReaderService service = new ReaderService(ReaderSettings);
			var res = service.GetReaderData().Result;

			if (res.Message == null)
			{
				string input = "";
				while (true)
				{
					Console.Clear();
					Console.WriteLine($"Reader id: { ReaderSettings.ReaderId }");
					Console.WriteLine($"Reader name: { res.Name }");
					Console.WriteLine($"Reader description: { res.Description }");

					Console.WriteLine();
					Console.WriteLine("0. Return to previous menu");
					Console.WriteLine("\nType 'off' to turn off reader");
					input = Console.ReadLine();
					if (input == "0")
						return;
					else if (input == "off")
						Environment.Exit(1);
					else
						ConsoleMenu.DrawError("Wrong input");
				}
			}
			else
			{
				ConsoleMenu.DrawError(res.Message);
			}
			
		}
	}
}
