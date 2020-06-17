using System;

namespace LB.Models
{
	public class ErrorViewModel
	{
		public string RequestId { get; set; }

		public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
	}

	public enum ResponseStatus
	{
		Success, Warning, Error
	}

	public class SiteResponse
	{
		public ResponseStatus Code { get; set; }
		public string Message { get; set; }

		public string Status
		{
			get
			{
				return Code.ToString();
			}
		}
	}

	public class ResourceAccessError
	{
		public string Resource{ get; set; }
		public Guid? Member { get; set; }
	}
}