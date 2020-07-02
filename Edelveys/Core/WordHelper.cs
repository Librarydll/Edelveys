using Edelveys.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Edelveys.Core
{
	public class WordHelper
	{
		private int width = 240;
		private int height = 163;

		private int imageW = 700;
		private int imageH = 130;
		private readonly string _saveWordDocumentsPath = Path.Combine(Directory.GetCurrentDirectory(), "ready.docx");
		private readonly string _saveWordDocumentsPath2 = Path.Combine(Directory.GetCurrentDirectory(), "ready2.docx");

		private readonly string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "template.docx");
		private readonly string templatePath2 = Path.Combine(Directory.GetCurrentDirectory(), "templatealbum.docx");
		public string FIO { get; set; } = "<FIO>";
		public string DATE { get; set; } = "<DATE>";
		public string AGE { get; set; } = "<AGE>";
		DocX _document;
		public WordHelper()
		{

		}
		public void Create(Person person, IEnumerable<ImageSource> filepathCollection)
		{

			using (_document = DocX.Load(templatePath))
			{
				CreateDocument(person, filepathCollection);

				_document.SaveAs(EnsureCreateFile(templatePath,_saveWordDocumentsPath));
				Process.Start(_saveWordDocumentsPath);

			}
		}
		public void CreateImage(IEnumerable<string> filepathCollection) 	
		{
			using (_document = DocX.Load(templatePath2))
			{
				CreateImageDocument(filepathCollection);

				_document.SaveAs(EnsureCreateFile(templatePath2, _saveWordDocumentsPath2));
				Process.Start(_saveWordDocumentsPath2);

			}
		}
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

		private void CreateImageDocument(IEnumerable<string> filepathCollection)
		{
			try
			{
				ImageHelper.CreateAndFitImages(filepathCollection);
				var image1 = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "1.jpeg"));
				var image2 = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "2.jpeg"));
				var image3 = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "3.jpeg"));
				var image4 = _document.AddImage(Path.Combine(Directory.GetCurrentDirectory(), "4.jpeg"));
				var picture1 = image1.CreatePicture(imageH, imageW);
				var picture2 = image2.CreatePicture(imageH, imageW);
				var picture3 = image3.CreatePicture(imageH, imageW);
				var picture4 = image4.CreatePicture(imageH, imageW);

				_document.Paragraphs[0].InsertPicture(picture1);
				_document.Paragraphs[0].InsertPicture(picture2);
				_document.Paragraphs[0].InsertPicture(picture3);
				_document.Paragraphs[0].InsertPicture(picture4);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}

		public string EnsureCreateFile(string path, string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			File.Copy(path, fileName);
			return fileName;
		}

		

	}
}
