using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	/// <summary>
	/// Published Status
	/// </summary>
	public enum Published
	{
		Draft, Publish
	}

	public enum PostTypes
	{
		Default, Video, Audio
	}

	/// <summary>
	/// Blog Post
	/// </summary>
	public class Post
	{
		public Post()
		{
			Categories = new List<PostCategory>();
			Tags = new List<PostTag>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[MaxLength(255)]
		[Required]
		public string Title { get; set; }
		public string Slug
		{
			get
			{
				return Utils.ConvertToSlug(Title);
			}
		}
		public PostTypes Type { get; set; }
		public string Body { get; set; }
		[Required]
		public string Caption { get; set; }
		public string Author { get; set; }

		public virtual SiteImage Image { get; set; }
		public int? ImageId { get; set; }

		public DateTime Created { get; set; }
		public DateTime Modified { get; set; }
		public bool Deleted { get; set; }
		public Published Published { get; set; }

		public string AudioUrl { get; set; }
		public string VideoUrl { get; set; }

		public virtual List<Comment> Comments { get; set; }
		public virtual List<PostCategory> Categories { get; set; }
		public virtual List<PostTag> Tags { get; set; }

		[NotMapped]
		public string[] SelectedCategories { get; set; }
		[NotMapped]
		public string[] SelectedTags{ get; set; }
	}
}
