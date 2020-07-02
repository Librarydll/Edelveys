using Edelveys.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Edelveys.Core
{
	public class WordHelper
	{
		private static int width = 240;
		private static int height = 163;
		private readonly string _saveWordDocumentsPath = Path.Combine(Directory.GetCurrentDirectory(), "ready.docx");

		private readonly string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "template.docx");
		public string FIO { get; set; } = "<FIO>";
		public string DATE { get; set; } = "<DATE>";
		public string AGE { get; set; } = "<AGE>";
		DocX _document;
		public WordHelper()
		{

		}
		public void Create(Person person, IEnumerable<string> filepathCollection)
		{

			using (_document = DocX.Load(templatePath))
			{
				CreateDocument(person, filepathCollection);

				_document.SaveAs(EnsureCreateFile());
				Process.Start(_saveWordDocumentsPath);

			}
		}
		//asdasd
		private void CreateDocument(Person person, IEnumerable<string> filepathCollection)
		{
			try
			{
				ImageHelper.CreateAndFitImages(filepathCollection);
				person.FIO = person.FIO == null ? string.Empty : person.FIO;
				person.Age = person.Age == null ? string.Empty : person.Age;
				_document.ReplaceText(FIO, person.FIO);
				_document.ReplaceText(AGE, person.Age);
				_document.ReplaceText(DATE, person.Date.ToShortDateString());
				var curruntTable = _document.Tables[2];

				var count = filepathCollection.Count();
				if (count >= 1)
				{
					var image = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "1.jpeg"));
					var picture = image.CreatePicture(height, width);
					curruntTable.Rows[0].Cells[0].Paragraphs[0].InsertPicture(picture);
				}
				if (count >= 2)
				{
					var image = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "2.jpeg"));
					var picture = image.CreatePicture(height, width);
					curruntTable.Rows[0].Cells[1].Paragraphs[0].InsertPicture(picture);
				}
				if (count >= 3)
				{
					var image = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "3.jpeg"));
					var picture = image.CreatePicture(height, width);

					curruntTable.Rows[1].Cells[0].Paragraphs[0].InsertPicture(picture);
					//curruntTable.InsertRow();
					//curruntTable.Rows[1].Cells[0].Paragraphs[0].InsertPicture(picture);
				}
				if (count >= 4)
				{
					var image = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "4.jpeg"));
					var picture = image.CreatePicture(height, width);
					//curruntTable.Rows[1].Cells[1].Paragraphs[0].InsertPicture(picture);
					curruntTable.Rows[1].Cells[1].Paragraphs[0].InsertPicture(picture);

				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public string EnsureCreateFile()
		{
			if (File.Exists(_saveWordDocumentsPath))
			{
				File.Delete(_saveWordDocumentsPath);
			}

			File.Copy(templatePath, _saveWordDocumentsPath);
			return _saveWordDocumentsPath;
		}

		

	}
}
