﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LB.Models
{
	public class PostTag
	{
		public int PostId { get; set; }
		public Post Post { get; set; }
		public int TagId { get; set; }
		public Tag Tag { get; set; }
	}
}
