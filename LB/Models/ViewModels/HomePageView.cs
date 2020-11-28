using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models.ViewModels
{
	public class HomePageView
	{
		public List<LBEvent> Events { get; set; }
		public List<Post> Posts { get; set; }
		public List<GalleryItem> Gallery { get; set; }
		public List<Resource> Resources { get; set; }
		public IEnumerable<Sermon> Sermons { get; set; }
		public LBEvent NextEvent { get; set; }
		public List<FrontPageCarousel> Carousels { get; set; }
	}

	public class FrontPageCarouselView
	{
		public PaginatedList<FrontPageCarousel> FrontPageCarousels { get; set; }
		public bool IsEditing { get; set; }
		public FrontPageCarousel FrontPageCarousel { get; set; }
	}
}
