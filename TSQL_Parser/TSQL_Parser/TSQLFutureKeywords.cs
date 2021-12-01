using System;
using System.Collections.Generic;
using System.Linq;

namespace TSQL
{
	public struct TSQLFutureKeywords
	{
		private static Dictionary<string, TSQLFutureKeywords> keywordLookup =
			new Dictionary<string, TSQLFutureKeywords>(StringComparer.OrdinalIgnoreCase);

		public static readonly TSQLFutureKeywords None = new TSQLFutureKeywords("");



		public static readonly TSQLFutureKeywords OUTPUT = new TSQLFutureKeywords("OUTPUT");
		public static readonly TSQLFutureKeywords USING = new TSQLFutureKeywords("USING");
		public static readonly TSQLFutureKeywords OFFSET = new TSQLFutureKeywords("OFFSET");



		private readonly string Keyword;

		private TSQLFutureKeywords(
			string keyword)
		{
			Keyword = keyword;
			if (!string.IsNullOrWhiteSpace(keyword))
			{
				keywordLookup[keyword] = this;
			}
		}

		public static TSQLFutureKeywords Parse(
			string token)
		{
			if (keywordLookup.ContainsKey(token))
			{
				return keywordLookup[token];
			}
			else
			{
				return TSQLFutureKeywords.None;
			}
		}

		public static bool IsFutureKeyword(
			string token)
		{
			if (!string.IsNullOrWhiteSpace(token))
			{
				return keywordLookup.ContainsKey(token);
			}
			else
			{
				return false;
			}
		}

		public bool In(params TSQLFutureKeywords[] keywords)
		{
			return
				keywords != null &&
				keywords.Contains(this);
		}



		public static bool operator ==(
			TSQLFutureKeywords a,
			TSQLFutureKeywords b)
		{
			return a.Equals(b);
		}

		public static bool operator !=(
			TSQLFutureKeywords a,
			TSQLFutureKeywords b)
		{
			return !(a == b);
		}

		private bool Equals(TSQLFutureKeywords obj)
		{
			return Keyword == obj.Keyword;
		}

		public override bool Equals(object obj)
		{
			if (obj is TSQLFutureKeywords)
			{
				return Equals((TSQLFutureKeywords)obj);
			}
			else
			{
				return false;
			}
		}

		public override int GetHashCode()
		{
			return Keyword.GetHashCode();
		}


	}
}
