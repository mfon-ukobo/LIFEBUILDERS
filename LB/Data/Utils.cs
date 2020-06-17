using LB.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB
{
	public static class SessionExtensions
	{
		public static SiteResponse GetObject<SiteResponse>(this ISession session, string key)
		{
			var value = session.GetString(key);
			return value == null ? default : JsonConvert.DeserializeObject<SiteResponse>(value);
		}

		public static void SetObject(this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}
	}

	public class Crypto
	{
		public static string Hash(string value)
		{
			return Convert.ToBase64String(
				System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value))
				);
		}
	}

	/// <summary>
	/// Paginated list extensions
	/// </summary>
	public static class PaginatedListExtensions
	{
		public static int GetIndex<T>(this PaginatedList<T> list, T item)
		{
			int index = list.CompleteList.IndexOf(item) + 1;
			return index;
		}
	}

	/// <summary>
	/// DateTime Extensions
	/// </summary>
	public static class DateTimeExtensions
	{
		public static string MyString(this DateTime dateTime)
		{
			return dateTime.ToString("MMM, dd yyyy. hh:mm tt");
		}

		public static string MyString(this DateTime? dateTime)
		{
			return dateTime.HasValue ? dateTime.Value.ToString("MMM, dd yyyy. hh:mm tt") : null;
		}

		public static string MyString(this DateTime dateTime, bool dateOnly)
		{
			if (dateOnly)
			{
				return dateTime.ToString("MMM, dd yyyy");
			}
			else
			{
				return dateTime.ToString("MMM, dd yyyy. hh:mm tt");
			}
		}
	}

	/// <summary>
	/// General Utilities
	/// </summary>
	public static class Utils
	{
		/// <summary>
		/// Conver string to slug valid string
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string ConvertToSlug(string value)
		{
			if (value != null)
			{
				return value.Replace(" ", "-").ToLower();
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// Create Directory
		/// </summary>
		/// <param name="path"></param>
		public static void CreateDirectory(string path)
		{
			var dir = new DirectoryInfo(path);
			dir.Create();
		}

		/// <summary>
		/// Delete File
		/// </summary>
		/// <param name="path"></param>
		public static bool DeleteFile(string path)
		{
			var file = new FileInfo(path);
			file.Delete();
			if (Utils.FileExists(path))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		/// <summary>
		/// Check if file exists
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		public static bool FileExists(string path)
		{
			var file = new FileInfo(path);
			return file.Exists;
		}

		/// <summary>
		/// Generates a Random Password
		/// respecting the given strength requirements.
		/// </summary>
		/// <param name="opts">A valid PasswordOptions object
		/// containing the password strength requirements.</param>
		/// <returns>A random password</returns>
		public static string GenerateRandomPassword(PasswordOptions opts = null)
		{
			if (opts == null) opts = new PasswordOptions()
			{
				RequiredLength = 8,
				RequiredUniqueChars = 4,
				RequireDigit = true,
				RequireLowercase = true,
				RequireNonAlphanumeric = true,
				RequireUppercase = true
			};

			string[] randomChars = new[] {
				"ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase
				"abcdefghijkmnopqrstuvwxyz",    // lowercase
				"0123456789",                   // digits
				"!@$?_-"                        // non-alphanumeric
			};
			Random rand = new Random(Environment.TickCount);
			List<char> chars = new List<char>();

			if (opts.RequireUppercase)
				chars.Insert(rand.Next(0, chars.Count),
					randomChars[0][rand.Next(0, randomChars[0].Length)]);

			if (opts.RequireLowercase)
				chars.Insert(rand.Next(0, chars.Count),
					randomChars[1][rand.Next(0, randomChars[1].Length)]);

			if (opts.RequireDigit)
				chars.Insert(rand.Next(0, chars.Count),
					randomChars[2][rand.Next(0, randomChars[2].Length)]);

			if (opts.RequireNonAlphanumeric)
				chars.Insert(rand.Next(0, chars.Count),
					randomChars[3][rand.Next(0, randomChars[3].Length)]);

			for (int i = chars.Count; i < opts.RequiredLength
				|| chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
			{
				string rcs = randomChars[rand.Next(0, randomChars.Length)];
				chars.Insert(rand.Next(0, chars.Count),
					rcs[rand.Next(0, rcs.Length)]);
			}

			return new string(chars.ToArray());
		}

		/// <summary>
		/// Get error message from session
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static SiteResponse GetSessionError(HttpContext context)
		{
			var sess = context.Session.GetObject<SiteResponse>("response");
			context.Session.Remove("response");
			return sess;
		}

		/// <summary>
		/// Add Session Error
		/// </summary>
		/// <param name="controller"></param>
		/// <param name="type"></param>
		/// <param name="message"></param>
		public static void AddSessionError(this Controller controller, ResponseStatus type, string message)
		{
			var context = controller.HttpContext;
			SiteResponse response = new SiteResponse
			{
				Code = type,
				Message = message
			};
			context.Session.SetObject("AppError", response);
		}
	}

	public class NotificationManager
	{
		private readonly Context context;
		public NotificationManager(Context _context)
		{
			context = _context;
		}

		public async Task<bool> CreateNotification(string description, string url)
		{
			try
			{
				var notification = new Notification
				{
					Description = description,
					DateTime = DateTime.Now,
					IsSeen = false,
					Url = url
				};
				context.Notifications.Add(notification);
				await context.SaveChangesAsync();
				return true;
			}
			catch
			{
				return false;
			}
		}
	}




	public static class StringExtensions
	{
		public static bool Match(this string opt1, string opt2)
		{
			return opt1.ToUpper().Contains(opt2.ToUpper());
		}
	}
}