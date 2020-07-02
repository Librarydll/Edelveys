using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Edelveys.Models
{
	public class ImageContainer
	{
		public ImageContainer()
		{
			ImagePathes = new List<string>();
		}
		public IList<string> ImagePathes { get; set; }
	}
}
