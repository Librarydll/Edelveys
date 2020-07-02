using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Edelveys
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		const string subkey = @"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\";

		public App()
		{
			this.Dispatcher.UnhandledException += OnDispatcherUnhandledException;
		}
		void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			string version = string.Empty;
			using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(subkey))
			{
				if (ndpKey != null && ndpKey.GetValue("Release") != null)
				{
					version = $".NET Framework Version: {CheckFor45PlusVersion((int)ndpKey.GetValue("Release"))}";
				}
				else
				{
					version = ".NET Framework Version 4.5 or later is not detected.";
				}
			}
			string errorMessage = string.Format("An unhandled exception occurred: {0}", e.Exception.Message);
			MessageBox.Show(errorMessage +"\n"+e.Exception?.InnerException, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			using (var wr =new StreamWriter("log.txt"))
			{
				wr.WriteLine("error "+errorMessage);
				wr.WriteLine("inner exception "+e.Exception?.InnerException?.Message);
				wr.WriteLine(version);
			}
			// OR whatever you want like logging etc. MessageBox it's just example
			// for quick debugging etc.
		}

		string CheckFor45PlusVersion(int releaseKey)
		{
			if (releaseKey >= 528040)
				return "4.8 or later";
			if (releaseKey >= 461808)
				return "4.7.2";
			if (releaseKey >= 461308)
				return "4.7.1";
			if (releaseKey >= 460798)
				return "4.7";
			if (releaseKey >= 394802)
				return "4.6.2";
			if (releaseKey >= 394254)
				return "4.6.1";
			if (releaseKey >= 393295)
				return "4.6";
			if (releaseKey >= 379893)
				return "4.5.2";
			if (releaseKey >= 378675)
				return "4.5.1";
			if (releaseKey >= 378389)
				return "4.5";
			// This code should never execute. A non-null release key should mean
			// that 4.5 or later is installed.
			return "No 4.5 or later version detected";
		}
	}
}
