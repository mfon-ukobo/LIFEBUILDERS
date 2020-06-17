using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class SiteImage
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }
		public string Alt { get; set; }
		public string Image { get; set; }
		public string Thumbnail { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }

		public virtual List<Post> Posts { get; set; }
		public virtual List<LBEvent> Events { get; set; }
		public virtual List<Resource> Resources { get; set; }
		public virtual GalleryItem GalleryImage { get; set; }
		public virtual List<FrontPageCarousel> FrontPageCarousels { get; set; }
	}

	public class GalleryItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public string Title { get; set; }
		public string Caption { get; set; }
		public SiteImage Image { get; set; }
		public int ImageId { get; set; }
		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
	}
}
